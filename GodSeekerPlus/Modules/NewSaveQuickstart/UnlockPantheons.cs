namespace GodSeekerPlus.Modules.NewSaveQuickstart;

public sealed class UnlockPantheons : Module {
	private static readonly (string goName, string prompt)[] atriumRoofObjects = new[] {
		("Breakable Wall_Silhouette", "Land of Storms shortcut"),
		("GG Fall Platform", "Stepping stone platform"),
		("gg_roof_lever", "Stepping stone platform lever"),
		("Secret Mask", "Mask above spa")
	};

	public override bool DefaultEnabled => true;

	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ReloadSave;

	private protected override void Load() =>
		On.HeroController.Start += Unlock;

	private protected override void Unload() =>
		On.HeroController.Start -= Unlock;

	private static void Unlock(On.HeroController.orig_Start orig, HeroController self) {
		orig(self);

		if (!Ref.PD.bossRushMode) {
			return;
		}

		if (!Ref.PD.bossDoorCageUnlocked) {
			Ref.PD.bossDoorCageUnlocked = true;
			LogDebug("P4 Unlocked");
		}

		if (!Ref.PD.finalBossDoorUnlocked) {
			Ref.PD.finalBossDoorUnlocked = true;
			Ref.SD.persistentBoolItems.Set("GG_Atrium", "gg_roof_lever", true);

			LogDebug("P5 Unlocked");
		}

		List<PersistentBoolData>? items = Ref.SD.persistentBoolItems;
		atriumRoofObjects.ForEach(tuple => {
			if (!items.IsActivated("GG_Atrium_Roof", tuple.goName)) {
				items.Set("GG_Atrium_Roof", tuple.goName, true);
				LogDebug(tuple.prompt + " activated");
			}
		});
	}
}
