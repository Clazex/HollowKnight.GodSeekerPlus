namespace GodSeekerPlus.Modules;

[Module(toggleableLevel = ToggleableLevel.AnyTime, defaultEnabled = true)]
internal sealed class GreyPrinceEnterShort : Module {
	private protected override void Load() =>
		On.PlayMakerFSM.Start += ModifyGPFSM;

	private protected override void Unload() =>
		On.PlayMakerFSM.Start -= ModifyGPFSM;

	private void ModifyGPFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		orig(self);

		if (self.gameObject.name == "Grey Prince" && self.FsmName == "Control") {
			self.ChangeTransition("Enter 1", FsmEvent.Finished.Name, "Enter Short");

			Logger.LogDebug("Grey Prince FSM modified");
		}
	}
}
