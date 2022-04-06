namespace GodSeekerPlus.Modules.BossChallenge;

[ToggleableLevel(ToggleableLevel.ChangeScene)]
[DefaultEnabled]
internal sealed class SegmentedP5 : Module {
	private const string dummySeqPD = "bossDoorStateTier5Segmented";

	internal static readonly (int start, int end)[] segments = new[] {
		(0, 10),
		(11, 16),
		(17, 22),
		(23, 28),
		(29, 35),
		(36, 41),
		(42, 48),
		(49, 52)
	};

	internal static readonly (string start, string end)[] segmentsName = new[] {
		("VENGEFLY_MAIN", "TEMP_NM_SUPER"),
		("GH_XERO_C_MAIN", "SISTERS_MAIN"),
		("GH_MUMCAT_C_MAIN", "PAINTMASTER_MAIN"),
		("HIVE_KNIGHT_MAIN", "GRIMM_SUPER"),
		("BLACK_KNIGHT_MAIN", "HORNET_MAIN"),
		("ENRAGED_GUARDIAN_MAIN", "WHITE_DEFENDER_MAIN"),
		("MAGE_LORD_DREAM_MAIN", "NIGHTMARE_GRIMM_SUPER"),
		("HK_PRIME_MAIN", "ABSOLUTE_RADIANCE_MAIN")
	};

	private bool doorOnline = false;
	private bool running = false;
	private GameObject? segP5 = null;

	private protected override void Load() {
		On.GameManager.BeginScene += SetupScene;
		On.BossSequenceController.SetupNewSequence += RecalculateStartScene;
		On.BossSequence.CanLoad += SkipNotSelectedScenes;
		On.BossDoorChallengeUI.Setup += SetupUI;
		ModHooks.GetPlayerVariableHook += GetVarHook;
	}

	private protected override void Unload() {
		if (doorOnline) {
			doorOnline = false;
			UObject.Destroy(segP5);
		} else if (running) {
			Ref.HC.StartCoroutine(Quit());
		}

		On.GameManager.BeginScene -= SetupScene;
		On.BossSequenceController.SetupNewSequence -= RecalculateStartScene;
		On.BossSequence.CanLoad -= SkipNotSelectedScenes;
		On.BossDoorChallengeUI.Setup -= SetupUI;
		ModHooks.GetPlayerVariableHook -= GetVarHook;
	}
	private GameObject? selectBtn;
	private Text? selectBtnText;
	private string GetSegmentName() {
		var seg = segmentsName[GodSeekerPlus.LocalSettings.selectedP5Segment];
		return Language.Language.Get(seg.start, "Titles") + "---" + Language.Language.Get(seg.end, "Titles");
	}
	private void SetupUI(On.BossDoorChallengeUI.orig_Setup orig, BossDoorChallengeUI self, BossSequenceDoor door) {
		orig(self, door);
		if(door.gameObject != segP5) {
			if(selectBtn == null) UObject.Destroy(selectBtn);
			return;
		}
		self.descriptionText.text = "SegmentedP5/SelectionPhase".Localize();
		if(selectBtn == null) {
			var btn = self.gameObject.Child("Panel", "BeginButton");
			if(btn is null) return;
			selectBtn = UObject.Instantiate(
					btn, new Vector3(0, 2, 0), Quaternion.identity,
					btn.transform.parent
					);
			selectBtn.name = "SelectPhase";
			selectBtn.transform.SetSiblingIndex(6);
			selectBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(450, 60);

			selectBtnText = selectBtn.Child("Text")?.GetComponent<Text>();
			if(selectBtnText != null) {
				UObject.Destroy(selectBtnText.GetComponent<AutoLocalizeTextUI>());
				selectBtnText.text = GetSegmentName();
			}
			var sbtn = selectBtn.GetComponent<MenuButton>();

			var et = selectBtn.GetComponent<EventTrigger>();
			et.triggers.Clear();
			var entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.Submit;
			entry.callback.AddListener((_) => {
				sbtn.Select();
				OnSelectSeg();
			});
			et.triggers.Add(entry);
			
			var mbtn = btn.GetComponent<MenuButton>();

			var nav = mbtn.navigation;
			var nailBtn = (MenuButton)nav.selectOnDown;
			nav.selectOnDown = sbtn;
			mbtn.navigation = nav;

			nav = nailBtn.navigation;
			nav.selectOnUp = sbtn;
			nailBtn.navigation = nav;

			nav = sbtn.navigation;
			nav.selectOnUp = mbtn;
			nav.selectOnDown = nailBtn;
			sbtn.navigation = nav;
		}
	}
	private void OnSelectSeg() {
		if(selectBtn is null) return;

		GodSeekerPlus.LocalSettings.selectedP5Segment++;
		if(GodSeekerPlus.LocalSettings.selectedP5Segment >= segments.Length) {
			GodSeekerPlus.LocalSettings.selectedP5Segment = 0;
		}

		if(selectBtnText != null) {
			selectBtnText.text = GetSegmentName();
		}
	}

