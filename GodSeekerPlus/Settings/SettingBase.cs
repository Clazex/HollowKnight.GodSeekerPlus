using System.Runtime.Serialization;

using Modding.Utils;

using Satchel.BetterMenus;

namespace GodSeekerPlus.Settings;

public abstract class SettingBase<TAttr> where TAttr : Attribute {
	internal Dictionary<string, Dictionary<string, SettingInfo<bool>>> boolFields = null!;
	internal Dictionary<string, Dictionary<string, SettingInfo<int>>> intFields = null!;
	internal Dictionary<string, Dictionary<string, SettingInfo<float>>> floatFields = null!;
	internal Dictionary<string, Dictionary<string, SettingInfo<object>>> enumFields = null!;

	public Dictionary<string, bool>? booleans;
	public Dictionary<string, int>? integers;
	public Dictionary<string, float>? floats;
	public Dictionary<string, object>? enums;

	[OnSerializing]
	public void OnBeforeSerialize(StreamingContext context) => ReadFields();

	[OnDeserialized]
	public void OnAfterDeserialize(StreamingContext context) => WriteFields();


	private void ReadFields() {
		static Dictionary<string, T> Read<T>(Dictionary<string, Dictionary<string, SettingInfo<T>>> fields) =>
			fields.Values.Flatten().ToDictionary(
				pair => pair.Key,
				pair => pair.Value.getter.Invoke()
			);

		booleans = Read(boolFields);
		integers = Read(intFields);
		floats = Read(floatFields);
		enums = Read(enumFields);
	}

	private void WriteFields() {
		static void Write<T>(Dictionary<string, Dictionary<string, SettingInfo<T>>> fields, Dictionary<string, T> values) =>
			fields.Values.Flatten().ForEach(
				pair => pair.Value.setter.Invoke(values![pair.Key]!)
			);
		
		Write(boolFields, booleans!);
		Write(intFields, integers!);
		Write(floatFields, floats!);
		enumFields.Values.Flatten().ForEach(pair => pair.Value.setter.Invoke(
			Enum.ToObject(pair.Value.fi.FieldType, enums![pair.Key]!)
		));

		booleans = null;
		integers = null;
		floats = null;
		enums = null;
	}


