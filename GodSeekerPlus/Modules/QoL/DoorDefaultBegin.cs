using Mono.Cecil.Cil;

using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;

namespace GodSeekerPlus.Modules.QoL;

public sealed class DoorDefaultBegin : Module {
	private static readonly ILHook hook = new(
		Info.OfMethod<BossDoorChallengeUI>("ShowSequence").GetStateMachineTarget(),
		ChangeSelection,
		new() { ManualApply = true }
	);

	public override bool DefaultEnabled => true;

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
			i => i.MatchLdfld<BossDoorChallengeUI>("buttons")
		)
		.RemoveUntilEnd()

		.Emit(OpCodes.Ldloc_1) // self
		.Emit(OpCodes.Call, Info.OfMethod<DoorDefaultBegin>(nameof(SelectBegin)))

		.Emit(OpCodes.Ldc_I4_0); // Fix return

	private static void SelectBegin(BossDoorChallengeUI self) {
		EventSystem.current.SetSelectedGameObject(
			self.gameObject.Child("Panel", "BeginButton")
		);

		InputHandler.Instance.StartUIInput();
	}
}
