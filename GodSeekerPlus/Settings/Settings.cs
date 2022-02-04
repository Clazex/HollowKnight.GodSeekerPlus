namespace GodSeekerPlus;

public sealed partial class GodSeekerPlus
	: IGlobalSettings<GlobalSettings>, ILocalSettings<LocalSettings> {
	public static GlobalSettings GlobalSettings { get; private set; } = new();
	public void OnLoadGlobal(GlobalSettings s) => GlobalSettings = s;
	public GlobalSettings OnSaveGlobal() => GlobalSettings;

	public static LocalSettings LocalSettings { get; private set; } = new();
	public void OnLoadLocal(LocalSettings s) => LocalSettings = s;
	public LocalSettings OnSaveLocal() => LocalSettings;
}
