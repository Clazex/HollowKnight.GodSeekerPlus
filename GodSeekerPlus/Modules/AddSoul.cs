using UnityEngine;

namespace GodSeekerPlus.Modules;

[Module(toggleableLevel = ToggleableLevel.AnyTime, defaultEnabled = false)]
internal sealed class AddSoul : Module {
	private protected override void Load() =>
		On.BossSceneController.Start += Add;

	private protected override void Unload() =>
		On.BossSceneController.Start -= Add;

	private IEnumerator Add(On.BossSceneController.orig_Start orig, BossSceneController self) {
		yield return orig(self);

		if (!BossSequenceController.IsInSequence) {
			HeroController.instance.AddMPCharge(
				GodSeekerPlus.Instance.GlobalSettings.soulAmount
			);

			HeroController.instance.StartCoroutine(UpdateHUD());

			Logger.LogDebug("Soul added");
		}
	}

	private IEnumerator UpdateHUD() {
		yield return new WaitForSeconds(0.25f);

		GameCameras.instance.soulOrbFSM.SendEvent("MP GAIN SPA");
		GameCameras.instance.soulVesselFSM.SendEvent("MP RESERVE UP");
	}
}
