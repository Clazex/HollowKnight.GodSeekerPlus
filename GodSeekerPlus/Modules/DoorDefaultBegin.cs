using Mono.Cecil.Cil;

using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using ReflectionHelper = Modding.ReflectionHelper;

namespace GodSeekerPlus.Modules;

[Module(toggleableLevel = ToggleableLevel.AnyTime, defaultEnabled = true)]
internal sealed class DoorDefaultBegin : Module {
	private ILHook hook = null;

	private protected override void Load() {
		hook = new(
			typeof(BossDoorChallengeUI)
				.GetMethod("ShowSequence", BindingFlags.Instance | BindingFlags.NonPublic)
				.GetStateMachineTarget(),
			RemoveSelection
		);

		On.BossDoorChallengeUI.ShowSequence += AddSelection;
	}

	private protected override void Unload() {
		hook?.Dispose();

		On.BossDoorChallengeUI.ShowSequence -= AddSelection;
	}

	private void RemoveSelection(ILContext il) {
		ILCursor cursor = new ILCursor(il).Goto(0);

		//
		// The following lines are going to be removed:
		//
		// if (bossDoorChallengeUI.buttons.Length != 0) {
		//	EventSystem.current.SetSelectedGameObject(bossDoorChallengeUI.buttons[0].gameObject);
		// }
		// InputHandler.Instance.StartUIInput();
		//

		// The first IL line of the above lines are `LdFld`
		// for accessing the `buttons` field
		cursor.GotoNext(i => i.MatchLdfld(
			typeof(BossDoorChallengeUI)
				.GetField("buttons", BindingFlags.Instance | BindingFlags.NonPublic)
		));

		// Move the cursor to before the `LdFld`
		cursor.GotoPrev();
		// Move the cursor to one extra IL line before the `LdFld`, that is, `LdLoc.1`
		cursor.GotoPrev();

		// Remove all IL lines from the `LdLoc.1` to the one before `Ret`
		while (cursor.TryGotoNext(i => !i.MatchRet())) {
			cursor.Remove();
		}

		// Fix return
		cursor.Emit(OpCodes.Ldc_I4_0);
	}

	private IEnumerator AddSelection(On.BossDoorChallengeUI.orig_ShowSequence orig, BossDoorChallengeUI self) {
		yield return orig(self);

		MenuButton beginBtn = ReflectionHelper
			.GetField<BossDoorChallengeUI, CanvasGroup>(self, "group")
			.GetComponentsInChildren<MenuButton>()
			.Filter(btn => btn.name == "BeginButton")
			.FirstOrDefault();
		EventSystem.current.SetSelectedGameObject(beginBtn.gameObject);

		InputHandler.Instance.StartUIInput();
	}
}
