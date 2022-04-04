namespace GodSeekerPlus.Modules.Cosmetic;

[Category(nameof(Cosmetic))]
internal sealed class NoFuryEffect : Module {
	private const string goName = "fury_effects_v2";

	private protected override void Load() =>
		Ref.GC.hudCamera.gameObject.Child(goName)?.SetActive(false);

	private protected override void Unload() =>
		Ref.GC.hudCamera.gameObject.Child(goName)?.SetActive(true);
}
