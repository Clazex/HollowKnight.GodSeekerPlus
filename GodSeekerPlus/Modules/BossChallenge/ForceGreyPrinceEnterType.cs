namespace GodSeekerPlus.Modules.BossChallenge;

public sealed class ForceGreyPrinceEnterType : Module {
	[GlobalSetting]
	[EnumOption]
	public static EnterType gpzEnterType = EnterType.Long;

	private static readonly SceneEdit handle = new(
		new("GG_Grey_Prince_Zote", "Grey Prince"),
		go => {
			PlayMakerFSM fsm = go.LocateMyFSM("Control");
			fsm.AddCustomAction(
				"Enter 1",
				() => fsm.GetVariable<FsmBool>("Faced Zote").Value = gpzEnterType switch {
					EnterType.Long => false,
					EnterType.Short => true,
					_ => throw new IndexOutOfRangeException()
				}
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

	public enum EnterType {
		Long,
		Short
	}
}
