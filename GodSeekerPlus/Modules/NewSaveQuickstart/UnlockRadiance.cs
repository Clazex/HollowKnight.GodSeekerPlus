namespace GodSeekerPlus.Modules.NewSaveQuickstart;

public sealed class UnlockRadiance : Module {
	private const string sceneName = "Radiance Boss Scene";

	public override bool DefaultEnabled => true;

	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ReloadSave;

	private protected override void Load() =>
		On.HeroController.Start += SetRadianceUnlocked;

	private protected override void Unload() =>
		On.HeroController.Start -= SetRadianceUnlocked;

	private static void SetRadianceUnlocked(On.HeroController.orig_Start orig, HeroController self) {
		orig(self);

		if (
			Ref.PD.bossRushMode
			&& !Ref.PD.unlockedBossScenes.Contains(sceneName)
		) {
			Ref.PD.unlockedBossScenes.Add(sceneName);
			Ref.SD.persistentBoolItems
				.Set("GG_Workshop", "Radiance Statue Cage", true);

			Logger.LogDebug("Radiance Unlocked");
		}
	}
}
