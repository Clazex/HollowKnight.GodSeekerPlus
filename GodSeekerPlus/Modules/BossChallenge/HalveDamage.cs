namespace GodSeekerPlus.Modules.BossChallenge;

[Category(nameof(BossChallenge))]
internal sealed class HalveDamage : Module {
	private protected override void Load() =>
		ModHooks.AfterTakeDamageHook += MakeDamageHalved;

	private protected override void Unload() =>
		ModHooks.AfterTakeDamageHook -= MakeDamageHalved;

	private int MakeDamageHalved(int _, int damage) =>
		(int) Math.Ceiling(damage / 2d);
}
