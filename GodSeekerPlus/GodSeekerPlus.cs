using System.Reflection;
using Modding;
using GodSeekerPlus.Modules;

namespace GodSeekerPlus {
	public sealed class GodSeekerPlus : Mod, IGlobalSettings<GlobalSettings>, ILocalSettings<LocalSettings> {
		public static GodSeekerPlus Instance { get; private set; }
		public override string GetVersion() => Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

		public GlobalSettings GlobalSettings { get; set; } = new GlobalSettings();
		public LocalSettings LocalSettings { get; set; } = new LocalSettings();


		public override void Initialize() {
			if (Instance != null) {
				return;
			}

			Instance = this;
			GlobalSettings.Coerce();

			this.LoadModules();
		}

		public void Unload() {
			this.UnloadModules();

			Instance = null;
		}

		public void OnLoadGlobal(GlobalSettings s) => GlobalSettings = s;

		public GlobalSettings OnSaveGlobal() => GlobalSettings;

		public void OnLoadLocal(LocalSettings s) => LocalSettings = s;

		public LocalSettings OnSaveLocal() => LocalSettings;
	}
}
