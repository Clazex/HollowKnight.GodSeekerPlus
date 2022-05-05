namespace GodSeekerPlus.Modules.Bugfix;

[DefaultEnabled]
internal sealed class TransitionDeath : Module {
	private bool deadInSequence = false;

	private protected override void Load() {
		On.HeroController.Die += RecordDeath;
		On.GameManager.EnterHero += CheckDeath;
	}

	private protected override void Unload() {
		deadInSequence = false;
		On.HeroController.Die -= RecordDeath;
		On.GameManager.EnterHero -= CheckDeath;
	}

	private IEnumerator RecordDeath(On.HeroController.orig_Die orig, HeroController self) {
		if (BossSequenceController.IsInSequence) {
			deadInSequence = true;
		}

		yield return orig(self);
	}

	private void CheckDeath(On.GameManager.orig_EnterHero orig, GameManager self, bool additiveGateSearch) {
		orig(self, additiveGateSearch);

		if (self.RespawningHero || GameManagerR.hazardRespawningHero) {
			return;
		}

		if (deadInSequence && BossSequenceController.IsInSequence) {
			Logger.LogWarn("Dead in sequence while not finishing it, trying again");
			Ref.HC.StartCoroutine("Die");
		} else {
			deadInSequence = false;
		}
	}
}
