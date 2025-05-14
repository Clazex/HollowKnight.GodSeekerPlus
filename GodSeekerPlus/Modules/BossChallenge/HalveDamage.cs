namespace GodSeekerPlus.Modules.BossChallenge;

internal sealed class HalveDamage : Module {
	internal static event Func<bool> ShouldFunctionHook = null!;

	public override bool Hidden => true;

	private protected override void Load() {
		ModHooks.TakeHealthHook += MakeDamageHalved;
		On.HeroController.StartRecoil += FixTakeHitEffect;
	}

	private static bool ShouldActivate() {
		if (ShouldFunctionHook == null) {
			return false;
		}

		foreach (Func<bool> predicate in ShouldFunctionHook.GetInvocationList().Cast<Func<bool>>()) {
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

public abstract class HalveDamageConditioned : Module {
	private protected sealed override void Load() =>
		HalveDamage.ShouldFunctionHook += Predicate;

	private protected sealed override void Unload() =>
		HalveDamage.ShouldFunctionHook -= Predicate;

	private protected abstract bool Predicate();
}

public sealed class HalveDamageHoGAscendedOrAbove : HalveDamageConditioned {
	private protected override bool Predicate() =>
		!BossSequenceController.IsInSequence
		&& BossSceneController.IsBossScene
		&& BossSceneController.Instance.Reflect().bossLevel > 0;
}

public sealed class HalveDamageHoGAttuned : HalveDamageConditioned {
	private protected override bool Predicate() =>
		!BossSequenceController.IsInSequence
		&& BossSceneController.IsBossScene
		&& BossSceneController.Instance.Reflect().bossLevel == 0;
}

public sealed class HalveDamageOtherPlace : HalveDamageConditioned {
	private protected override bool Predicate() => !BossSceneController.IsBossScene;
}

public sealed class HalveDamagePantheons : HalveDamageConditioned {
	private protected override bool Predicate() => BossSequenceController.IsInSequence;
}
