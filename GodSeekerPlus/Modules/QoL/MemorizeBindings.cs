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

		LocalSettings settings = Ref.GSP.LocalSettings;
		SetButtonState(self.boundNailButton, settings.boundNail);
		SetButtonState(self.boundHeartButton, settings.boundHeart);
		SetButtonState(self.boundCharmsButton, settings.boundCharms);
		SetButtonState(self.boundSoulButton, settings.boundSoul);

		Logger.LogDebug("Binding states applied");
	}

	private IEnumerator RecordBindingStates(On.BossDoorChallengeUI.orig_HideSequence orig, BossDoorChallengeUI self, bool sendEvent) {
		LocalSettings settings = Ref.GSP.LocalSettings;
		settings.boundNail = self.boundNailButton.Selected;
		settings.boundHeart = self.boundHeartButton.Selected;
		settings.boundCharms = self.boundCharmsButton.Selected;
		settings.boundSoul = self.boundSoulButton.Selected;

		Logger.LogDebug("Binding states recorded");

		yield return orig(self, sendEvent);
	}

	private static void SetButtonState(BossDoorChallengeUIBindingButton self, bool state) {
		if (state) {
			self.OnSubmit(null);
		}
	}
}
