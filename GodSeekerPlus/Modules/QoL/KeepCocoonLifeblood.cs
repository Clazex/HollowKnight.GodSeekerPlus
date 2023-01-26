namespace GodSeekerPlus.Modules.QoL;

public sealed class KeepCocoonLifeblood : Module {
	public override bool DefaultEnabled => true;

	private protected override void Load() =>
		ModHooks.BlueHealthHook += CocoonCompensate;

	private protected override void Unload() =>
		ModHooks.BlueHealthHook -= CocoonCompensate;

	private static int CocoonCompensate() {
		if (Ref.GM.sceneName != "GG_Spa" || !Ref.PD.blueRoomActivated) {
			return 0;
		}

		HealthCocoon cocoon = USceneManager
			.GetActiveScene()
			.GetRootGameObjects()
			.First(go => go.name == "blue stuff")
			.Child("Health Cocoon (1)")!
			.GetComponent<HealthCocoon>();

		return !cocoon.Reflect().activated
			? 0
			: cocoon.flingPrefabs
				.First(fp => fp.prefab.name == "Health Scuttler")
				.Reflect()
				.pool
				.Filter(go => !go.activeSelf)
				.Count();
	}
}
