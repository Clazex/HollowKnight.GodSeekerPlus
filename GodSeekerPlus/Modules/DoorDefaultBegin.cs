
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using orig_ShowSequence = On.BossDoorChallengeUI.orig_ShowSequence;

namespace GodSeekerPlus.Modules;

[Module(toggleable = true, defaultEnabled = true)]
internal sealed class DoorDefaultBegin : Module {
	private protected override void Load() =>
		On.BossDoorChallengeUI.ShowSequence += OverrideOrig;

	private protected override void Unload() =>
		On.BossDoorChallengeUI.ShowSequence -= OverrideOrig;

	private static IEnumerator OverrideOrig(orig_ShowSequence _, BossDoorChallengeUI self) {
		CanvasGroup group = ReflectionHelper
			.GetField<BossDoorChallengeUI, CanvasGroup>(self, "group");
		Animator animator = ReflectionHelper
			.GetField<BossDoorChallengeUI, Animator>(self, "animator");

		group.interactable = false;
		EventSystem.current.SetSelectedGameObject(null);
		yield return null;

		if (animator) {
			animator.Play("Open");
			yield return null;
			yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
		}

		group.interactable = true;

		//	if (self.buttons.Length != 0) {
		//		EventSystem.current.SetSelectedGameObject(self.buttons[0].gameObject); // <-- Removed
		//	}

		group
			.GetComponentsInChildren<MenuButton>()
			.Filter(btn => btn.name == "BeginButton")
			.ForEach(btn => EventSystem.current.SetSelectedGameObject(btn.gameObject));

		InputHandler.Instance.StartUIInput();

		yield break;
	}
}
