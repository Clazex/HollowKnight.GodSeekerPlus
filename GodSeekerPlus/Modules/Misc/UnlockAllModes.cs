namespace GodSeekerPlus.Modules.Misc;

public sealed class UnlockAllModes : Module {
	public override bool DefaultEnabled => true;

	private protected override void Load() {
		Platform.ISharedData data = Platform.Current.EncryptedSharedData;
		data.SetInt("RecPermadeathMode", 1);
		data.SetInt("RecBossRushMode", 1);
	}
}
