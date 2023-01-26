using System.Runtime.Serialization;

using Modding.Utils;

using Satchel.BetterMenus;

namespace GodSeekerPlus.Settings;

public abstract class SettingBase {
	internal Dictionary<string, Dictionary<string, (FieldInfo fi, Func<bool> getter, Action<bool> setter, bool isOption)>> boolFields = null!;
	internal Dictionary<string, Dictionary<string, (FieldInfo fi, Func<int> getter, Action<int> setter, bool isOption)>> intFields = null!;
	internal Dictionary<string, Dictionary<string, (FieldInfo fi, Func<float> getter, Action<float> setter, bool isOption)>> floatFields = null!;
}

public abstract class SettingBase<TAttr> : SettingBase where TAttr : Attribute {
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

		if (this.boolFields.TryGetValue(category, out Dictionary<string, (FieldInfo fi, Func<bool> getter, Action<bool> setter, bool isOption)> boolFields)) {
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

		if (this.intFields.TryGetValue(category, out Dictionary<string, (FieldInfo fi, Func<int> getter, Action<int> setter, bool isOption)> intFields)) {
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

		if (this.floatFields.TryGetValue(category, out Dictionary<string, (FieldInfo fi, Func<float> getter, Action<float> setter, bool isOption)> floatFields)) {
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


	private static Dictionary<string, Dictionary<string, (FieldInfo fi, Func<TField> getter, Action<TField> setter, bool isOption)>> ProcessFields<TField>(FieldInfo[] fields) => fields
		.Filter(fi => fi.FieldType == typeof(TField))
		.Map(fi => {
			(Func<TField> getter, Action<TField> setter) = fi.GetFastStaticAccessors<TField>();
			return (fi, getter, setter);
		})
		.GroupBy(tuple => {
			_ = ModuleManager.TryGetModule(tuple.fi.DeclaringType, out Module? module);
			return module!.Category;
		})
		.ToDictionary(
			group => group.Key,
			group => group.ToDictionary(
				tuple => tuple.fi.Name,
				tuple => (tuple.fi, tuple.getter, tuple.setter, Attribute.IsDefined(tuple.fi, typeof(OptionAttribute)))
			)
		);
}
