namespace GodSeekerPlus.Modules.BossChallenge;

[ToggleableLevel(ToggleableLevel.ChangeScene)]
[DefaultEnabled]
internal sealed class ForceGreyPrinceEnterType : Module {
	[GlobalSetting]
	[BoolOption(true, true)]
	public static readonly bool gpzEnterType = false;

	private static readonly SceneEdit handle = new(
		new("GG_Grey_Prince_Zote", "Grey Prince"),
		go => {
			PlayMakerFSM fsm = go.LocateMyFSM("Control");
			fsm.AddCustomAction(
				"Enter 1",
				() => fsm.GetVariable<FsmBool>("Faced Zote").Value = gpzEnterType
			);

			Logger.LogDebug("Grey Prince enter type modified");
		}
	);

	private protected override void Load() =>
		handle.Enable();

	private protected override void Unload() =>
		handle.Disable();
}
