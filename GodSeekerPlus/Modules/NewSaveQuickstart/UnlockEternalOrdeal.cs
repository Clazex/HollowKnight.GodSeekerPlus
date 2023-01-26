namespace GodSeekerPlus.Modules.NewSaveQuickstart;

public sealed class UnlockEternalOrdeal : Module {
	public override bool DefaultEnabled => true;

	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ReloadSave;

	private protected override void Load() =>
		On.HeroController.Start += SetOrdealUnlocked;

	private protected override void Unload() =>
		On.HeroController.Start -= SetOrdealUnlocked;

	private void SetOrdealUnlocked(On.HeroController.orig_Start orig, HeroController self) {
		orig(self);

		if (Ref.SD.persistentBoolItems.IsActivated("GG_Workshop", "Zote_Break_wall")) {
			return;
		}

		Ref.SD.persistentBoolItems
			.Set("GG_Workshop", "Breakable Wall_Silhouette", true);

		Ref.SD.persistentBoolItems
			.Set("GG_Workshop", "Zote_Break_wall", true);

		Ref.PD.zoteStatueWallBroken = true;

		Logger.LogDebug("Eternal Ordeal unlocked");
	}
}
