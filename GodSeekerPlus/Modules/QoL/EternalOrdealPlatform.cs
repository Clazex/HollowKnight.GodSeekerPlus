namespace GodSeekerPlus.Modules.QoL;

[ToggleableLevel(ToggleableLevel.ChangeScene)]
[DefaultEnabled]
internal sealed class EternalOrdealPlatform : Module {
	private protected override void Load() =>
		OsmiHooks.SceneChangeHook += AddPlatform;

	private protected override void Unload() =>
		OsmiHooks.SceneChangeHook -= AddPlatform;

	private void AddPlatform(Scene prev, Scene next) {
		if (next.name != "GG_Workshop") {
			return;
		}

		GameObject plat = next.GetRootGameObjects()
			.First(go => go.name == "gg_plat_float_wide");

		_ = GameObjectUtil.Instantiate(
			plat,
			plat.transform.position with { x = 204.15f, y = 42.3f }
		);
	}
}
