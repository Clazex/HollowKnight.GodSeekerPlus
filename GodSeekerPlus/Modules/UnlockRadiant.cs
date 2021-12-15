namespace GodSeekerPlus.Modules;

[Module(toggleableLevel = ToggleableLevel.AnyTime, defaultEnabled = true)]
internal sealed class UnlockRadiant : Module {
	private protected override void Load() =>
		On.BossChallengeUI.Setup += UnlockTier3;

	private protected override void Unload() =>
		On.BossChallengeUI.Setup -= UnlockTier3;

	private void UnlockTier3(
		On.BossChallengeUI.orig_Setup orig,
		BossChallengeUI self,
		BossStatue statue,
		string nameSheet,
		string nameKey,
		string descSheet,
		string descKey
	) {
		void invokeOrig() => orig(self, statue, nameSheet, nameKey, descSheet, descKey);

		if (statue.hasNoTiers) {
			invokeOrig();
		} else {
			BossStatue.Completion completion = MiscUtil.GetStatueCompletion(statue);

			if (completion.completedTier2) {
				invokeOrig();
			} else {
				completion.completedTier2 = true;
				completion.seenTier3Unlock = true;
				MiscUtil.SetStatueCompletion(statue, completion);

				invokeOrig();

				completion.completedTier2 = false;

				Logger.LogDebug($"Unlocked Radiant for {statue.name}");
			}

			CompleteLowerDifficulty cld = GodSeekerPlus.Instance
				.FindModule<CompleteLowerDifficulty>();
			if (cld.Enabled) {
				cld.CompleteLower(statue.name, ref completion);
			}
			MiscUtil.SetStatueCompletion(statue, completion);

			self.tier1Button.SetState(completion.completedTier1);
			self.tier2Button.SetState(completion.completedTier2);
			self.tier3Button.SetState(completion.completedTier3);
		}
	}
}
