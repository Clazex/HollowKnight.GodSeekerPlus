using System.Collections;

namespace GodSeekerPlus.Modules {
	public sealed class MemorizeBindings : Module {
		public override void Load() {
			On.BossDoorChallengeUI.ShowSequence += ApplyBindingStates;
			On.BossDoorChallengeUI.HideSequence += RecordBindingStates;
		}

		public override void Unload() {
			On.BossDoorChallengeUI.ShowSequence -= ApplyBindingStates;
			On.BossDoorChallengeUI.HideSequence -= RecordBindingStates;
		}

		public override bool ShouldLoad() => GodSeekerPlus.Instance.GlobalSettings.memorizeBindings;

		private static IEnumerator ApplyBindingStates(On.BossDoorChallengeUI.orig_ShowSequence orig, BossDoorChallengeUI self) {
			yield return orig(self);

			SetButtonState(self.boundNailButton, GodSeekerPlus.Instance.LocalSettings.boundNail);
			SetButtonState(self.boundHeartButton, GodSeekerPlus.Instance.LocalSettings.boundHeart);
			SetButtonState(self.boundCharmsButton, GodSeekerPlus.Instance.LocalSettings.boundCharms);
			SetButtonState(self.boundSoulButton, GodSeekerPlus.Instance.LocalSettings.boundSoul);

			GodSeekerPlus.Instance.Log("Binding states applied");
		}

		private static IEnumerator RecordBindingStates(On.BossDoorChallengeUI.orig_HideSequence orig, BossDoorChallengeUI self, bool sendEvent) {
			GodSeekerPlus.Instance.LocalSettings.boundNail = self.boundNailButton.Selected;
			GodSeekerPlus.Instance.LocalSettings.boundHeart = self.boundHeartButton.Selected;
			GodSeekerPlus.Instance.LocalSettings.boundCharms = self.boundCharmsButton.Selected;
			GodSeekerPlus.Instance.LocalSettings.boundSoul = self.boundSoulButton.Selected;

			GodSeekerPlus.Instance.Log("Binding states recorded");

			yield return orig(self, sendEvent);
		}

		private static void SetButtonState(BossDoorChallengeUIBindingButton self, bool state) {
			if (state) {
				self.OnSubmit(null);
			}
		}
	}
}
