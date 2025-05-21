using Osmi.FsmActions;

using WaitUntil = UnityEngine.WaitUntil;

namespace GodSeekerPlus.Modules.GodseekerMode;

public sealed class ColosseumOfFools : Module {
	public override bool DefaultEnabled => true;

	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ChangeScene;

	public static bool IsInGodseekerColosseum => PlayerDataR.bossRushMode
		&& Ref.GM.sceneName is "Room_Colosseum_01" or "Room_Colosseum_Bronze"
			or "Room_Colosseum_Silver" or "Room_Colosseum_Gold";

	public ColosseumOfFools() {
		On.PlayMakerFSM.Start += ModifyDreamNailFSM;
		On.PlayMakerFSM.Start += ModifyDeathFSM;
	}

	private protected override void Load() {
		OsmiHooks.SceneChangeHook += SetupWorkshop;
		OsmiHooks.SceneChangeHook += SetupColosseumEntrance;
		On.PlayMakerFSM.Start += RemoveReward;
	}

	private protected override void Unload() {
		OsmiHooks.SceneChangeHook -= SetupWorkshop;
		OsmiHooks.SceneChangeHook -= SetupColosseumEntrance;
		On.PlayMakerFSM.Start -= RemoveReward;

		if (IsInGodseekerColosseum) {
			_ = GlobalCoroutineExecutor.Start(Exit());
		}
	}

	private void ModifyDreamNailFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		orig(self);

		if (self.FsmName != "Dream Nail" || self.gameObject != Ref.HC.gameObject) {
			return;
		}

		self.InsertAction("Can Warp?", new InvokePredicate() {
			predicate = () => IsInGodseekerColosseum,
			trueEvent = FsmEvent.Finished
		}, 10);

		self.AddAction("Leave Type", new InvokePredicate() {
			predicate = () => IsInGodseekerColosseum,
			trueEvent = FsmEvent.FindEvent("GODS GLORY")
		});

