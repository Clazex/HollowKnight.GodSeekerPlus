namespace GodSeekerPlus.Util;

internal static class SceneDataUtil {
	internal static bool IsActivated(this List<PersistentBoolData> self, string sceneName, string id) => self
		.Filter(item => item.sceneName == sceneName && item.id == id && item.activated)
		.Any();

	internal static void Set(this PersistentBoolData self, bool activated, bool semiPersistent = false) {
		self.activated = activated;
		self.semiPersistent = semiPersistent;
	}

	internal static void Set(this List<PersistentBoolData> self, string sceneName, string id, bool activated, bool semiPersistent = false) {
		PersistentBoolData? entry = self
			.Filter(item => item.sceneName == sceneName && item.id == id)
			.FirstOrDefault();

		if (entry != null) {
			entry.Set(activated, semiPersistent);
		} else {
			self.Add(new() {
				sceneName = sceneName,
				id = id,
				activated = activated,
				semiPersistent = semiPersistent
			});
		}
	}
}
