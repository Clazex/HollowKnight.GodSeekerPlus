using GodSeekerPlus.Util;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Vasi;

namespace GodSeekerPlus.Modules {
	[Module(toggleable = false, defaultState = true)]
	internal sealed class FastDreamWarp : Module {
		private protected override void Load() => On.PlayMakerFSM.OnEnable += ModifyDreamNailFSM;

		private protected override void Unload() => On.PlayMakerFSM.OnEnable -= ModifyDreamNailFSM;

		private protected override bool ShouldLoad() => GodSeekerPlus.Instance.GlobalSettings.fastDreamWarp;

		private static void ModifyDreamNailFSM(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self) {
			if (self.gameObject.name == "Knight" && self.FsmName == "Dream Nail") {
				FsmState stateWarpCharge = self.GetState("Warp Charge");
				stateWarpCharge.InsertAction(0, new GGCheckIfBossScene {
					// If in boss scene, fire CHARGED event immediately
					bossSceneEvent = stateWarpCharge.GetAction<Wait>().finishEvent,
				});

				Logger.LogDebug("Dream Nail FSM modified");
			}

			orig(self);
		}
	}
}
