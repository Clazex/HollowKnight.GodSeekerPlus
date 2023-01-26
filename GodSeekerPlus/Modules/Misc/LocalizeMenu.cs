using UnityEngine.UI;

namespace GodSeekerPlus.Modules.Misc;

internal sealed class LocalizeMenu : Module {
	private static readonly Lazy<string> versionWithHash = AssemblyUtil
		.GetMyDefaultVersionWithHash();

	public override bool Hidden => true;

	private protected override void Load() =>
		OsmiHooks.MenuBuildHook += EditText;

	private static void EditText() {
		GameObject btn = UIManager
			.instance
			.UICanvas
			.gameObject
			.Child(
				"ModListMenu",
				"Content",
				"ScrollMask",
				"ScrollingPane",
				$"{nameof(GodSeekerPlus)}_Settings"
			)!;

		btn.Child("Label")!.GetComponent<Text>().text =
			"ModName".Localize() + ' ' + "Settings".Localize();

		btn.Child("Description")!.GetComponent<Text>().text =
			'v' + versionWithHash.Value;

		Logger.LogDebug("Menu localized");
	}
}
