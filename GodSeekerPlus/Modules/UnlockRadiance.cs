namespace GodSeekerPlus.Modules;

[Category("NewSaveQuickstart")]
[ToggleableLevel(ToggleableLevel.ReloadSave)]
[DefaultEnabled]
internal sealed class UnlockRadiance : Module {
	private const string sceneName = "Radiance Boss Scene";

	private protected override void Load() =>
		On.HeroController.Start += SetRadianceUnlocked;

	private protected override void Unload() =>
		On.HeroController.Start -= SetRadianceUnlocked;

	private void SetRadianceUnlocked(On.HeroController.orig_Start orig, HeroController self) {
		orig(self);

		if (
			self.playerData.bossRushMode
			&& !self.playerData.unlockedBossScenes.Contains(sceneName)
		) {
			self.playerData.unlockedBossScenes.Add(sceneName);
			SceneData.instance.persistentBoolItems
				.Set("GG_Workshop", "Radiance Statue Cage", true);

			Logger.LogDebug("Radiance Unlocked");
		}
	}
}
