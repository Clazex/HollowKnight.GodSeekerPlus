using System.Collections;

namespace GodSeekerPlus.Modules {
	public static class MemorizeBindings {
		public static void Load() {
			On.BossDoorChallengeUI.ShowSequence += ApplyBindingStates;
			On.BossDoorChallengeUI.HideSequence += RecordBindingStates;
		}

		public static void Unload() {
			On.BossDoorChallengeUI.ShowSequence -= ApplyBindingStates;
			On.BossDoorChallengeUI.HideSequence -= RecordBindingStates;
		}

		private static IEnumerator ApplyBindingStates(On.BossDoorChallengeUI.orig_ShowSequence orig, BossDoorChallengeUI self) {
			yield return orig(self);

			self.boundNailButton.SetSelected(GodSeekerPlus.Instance.LocalSettings.boundNail);
			self.boundHeartButton.SetSelected(GodSeekerPlus.Instance.LocalSettings.boundHeart);
			self.boundCharmsButton.SetSelected(GodSeekerPlus.Instance.LocalSettings.boundCharms);
			self.boundSoulButton.SetSelected(GodSeekerPlus.Instance.LocalSettings.boundSoul);

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

		private static void SetSelected(this BossDoorChallengeUIBindingButton self, bool state) {
			if (state) {
				self.OnSubmit(null);
			}
		}
	}
}
