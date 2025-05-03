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
		booleans = boolFields.Values.Flatten().ToDictionary(
			pair => pair.Key,
			pair => pair.Value.getter.Invoke()
		);

		integers = intFields.Values.Flatten().ToDictionary(
			pair => pair.Key,
			pair => pair.Value.getter.Invoke()
		);

		floats = floatFields.Values.Flatten().ToDictionary(
			pair => pair.Key,
			pair => pair.Value.getter.Invoke()
		);

		enums = enumFields.Values.Flatten().ToDictionary(
			pair => pair.Key,
			pair => pair.Value.getter.Invoke()
		);
	}

	private void WriteFields() {
		boolFields.Values.Flatten().ForEach(pair => pair.Value.setter.Invoke(booleans![pair.Key]));
		intFields.Values.Flatten().ForEach(pair => pair.Value.setter.Invoke(integers![pair.Key]));
		floatFields.Values.Flatten().ForEach(pair => pair.Value.setter.Invoke(floats![pair.Key]));
		enumFields.Values.Flatten().ForEach(pair => pair.Value.setter.Invoke(enums![pair.Key]));

		booleans = null;
		integers = null;
		floats = null;
		enums = null;
	}


	public SettingBase() {
		FieldInfo[] fields = Assembly
			.GetExecutingAssembly()
			.GetTypesSafely()
			.FlatMap(t => t.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
			.Filter(fi => Attribute.IsDefined(fi, typeof(TAttr)))
			.ToArray();

		boolFields = ProcessFields<bool>(fields);
		intFields = ProcessFields<int>(fields);
		floatFields = ProcessFields<float>(fields);
		enumFields = ProcessFields<object, Enum>(fields);

		ReadFields();
	}

	internal IEnumerable<HorizontalOption> GetMenuOptions(string category) {
		List<HorizontalOption> options = new();
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

				options.Add(Blueprints.GenericHorizontalOption(
					$"Settings/{name}".Localize(),
					descPrefix + $"Modules/{fi.DeclaringType.Name}".Localize(),
					attr.Options,
					setter,
					getter
				));
			}
		}

		if (this.floatFields.TryGetValue(category, out Dictionary<string, SettingInfo<float>> floatFields)) {
			foreach ((string name, (FieldInfo fi, Func<float> getter, Action<float> setter)) in floatFields) {
				if (fi.GetCustomAttribute<FloatOptionAttribute>() is not FloatOptionAttribute attr) {
					continue;
				}

				options.Add(Blueprints.GenericHorizontalOption(
					$"Settings/{name}".Localize(),
					descPrefix + $"Modules/{fi.DeclaringType.Name}".Localize(),
					attr.Options,
					setter,
					getter
				));
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
					Enum.GetValues(fi.FieldType).Cast<object>()
						.Map((val) => new EnumWrapper(name, fi.FieldType, val))
						.ToArray(),
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

		public void Deconstruct(out FieldInfo fi, out Func<T> getter, out Action<T> setter) {
			fi = this.fi;
			getter = this.getter;
			setter = this.setter;
		}
	}

	public class EnumWrapper : IFormattable {
		public string Name { get; private init; }
		public string Variant { get; private init; }
		public object Value { get; private init; }

		public EnumWrapper(string name, Type enumType, object value) {
			Name = name;
			Variant = Enum.GetName(enumType, value);
			Value = value;
		}

		public override bool Equals(object obj) =>
			obj is EnumWrapper other && other.Value.Equals(Value);

		// This is unused, just for suppressing the warning.
		public override int GetHashCode() => Value.GetHashCode();

		public override string ToString() => $"Settings/{Name}/{Variant}".Localize();

		string IFormattable.ToString(string format, IFormatProvider formatProvider) => ToString();
	}
}
