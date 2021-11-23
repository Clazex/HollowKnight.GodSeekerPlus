namespace GodSeekerPlus.Modules;

[Module(toggleableLevel = ToggleableLevel.AnyTime, defaultEnabled = true)]
internal sealed class UnlockRadiance : Module {
	private const string scene = "Radiance Boss Scene";

	private protected override void Load() =>
		ModHooks.AfterSavegameLoadHook += SetRadianceUnlocked;

	private protected override void Unload() =>
		ModHooks.AfterSavegameLoadHook -= SetRadianceUnlocked;

	private void SetRadianceUnlocked(SaveGameData saveData) {
		if (
			saveData.playerData.bossRushMode
			&& !saveData.playerData.unlockedBossScenes.Contains(scene)
		) {
			saveData.playerData.unlockedBossScenes.Add(scene);
			saveData.sceneData.persistentBoolItems
				.Set("GG_Workshop", "Radiance Statue Cage", true);

			Logger.LogDebug("Radiance Unlocked");
		}
	}
}
