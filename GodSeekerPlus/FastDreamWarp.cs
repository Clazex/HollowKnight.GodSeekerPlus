using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Vasi;

namespace GodSeekerPlus {
	internal static class FastDreamWarp {
		public static void Hook() => On.PlayMakerFSM.OnEnable += ModifyFSM;

		public static void UnHook() => On.PlayMakerFSM.OnEnable -= ModifyFSM;

		private static void ModifyFSM(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self) {
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
