using System.Reflection;
using GodSeekerPlus.Modules;
using Modding;

namespace GodSeekerPlus {
	public sealed class GodSeekerPlus : Mod, IGlobalSettings<GlobalSettings>, ILocalSettings<LocalSettings> {
		public static GodSeekerPlus Instance { get; private set; }

		public override string GetVersion() => Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

		public GlobalSettings GlobalSettings { get; internal set; } = new();
		public LocalSettings LocalSettings { get; internal set; } = new();

		public override void Initialize() {
			if (Instance != null) {
				return;
			}

			Instance = this;
			GlobalSettings.Coerce();

			ModuleManager.LoadModules();
		}

		public void Unload() {
			ModuleManager.UnloadModules();

			Instance = null;
		}

		public void OnLoadGlobal(GlobalSettings s) => GlobalSettings = s;
		public GlobalSettings OnSaveGlobal() => GlobalSettings;

		public void OnLoadLocal(LocalSettings s) => LocalSettings = s;
		public LocalSettings OnSaveLocal() => LocalSettings;
	}
}
