namespace GodSeekerPlus.Modules;

[Module(toggleableLevel = ToggleableLevel.AnyTime, defaultEnabled = true)]
internal sealed class UnlockEternalOrdeal : Module {
	private protected override void Load() =>
		ModHooks.AfterSavegameLoadHook += SetOrdealUnlocked;

	private protected override void Unload() =>
		ModHooks.AfterSavegameLoadHook -= SetOrdealUnlocked;

	private void SetOrdealUnlocked(SaveGameData data) {
		IEnumerable<PersistentBoolData> items = data
			.sceneData
			.persistentBoolItems
			.Filter(item =>
				item.sceneName == "GG_Workshop"
				&& item.id == "Breakable Wall_Silhouette"
			);

		if (!items.Any() || items.Filter(item => !item.activated).Any()) {
			data.sceneData.persistentBoolItems
				.Set("GG_Workshop", "Breakable Wall_Silhouette", true);

			data.sceneData.persistentBoolItems
				.Set("GG_Workshop", "Zote_Break_wall", true);

			data.playerData.zoteStatueWallBroken = true;

			Logger.LogDebug("Eternal Ordeal unlocked");
		}
	}
}
