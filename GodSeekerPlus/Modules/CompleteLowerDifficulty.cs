namespace GodSeekerPlus.Modules;

[Module(toggleable = true, defaultEnabled = true)]
internal sealed class CompleteLowerDifficulty : Module {
	private protected override void Load() {
	}

	private protected override void Unload() {
	}

	internal void CompleteLower(string name, ref BossStatue.Completion completion) {
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
