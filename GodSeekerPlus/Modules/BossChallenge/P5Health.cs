using HKMirror.Hooks.OnHooks;

namespace GodSeekerPlus.Modules.BossChallenge;

[ToggleableLevel(ToggleableLevel.ChangeScene)]
internal sealed class P5Health : Module {
	private protected override void Load() {
		OnBossSceneController.AfterOrig.get_BossLevel += OverrideLevel;

		ModHooks.TakeDamageHook += FixDamage;
	}

	private protected override void Unload() {
		OnBossSceneController.AfterOrig.get_BossLevel -= OverrideLevel;

		ModHooks.TakeDamageHook -= FixDamage;
	}

	private int OverrideLevel(OnBossSceneController.Delegates.Params_get_BossLevel @params, int level) => 0;

	private int FixDamage(ref int hazardType, int damage) => damage switch {
		int i when i <= 0 => i,
		int i when BossSceneController.IsBossScene is false => i,
		int i => BossSceneController.Instance.Reflect().bossLevel switch {
			1 => i * 2,
			2 => 9999,
			_ => i,
		}
	};
}
