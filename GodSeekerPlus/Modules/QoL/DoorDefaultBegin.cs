using Mono.Cecil.Cil;

using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;

namespace GodSeekerPlus.Modules.QoL;

[DefaultEnabled]
internal sealed class DoorDefaultBegin : Module {
	private readonly ILHook hook = new(
		typeof(BossDoorChallengeUI)
			.GetMethod("ShowSequence", BindingFlags.Instance | BindingFlags.NonPublic)
			.GetStateMachineTarget(),
		ChangeSelection,
		new() { ManualApply = true }
	);

	private protected override void Load() => hook.Apply();

	private protected override void Unload() => hook.Undo();

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
