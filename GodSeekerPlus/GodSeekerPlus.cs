using System.Reflection;
using Modding;

namespace GodSeekerPlus {
	public sealed class GodSeekerPlus : Mod, IGlobalSettings<GlobalSettings>, ILocalSettings<LocalSettings> {
		public static GodSeekerPlus LoadedInstance { get; private set; }
		public override string GetVersion() => Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

		public GlobalSettings GlobalSettings { get; set; } = new GlobalSettings();
		public LocalSettings LocalSettings { get; set; } = new LocalSettings();


		public override void Initialize() {
			if (LoadedInstance != null) {
				return;
			}

			LoadedInstance = this;
			Hook();
		}

		public void Unload() {
			UnHook();
			LoadedInstance = null;
		}


		private void Hook() {
			if (GlobalSettings.fastDreamWarp) {
				FastDreamWarp.Hook();
			}
		}

		private void UnHook() {
			FastDreamWarp.UnHook();
		}


		public void OnLoadGlobal(GlobalSettings s) => GlobalSettings = s;

		public GlobalSettings OnSaveGlobal() => GlobalSettings;

		public void OnLoadLocal(LocalSettings s) => LocalSettings = s;

		public LocalSettings OnSaveLocal() => LocalSettings;
	}
}
