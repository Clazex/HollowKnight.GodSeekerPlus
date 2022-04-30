namespace GodSeekerPlus.Modules.QoL;

[ToggleableLevel(ToggleableLevel.ChangeScene)]
[DefaultEnabled]
internal sealed class P5Teleport : Module {
	private protected override void Load() =>
		USceneManager.activeSceneChanged += AddComponentToLever;

	private protected override void Unload() =>
		USceneManager.activeSceneChanged -= AddComponentToLever;

	private void AddComponentToLever(Scene prev, Scene next) {
		if (next.name == "GG_Atrium") {
			next.GetGameObjectByName("gg_roof_door_pieces")
				.Child("GG_door_caps", "gg_roof_lever")!
				.AddComponent<CustomDreamnailReaction>()
				.SetMethod((behaviour, _) => behaviour.StartCoroutine(Teleport()));
		}
	}

	private static IEnumerator Teleport() {
		if (!Ref.PD.finalBossDoorUnlocked) {
			yield break;
		}

		Logger.LogDebug("P5 teleport start");

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

		yield return new WaitWhile(() => Ref.PD.disablePause);
		Ref.PD.dreamGateX = origDGateX;
		Ref.PD.dreamGateY = origDGateY;

		#endregion
	}
}
