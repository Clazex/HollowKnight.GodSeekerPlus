using GodSeekerPlus.Util;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Vasi;

namespace GodSeekerPlus.Modules {
	[Module(toggleable = false, defaultState = true)]
	internal sealed class FastSuperDash : Module {
		private protected override void Load() => On.PlayMakerFSM.OnEnable += ModifySuperDashFSM;

		private protected override void Unload() => On.PlayMakerFSM.OnEnable -= ModifySuperDashFSM;

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

				Logger.LogDebug("Superdash FSM modified");
			}

			orig(self);
		}
	}
}
