namespace GodSeekerPlus.Modules.QoL;

[ToggleableLevel(ToggleableLevel.ChangeScene)]
[DefaultEnabled]
internal sealed class GreyPrinceToggle : Module {
	private bool running = false;

	private protected override void Load() {
		On.GameManager.BeginScene += StartSetup;
		ModHooks.GetPlayerVariableHook += GetVarHook;
		ModHooks.SetPlayerVariableHook += SetVarHook;

		if (Ref.GM?.sceneName == "GG_Workshop") {
			Ref.HC.transform.SetPosition2D(2, 9); // leave HoG
		}
	}

	private protected override void Unload() {
		On.GameManager.BeginScene -= StartSetup;
		ModHooks.GetPlayerVariableHook -= GetVarHook;
		ModHooks.SetPlayerVariableHook -= SetVarHook;

		if (running) {
			running = false;
			Ref.HC.transform.SetPosition2D(2, 9); // leave HoG
		}
	}

	private void StartSetup(On.GameManager.orig_BeginScene orig, GameManager self) {
		orig(self);

		if (!Ref.PD.bossRushMode || Ref.GM.sceneName != "GG_Workshop") {
			running = false;
			return;
		}

		Ref.GM.StartCoroutine(SetupScene());
	}

	private IEnumerator SetupScene() {
		running = true;

		GameObject gpStatue = USceneManager.GetActiveScene()
			.GetRootGameObjects()
			.First(go => go.name == "GG_Statue_GreyPrince");
		gpStatue.RemoveComponent<DeactivateIfPlayerdataTrue>();
		gpStatue.SetActive(true);
		GameObject dreamSwitch = gpStatue.Child("dream_version_switch")!;
		GameObject litPieces = dreamSwitch.Child("lit_pieces")!;
		GameObject burstPt = litPieces.Child("Burst Pt")!;

		// Prevents bursting particles on entering save
		burstPt.transform.SetPositionY(burstPt.transform.GetPositionY() + 1000f);

		// Make a dummy dream variant
		BossStatue statue = gpStatue.GetComponent<BossStatue>();
		statue.dreamBossDetails = statue.bossDetails;
		statue.dreamBossScene = statue.bossScene;
		statue.dreamStatueStatePD = statue.statueStatePD;
		statue.SetDreamVersion(statue.UsingDreamVersion, false, false);

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

		yield return new WaitUntil(() => dreamSwitch
			.Child("GG_statue_plinth_orb_off")
			?.GetComponent<BossStatueDreamToggle>() != null
		);

		BossStatueDreamToggle toggle = dreamSwitch
			.Child("GG_statue_plinth_orb_off")!
			.GetComponent<BossStatueDreamToggle>();
		UObject.DestroyImmediate(toggle.dreamBurstSpawnPoint.gameObject);
		toggle.SetOwner(statue);

		// Fix dual plaques
		statue.altPlaqueL.gameObject.SetActive(false);
		statue.altPlaqueR.gameObject.SetActive(false);
		statue.regularPlaque.gameObject.SetActive(true);
		statue.SetPlaqueState(statue.StatueState, statue.regularPlaque, statue.statueStatePD);

		yield return new WaitUntil(() => Ref.HC.isHeroInPosition);
		yield return new WaitForSeconds(0.2f);
		yield return new WaitWhile(() => Ref.HC.controlReqlinquished || Ref.PD.atBench);
		burstPt.transform.SetPositionY(burstPt.transform.GetPositionY() - 1000f); // Restore
	}


	private object GetVarHook(Type type, string name, object value) {
		if (!Ref.PD.bossRushMode || name != "statueStateGreyPrince") {
			return value;
		}

		var completion = (BossStatue.Completion) value;
		completion.usingAltVersion = Ref.PD.greyPrinceDefeated;
		return completion;
	}

	private object SetVarHook(Type type, string name, object value) {
		if (!Ref.PD.bossRushMode || name != "statueStateGreyPrince") {
			return value;
		}

		var completion = (BossStatue.Completion) value;
		Ref.PD.greyPrinceDefeated = completion.usingAltVersion;
		completion.usingAltVersion = false;
		return completion;
	}
}
