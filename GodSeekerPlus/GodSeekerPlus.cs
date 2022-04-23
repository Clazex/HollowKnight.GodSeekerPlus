namespace GodSeekerPlus;

public sealed partial class GodSeekerPlus : Mod, ITogglableMod {
	public static GodSeekerPlus? Instance { get; private set; }
	public static GodSeekerPlus UnsafeInstance => Instance!;

	private static readonly Lazy<string> version = AssemblyUtil
#if DEBUG
		.GetMyDefaultVersionWithHash();
#else
		.GetMyDefaultVersion();
#endif

	public override string GetVersion() => version.Value;

	internal static bool satchelPresent = false;

	static GodSeekerPlus() {
		try {
			DetectSatchel();
		} catch (System.IO.FileNotFoundException) {
		} catch (Exception e) {
			Logger.LogError(e.ToString());
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
			satchelPresent = typeof(Satchel.BetterMenus.Menu)
				.GetConstructor(new[] {
					typeof(string),
					typeof(Satchel.BetterMenus.Element[])
				}) != null;
		} catch (TypeLoadException) {
		} catch (MissingMethodException) {
		}
	}
}
