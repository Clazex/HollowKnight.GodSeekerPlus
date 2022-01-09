namespace GodSeekerPlus.Modules.Misc;

[DefaultEnabled]
internal sealed class UnlockAllModes : Module {
	private protected override void Load() {
		Platform.ISharedData data = Platform.Current.EncryptedSharedData;
		data.SetInt("RecPermadeathMode", 1);
		data.SetInt("RecBossRushMode", 1);
	}
}
