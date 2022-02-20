namespace GodSeekerPlus.Modules.QoL;

[Category(nameof(QoL))]
[DefaultEnabled]
internal sealed class MemorizeBindings : Module {
	private protected override void Load() {
		On.BossDoorChallengeUI.ShowSequence += ApplyBindingStates;
		On.BossDoorChallengeUI.HideSequence += RecordBindingStates;
	}

	private protected override void Unload() {
		On.BossDoorChallengeUI.ShowSequence -= ApplyBindingStates;
		On.BossDoorChallengeUI.HideSequence -= RecordBindingStates;
	}

	private IEnumerator ApplyBindingStates(On.BossDoorChallengeUI.orig_ShowSequence orig, BossDoorChallengeUI self) {
		yield return orig(self);

		SetButtonState(self.boundNailButton, Ref.LS.boundNail);
		SetButtonState(self.boundHeartButton, Ref.LS.boundHeart);
		SetButtonState(self.boundCharmsButton, Ref.LS.boundCharms);
		SetButtonState(self.boundSoulButton, Ref.LS.boundSoul);

		Logger.LogDebug("Binding states applied");
	}

	private IEnumerator RecordBindingStates(On.BossDoorChallengeUI.orig_HideSequence orig, BossDoorChallengeUI self, bool sendEvent) {
		Ref.LS.boundNail = self.boundNailButton.Selected;
		Ref.LS.boundHeart = self.boundHeartButton.Selected;
		Ref.LS.boundCharms = self.boundCharmsButton.Selected;
		Ref.LS.boundSoul = self.boundSoulButton.Selected;

		Logger.LogDebug("Binding states recorded");

		yield return orig(self, sendEvent);
	}

	private static void SetButtonState(BossDoorChallengeUIBindingButton self, bool state) {
		if (state) {
			self.OnSubmit(null);
		}
	}
}
