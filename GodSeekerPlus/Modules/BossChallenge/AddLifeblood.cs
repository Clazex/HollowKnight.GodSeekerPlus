namespace GodSeekerPlus.Modules.BossChallenge;

public sealed class AddLifeblood : Module {
	[GlobalSetting]
	[IntOption(0, 35)]
	private static readonly int lifebloodAmount = 5;

	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ChangeScene;

	private protected override void Load() =>
		On.BossSceneController.Start += Add;

	private protected override void Unload() =>
		On.BossSceneController.Start -= Add;

	private static IEnumerator Add(On.BossSceneController.orig_Start orig, BossSceneController self) {
		yield return orig(self);

		if (BossSequenceController.IsInSequence) {
			yield break;
		}

		Add();
	}

	internal static void Add() {
		for (int i = 0; i < lifebloodAmount; i++) {
			EventRegister.SendEvent("ADD BLUE HEALTH");
		}

		Logger.LogDebug("Lifeblood added");
	}
}
