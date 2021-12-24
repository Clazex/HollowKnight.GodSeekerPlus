using UnityEngine.SceneManagement;

namespace GodSeekerPlus.Modules;

[Module(toggleableLevel = ToggleableLevel.AnyTime, defaultEnabled = false)]
internal sealed class AddLifebloodOnEnter : Module {
	private protected override void Load() =>
	UnityEngine.SceneManagement.SceneManager.activeSceneChanged += AddLifeblood;

	private protected override void Unload() =>
		UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= AddLifeblood;

	private void AddLifeblood(Scene From, Scene To) {

		if (MiscUtil.isGodHomeBossScene(To.name)) {
			int lifebloodAmount = MiscUtil.ForceInRange(GodSeekerPlus.Instance.GlobalSettings.LifebloodAmount - PlayerData.instance.healthBlue, 0, 10);

			for (int lifeblood_increment = 0; lifeblood_increment < lifebloodAmount; lifeblood_increment++) {
				EventRegister.SendEvent("ADD BLUE HEALTH");
			}
		}
	}
}
