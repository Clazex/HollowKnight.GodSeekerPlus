using UnityEngine;
using UnityEngine.SceneManagement;

namespace GodSeekerPlus.Modules;

[Module(toggleableLevel = ToggleableLevel.AnyTime, defaultEnabled = false)]
internal sealed class AddSoulOnEnter : Module {
	private protected override void Load() =>
		On.HeroController.EnterScene += AddSoul;

	private protected override void Unload() =>
		On.HeroController.EnterScene -= AddSoul;

	private IEnumerator AddSoul(On.HeroController.orig_EnterScene orig, HeroController self, TransitionPoint enterGate, float delayBeforeEnter) {

		yield return orig(self,enterGate,delayBeforeEnter);
		if (MiscUtil.isGodHomeBossScene(GameManager.instance.GetSceneNameString())) {

			HeroController.instance.AddMPCharge(GodSeekerPlus.Instance.GlobalSettings.SoulAmount);

		}
	}
}
