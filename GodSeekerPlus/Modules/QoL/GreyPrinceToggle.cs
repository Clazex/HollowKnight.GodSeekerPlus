namespace GodSeekerPlus.Modules.QoL;

[ToggleableLevel(ToggleableLevel.ChangeScene)]
[DefaultEnabled]
internal sealed class GreyPrinceToggle : Module {
	private bool running = false;

	private protected override void Load() {
		OsmiHooks.SceneChangeHook += StartSetup;

		if (Ref.GM?.sceneName == "GG_Workshop") {
			Ref.HC.transform.SetPosition2D(2, 9); // leave HoG
		}
	}

	private protected override void Unload() {
		OsmiHooks.SceneChangeHook -= StartSetup;

		if (running) {
			running = false;
			Ref.HC.transform.SetPosition2D(2, 9); // leave HoG
		}
	}

	private void StartSetup(Scene prev, Scene next) {
		if (!Ref.PD.bossRushMode || next.name != "GG_Workshop") {
			running = false;
			return;
		}

		running = true;

		GameObject gpStatue = USceneManager.GetActiveScene()
			.GetRootGameObjects()
			.First(go => go.name == "GG_Statue_GreyPrince");
		_ = gpStatue.RemoveComponent<DeactivateIfPlayerdataTrue>();
		gpStatue.SetActive(true);
		GameObject dreamSwitch = gpStatue.Child("dream_version_switch")!;
		GameObject litPieces = dreamSwitch.Child("lit_pieces")!;

		dreamSwitch.transform.Translate(0.5f, 0, 0);
		dreamSwitch
			.Child("lit_pieces", "Base Glow")!
			.GetComponent<tk2dSprite>().scale = Vector3.zero;
		dreamSwitch
			.Child("Statue Pt")!
			.SetActive(false);
		dreamSwitch.SetActive(true);

		// Change colors
		litPieces.Child("haze")!.GetComponent<SpriteRenderer>().color =
			new(0.8008f, 0.4453f, 0.707f);
		litPieces.Child("plinth_glow")!.GetComponent<ColorFader>().upColour =
			new(1f, 0.8438f, 0.9219f);
		litPieces.Child("dream_glowy_guy")!.GetComponent<ColorFader>().upColour =
			new(1f, 0.9102f, 0.9219f);

		_ = Ref.GM.StartCoroutine(SetupScene(dreamSwitch));
	}

	private IEnumerator SetupScene(GameObject dreamSwitch) {
		yield return new WaitUntil(() => dreamSwitch
			.Child("GG_statue_plinth_orb_off") != null
		);

		GameObject orb = dreamSwitch
			.Child("GG_statue_plinth_orb_off")!;
		_ = orb.AddComponent<FakeDreamToggle>();
		orb.SetActive(true);
	}

#nullable disable
	private sealed class FakeDreamToggle : MonoBehaviour {
		private GameObject litPieces;

		private GameObject dreamImpactPrefab;
		private Vector3 dreamImpactScale;
		private Transform dreamImpactPoint;

		private ColorFader[] colorFaders;

		private bool On {
			get => Ref.PD.greyPrinceDefeated;
			set => Ref.PD.greyPrinceDefeated = value;
		}

		public void Awake() {
			BossStatueDreamToggle toggle = GetComponent<BossStatueDreamToggle>();
			litPieces = toggle.litPieces;
			dreamImpactPrefab = toggle.dreamImpactPrefab;
			dreamImpactScale = toggle.dreamImpactScale;
			dreamImpactPoint = toggle.dreamImpactPoint;
			UObject.DestroyImmediate(toggle);
		}

		public void Start() {
			litPieces.SetActive(true);
			colorFaders = litPieces.GetComponentsInChildren<ColorFader>(true);
			colorFaders.ForEach(fader => fader.Fade(On));
		}

		public void OnTriggerEnter2D(Collider2D collision) {
			if (!gameObject.activeInHierarchy || collision.tag != "Dream Attack") {
				return;
			}

			On = !On;
			dreamImpactPrefab.Spawn(dreamImpactPoint.position).transform.localScale = dreamImpactScale;
			colorFaders.ForEach(fader => fader.Fade(On));

			Logger.LogDebug("Grey Prince toggle triggered");
		}
	}
#nullable restore
}