	private void SetupScene(On.GameManager.orig_BeginScene orig, GameManager self) {
		orig(self);

		if (running && !BossSceneController.IsBossScene) {
			running = false;
			Logger.LogDebug("Segmented P5 sequence finished");
		}

		if (self.sceneName != "GG_Atrium_Roof") {
			doorOnline = false;
			return;
		}

		doorOnline = true;

		SetupDoor();
	}


	private void SetupDoor() {
		#region Setup door

		var origP5 = GameObject.Find("GG_Final_Challenge_Door");
		segP5 = UObject.Instantiate(origP5);
		segP5.SetActive(false);
		segP5.name = "GG_Final_Challenge_Door_Segmented";
		segP5.transform.SetPosition2D(54.9279f, 21.85f);

		BossSequenceDoor door = segP5.GetComponent<BossSequenceDoor>();
		door.playerDataString = dummySeqPD;
		door.dreamReturnGate.name = "door_dreamReturnGG_" + segP5.name;

		BossSequence sequence = door.bossSequence;
		sequence.achievementKey = "";

		#endregion

		#region Setup child objects

		GameObject orb = segP5.Child("gg_final_door_pieces", "gate orb")!;
		orb.transform.parent = segP5.transform;
		orb.transform.SetPositionY(orb.transform.GetPositionY() + 2.5f);

		GameObject smoke = segP5.Child("gg_final_door_pieces", "shadow gate effects", "abyss particles", "wispy smoke")!;
		smoke.transform.parent = segP5.transform;
		smoke.transform.SetPositionY(smoke.transform.GetPositionY() + 2.5f);

		CameraLockArea camLock = segP5.Child("CameraLockArea")!.GetComponent<CameraLockArea>();
		camLock.cameraXMax += -42.5031f;
		camLock.cameraXMin += -42.5031f;
		camLock.cameraYMax += -54.1f;
		camLock.cameraYMin += -54.1f;

		PlayMakerFSM uiFsm = door.challengeFSM;
		uiFsm.ChangeTransition("Wait", FsmEvent.Finished.Name, "Take Control");
		uiFsm.GetAction<SetFsmFloat>("Impact", 5).setValue = 0.75f;
		uiFsm.GetAction<Wait>("Dream Box Down", 3).time = 0.75f;

		#endregion

		#region Destroy unused

		UObject.Destroy(segP5.Child("gg_top_door_open"));
		UObject.Destroy(segP5.Child("gg_final_door_pieces"));
		UObject.Destroy(segP5.Child("Lock Break Rumble Sound"));
		UObject.Destroy(segP5.Child("Lock Break Antic"));
		UObject.Destroy(segP5.Child("Lock Break"));
		UObject.Destroy(segP5.Child("Lock Set"));

		#endregion

		segP5.SetActive(true);

		Logger.LogDebug("Segmented P5 door setup finished");
	}

	private void RecalculateStartScene(
		On.BossSequenceController.orig_SetupNewSequence orig,
		BossSequence sequence,
		BossSequenceController.ChallengeBindings bindings,
		string playerData
	) {
		orig(sequence, bindings, playerData);

		if (playerData == dummySeqPD) {
			running = true;
		} else {
			return;
		}

		ReflectionHelper.SetField(typeof(BossSequenceController), "bossIndex", -1);
		ReflectionHelper.CallMethod(typeof(BossSequenceController), "IncrementBossIndex");

		segP5!.GetComponent<BossSequenceDoor>()
			.challengeFSM.FsmVariables
			.FindFsmString("To Scene")
			.Value = sequence.GetSceneAt(BossSequenceController.BossIndex);

		Logger.LogDebug("Starting segmented P5 sequence");
	}

	private bool SkipNotSelectedScenes(On.BossSequence.orig_CanLoad orig, BossSequence self, int index) {
		(int start, int end) = segments[GodSeekerPlus.LocalSettings.selectedP5Segment];
		return (!running || (index >= start && index <= end)) && orig(self, index);
	}


	private static object GetVarHook(Type type, string name, object value) => name == dummySeqPD
		? BossSequenceDoor.Completion.None with {
			canUnlock = true,
			unlocked = true
		}
		: value;

	private IEnumerator Quit() {
		running = false;

		yield return new WaitWhile(() => Ref.GM.gameState == GameState.PAUSED);

		Ref.HC.StartCoroutine("Die");

		Logger.LogDebug("Force quiting Segmented P5 sequence");
	}
}
