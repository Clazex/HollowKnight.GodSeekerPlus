namespace GodSeekerPlus;

public sealed partial class GodSeekerPlus : Mod, ITogglableMod {
	public static GodSeekerPlus? Instance { get; private set; }
	public static GodSeekerPlus UnsafeInstance => Instance!;

	public override string GetVersion() => VersionUtil.Version.Value;

	internal static ModuleManager? ModuleManager { get; set; }

	public override void Initialize() {
		if (Instance != null) {
			Logger.LogWarn("Attempting to initialize multiple times, operation rejected");
			return;
		}

		Instance = this;

		ModuleManager = new();
	}

	public void Unload() {
		ModuleManager?.Dispose();

		Instance = null;
	}
}
