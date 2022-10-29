using System.Runtime.Serialization;

using Mono.Cecil.Cil;

using MonoMod.Utils;

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
			.GetTypes()
			.FlatMap(t => t.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
			.Filter(fi => Attribute.IsDefined(fi, typeof(TAttr)))
			.ToArray();

		boolFields = ProcessFields<bool>(fields);
		intFields = ProcessFields<int>(fields);
		floatFields = ProcessFields<float>(fields);

		ReadFields();
	}

	private static Dictionary<string, Dictionary<string, (FieldInfo fi, Func<TField> getter, Action<TField> setter, bool isOption)>> ProcessFields<TField>(FieldInfo[] fields) => fields
		.Filter(fi => fi.FieldType == typeof(TField))
		.Map(fi => {
			DynamicMethodDefinition gdmd = ILUtil.CreateDMD<Func<TField>>($"<GSPSetting>_get_{fi.Name}");
			ILProcessor gilp = gdmd.GetILProcessor();
			gilp.Emit(OpCodes.Ldsfld, fi);
			gilp.Emit(OpCodes.Ret);
			Func<TField> getter = gdmd.Generate().CreateDelegate<Func<TField>>();

			DynamicMethodDefinition sdmd = ILUtil.CreateDMD<Action<TField>>($"<GSPSetting>_set_{fi.Name}");
			ILProcessor silp = sdmd.GetILProcessor();
			silp.Emit(OpCodes.Ldarg_0);
			silp.Emit(OpCodes.Stsfld, fi);
			silp.Emit(OpCodes.Ret);
			Action<TField> setter = sdmd.Generate().CreateDelegate<Action<TField>>();

			return (fi, getter, setter);
		})
		.GroupBy(tuple => tuple.fi.DeclaringType.Namespace
			.StripStart(nameof(GodSeekerPlus) + '.' + nameof(Modules) + '.')
			?? nameof(Modules.Misc)
		)
		.ToDictionary(
			group => group.Key,
			group => group.ToDictionary(
				tuple => tuple.fi.Name,
				tuple => (tuple.fi, tuple.getter, tuple.setter, Attribute.IsDefined(tuple.fi, typeof(OptionAttribute)))
			)
		);
}
