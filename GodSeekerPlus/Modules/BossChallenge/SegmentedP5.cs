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

	private static BossSequence? sequence = null;

	private bool doorOnline = false;
	private bool running = false;
	private GameObject? segP5 = null;

	private GameObject? selectBtn;
	private Text? selectBtnText;

	private static string CurrentSegmentName =>
		$"SegmentedP5/Segment/{GodSeekerPlus.LocalSettings.SelectedP5Segment}".Localize();


	private protected override void Load() {
		On.GameManager.BeginScene += SetupScene;
		On.BossSequenceController.SetupNewSequence += StartSequence;
		On.BossSequence.CanLoad += SkipNotSelectedScenes;
		On.BossDoorChallengeUI.Setup += SetupUI;
		On.PlayMakerFSM.Start += ModifyAbsRadFSM;
		ModHooks.GetPlayerVariableHook += GetVarHook;
	}

	private protected override void Unload() {
		if (doorOnline) {
			doorOnline = false;
			UObject.Destroy(segP5);
		} else if (running) {
			Ref.HC.StartCoroutine(Quit());
		}

		if (sequence != null) {
			sequence.achievementKey = "ENDINGD";
		}

		On.GameManager.BeginScene -= SetupScene;
		On.BossSequenceController.SetupNewSequence -= StartSequence;
		On.BossSequence.CanLoad -= SkipNotSelectedScenes;
		On.BossDoorChallengeUI.Setup -= SetupUI;
		On.PlayMakerFSM.Start -= ModifyAbsRadFSM;
		ModHooks.GetPlayerVariableHook -= GetVarHook;
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

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
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

		sequence ??= door.bossSequence;

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


	private void SetupUI(On.BossDoorChallengeUI.orig_Setup orig, BossDoorChallengeUI self, BossSequenceDoor door) {
		orig(self, door);

		if (door.gameObject != segP5) {
			// Not Segmented P5
			if (selectBtn != null) {
				// Restore navigation
				MenuButton oldSelectBtn = selectBtn.GetComponent<MenuButton>();
				Navigation oldNav = oldSelectBtn.navigation;

				var oldBeginBtn = oldNav.selectOnUp as MenuButton;
				var oldNailBtn = oldNav.selectOnDown as MenuButton;

				oldBeginBtn!.navigation = oldBeginBtn.navigation with {
					selectOnDown = oldNailBtn
				};
				oldNailBtn!.navigation = oldNailBtn.navigation with {
					selectOnUp = oldBeginBtn
				};

				UObject.Destroy(selectBtn);
			}

			return;
		}

		self.descriptionText.text = "SegmentedP5/SelectSegment".Localize();

		if (selectBtn != null) {
			// Created last time
			return;
		}

		if (self.gameObject.Child("Panel", "BeginButton") is not GameObject beginBtn) {
			return;
		}

		selectBtn = UObject.Instantiate(
			beginBtn, new(0, 2.075f, 0),
			Quaternion.identity, beginBtn.transform.parent
		);
		selectBtn.name = "SelectSegment";
		selectBtn.transform.SetSiblingIndex(6);
		selectBtn.GetComponent<RectTransform>().sizeDelta = new(440, 60);

		if (selectBtn.Child("Text")?.GetComponent<Text>() is Text text) {
			selectBtnText = text;
			UObject.Destroy(text.GetComponent<AutoLocalizeTextUI>());

			text.fontSize = 28;
			text.font = Modding.Menu.MenuResources.Perpetua;
			text.horizontalOverflow = HorizontalWrapMode.Overflow;
			text.text = CurrentSegmentName;
		}

		#region Setup UI event

		EventTrigger eventTrigger = selectBtn.GetComponent<EventTrigger>();
		eventTrigger.triggers = new() {
			eventTrigger.triggers[1]
		};

		MenuButton selectMenuBtn = selectBtn.GetComponent<MenuButton>();
		selectMenuBtn.buttonType = MenuButton.MenuButtonType.CustomSubmit;
		selectMenuBtn.proceed = false;
		selectMenuBtn.submitAction = btn => {
			if (selectBtn == null) {
				return;
			}

			GodSeekerPlus.LocalSettings.IncreamentP5SegmentSelection();

			if (selectBtnText != null) {
				selectBtnText.text = CurrentSegmentName;
			}
		};

		#endregion

		#region Setup navigation

		MenuButton beginMenuBtn = beginBtn.GetComponent<MenuButton>();
		var nailBtn = beginMenuBtn.navigation.selectOnDown as MenuButton;

		beginMenuBtn.navigation = beginMenuBtn.navigation with {
			selectOnDown = selectMenuBtn
		};

		nailBtn!.navigation = nailBtn.navigation with {
			selectOnUp = selectMenuBtn
		};

		selectMenuBtn.navigation = selectMenuBtn.navigation with {
			selectOnUp = beginMenuBtn,
			selectOnDown = nailBtn
		};

		#endregion

		Logger.LogDebug("Segmented P5 UI setup finished");
	}


	private void StartSequence(
		On.BossSequenceController.orig_SetupNewSequence orig,
		BossSequence sequence,
		BossSequenceController.ChallengeBindings bindings,
		string playerData
	) {
		orig(sequence, bindings, playerData);

		if (playerData == dummySeqPD) {
			running = true;
			sequence.achievementKey = "";
		} else {
			if (sequence.name == "Boss Sequence Tier 5") {
				sequence.achievementKey = "ENDINGD";
			}

			return;
		}

		ReflectionHelper.SetField(typeof(BossSequenceController), "bossIndex", -1);
		ReflectionHelper.CallMethod(typeof(BossSequenceController), "IncrementBossIndex");

		int firstSceneIndex = BossSequenceController.BossIndex;
		segP5!.GetComponent<BossSequenceDoor>()
			.challengeFSM.FsmVariables
			.FindFsmString("To Scene")
			.Value = sequence.GetSceneAt(firstSceneIndex);

		if (firstSceneIndex != 0) {
			BossSceneController.SetupEventDelegate oldSetupEvent = BossSceneController.SetupEvent;
			BossSceneController.SetupEvent = self => {
				oldSetupEvent(self);

				BossSequenceController.ApplyBindings();
			};
		}

		Logger.LogDebug("Starting segmented P5 sequence");
	}

	private bool SkipNotSelectedScenes(On.BossSequence.orig_CanLoad orig, BossSequence self, int index) {
		(int start, int end) = segments[GodSeekerPlus.LocalSettings.selectedP5Segment];
		return (!running || (index >= start && index <= end)) && orig(self, index);
	}

	private void ModifyAbsRadFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		orig(self);

		if (self is {
			name: "Absolute Radiance",
			FsmName: "Control"
		}) {
			ModifyAbsRadFSM(self);
		}
	}

	private void ModifyAbsRadFSM(PlayMakerFSM fsm) =>
		fsm.GetAction<SetStaticVariable>("Ending Scene", 1).setValue.boolValue = !running;

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
