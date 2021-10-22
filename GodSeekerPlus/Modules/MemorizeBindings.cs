using System.Collections;

namespace GodSeekerPlus.Modules {
	internal sealed class MemorizeBindings : Module {
		private protected override void Load() {
			On.BossDoorChallengeUI.ShowSequence += ApplyBindingStates;
			On.BossDoorChallengeUI.HideSequence += RecordBindingStates;
		}

		private protected override void Unload() {
			On.BossDoorChallengeUI.ShowSequence -= ApplyBindingStates;
			On.BossDoorChallengeUI.HideSequence -= RecordBindingStates;
		}

		private protected override bool ShouldLoad() => GodSeekerPlus.Instance.GlobalSetting.memorizeBindings;

		private static IEnumerator ApplyBindingStates(On.BossDoorChallengeUI.orig_ShowSequence orig, BossDoorChallengeUI self) {
			yield return orig(self);

			SetButtonState(self.boundNailButton, GodSeekerPlus.Instance.LocalSetting.boundNail);
			SetButtonState(self.boundHeartButton, GodSeekerPlus.Instance.LocalSetting.boundHeart);
			SetButtonState(self.boundCharmsButton, GodSeekerPlus.Instance.LocalSetting.boundCharms);
			SetButtonState(self.boundSoulButton, GodSeekerPlus.Instance.LocalSetting.boundSoul);

			Logger.LogDebug("Binding states applied");
		}

		private static IEnumerator RecordBindingStates(On.BossDoorChallengeUI.orig_HideSequence orig, BossDoorChallengeUI self, bool sendEvent) {
			GodSeekerPlus.Instance.LocalSetting.boundNail = self.boundNailButton.Selected;
			GodSeekerPlus.Instance.LocalSetting.boundHeart = self.boundHeartButton.Selected;
			GodSeekerPlus.Instance.LocalSetting.boundCharms = self.boundCharmsButton.Selected;
			GodSeekerPlus.Instance.LocalSetting.boundSoul = self.boundSoulButton.Selected;

			Logger.LogDebug("Binding states recorded");

			yield return orig(self, sendEvent);
		}

		private static void SetButtonState(BossDoorChallengeUIBindingButton self, bool state) {
			if (state) {
				self.OnSubmit(null);
			}
		}
	}
}
