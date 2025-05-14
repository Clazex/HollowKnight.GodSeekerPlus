namespace GodSeekerPlus.Modules.Bugfix;

public sealed class HUDDisplayChecker : Module {
	private static readonly string[] sceneExclusions = [
		"GG_Hollow_Knight",
		"GG_Radiance"
	];

	public override bool DefaultEnabled => true;

	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ChangeScene;

	private protected override void Load() =>
		On.BossSceneController.Start += Check;

	private protected override void Unload() =>
		On.BossSceneController.Start -= Check;

	private static IEnumerator Check(On.BossSceneController.orig_Start orig, BossSceneController self) {
		yield return orig(self);

		if (!sceneExclusions.Contains(Ref.GM.sceneName)) {
			yield return new WaitUntil(() => Ref.GM.gameState == GameState.PLAYING);
			Ref.GC.hudCanvas.LocateMyFSM("Slide Out").SendEvent("IN");
		}
	}
}
