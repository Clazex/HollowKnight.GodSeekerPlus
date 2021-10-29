using Modding;

namespace GodSeekerPlus {
	public sealed partial class GodSeekerPlus :
		IGlobalSettings<GlobalSettings>, ILocalSettings<LocalSettings> {
		public GlobalSettings GlobalSettings { get; internal set; } = new();
		public void OnLoadGlobal(GlobalSettings s) => GlobalSettings = s;
		public GlobalSettings OnSaveGlobal() => GlobalSettings;

		public LocalSettings LocalSettings { get; internal set; } = new();
		public void OnLoadLocal(LocalSettings s) => LocalSettings = s;
		public LocalSettings OnSaveLocal() => LocalSettings;
	}
}
