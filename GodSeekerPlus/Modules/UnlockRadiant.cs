using GodSeekerPlus.Util;

namespace GodSeekerPlus.Modules {
	[Module(toggleable = true, defaultEnabled = true)]
	internal sealed class UnlockRadiant : Module {
		private protected override void Load() =>
			On.BossChallengeUI.Setup += UnlockTier3;

		private protected override void Unload() =>
			On.BossChallengeUI.Setup -= UnlockTier3;

		private static void UnlockTier3(
			On.BossChallengeUI.orig_Setup orig,
			BossChallengeUI self,
			BossStatue statue,
			string nameSheet,
			string nameKey,
			string descSheet,
			string descKey
		) {
			if (statue.hasNoTiers) {
				orig(self, statue, nameSheet, nameKey, descSheet, descKey);
			} else {
				BossStatue.Completion completion = MiscUtil.GetStatueCompletion(statue);

				if (completion.completedTier2) {
					orig(self, statue, nameSheet, nameKey, descSheet, descKey);
				} else {
					completion.completedTier2 = true;
					completion.seenTier3Unlock = true;
					MiscUtil.SetStatueCompletion(statue, completion);

					orig(self, statue, nameSheet, nameKey, descSheet, descKey);

					completion.completedTier2 = false;
					MiscUtil.SetStatueCompletion(statue, completion);

					self.tier1Button.SetState(completion.completedTier1);
					self.tier2Button.SetState(completion.completedTier2);
					self.tier3Button.SetState(completion.completedTier3);

					Logger.LogDebug($"Unlocked Radiant for {statue.name}");
				}
			}

		}
	}
}
