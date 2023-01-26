namespace GodSeekerPlus.Modules.QoL;

public sealed class CompleteLowerDifficulty : Module {
	public override bool DefaultEnabled => true;

	internal static void Complete(string name, ref BossStatue.Completion completion) {
		if ((completion.completedTier2 || completion.completedTier3) && !completion.completedTier1) {
			completion.completedTier1 = true;
			Logger.LogDebug($"Unlocked Tier 1 for {name}");
		}

		if (completion.completedTier3 && !completion.completedTier2) {
			completion.completedTier2 = true;
			Logger.LogDebug($"Unlocked Tier 2 for {name}");
		}
	}
}
