using MonoMod.RuntimeDetour;

namespace GodSeekerPlus.Modules.BossChallenge;

[ToggleableLevel(ToggleableLevel.ChangeScene)]
internal sealed class P5Health : Module {
	private Detour? detour = null;

	private protected override void Load() {
		detour = new(
			ReflectionHelper.GetPropertyInfo(typeof(BossSceneController), "BossLevel").GetGetMethod(),
			ReflectionHelper.GetMethodInfo(typeof(P5Health), nameof(OverrideLevel))
		);

		ModHooks.TakeDamageHook += FixDamage;
	}

	private protected override void Unload() {
		detour?.Dispose();

		ModHooks.TakeDamageHook -= FixDamage;
	}

	private int OverrideLevel() => 0;

	private int FixDamage(ref int hazardType, int damage) => damage switch {
		int i when i <= 0 => i,
		int i when BossSceneController.IsBossScene is false => i,
		int i => ReflectionHelper.GetField<BossSceneController, int>(BossSceneController.Instance, "bossLevel") switch {
			1 => i * 2,
			2 => 9999,
			_ => i,
		}
	};
}
