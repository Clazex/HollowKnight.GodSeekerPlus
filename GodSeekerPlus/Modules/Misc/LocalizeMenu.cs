using UnityEngine.UI;

namespace GodSeekerPlus.Modules.Misc;

[Hidden]
internal sealed class LocalizeMenu : Module {
	private Coroutine? coroutine = null;

	private protected override void Load() =>
		coroutine = GameManager.instance.StartCoroutine(WaitForTitle());

	private protected override void Unload() {
		GameManager.instance.StopCoroutine(coroutine);
		UIManager.EditMenus -= EditText;
	}

	private IEnumerator WaitForTitle() {
		yield return new WaitUntil(() => GameObject.Find("LogoTitle") != null);

		UIManager.EditMenus += EditText;
	}

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
				$"{GodSeekerPlus.UnsafeInstance.GetName()}_Settings"
			)!;

		btn.Child("Label")!.GetComponent<Text>().text =
			"ModName".Localize() + ' ' + "Settings".Localize();

		btn.Child("Description")!.GetComponent<Text>().text =
			'v' + MiscUtil.GetVersionWithHash();

		Logger.LogDebug("Menu localized");
	}
}
