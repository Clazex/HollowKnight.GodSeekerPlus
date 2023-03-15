namespace GodSeekerPlus.Settings;

[PublicAPI]
public sealed class GlobalSettings : SettingBase<GlobalSettingAttribute> {
	private readonly Dictionary<string, bool> modules = ModuleManager
		.Modules
		.Filter(pair => !pair.Value.Hidden)
		.ToDictionary(
			pair => pair.Key,
			pair => pair.Value.DefaultEnabled
		);

	[JsonProperty(PropertyName = nameof(modules))]
	public Dictionary<string, bool> Modules {
		get => modules;
		set {
			foreach (KeyValuePair<string, bool> pair in value) {
				if (modules.ContainsKey(pair.Key)) {
					modules[pair.Key] = pair.Value;

					_ = ModuleManager.TryGetModule(pair.Key, out Module? module);
					module!.Enabled = pair.Value;
				}
			}
		}
	}
}
