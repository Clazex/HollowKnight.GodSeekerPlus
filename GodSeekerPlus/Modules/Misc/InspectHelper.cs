namespace GodSeekerPlus.Modules.Misc;

[Hidden]
internal sealed class InspectHelper : Module {
	private static readonly string GO_NAME = $"{nameof(GodSeekerPlus)} Inspect Helper";

	private protected override void Load() =>
		ModHooks.FinishedLoadingModsHook += CreateGameObject;

	private static void CreateGameObject() {
		if (ModHooks.GetMod("Unity Explorer", true) == null) {
			return;
		}

		if (GameObject.Find(GO_NAME) is not null and {
			scene.name: "DontDestroyOnLoad"
		}) {
			Logger.LogError("Inspect Helper GameObject already existed!");

			return;
		}

		Logger.Log("Creating Inspect Helper GameObject");
		var go = new GameObject(GO_NAME, typeof(Inspector));
		UObject.DontDestroyOnLoad(go);
	}

	private sealed class Inspector : MonoBehaviour {
		public GodSeekerPlus Instance => GodSeekerPlus.UnsafeInstance;

		public Dictionary<string, Module> Modules => ModuleManager.Modules;

		public GlobalSettings GlobalSettings => GodSeekerPlus.GlobalSettings;
		public LocalSettings LocalSettings => GodSeekerPlus.LocalSettings;

		public Dictionary<string, Dictionary<string, string>> Dict => L11nUtil.Dict.Value;

		public Dictionary<string, string>? GetL11n(string lang) =>
			L11nUtil.Dict.Value.TryGetValue(lang, out Dictionary<string, string>? dict) ? dict : null;

		public string Localize(string key) => L11nUtil.Localize(key);
	}
}
