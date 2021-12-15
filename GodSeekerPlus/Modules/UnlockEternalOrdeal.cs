namespace GodSeekerPlus.Modules;

[Module(toggleableLevel = ToggleableLevel.AnyTime, defaultEnabled = true)]
internal sealed class UnlockEternalOrdeal : Module {
	private protected override void Load() =>
		On.HeroController.Start += SetOrdealUnlocked;

	private protected override void Unload() =>
		On.HeroController.Start -= SetOrdealUnlocked;

	private void SetOrdealUnlocked(On.HeroController.orig_Start orig, HeroController self) {
		orig(self);

		IEnumerable<PersistentBoolData> items = SceneData
			.instance
			.persistentBoolItems
			.Filter(item => item is {
				sceneName: "GG_Workshop",
				id: "Breakable Wall_Silhouette"
			});

		if (!items.Any() || items.Filter(item => !item.activated).Any()) {
			SceneData.instance.persistentBoolItems
				.Set("GG_Workshop", "Breakable Wall_Silhouette", true);

			SceneData.instance.persistentBoolItems
				.Set("GG_Workshop", "Zote_Break_wall", true);

			self.playerData.zoteStatueWallBroken = true;

			Logger.LogDebug("Eternal Ordeal unlocked");
		}
	}
}
