namespace GodSeekerPlus.Settings;

[PublicAPI]
public sealed class GlobalSettings : SettingBase<GlobalSettingAttribute> {
	private readonly Dictionary<string, bool> modules = ModuleManager
		.FindModules()
		.Filter(type => !Attribute.IsDefined(type, typeof(HiddenAttribute)))
		.ToDictionary(
			type => type.Name,
			type => Attribute.IsDefined(type, typeof(DefaultEnabledAttribute))
		);

	[JsonProperty(PropertyName = nameof(modules))]
	public Dictionary<string, bool> Modules {
		get => modules;
		set {
			foreach (KeyValuePair<string, bool> pair in value) {
				if (modules.ContainsKey(pair.Key)) {
					modules[pair.Key] = pair.Value;
				}
			}
		}
	}
}
