using GodSeekerPlus.Modules.NewSaveQuickstart;
using GodSeekerPlus.Modules.QoL;

namespace GodSeekerPlus.Modules.Misc;

[Hidden]
internal sealed class BossChallengeUIEdit : Module {
	private protected override void Load() =>
		On.BossChallengeUI.Setup += HookSetup;

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
			return;
		}

		BossStatue.Completion completion = statue.UsingDreamVersion ? statue.DreamStatueState : statue.StatueState;

		if (ModuleManager.TryGetActiveModule(out UnlockRadiant? ur)) {
			ur.Unlock(invokeOrig, statue, ref completion);
		} else {
			invokeOrig();
		}

		if (ModuleManager.TryGetActiveModule(out CompleteLowerDifficulty? cld)) {
			cld.CompleteLower(statue.name, ref completion);
		}

		SetStatueCompletion(statue, completion);

		self.tier1Button.SetState(completion.completedTier1);
		self.tier2Button.SetState(completion.completedTier2);
		self.tier3Button.SetState(completion.completedTier3);
	}

	internal static void SetStatueCompletion(BossStatue statue, BossStatue.Completion completion) {
		if (statue.UsingDreamVersion) {
			statue.DreamStatueState = completion;
		} else {
			statue.StatueState = completion;
		}
	}
}
