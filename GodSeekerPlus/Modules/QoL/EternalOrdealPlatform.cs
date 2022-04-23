namespace GodSeekerPlus.Modules.QoL;

[ToggleableLevel(ToggleableLevel.ChangeScene)]
[DefaultEnabled]
internal sealed class EternalOrdealPlatform : Module {
	private protected override void Load() =>
		USceneManager.activeSceneChanged += AddPlatform;

	private protected override void Unload() =>
		USceneManager.activeSceneChanged -= AddPlatform;

	private void AddPlatform(Scene _, Scene next) {
		if (next.name != "GG_Workshop") {
			return;
		}

		GameObject origPlat = next.GetRootGameObjects()
			.First(go => go.name == "gg_plat_float_wide");

		GameObject plat = UObject.Instantiate(origPlat);
		plat.transform.SetPosition2D(204.15f, 42.3f);
	}
}
