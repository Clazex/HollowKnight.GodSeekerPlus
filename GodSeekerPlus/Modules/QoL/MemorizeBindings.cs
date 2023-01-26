namespace GodSeekerPlus.Modules.QoL;

public sealed class MemorizeBindings : Module {
	[LocalSetting] public static bool boundNail = false;
	[LocalSetting] public static bool boundHeart = false;
	[LocalSetting] public static bool boundCharms = false;
	[LocalSetting] public static bool boundSoul = false;

	public override bool DefaultEnabled => true;

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

		SetButtonState(self.boundNailButton, boundNail);
		SetButtonState(self.boundHeartButton, boundHeart);
		SetButtonState(self.boundCharmsButton, boundCharms);
		SetButtonState(self.boundSoulButton, boundSoul);

		Logger.LogDebug("Binding states applied");
	}

	private IEnumerator RecordBindingStates(On.BossDoorChallengeUI.orig_HideSequence orig, BossDoorChallengeUI self, bool sendEvent) {
		boundNail = self.boundNailButton.Selected;
		boundHeart = self.boundHeartButton.Selected;
		boundCharms = self.boundCharmsButton.Selected;
		boundSoul = self.boundSoulButton.Selected;

		Logger.LogDebug("Binding states recorded");

		yield return orig(self, sendEvent);
	}

	private static void SetButtonState(BossDoorChallengeUIBindingButton self, bool state) {
		if (state) {
			self.OnSubmit(null);
		}
	}
}
