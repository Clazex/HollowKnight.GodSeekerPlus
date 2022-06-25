namespace GodSeekerPlus.Modules.BossChallenge;

[Hidden]
internal sealed class HalveDamage : Module {
	internal static event Func<bool> ShouldFunctionHook = null!;

	private protected override void Load() {
		ModHooks.TakeHealthHook += MakeDamageHalved;
		On.HeroController.StartRecoil += FixTakeHitEffect;
	}

	private static bool ShouldActivate() {
		if (ShouldFunctionHook == null) {
			return false;
		}

		foreach (Func<bool> predicate in ShouldFunctionHook.GetInvocationList()) {
			if (predicate.Invoke()) {
				return true;
			}
		}

		return false;
	}

	private int MakeDamageHalved(int damage) =>
		ShouldActivate() ? (int) Math.Ceiling(damage / 2f) : damage;

	private IEnumerator FixTakeHitEffect(On.HeroController.orig_StartRecoil orig, HeroController self, CollisionSide impactSide, bool spawnDamageEffect, int damageAmount) =>
		orig(self, impactSide, spawnDamageEffect, MakeDamageHalved(damageAmount));
}

internal abstract class HalveDamageConditioned : Module {
	private protected override void Load() =>
		HalveDamage.ShouldFunctionHook += Predicate;

	private protected override void Unload() =>
		HalveDamage.ShouldFunctionHook -= Predicate;

	protected abstract bool Predicate();
}

internal sealed class HalveDamageHoGAscendedOrAbove : HalveDamageConditioned {
	protected override bool Predicate() =>
		!BossSequenceController.IsInSequence
		&& BossSceneController.IsBossScene
		&& ReflectionHelper.GetField<BossSceneController, int>(
			BossSceneController.Instance,
			"bossLevel"
		) > 0;
}

internal sealed class HalveDamageHoGAttuned : HalveDamageConditioned {
	protected override bool Predicate() =>
		!BossSequenceController.IsInSequence
		&& BossSceneController.IsBossScene
		&& ReflectionHelper.GetField<BossSceneController, int>(
			BossSceneController.Instance,
			"bossLevel"
		) == 0;
}

internal sealed class HalveDamageOtherPlace : HalveDamageConditioned {
	protected override bool Predicate() => !BossSceneController.IsBossScene;
}

internal sealed class HalveDamagePantheons : HalveDamageConditioned {
	protected override bool Predicate() => BossSequenceController.IsInSequence;
}
