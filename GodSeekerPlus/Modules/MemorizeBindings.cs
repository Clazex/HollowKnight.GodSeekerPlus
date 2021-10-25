using System.Collections;
using GodSeekerPlus.Util;

namespace GodSeekerPlus.Modules {
	[Module(toggleable = true, defaultEnabled = true)]
	internal sealed class MemorizeBindings : Module {
		private protected override void Load() {
			On.BossDoorChallengeUI.ShowSequence += ApplyBindingStates;
			On.BossDoorChallengeUI.HideSequence += RecordBindingStates;
		}

		private protected override void Unload() {
			On.BossDoorChallengeUI.ShowSequence -= ApplyBindingStates;
			On.BossDoorChallengeUI.HideSequence -= RecordBindingStates;
		}

		private static IEnumerator ApplyBindingStates(On.BossDoorChallengeUI.orig_ShowSequence orig, BossDoorChallengeUI self) {
			yield return orig(self);

			SetButtonState(self.boundNailButton, GodSeekerPlus.Instance.LocalSettings.boundNail);
			SetButtonState(self.boundHeartButton, GodSeekerPlus.Instance.LocalSettings.boundHeart);
			SetButtonState(self.boundCharmsButton, GodSeekerPlus.Instance.LocalSettings.boundCharms);
			SetButtonState(self.boundSoulButton, GodSeekerPlus.Instance.LocalSettings.boundSoul);

			Logger.LogDebug("Binding states applied");
		}

		private static IEnumerator RecordBindingStates(On.BossDoorChallengeUI.orig_HideSequence orig, BossDoorChallengeUI self, bool sendEvent) {
			GodSeekerPlus.Instance.LocalSettings.boundNail = self.boundNailButton.Selected;
			GodSeekerPlus.Instance.LocalSettings.boundHeart = self.boundHeartButton.Selected;
			GodSeekerPlus.Instance.LocalSettings.boundCharms = self.boundCharmsButton.Selected;
			GodSeekerPlus.Instance.LocalSettings.boundSoul = self.boundSoulButton.Selected;

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
