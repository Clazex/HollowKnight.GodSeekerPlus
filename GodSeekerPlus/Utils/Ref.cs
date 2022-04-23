namespace GodSeekerPlus.Utils;

internal static class Ref {
	internal static GameManager GM => GameManager.instance;

	internal static HeroController HC => HeroController.instance;

	internal static PlayerData PD => PlayerData.instance;

	internal static SceneData SD => SceneData.instance;

	internal static GameCameras GC => GameCameras.instance;

	internal static GodSeekerPlus GSP => GodSeekerPlus.UnsafeInstance;

	internal static GlobalSettings GS => GodSeekerPlus.GlobalSettings;
	internal static LocalSettings LS => GodSeekerPlus.LocalSettings;
}
