using Osmi.FsmActions;

using WaitUntil = UnityEngine.WaitUntil;

namespace GodSeekerPlus.Modules.BossChallenge;

public sealed class InfiniteRadianceClimbing : Module {
	private static readonly float heroX = 60.4987f;
	private static readonly float heroY = 34.6678f;

	private static bool running = false;
	private static GameObject? bossCtrl = null;
	private static PlayMakerFSM? radCtrl = null;
	private static PlayMakerFSM? pitCtrl = null;
	private static Coroutine? rewindCoro = null;

	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ChangeScene;

	private protected override void Load() =>
		OsmiHooks.SceneChangeHook += SetupScene;

	private protected override void Unload() {
		OsmiHooks.SceneChangeHook -= SetupScene;

		if (running) {
			Quit(true);
		}
	}

	private static void SetupScene(Scene prev, Scene next) {
		if (prev.name != "GG_Workshop" || next.name != "GG_Radiance") {
			if (running) {
				Quit();
			}

			return;
		}

		if (running) {
			Quit(true);
			throw new InvalidOperationException("Running multiple times at the same time");
		}

		running = true;
		bossCtrl = next.GetGameObjectByName("Boss Control");
		radCtrl = bossCtrl.Child("Absolute Radiance")!.LocateMyFSM("Control");
		pitCtrl = bossCtrl.Child("Abyss Pit")!.LocateMyFSM("Ascend");

		bossCtrl!
			.Child("Ascend Respawns", "Hazard Respawn Trigger v2 (15)")!
			.SetActive(false); // Remove final phase hazard respawn

		// Don't remove intro wall
		bossCtrl!.LocateMyFSM("Control").RemoveAction("Battle Start", 3);

		ModifyAbsRadFSM(radCtrl);

		radCtrl!.gameObject.LocateMyFSM("Phase Control")
			.Fsm.SetState("Set Ascend"); // Skip Phase 1-2

		LogDebug("Scene setup finished");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void ModifyAbsRadFSM(PlayMakerFSM fsm) {
		fsm.RemoveAction("Set Arena 1", 3); // Don't activate P1 cam lock

		// Spawn P2 plats before appearance
		fsm.ChangeTransition("Set Arena 1", "FINISHED", "Climb Plats1");

		fsm.InsertAction("Set Arena 1", new InvokeCoroutine(TeleportSetup), 0);

		FsmState spawnPlatsState = fsm.GetState("Climb Plats1");
		spawnPlatsState.Actions = new[] {
			spawnPlatsState.Actions[2], // Spawn plats
			new InvokeMethod(() => fsm.gameObject.manageHealth(int.MaxValue))
		};
		(spawnPlatsState.Actions[0] as SendEventByName)!.delay = 0;

		FsmState screamState = fsm.GetState("Scream");
		screamState.Actions = new[] {
			screamState.Actions[0], // Play audio clip
			new InvokeMethod(() => rewindCoro ??= radCtrl!.StartCoroutine(Rewind())),
			new Wait() { time = 60f }, // Wait for Rewind coroutine
			screamState.Actions[7] // Reserved for compatibility with CorrectRadianceHP
		};
	}

	private static IEnumerator TeleportSetup() {
		SpriteFlash flasher = Ref.HC.GetComponent<SpriteFlash>();

		Ref.HC.RelinquishControl();
		flasher.FlashingSuperDash();

		yield return new WaitForSeconds(0.5f);

		Ref.HC.transform.SetPosition2D(heroX, heroY);
		Ref.HC.FaceRight();
		Ref.HC.SetHazardRespawn(Ref.HC.transform.position, true);

		bossCtrl!.Child("Intro Wall")!.SetActive(false);

		yield return new WaitForSeconds(3.75f);

		flasher.CancelFlash();
		Ref.HC.RegainControl();

		LogDebug("Hero teleported");
	}

	private static IEnumerator Rewind() {
		LogDebug("AbsRad final phase started, rewinding...");

		SpriteFlash flasher = Ref.HC.GetComponent<SpriteFlash>();
		GameObject beam = radCtrl!.gameObject.Child("Eye Beam Glow", "Ascend Beam")!;

		radCtrl!.gameObject.LocateMyFSM("Attack Commands").Fsm.SetState("Idle"); // Stop final orbs
		beam.SetActive(false); // Remove last beam

		// Reset abyss pit
		pitCtrl!.GetVariable<FsmFloat>("Hero Y").Value = 33f;
		pitCtrl!.SendEvent("ASCEND");

		// Teleport hero back
		PlayerDataR.isInvincible = true;
		Ref.HC.RelinquishControl();
		Ref.HC.transform.SetPosition2D(heroX, heroY);
		Ref.HC.FaceRight();
		Ref.HC.SetHazardRespawn(Ref.HC.transform.position, true);
		flasher.FlashingSuperDash();

		yield return new WaitUntil(() => pitCtrl!.transform.position.y == 30f);

		// Reset hazard respawns and pit raiser
		bossCtrl!
			.Child("Ascend Respawns")!
			.GetChildren()
			.Filter(go => go.name.StartsWith("Hazard Respawn Trigger v2"))
			.ForEach(go => {
				go.GetComponent<HazardRespawnTrigger>().Reflect().inactive = false;
				go.LocateMyFSM("raise_abyss_pit").Fsm.SetState("Idle");
			});

		// Give back hero control
		flasher.CancelFlash();
		Ref.HC.RegainControl();
		PlayerDataR.isInvincible = false;

		yield return new WaitForSeconds(1.5f);

		// Restart beam cast
		radCtrl!.Fsm.SetState("Ascend Cast");
		radCtrl!.transform.SetPositionX(62.94f);
		beam.transform.parent.gameObject.SetActive(true);
		beam.SetActive(true);

		rewindCoro = null; // release lock
	}

	private static void Quit(bool killPlayer = false) {
		running = false;
		bossCtrl = null;
		radCtrl = null;
		pitCtrl = null;

		if (killPlayer) {
			_ = Ref.HC.StartCoroutine(DelayedKill());
		}
	}

	private static IEnumerator DelayedKill() {
		yield return new WaitUntil(() => Ref.GM.gameState == GameState.PLAYING);
		_ = Ref.HC.StartCoroutine("Die");
	}
}
