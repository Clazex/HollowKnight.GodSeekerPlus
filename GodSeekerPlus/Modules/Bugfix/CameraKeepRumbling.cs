namespace GodSeekerPlus.Modules.Bugfix;

public sealed class CameraKeepRumbling : Module {
	public override bool DefaultEnabled => true;

	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ChangeScene;

	private protected override void Load() =>
		On.CameraController.DoPositionToHero += ResetRumbling;

	private protected override void Unload() =>
		On.CameraController.DoPositionToHero -= ResetRumbling;

	private static IEnumerator ResetRumbling(
		On.CameraController.orig_DoPositionToHero orig,
		CameraController self,
		bool forceDirect
	) {
		yield return orig(self, forceDirect);

		if (Ref.GM.GetCurrentMapZone() == MapZone.GODS_GLORY.ToString()) {
			Ref.GC.cameraShakeFSM.SendEvent("LEVEL LOADED");
			LogDebug("Try resetting camera shake");
		}
	}
}
