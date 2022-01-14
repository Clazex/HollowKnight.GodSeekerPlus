namespace GodSeekerPlus.Modules.Visual;

[Category(nameof(Visual))]
internal sealed class NoLowHealthEffect : Module {
	private Coroutine? coroutine = null;

	private protected override void Load() {
		On.HeroController.Start += DeactivateForNewLevel;

		if (Ref.HC != null) {
			GetGO()!.SetActive(false);
		}
	}

	private protected override void Unload() {
		if (coroutine != null) {
			Ref.HC?.StopCoroutine(coroutine);
			coroutine = null;
		}

		On.HeroController.Start -= DeactivateForNewLevel;

		if (Ref.HC != null) {
			GetGO()!.SetActive(true);
		}
	}

	private void DeactivateForNewLevel(On.HeroController.orig_Start orig, HeroController self) {
		orig(self);
		coroutine = self.StartCoroutine(DeactivateThisLevel());
	}

	private static GameObject? GetGO() =>
		Ref.GC.hudCamera.gameObject.Child("Low Health Vignette");

	private static IEnumerator DeactivateThisLevel() {
		GameObject? go = null;
		yield return new WaitUntil(() => (go = GetGO()) != null);
		go!.SetActive(false);
	}
}