	public SettingBase() {
		FieldInfo[] fields = [..Assembly
			.GetExecutingAssembly()
			.GetTypesSafely()
			.FlatMap(t => t.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
			.Filter(fi => Attribute.IsDefined(fi, typeof(TAttr)))
		];

		boolFields = ProcessFields<bool>(fields);
		intFields = ProcessFields<int>(fields);
		floatFields = ProcessFields<float>(fields);
		enumFields = ProcessFields<object, Enum>(fields);

		ReadFields();
	}

	internal IEnumerable<Element> GetMenuElements(string category) {
		List<Element> options = [];
		string descPrefix = "BelongsToModule".Localize();

		if (this.boolFields.TryGetValue(category, out Dictionary<string, SettingInfo<bool>> boolFields)) {
			foreach ((string name, (FieldInfo fi, Func<bool> getter, Action<bool> setter)) in boolFields) {
				if (!Attribute.IsDefined(fi, typeof(BoolOptionAttribute))) {
					continue;
				}

				options.Add(Blueprints.HorizontalBoolOption(
					$"Settings/{name}".Localize(),
					descPrefix + $"Modules/{fi.DeclaringType.Name}".Localize(),
					setter,
					getter
				));
			}
		}

		if (this.intFields.TryGetValue(category, out Dictionary<string, SettingInfo<int>> intFields)) {
			foreach ((string name, (FieldInfo fi, Func<int> getter, Action<int> setter)) in intFields) {
				if (fi.GetCustomAttribute<IntOptionAttribute>() is not IntOptionAttribute attr) {
					continue;
				}

				options.Add(attr.Type switch {
					OptionType.Option => Blueprints.GenericHorizontalOption(
						$"Settings/{name}".Localize(),
						descPrefix + $"Modules/{fi.DeclaringType.Name}".Localize(),
						attr.Options,
						setter,
						getter
					),
					OptionType.Slider => new CustomSlider(
						$"Settings/{name}".Localize(),
						(val) => setter((int) val),
						() => getter(),
						attr.Options.First(),
						attr.Options.Last(),
						true
					),
					_ => throw new NotImplementedException()
				});
			}
		}

		if (this.floatFields.TryGetValue(category, out Dictionary<string, SettingInfo<float>> floatFields)) {
			foreach ((string name, (FieldInfo fi, Func<float> getter, Action<float> setter)) in floatFields) {
				if (fi.GetCustomAttribute<FloatOptionAttribute>() is not FloatOptionAttribute attr) {
					continue;
				}

				options.Add(attr.Type switch {
					OptionType.Option => Blueprints.GenericHorizontalOption(
						$"Settings/{name}".Localize(),
						descPrefix + $"Modules/{fi.DeclaringType.Name}".Localize(),
						attr.Options,
						setter,
						getter
					),
					OptionType.Slider => new CustomSlider(
						$"Settings/{name}".Localize(),
						(val) => setter((int) val),
						() => getter(),
						attr.Options.First(),
						attr.Options.Last(),
						false
					),
					_ => throw new NotImplementedException()
				});
			}
		}

		if (this.enumFields.TryGetValue(category, out Dictionary<string, SettingInfo<object>> enumFields)) {
			foreach ((string name, (FieldInfo fi, Func<object> getter, Action<object> setter)) in enumFields) {
				if (!Attribute.IsDefined(fi, typeof(EnumOptionAttribute))) {
					continue;
				}

				options.Add(Blueprints.GenericHorizontalOption(
					$"Settings/{name}".Localize(),
					descPrefix + $"Modules/{fi.DeclaringType.Name}".Localize(),
					[..Enum.GetValues(fi.FieldType).Cast<object>()
						.Map((val) => new EnumWrapper(name, fi.FieldType, val))
					],
					(val) => setter(val.Value),
					() => new EnumWrapper(name, fi.FieldType, getter())
				));
			}
		}

		return options;
	}

	private static Dictionary<string, Dictionary<string, SettingInfo<TField>>> ProcessFields<TField>(FieldInfo[] fields) =>
		ProcessFields<TField, TField>(fields);

	private static Dictionary<string, Dictionary<string, SettingInfo<TField>>> ProcessFields<TField, TAs>(FieldInfo[] fields) => fields
		.Filter(fi => typeof(TAs).IsAssignableFrom(fi.FieldType))
		.Map(fi => {
			_ = ModuleManager.TryGetModule(fi.DeclaringType, out Module? module);
			return (module!, fi);
		})
		.Map((tuple) => {
			(Module module, FieldInfo fi) = tuple;
			(Func<TField> getter, Action<TField> setter) = fi.GetFastStaticAccessors<TField>();
			bool reloadOnUpdate = Attribute.IsDefined(fi, typeof(ReloadOnUpdateAttribute));

			SettingInfo<TField> info = new() {
				fi = fi,
				setter = reloadOnUpdate
					? (val) => {
						setter(val);
						if (module.Active) {
							module.Active = false;
							module.Active = true;
						}
					}
				: setter,
				getter = getter
			};

			return (module, info);
		})
		.GroupBy((tuple) => tuple.module.Category)
		.ToDictionary(
			group => group.Key,
			group => group.ToDictionary(
				tuple => tuple.info.fi.Name,
				tuple => tuple.info
			)
		);

	public struct SettingInfo<T> {
		public FieldInfo fi;
		public Func<T> getter;
		public Action<T> setter;

		public readonly void Deconstruct(out FieldInfo fi, out Func<T> getter, out Action<T> setter) {
			fi = this.fi;
			getter = this.getter;
			setter = this.setter;
		}
	}

	public class EnumWrapper(string name, Type enumType, object value) : IFormattable {
		public string Name { get; private init; } = name;
		public string Variant { get; private init; } = Enum.GetName(enumType, value);
		public object Value { get; private init; } = value;

		public override bool Equals(object obj) =>
			obj is EnumWrapper other && other.Value.Equals(Value);

		// This is unused, just for suppressing the warning.
		public override int GetHashCode() => Value.GetHashCode();

		public override string ToString() => $"Settings/{Name}/{Variant}".Localize();

		string IFormattable.ToString(string format, IFormatProvider formatProvider) => ToString();
	}
}
