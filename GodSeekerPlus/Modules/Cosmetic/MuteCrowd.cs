namespace GodSeekerPlus.Modules.Cosmetic;

public sealed class MuteCrowd : Module {
	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ChangeScene;

	private protected override void Load() =>
		OsmiHooks.SceneChangeHook += DisableAudio;

	private protected override void Unload() =>
		OsmiHooks.SceneChangeHook -= DisableAudio;

	private static void DisableAudio(Scene prev, Scene next) {
		if (!BossSceneController.IsBossScene) {
			return;
		}

		GameObject? arena = next.GetRootGameObject("GG_Arena_Prefab");
		if (arena == null) {
			return;
		}

		AudioSource? source = arena.GetComponents<AudioSource>()
			.FirstOrDefault(x => x.clip != null && x.clip.name == "gg_crowd_loop");
		if (source == null) {
			return;
		}

		source.Stop();
		LogDebug("Crowd audio stopped");
	}
}
