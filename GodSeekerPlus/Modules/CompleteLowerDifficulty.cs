namespace GodSeekerPlus.Modules;

[Module(toggleable = true, defaultEnabled = true)]
internal sealed class CompleteLowerDifficulty : Module {
	private protected override void Load() =>
		On.BossChallengeUI.Setup += CompleteLower;

	private protected override void Unload() =>
		On.BossChallengeUI.Setup -= CompleteLower;

	private static void CompleteLower(
		On.BossChallengeUI.orig_Setup orig,
		BossChallengeUI self,
		BossStatue statue,
		string nameSheet,
		string nameKey,
		string descSheet,
		string descKey
	) {
		if (!statue.hasNoTiers) {
			BossStatue.Completion completion = MiscUtil.GetStatueCompletion(statue);

			if ((completion.completedTier2 || completion.completedTier3) && !completion.completedTier1) {
				completion.completedTier1 = true;
				Logger.LogDebug($"Unlocked Tier 1 for {statue.name}");
			}
			if (completion.completedTier3 && !completion.completedTier2) {
				completion.completedTier2 = true;
				Logger.LogDebug($"Unlocked Tier 2 for {statue.name}");
			}

			MiscUtil.SetStatueCompletion(statue, completion);
		}

		orig(self, statue, nameSheet, nameKey, descSheet, descKey);
	}
}
