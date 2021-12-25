using UnityEngine.SceneManagement;

namespace GodSeekerPlus.Modules;

[Module(toggleableLevel = ToggleableLevel.AnyTime, defaultEnabled = false)]
internal sealed class ActivateFuryOnEnter : Module {
	private protected override void Load() =>
		UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ActivateFury;

	private protected override void Unload() =>
		UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= ActivateFury;

	private void ActivateFury(Scene From, Scene To) {
		if (MiscUtil.isGodHomeBossScene(To.name)) {

			//check for fury charm
			if (PlayerData.instance.equippedCharm_6) {
				//this makes sure blue health isnt removed
				PlayerData.instance.health = 1;
				HeroController.instance.proxyFSM.SendEvent("HeroCtrl-HeroDamaged");
			}
		}
	}
}
