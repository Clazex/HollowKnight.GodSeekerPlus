namespace GodSeekerPlus.Modules.BossChallenge;

public sealed class ForceGreyPrinceEnterType : Module {
	[GlobalSetting]
	[BoolOption(true)]
	public static bool gpzEnterType = false;

	private static readonly SceneEdit handle = new(
		new("GG_Grey_Prince_Zote", "Grey Prince"),
		go => {
			PlayMakerFSM fsm = go.LocateMyFSM("Control");
			fsm.AddCustomAction(
				"Enter 1",
				() => fsm.GetVariable<FsmBool>("Faced Zote").Value = gpzEnterType
			);

			LogDebug("Grey Prince enter type modified");
		}
	);

	public override bool DefaultEnabled => true;

	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ChangeScene;

	private protected override void Load() =>
		handle.Enable();

	private protected override void Unload() =>
		handle.Disable();
}
