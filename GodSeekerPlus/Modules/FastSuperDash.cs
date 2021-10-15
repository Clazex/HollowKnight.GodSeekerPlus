using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Vasi;

namespace GodSeekerPlus.Modules {
	internal sealed class FastSuperDash : Module {
		public override void Load() => On.PlayMakerFSM.OnEnable += ModifySuperDashFSM;

		public override void Unload() => On.PlayMakerFSM.OnEnable -= ModifySuperDashFSM;

		public override bool ShouldLoad() => GodSeekerPlus.Instance.GlobalSettings.fastSuperDash;

		private static void ModifySuperDashFSM(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self) {
			if (self.gameObject.name == "Knight" && self.FsmName == "Superdash") {
				FsmState stateWsSpdBuff = self.CreateState("GSP Workshop Speed Buff");

				stateWsSpdBuff.AddAction(new CheckSceneName() {
					sceneName = "GG_Workshop",
					notEqualEvent = FsmEvent.Finished
				});
				stateWsSpdBuff.AddAction(new FloatMultiply() {
					floatVariable = self.FsmVariables.FindFsmFloat("Current SD Speed"),
					multiplyBy = GodSeekerPlus.Instance.GlobalSettings.fastSuperDashSpeedMultiplier
				});

				self.GetState("Left").ChangeTransition(FsmEvent.Finished.Name, stateWsSpdBuff.Name);
				self.GetState("Right").ChangeTransition(FsmEvent.Finished.Name, stateWsSpdBuff.Name);

				stateWsSpdBuff.AddTransition(FsmEvent.Finished.Name, "Dash Start");

				GodSeekerPlus.Instance.Log("Superdash FSM modified");
			}

			orig(self);
		}
	}
}
