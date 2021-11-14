using HutongGames.PlayMaker;
using Vasi;
using GodSeekerPlus.Util;

namespace GodSeekerPlus.Modules {
	[Module(toggleable = true, defaultEnabled = true)]
	internal sealed class GreyPrinceEnterShort : Module {
		private protected override void Load() =>
			On.PlayMakerFSM.OnEnable += ModifyGPFSM;

		private protected override void Unload() =>
			On.PlayMakerFSM.OnEnable -= ModifyGPFSM;

		private static void ModifyGPFSM(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self) {
			if (self.gameObject.name == "Grey Prince" && self.FsmName == "Control") {
				self.ChangeTransition("Enter 1", FsmEvent.Finished.Name, "Enter Short");

				Logger.LogDebug("Grey Prince FSM modified");
			}

			orig(self);
		}
	}
}
