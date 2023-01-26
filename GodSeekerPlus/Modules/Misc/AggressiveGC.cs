namespace GodSeekerPlus.Modules.Misc;

public sealed class AggressiveGC : Module {
	private protected override void Load() =>
		On.GameManager.IsUnloadAssetsRequired += SetUnloadRequired;

	private protected override void Unload() =>
		On.GameManager.IsUnloadAssetsRequired -= SetUnloadRequired;

	private static bool SetUnloadRequired(On.GameManager.orig_IsUnloadAssetsRequired orig, GameManager self, string src, string dest) =>
		true;
}
