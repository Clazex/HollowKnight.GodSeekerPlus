using HKMirror.Hooks.OnHooks;

namespace GodSeekerPlus.Modules.BossChallenge;

public sealed class P5Health : Module {
	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ChangeScene;

	private protected override void Load() {
		OnBossSceneController.AfterOrig.get_BossLevel += OverrideLevel;
		OsmiHooks.SceneChangeHook += FixAscendedMarkoth;

		ModHooks.TakeDamageHook += FixDamage;
	}

	private protected override void Unload() {
		OnBossSceneController.AfterOrig.get_BossLevel -= OverrideLevel;
		OsmiHooks.SceneChangeHook -= FixAscendedMarkoth;

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

	private void FixAscendedMarkoth(Scene prev, Scene next) {
		if (next.name != "GG_Markoth_V" || BossSequenceController.IsInSequence) {
			return;
		}

		_ = next.GetRootGameObjects()
			.First(go => go.name == "Warrior")
			.Child("Ghost Warrior Markoth")
			.manageHealth(650);
	}
}
