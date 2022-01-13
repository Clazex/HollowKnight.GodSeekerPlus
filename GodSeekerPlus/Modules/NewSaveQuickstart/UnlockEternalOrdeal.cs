namespace GodSeekerPlus.Modules.NewSaveQuickstart;

[Category(nameof(NewSaveQuickstart))]
[ToggleableLevel(ToggleableLevel.ReloadSave)]
[DefaultEnabled]
internal sealed class UnlockEternalOrdeal : Module {
	private protected override void Load() =>
		On.HeroController.Start += SetOrdealUnlocked;

	private protected override void Unload() =>
		On.HeroController.Start -= SetOrdealUnlocked;

	private void SetOrdealUnlocked(On.HeroController.orig_Start orig, HeroController self) {
		orig(self);

		IEnumerable<PersistentBoolData> items = Ref
			.SD
			.persistentBoolItems
			.Filter(item => item is {
				sceneName: "GG_Workshop",
				id: "Breakable Wall_Silhouette"
			});

		if (!items.Any() || items.Filter(item => !item.activated).Any()) {
			Ref.SD.persistentBoolItems
				.Set("GG_Workshop", "Breakable Wall_Silhouette", true);

			Ref.SD.persistentBoolItems
				.Set("GG_Workshop", "Zote_Break_wall", true);

			Ref.PD.zoteStatueWallBroken = true;

			Logger.LogDebug("Eternal Ordeal unlocked");
		}
	}
}
