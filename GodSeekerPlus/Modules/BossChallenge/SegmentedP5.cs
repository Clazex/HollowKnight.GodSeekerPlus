using UnityEngine.UI;

namespace GodSeekerPlus.Modules.BossChallenge;

public sealed class SegmentedP5 : Module {
	private const string dummySeqPD = "bossDoorStateTier5Segmented";

	private static readonly (int start, int end)[] segments = new[] {
		(0, 10),
		(11, 16),
		(17, 22),
		(23, 28),
		(29, 35),
		(36, 41),
		(42, 48),
		(49, 52)
	};

	[LocalSetting]
	public static int selectedP5Segment = 0;

	public override bool DefaultEnabled => true;

	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ChangeScene;

	private static readonly SceneEdit radianceHandle = new(
		new("GG_Radiance", "Boss Control", "Absolute Radiance"),
		go => go.LocateMyFSM("Control")
			.GetAction<SetStaticVariable>("Ending Scene", 1)
			.setValue.boolValue = !running
	);

	private static BossSequence? sequence = null;

	private static bool doorOnline = false;
	private static bool running = false;
	private static GameObject? segP5 = null;

	private static GameObject? selectBtn;
	private static Text? selectBtnText;

	private static string CurrentSegmentName =>
		$"SegmentedP5/Segment/{selectedP5Segment}".Localize();


	private protected override void Load() {
		On.GameManager.BeginScene += SetupScene;
		On.BossSequenceController.SetupNewSequence += StartSequence;
		On.BossSequence.CanLoad += SkipNotSelectedScenes;
		On.BossDoorChallengeUI.Setup += SetupUI;
		radianceHandle.Enable();
		ModHooks.GetPlayerVariableHook += GetVarHook;
	}

	private protected override void Unload() {
		if (doorOnline) {
			doorOnline = false;
			UObject.Destroy(segP5);
		} else if (running) {
			_ = Ref.HC.StartCoroutine(Quit());
		}

		if (sequence != null) {
			sequence.achievementKey = "ENDINGD";
		}

		On.GameManager.BeginScene -= SetupScene;
		On.BossSequenceController.SetupNewSequence -= StartSequence;
		On.BossSequence.CanLoad -= SkipNotSelectedScenes;
		On.BossDoorChallengeUI.Setup -= SetupUI;
		radianceHandle.Disable();
		ModHooks.GetPlayerVariableHook -= GetVarHook;
	}

	private static void SetupScene(On.GameManager.orig_BeginScene orig, GameManager self) {
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
	private static void SetupDoor() {
		#region Setup door

		var origP5 = GameObject.Find("GG_Final_Challenge_Door");
		segP5 = UObject.Instantiate(origP5);
		segP5.SetActive(false);
		segP5.name = "GG_Final_Challenge_Door_Segmented";
		segP5.transform.SetPosition2D(54.9279f, 21.85f);

		BossSequenceDoor door = segP5.GetComponent<BossSequenceDoor>();
		door.playerDataString = dummySeqPD;
		door.dreamReturnGate.name = "door_dreamReturnGG_" + segP5.name;
		door.dreamReturnGate.LocateMyFSM("Boss Sequence Finish")
			.RemoveTransition("Reset", FsmEvent.Finished.Name);

		sequence ??= door.bossSequence;

		#endregion

		#region Setup child objects

		GameObject orb = segP5.Child("gg_final_door_pieces", "gate orb")!;
		orb.SetParent(segP5);
		orb.GetTransformDelegate().Y += 2.5f;

		GameObject smoke = segP5.Child("gg_final_door_pieces", "shadow gate effects", "abyss particles", "wispy smoke")!;
		smoke.SetParent(segP5);
		smoke.GetTransformDelegate().Y += 2.5f;

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


	private static void SetupUI(On.BossDoorChallengeUI.orig_Setup orig, BossDoorChallengeUI self, BossSequenceDoor door) {
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

			IncreamentP5SegmentSelection();

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


	private static void StartSequence(
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

		BossSequenceControllerR.bossIndex = -1;
		BossSequenceControllerR.IncrementBossIndex();

		int firstSceneIndex = BossSequenceController.BossIndex;
		segP5!.GetComponent<BossSequenceDoor>()
			.challengeFSM.GetVariable<FsmString>("To Scene")
			.Value = sequence.GetSceneAt(firstSceneIndex);

		if (firstSceneIndex != 0) {
			BossSceneController.SetupEventDelegate oldSetupEvent = BossSceneController.SetupEvent;
			BossSceneController.SetupEvent = self => {
				oldSetupEvent(self);

				BossSequenceController.ApplyBindings();

				if (ModuleManager.IsModuleEnabled<ActivateFury>()) {
					BossSceneController.SetupEventDelegate oldSetupEvent = BossSceneController.SetupEvent;
					BossSceneController.SetupEvent = self => {
						oldSetupEvent(self);

						ActivateFury.Activate();
					};
				}

				if (ModuleManager.IsModuleEnabled<AddLifeblood>()) {
					AddLifeblood.Add();
				}
			};
		}

		Logger.LogDebug("Starting segmented P5 sequence");
	}

	private static bool SkipNotSelectedScenes(On.BossSequence.orig_CanLoad orig, BossSequence self, int index) {
		(int start, int end) = segments[selectedP5Segment];
		return (!running || (index >= start && index <= end)) && orig(self, index);
	}

	private static object GetVarHook(Type type, string name, object value) => name == dummySeqPD
		? BossSequenceDoor.Completion.None with {
			canUnlock = true,
			unlocked = true
		}
		: value;

	private static IEnumerator Quit() {
		running = false;

		yield return new WaitWhile(() => Ref.GM.gameState == GameState.PAUSED);

		_ = Ref.HC.StartCoroutine("Die");

		Logger.LogDebug("Force quiting Segmented P5 sequence");
	}


	private static void IncreamentP5SegmentSelection() {
		selectedP5Segment++;

		if (selectedP5Segment >= segments.Length) {
			selectedP5Segment = 0;
		}
	}
}
