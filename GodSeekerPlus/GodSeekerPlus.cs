using System.Reflection;
using GodSeekerPlus.Modules;
using Modding;

namespace GodSeekerPlus {
	public sealed class GodSeekerPlus : Mod{
		public static GodSeekerPlus Instance { get; private set; }

		public override string GetVersion() => "test";

		public GlobalSettings GlobalSetting=new GlobalSettings();
		public LocalSettings LocalSetting = new LocalSettings();

		public override void Initialize() {
			if (Instance != null) {
				return;
			}

			Instance = this;
			GlobalSetting.Coerce();

			ModuleManager.LoadModules();
		}

		public void Unload() {
			ModuleManager.UnloadModules();

			Instance = null;
		}
		public override ModSettings GlobalSettings { get => GlobalSetting; set => GlobalSetting = (GlobalSettings)value; }
		public override ModSettings SaveSettings { get => LocalSetting; set => LocalSetting = (LocalSettings)value; }
	}
}
