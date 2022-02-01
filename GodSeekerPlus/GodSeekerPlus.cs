namespace GodSeekerPlus;

public sealed partial class GodSeekerPlus : Mod, ITogglableMod {
	public static GodSeekerPlus? Instance { get; private set; }
	public static GodSeekerPlus UnsafeInstance => Instance!;

	public override string GetVersion() => VersionUtil.Version.Value;

	internal static bool satchelPresent = false;

	static GodSeekerPlus() {
		try {
			DetectSatchel();
		} catch (System.IO.FileNotFoundException) {
		} catch (Exception e) {
			Modding.Logger.LogError($"[GodSeekerPlus] - {e}");
		}
	}

	public override void Initialize() {
		if (Instance != null) {
			Logger.LogWarn("Attempting to initialize multiple times, operation rejected");
			return;
		}

		Instance = this;

		ModuleManager.Load();
	}

	public void Unload() {
		ModuleManager.Unload();

		Instance = null;
	}

	private static void DetectSatchel() {
		try {
			Activator.CreateInstance<Satchel.BetterMenus.Menu>();
		} catch (MissingMethodException) {
			satchelPresent = true;
		}
	}
}
