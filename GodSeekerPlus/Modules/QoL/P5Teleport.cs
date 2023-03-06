namespace GodSeekerPlus.Modules.QoL;

public sealed class P5Teleport : Module {
	private static bool teleporting = false;

	private static readonly SceneEdit leverEditHandle = new(
		new("GG_Atrium", "gg_roof_door_pieces", "GG_door_caps", "gg_roof_lever"),
		go => go.AddComponent<CustomDreamnailReaction>()
			.SetMethod(_ => GlobalCoroutineExecutor.Start(Teleport()))
	);

	public override bool DefaultEnabled => true;

	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ChangeScene;

	private protected override void Load() =>
		leverEditHandle.Enable();

	private protected override void Unload() =>
		leverEditHandle.Disable();

	private static IEnumerator Teleport() {
		if (teleporting || !PlayerDataR.finalBossDoorUnlocked) {
			yield break;
		}

		teleporting = true;
		LogDebug("P5 teleport start");

		#region Pre-teleport effects

		Ref.HC.RelinquishControl();
		Fsm dreamNailFSM = Ref.HC.gameObject.LocateMyFSM("Dream Nail").Fsm;

		new[] { "Warp Effect", "Warp End" }
			.Map(name => dreamNailFSM.GetState(name))
			.ForEach(state => ReflectionHelper.CallMethod(state, "ActivateActions", 0));

		Ref.HC.gameObject.LocateMyFSM("Roar Lock")
			.GetVariable<FsmBool>("No Roar")
			.Value = false;
		StaticVariableList.SetValue("skipRemoveDreamOrbs", true);

		yield return new WaitForSeconds(2);

		Ref.HC.FaceLeft();

		#endregion

		#region Substitute DGate data

		// Here we need to bypass every hook on dgate fields such that the
		// actual value is identical to what we need. This prevents
		// compatibility issue with mods like SmolKnight where dgate
		// position getting/setting is hooked for adding an offset
		string origDGateScene = Ref.PD.dreamGateScene;
		float origDGateX = Ref.PD.dreamGateX;
		float origDGateY = Ref.PD.dreamGateY;

		Ref.PD.dreamGateScene = "GG_Atrium_Roof";
		Ref.PD.dreamGateX = 105.6243f;
		Ref.PD.dreamGateY = 73.4129f;

		#endregion

		// Do teleport
		ReflectionHelper.CallMethod(dreamNailFSM.GetState("New Scene"), "ActivateActions", 0);

		#region Restore DGate Data

		// Restore this before scene loaded to prevent spawning DGate
		Ref.PD.dreamGateScene = origDGateScene;

		tk2dSpriteAnimator animator = Ref.HC.gameObject.GetComponent<tk2dSpriteAnimator>();
		yield return new WaitUntil(() => animator.IsPlaying("Super Hard Land"));

		Ref.PD.dreamGateX = origDGateX;
		Ref.PD.dreamGateY = origDGateY;

		#endregion

		teleporting = false;
	}
}
