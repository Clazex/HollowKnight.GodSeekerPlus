using System.Runtime.Serialization;

using Modding.Utils;

using Satchel.BetterMenus;

namespace GodSeekerPlus.Settings;

public abstract class SettingBase<TAttr> where TAttr : Attribute {
	internal Dictionary<string, Dictionary<string, SettingInfo<bool>>> boolFields = null!;
	internal Dictionary<string, Dictionary<string, SettingInfo<int>>> intFields = null!;
	internal Dictionary<string, Dictionary<string, SettingInfo<float>>> floatFields = null!;

	public Dictionary<string, bool> booleans = null!;
	public Dictionary<string, int> integers = null!;
	public Dictionary<string, float> floats = null!;


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
	}

	private void WriteFields() {
		boolFields.Values.Flatten().ForEach(pair => pair.Value.setter.Invoke(booleans[pair.Key]));
		intFields.Values.Flatten().ForEach(pair => pair.Value.setter.Invoke(integers[pair.Key]));
		floatFields.Values.Flatten().ForEach(pair => pair.Value.setter.Invoke(floats[pair.Key]));

		booleans.Clear();
		integers.Clear();
		floats.Clear();
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

		ReadFields();
	}

	internal IEnumerable<HorizontalOption> GetMenuOptions(string category) {
		List<HorizontalOption> options = new();

		if (this.boolFields.TryGetValue(category, out Dictionary<string, SettingInfo<bool>> boolFields)) {
			foreach ((string name, (FieldInfo fi, Func<bool> getter, Action<bool> setter, bool isOption)) in boolFields) {
				if (!isOption) {
					continue;
				}

				BoolOptionAttribute optionAttr = fi.GetCustomAttribute<BoolOptionAttribute>();

				options.Add(optionAttr.CustomText ? Blueprints.HorizontalBoolOption(
					$"Settings/{name}".Localize(),
					$"Modules/{fi.DeclaringType.Name}".Localize(),
					setter,
					getter,
					$"Settings/{name}/True".Localize(),
					$"Settings/{name}/False".Localize()
				) : Blueprints.HorizontalBoolOption(
					$"Settings/{name}".Localize(),
					$"Modules/{fi.DeclaringType.Name}".Localize(),
					setter,
					getter
				));
			}
		}

		if (this.intFields.TryGetValue(category, out Dictionary<string, SettingInfo<int>> intFields)) {
			foreach ((string name, (FieldInfo fi, Func<int> getter, Action<int> setter, bool isOption)) in intFields) {
				if (!isOption) {
					continue;
				}

				options.Add(Blueprints.GenericHorizontalOption(
					$"Settings/{name}".Localize(),
					$"Modules/{fi.DeclaringType.Name}".Localize(),
					fi.GetCustomAttribute<IntOptionAttribute>().Options,
					setter,
					getter
				));
			}
		}

		if (this.floatFields.TryGetValue(category, out Dictionary<string, SettingInfo<float>> floatFields)) {
			foreach ((string name, (FieldInfo fi, Func<float> getter, Action<float> setter, bool isOption)) in floatFields) {
				if (!isOption) {
					continue;
				}

				options.Add(Blueprints.GenericHorizontalOption(
					$"Settings/{name}".Localize(),
					$"Modules/{fi.DeclaringType.Name}".Localize(),
					fi.GetCustomAttribute<FloatOptionAttribute>().Options,
					setter,
					getter
				));
			}
		}

		return options;
	}


	private static Dictionary<string, Dictionary<string, SettingInfo<TField>>> ProcessFields<TField>(FieldInfo[] fields) => fields
		.Filter(fi => fi.FieldType == typeof(TField))
		.Map(fi => {
			(Func<TField> getter, Action<TField> setter) = fi.GetFastStaticAccessors<TField>();
			return new SettingInfo<TField>() {
				fi = fi,
				setter = setter,
				getter = getter,
				isOption = Attribute.IsDefined(fi, typeof(OptionAttribute))
			};
		})
		.GroupBy(info => {
			_ = ModuleManager.TryGetModule(info.fi.DeclaringType, out Module? module);
			return module!.Category;
		})
		.ToDictionary(
			group => group.Key,
			group => group.ToDictionary(
				info => info.fi.Name,
				info => info
			)
		);

	public struct SettingInfo<T> {
		public FieldInfo fi;
		public Func<T> getter;
		public Action<T> setter;
		public bool isOption;

		public void Deconstruct(out FieldInfo fi, out Func<T> getter, out Action<T> setter, out bool isOption) {
			fi = this.fi;
			getter = this.getter;
			setter = this.setter;
			isOption = this.isOption;
		}
	}
}
