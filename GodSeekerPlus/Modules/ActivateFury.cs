namespace GodSeekerPlus.Modules;

[Module(toggleableLevel = ToggleableLevel.AnyTime, defaultEnabled = false)]
internal sealed class ActivateFury : Module {
	private protected override void Load() =>
		On.BossSceneController.Start += Activate;

	private protected override void Unload() =>
		On.BossSceneController.Start -= Activate;

	private IEnumerator Activate(On.BossSceneController.orig_Start orig, BossSceneController self) {
		yield return orig(self);

		if (PlayerData.instance.equippedCharm_6) {
			if (PlayerData.instance.equippedCharm_27) {
				PlayerData.instance.joniHealthBlue = 1;
			} else {
				PlayerData.instance.health = 1;
			}

			HeroController.instance.proxyFSM.SendEvent("HeroCtrl-HeroDamaged");
			HeroController.instance.RelinquishControl();

			Logger.LogDebug("Fury activated");
		}
	}
}
