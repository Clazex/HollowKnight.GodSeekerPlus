using GodSeekerPlus.Modules.Misc;

namespace GodSeekerPlus.Modules.NewSaveQuickstart;

[DefaultEnabled]
internal sealed class UnlockRadiant : Module {
	internal void Unlock(Action orig, BossStatue statue, ref BossStatue.Completion completion) {
		if (completion.completedTier2) {
			orig.Invoke();
			return;
		}

		completion.completedTier2 = true;
		completion.seenTier3Unlock = true;
		BossChallengeUIEdit.SetStatueCompletion(statue, completion);

		orig.Invoke();

		completion.completedTier2 = false;

		Logger.LogDebug($"Unlocked Radiant for {statue.name}");
	}
}
