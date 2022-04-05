namespace GodSeekerPlus.Modules.NewSaveQuickstart;

[ToggleableLevel(ToggleableLevel.ReloadSave)]
[DefaultEnabled]
internal sealed class UnlockPantheons : Module {
	private static readonly List<(string goName, string prompt)> atriumRoofObjects = new() {
		("Breakable Wall_Silhouette", "Land of Storms shortcut"),
		("GG Fall Platform", "Stepping stone platform"),
		("gg_roof_lever", "Stepping stone platform lever"),
		("Secret Mask", "Mask above spa")
	};

	private protected override void Load() {
		On.HeroController.Start += SetP4Unlocked;
		On.HeroController.Start += SetP5Unlocked;
		On.HeroController.Start += ActivateAtriumRoofObjects;
	}

	private protected override void Unload() {
		On.HeroController.Start -= SetP4Unlocked;
		On.HeroController.Start -= SetP5Unlocked;
		On.HeroController.Start -= ActivateAtriumRoofObjects;
	}

	private void SetP4Unlocked(On.HeroController.orig_Start orig, HeroController self) {
		orig(self);

		if (!Ref.PD.bossRushMode) {
			return;
		}

		if (Ref.PD.bossDoorCageUnlocked) {
			return;
		}

		Ref.PD.bossDoorCageUnlocked = true;
		Logger.LogDebug("P4 Unlocked");
	}

	private void SetP5Unlocked(On.HeroController.orig_Start orig, HeroController self) {
		orig(self);

		if (!Ref.PD.bossRushMode) {
			return;
		}

		if (Ref.PD.finalBossDoorUnlocked) {
			return;
		}

		Ref.PD.finalBossDoorUnlocked = true;
		Ref.SD.persistentBoolItems.Set("GG_Atrium", "gg_roof_lever", true);

		Logger.LogDebug("P5 Unlocked");
	}

	private void ActivateAtriumRoofObjects(On.HeroController.orig_Start orig, HeroController self) {
		orig(self);

		if (!Ref.PD.bossRushMode) {
			return;
		}

		List<PersistentBoolData>? items = Ref.SD.persistentBoolItems;
		atriumRoofObjects
			.ForEach(tuple => {
				if (!items.IsActivated("GG_Atrium_Roof", tuple.goName)) {
					items.Set("GG_Atrium_Roof", tuple.goName, true);

					Logger.LogDebug(tuple.prompt + " activated");
				}
			});
	}
}
