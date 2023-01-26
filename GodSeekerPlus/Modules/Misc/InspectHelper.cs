namespace GodSeekerPlus.Modules.Misc;

internal sealed class InspectHelper : Module {
	public override bool Hidden => true;

	private protected override void Load() =>
		ModHooks.FinishedLoadingModsHook += CreateGameObject;

	private static void CreateGameObject() {
		if (ModHooks.GetMod("Unity Explorer", true) == null) {
			return;
		}

		_ = GameObjectUtil.CreateHolder<Inspector>($"{nameof(GodSeekerPlus)} Inspect Helper");
		Logger.Log("Creating Inspect Helper GameObject");
	}

	private sealed class Inspector : MonoBehaviour {
		public GodSeekerPlus Instance => GodSeekerPlus.UnsafeInstance;
		public bool Active => GodSeekerPlus.Active;

		public Dictionary<string, Module> Modules => ModuleManager.Modules;

		public GlobalSettings GlobalSettings => Setting.Global;
		public LocalSettings LocalSettings => Setting.Local;

		public Dict Dict => L11nUtil.dict;

		public string Localize(string key) => L11nUtil.Localize(key);
	}
}
