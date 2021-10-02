using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Vasi;

namespace GodSeekerPlus {
	internal static class FastSuperDash {
		public static int times = 0;

		public static void Hook() => On.PlayMakerFSM.OnEnable += ModifyFSM;

		public static void UnHook() => On.PlayMakerFSM.OnEnable -= ModifyFSM;

		private static void ModifyFSM(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self) {
			if (self.gameObject.name == "Knight" && self.FsmName == "Superdash") {
				FsmState stateWsSpdBuff = FsmUtil.CreateState(self, "GSP Workshop Speed Buff");

				FsmUtil.AddAction(stateWsSpdBuff, new CheckSceneName() {
					sceneName = "GG_Workshop",
					notEqualEvent = FsmEvent.Finished
				});
				FsmUtil.AddAction(stateWsSpdBuff, new FloatMultiply() {
					floatVariable = self.FsmVariables.FindFsmFloat("Current SD Speed"),
					multiplyBy = 1.5f
				});

				FsmUtil.ChangeTransition(FsmUtil.GetState(self, "Left"), FsmEvent.Finished.Name, stateWsSpdBuff.Name);
				FsmUtil.ChangeTransition(FsmUtil.GetState(self, "Right"), FsmEvent.Finished.Name, stateWsSpdBuff.Name);

				FsmUtil.AddTransition(stateWsSpdBuff, FsmEvent.Finished.Name, "Dash Start");

				GodSeekerPlus.Instance.Log("Superdash FSM modified");
			}

			orig(self);
		}
	}
}