		self.InsertAction("Boss?", new InvokePredicate() {
			predicate = () => IsInGodseekerColosseum,
			trueEvent = FsmEvent.FindEvent("TRUE")
		}, 0);
	}

	private static void ModifyDeathFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		orig(self);

		if (
			self.FsmName != "Hero Death Anim"
			|| self.gameObject != Ref.HC.heroDeathPrefab
		) {
			return;
		}

		self.AddAction("Map Zone", new InvokePredicate() {
			predicate = () => IsInGodseekerColosseum,
			trueEvent = FsmEvent.FindEvent("DREAM")
		});
	}

	private static void SetupWorkshop(Scene prev, Scene next) {
		if (!PlayerDataR.bossRushMode || next.name != "GG_Workshop") {
			return;
		}

		GameObject oblobblesStatue = next.GetRootGameObject("GG_Statue_Oblobbles")!;
		GameObject godTamerStatue = next.GetRootGameObject("GG_Statue_GodTamer")!;

		// Create Statue
		GameObject colosseumStatue = GameObjectUtil.Instantiate(
			oblobblesStatue,
			new(147f, 6f, 3.5f)
		);
		colosseumStatue.name = "GG_Statue_Colosseum";

		// Remove original controller
		BossStatue statueCtrl = colosseumStatue.GetComponent<BossStatue>();
		GameObject returnGate = statueCtrl.dreamReturnGate;
		UObject.DestroyImmediate(statueCtrl);
		returnGate.name = "door_dreamReturnGG_GG_Statue_Colosseum";
		colosseumStatue.Child("Base", "Plaque")!.DisableChildren();

		// Setup statue main part
		GameObject beesParent = colosseumStatue.Child("Base", "Statue", "Big_Bees")!;
		// This is referenced by a component so disable instead of destroy
		beesParent.Child("GG_statues_0037_36 (1)")!.SetActive(false);
		GameObject obleobble = beesParent.Child("GG_statues_0037_36")!;
		obleobble.transform.position = new(146.4f, 7.6f, 6.7f);
		obleobble.SetScale(0.6f, 0.6f);
		GameObject tamer = GameObjectUtil.Instantiate(
			godTamerStatue.Child("Base/Statue/Lobster Lancer/GG_statues_0006_5 (1)")!,
			new(147.5f, 10.45f, 7.3f),
			beesParent.transform
		);
		tamer.SetScale(0.7854f, 0.7953f);

		// Move prompt
		GameObject inspect = colosseumStatue.Child("Inspect")!;
		inspect.Child("Prompt Marker")!.transform.position = new(147f, 13f, 3.7f);

		// Modify inspect effect
		inspect.LocateMyFSM("npc_control")
			.GetVariable<FsmString>("Prompt Name").Value = "Enter";
		PlayMakerFSM uiFsm = inspect.LocateMyFSM("GG Boss UI");
		uiFsm.GetAction("Open UI", 1).Enabled = false;
		uiFsm.ChangeTransition("Open UI", FsmEvent.Finished.Name, "Take Control");
		uiFsm.ChangeTransition("Take Control", FsmEvent.Finished.Name, "Impact");
		uiFsm.AddCustomAction("Take Control", () => {
			StaticVariableList.SetValue("bossSceneToLoad", "Room_Colosseum_01");
			PlayerDataR.dreamReturnScene = "GG_Workshop";
			PlayerDataR.bossReturnEntryGate = "door_dreamReturnGG_GG_Statue_Colosseum";
		});
		uiFsm.GetAction<BeginSceneTransition>("Change Scene", 6)
			.entryGateName = "left1";

		LogDebug("Colosseum statue created");
	}

	private static void SetupColosseumEntrance(Scene prev, Scene next) {
		if (!PlayerDataR.bossRushMode || next.name != "Room_Colosseum_01") {
			_ = Ref.HC.gameObject.RemoveComponent<SoulFiller>();
			return;
		}

		PlayerDataR.colosseumBronzeOpened = true;
		PlayerDataR.colosseumSilverOpened = true;
		PlayerDataR.colosseumGoldOpened = true;

		// Fix states
		Ref.HC.EnterWithoutInput(false);
		PlayMakerFSM blankerWhiteFsm = Ref.GC.hudCamera.gameObject
			.Child("Blanker White")!
			.LocateMyFSM("Blanker Control");
		blankerWhiteFsm.GetVariable<FsmFloat>("Fade Time").Value = 0.0f;
		blankerWhiteFsm.SendEvent("FADE OUT");
		PlayMakerFSM blankerFsm = Ref.GC.hudCamera.gameObject
			.Child("2dtk Blanker")!
			.LocateMyFSM("Blanker Control");
		blankerFsm.SendEvent("FADE IN INSTANT");
		blankerFsm.SendEvent("FADE OUT");

		// Remove Little Fool
		new string[] { "Little Fool NPC", "Dream Dialogue", "col_NPC_chain" }
			.ForEach(name => next.GetRootGameObject(name)!.SetActive(false));

		// Close hatch
		next.GetRootGameObject("Colosseum Hatch")!
			.LocateMyFSM("Close")
			.AddTransition("Idle", FsmEvent.Finished.Name, "Close");

		_ = Ref.HC.gameObject.AddComponent<SoulFiller>();

		// Open 2nd and 3rd trials
		next.GetRootGameObjects()
			.Filter(i => i.name is "Silver Trial Board" or "Gold Trial Board")
			.Take(2)
			.ForEach(i => i.LocateMyFSM("Conversation Control")
				.RemoveAction("State Check", 0)
			);

		// Change transition
		GameObject left1 = next.GetRootGameObject("left1")!;
		TransitionPoint transition = left1.GetComponent<TransitionPoint>();
		transition.targetScene = "GG_Workshop";
		transition.entryPoint = "door_dreamReturnGG_GG_Statue_Colosseum";
		transition.customFade = true;
		transition.customFadeFSM = left1.AddComponent<PlayMakerFSM>();
		transition.sceneLoadVisualization = GameManager.SceneLoadVisualizations.GodsAndGlory;
		transition.OnBeforeTransition += () => {
			// Copied from boss scene Dream Return fsm

			// Statue
			Ref.HC.gameObject.LocateMyFSM("Dream Return")
				.GetVariable<FsmBool>("Dream Returning").Value = true;

			// Fade Out
			// Ref.GC.cameraFadeFSM.SendEvent("FADE OUT INSTANT");
			PlayMakerFSM blankerWhiteFsm = Ref.GC.hudCamera.gameObject
				.Child("Blanker White")!
				.LocateMyFSM("Blanker Control");
			blankerWhiteFsm.GetVariable<FsmFloat>("Fade Time").Value = 0.0f;
			blankerWhiteFsm.SendEvent("FADE IN");

			// Heal
			Ref.HC.MaxHealth();

			// Get PlayerData
			Ref.GC.cameraFadeFSM.GetVariable<FsmBool>("No Fade").Value = true;

			// New Scene
			Ref.GC.hudCanvas.LocateMyFSM("Slide Out").SendEvent("OUT");
			Ref.HC.RelinquishControl();
			Ref.HC.StopAnimationControl();
			Ref.HC.EnterWithoutInput(true);

			static void SetCameraFade() {
				Ref.GM.OnFinishedSceneTransition -= SetCameraFade;
				Ref.GC.hudCamera.gameObject.Child("2dtk Blanker")!
					.LocateMyFSM("Blanker Control")
					.SendEvent("FADE OUT INSTANT");
			}

			Ref.GM.OnFinishedSceneTransition += SetCameraFade;
		};

		LogDebug("Colosseum entrace setup finished");
	}

	private static void RemoveReward(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		orig(self);

		if (
			!PlayerDataR.bossRushMode
			|| self is not {
				FsmName: "Geo Pool",
				name: "Colosseum Manager",
				gameObject.scene.name: "Room_Colosseum_Bronze"
					or "Room_Colosseum_Silver" or "Room_Colosseum_Gold"
			}
		) {
			return;
		}

		FsmTransition transition = self.Fsm.GlobalTransitions
			.First(i => i.EventName == "GIVE GEO");
		FsmState targetState = self.GetValidState("Achieve Check");
		transition.ToFsmState = targetState;
		transition.ToState = targetState.Name;

		self.GetVariable<FsmBool>("Shiny Item").Value = false;

		LogDebug("Colosseum reward removal finished");
	}

	private static IEnumerator Exit() {
		yield return new WaitUntil(() => Ref.GM.gameState == GameState.PLAYING);

		Ref.GM.BeginSceneTransition(new() {
			SceneName = "GG_Workshop",
			EntryGateName = "left1",
			EntryDelay = 0,
			PreventCameraFadeOut = true,
			Visualization = GameManager.SceneLoadVisualizations.GodsAndGlory,
		});
	}

	private sealed class SoulFiller : MonoBehaviour {
		private HeroController hc = null!;

		public void Awake() {
			hc = GetComponent<HeroController>();
			ModHooks.BeforeSceneLoadHook += DestroySelf;
			LogDebug("Colosseum soul filling started");
		}

		public void FixedUpdate() =>
			hc.AddMPChargeSpa(11);

		public string DestroySelf(string sceneName) {
			ModHooks.BeforeSceneLoadHook -= DestroySelf;
			Destroy(this);
			LogDebug("Colosseum soul filling ended");
			return sceneName;
		}
	}
}
