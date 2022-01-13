namespace GodSeekerPlus.Modules.BossChallenge;

[Category(nameof(BossChallenge))]
[ToggleableLevel(ToggleableLevel.ChangeScene)]
internal sealed class ActivateFury : Module {
	private protected override void Load() =>
		On.BossSceneController.Start += Activate;

	private protected override void Unload() =>
		On.BossSceneController.Start -= Activate;

	private IEnumerator Activate(On.BossSceneController.orig_Start orig, BossSceneController self) {
		yield return orig(self);

		if (Ref.PD.equippedCharm_6) {
			if (Ref.PD.equippedCharm_27) {
				Ref.PD.joniHealthBlue = 1;
			} else {
				Ref.PD.health = 1;
			}

			Ref.HC.proxyFSM.SendEvent("HeroCtrl-HeroDamaged");
			Ref.HC.RelinquishControl();

			Logger.LogDebug("Fury activated");
		}
	}
}
