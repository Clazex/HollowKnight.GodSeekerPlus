using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Vasi;

namespace GodSeekerPlus.Modules {
	internal sealed class FastDreamWarp : Module {
		public override void Load() => On.PlayMakerFSM.OnEnable += ModifyDreamNailFSM;

		public override void Unload() => On.PlayMakerFSM.OnEnable -= ModifyDreamNailFSM;

		public override bool ShouldLoad() => GodSeekerPlus.Instance.GlobalSettings.fastDreamWarp;

		private static void ModifyDreamNailFSM(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self) {
			if (self.gameObject.name == "Knight" && self.FsmName == "Dream Nail") {
				FsmState stateWarpCharge = self.GetState("Warp Charge");
				stateWarpCharge.InsertAction(0, new GGCheckIfBossScene {
					// If in boss scene, fire CHARGED event immediately
					bossSceneEvent = stateWarpCharge.GetAction<Wait>().finishEvent,
				});

				GodSeekerPlus.Instance.Log("Dream Nail FSM modified");
			}

			orig(self);
		}
	}
}
