namespace GodSeekerPlus;

[PublicAPI]
public sealed partial class GodSeekerPlus : Mod, ITogglableMod {
	public static GodSeekerPlus? Instance { get; private set; }
	public static GodSeekerPlus UnsafeInstance => Instance!;

	public static bool Active { get; private set; }

	private static readonly Lazy<string> version = AssemblyUtil
#if DEBUG
		.GetMyDefaultVersionWithHash();
#else
		.GetMyDefaultVersion();
#endif

	public override string GetVersion() => version.Value;

	public override string GetMenuButtonText() =>
		"ModName".Localize() + ' ' + Lang.Get("MAIN_OPTIONS", "MainMenu");

	internal static bool satchelPresent = false;

	static GodSeekerPlus() {
		try {
			DetectSatchel();
		} catch (System.IO.FileNotFoundException) {
		} catch (Exception e) {
			Logger.LogError(e.ToString());
		}
	}

	public GodSeekerPlus() => Instance = this;

	public override void Initialize() {
		if (Active) {
			LogWarn("Attempting to initialize multiple times, operation rejected");
			return;
		}

		Active = true;

		ModuleManager.Load();
	}

	public void Unload() {
		ModuleManager.Unload();

		Active = false;
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
