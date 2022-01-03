namespace GodSeekerPlus.Modules;

[Hidden]
internal sealed class BossChallengeUIEdit : Module {
	private protected override void Load() =>
		On.BossChallengeUI.Setup += HookSetup;

	private protected override void Unload() =>
		On.BossChallengeUI.Setup -= HookSetup;

	private void HookSetup(
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
			BossStatue.Completion completion =
				MiscUtil.GetStatueCompletion(statue);

			if (GodSeekerPlus.Instance.ModuleEnabled<UnlockRadiant>()) {
				UnlockRadiant.Unlock(invokeOrig, statue, ref completion);
			} else {
				invokeOrig();
			}

			if (GodSeekerPlus.Instance.ModuleEnabled<CompleteLowerDifficulty>()) {
				CompleteLowerDifficulty.CompleteLower(statue.name, ref completion);
			}

			MiscUtil.SetStatueCompletion(statue, completion);

			self.tier1Button.SetState(completion.completedTier1);
			self.tier2Button.SetState(completion.completedTier2);
			self.tier3Button.SetState(completion.completedTier3);
		}
	}
}
