namespace GodSeekerPlus.Modules.Bugfix;

[ToggleableLevel(ToggleableLevel.ChangeScene)]
[DefaultEnabled]
internal sealed class CameraKeepRumbling : Module {
	private protected override void Load() =>
		On.CameraController.DoPositionToHero += ResetRumbling;

	private protected override void Unload() =>
		On.CameraController.DoPositionToHero -= ResetRumbling;

	private IEnumerator ResetRumbling(
		On.CameraController.orig_DoPositionToHero orig,
		CameraController self,
		bool forceDirect
	) {
		yield return orig(self, forceDirect);

		if (Ref.GM.sm.mapZone == MapZone.GODS_GLORY) {
			Ref.GC.cameraShakeFSM.SendEvent("LEVEL LOADED");
		}
	}
}
