namespace GodSeekerPlus.Modules.QoL;

[ToggleableLevel(ToggleableLevel.ChangeScene)]
[DefaultEnabled]
internal sealed class EternalOrdealPlatform : Module {
	private static readonly SceneEdit handle = new(
		new("GG_Workshop", "gg_plat_float_wide"),
		plat => GameObjectUtil.Instantiate(
			plat,
			plat.transform.position with { x = 204.15f, y = 42.3f }
		)
	);

	private protected override void Load() => handle.Enable();

	private protected override void Unload() => handle.Disable();
}
