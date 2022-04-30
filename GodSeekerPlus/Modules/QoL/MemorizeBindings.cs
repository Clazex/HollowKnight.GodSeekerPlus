namespace GodSeekerPlus.Modules.QoL;

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

		SetButtonState(self.boundNailButton, Setting.Local.boundNail);
		SetButtonState(self.boundHeartButton, Setting.Local.boundHeart);
		SetButtonState(self.boundCharmsButton, Setting.Local.boundCharms);
		SetButtonState(self.boundSoulButton, Setting.Local.boundSoul);

		Logger.LogDebug("Binding states applied");
	}

	private IEnumerator RecordBindingStates(On.BossDoorChallengeUI.orig_HideSequence orig, BossDoorChallengeUI self, bool sendEvent) {
		Setting.Local.boundNail = self.boundNailButton.Selected;
		Setting.Local.boundHeart = self.boundHeartButton.Selected;
		Setting.Local.boundCharms = self.boundCharmsButton.Selected;
		Setting.Local.boundSoul = self.boundSoulButton.Selected;

		Logger.LogDebug("Binding states recorded");

		yield return orig(self, sendEvent);
	}

	private static void SetButtonState(BossDoorChallengeUIBindingButton self, bool state) {
		if (state) {
			self.OnSubmit(null);
		}
	}
}
