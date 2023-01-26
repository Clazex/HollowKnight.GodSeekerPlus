namespace GodSeekerPlus.Modules.QoL;

public sealed class EternalOrdealPlatform : Module {
	private static readonly SceneEdit handle = new(
		new("GG_Workshop", "gg_plat_float_wide"),
		plat => GameObjectUtil.Instantiate(
			plat,
			plat.transform.position with { x = 204.15f, y = 42.3f }
		)
	);

	public override bool DefaultEnabled => true;

	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ChangeScene;

	private protected override void Load() => handle.Enable();

	private protected override void Unload() => handle.Disable();
}
