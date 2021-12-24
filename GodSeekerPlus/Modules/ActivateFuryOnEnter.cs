using UnityEngine.SceneManagement;

namespace GodSeekerPlus.Modules;

[Module(toggleableLevel = ToggleableLevel.AnyTime, defaultEnabled = false)]
internal sealed class ActivateFuryOnEnter : Module {
	private protected override void Load() =>
		UnityEngine.SceneManagement.SceneManager.activeSceneChanged += AddLifeblood;

	private protected override void Unload() =>
		UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= AddLifeblood;

	private void AddLifeblood(Scene From, Scene To) {
		if (MiscUtil.isGodHomeBossScene(To.name)) {

			PlayerData.instance.health = 1;
			HeroController.instance.proxyFSM.SendEvent("HeroCtrl-HeroDamaged");
		}
	}
}
