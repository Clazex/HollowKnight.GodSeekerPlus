using HKMirror.Hooks.OnHooks;

namespace GodSeekerPlus.Modules.BossChallenge;

public sealed class P5Health : Module {
	private static readonly SceneEdit ascendedMarkothHandle = new(
		new("GG_Markoth_V", "Warrior", "Ghost Warrior Markoth"),
		go => go.manageHealth(650)
	);

	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ChangeScene;

	private protected override void Load() {
		OnBossSceneController.AfterOrig.get_BossLevel += OverrideLevel;
		ascendedMarkothHandle.Enable();

		ModHooks.TakeDamageHook += FixDamage;
	}

	private protected override void Unload() {
		OnBossSceneController.AfterOrig.get_BossLevel -= OverrideLevel;
		ascendedMarkothHandle.Disable();

		ModHooks.TakeDamageHook -= FixDamage;
	}

	private static int OverrideLevel(OnBossSceneController.Delegates.Params_get_BossLevel @params, int level) => 0;

	private static int FixDamage(ref int hazardType, int damage) => damage switch {
		int i when i <= 0 => i,
		int i when BossSceneController.IsBossScene is false => i,
		int i => BossSceneController.Instance.Reflect().bossLevel switch {
			1 => i * 2,
			2 => 9999,
			_ => i,
		}
	};
}
