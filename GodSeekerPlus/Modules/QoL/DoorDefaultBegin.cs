using HKMirror.Hooks.ILHooks;

using Mono.Cecil.Cil;

using MonoMod.Cil;

namespace GodSeekerPlus.Modules.QoL;

public sealed class DoorDefaultBegin : Module {
	public override bool DefaultEnabled => true;

	private protected override void Load() => ILBossDoorChallengeUI.ShowSequence += ChangeSelection;

	private protected override void Unload() => ILBossDoorChallengeUI.ShowSequence -= ChangeSelection;

	// Remove:
	//
	// if (bossDoorChallengeUI.buttons.Length != 0) {
	//     EventSystem.current.SetSelectedGameObject(bossDoorChallengeUI.buttons[0].gameObject);
	// }
	// InputHandler.Instance.StartUIInput();
	//
	private static void ChangeSelection(ILContext il) => new ILCursor(il)
		.Goto(0)
		.GotoNext(
			i => i.MatchLdloc(1),
			i => i.MatchLdfld(typeof(BossDoorChallengeUI), "buttons")
		)
		.RemoveUntilEnd()

		.Emit(OpCodes.Ldloc_1) // self
		.EmitStaticMethodCall(SelectBegin)

		.Emit(OpCodes.Ldc_I4_0); // Fix return

	private static void SelectBegin(BossDoorChallengeUI self) {
		EventSystem.current.SetSelectedGameObject(
			self.gameObject.Child("Panel", "BeginButton")
		);

		InputHandler.Instance.StartUIInput();
	}
}
