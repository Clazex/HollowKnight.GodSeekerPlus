using UnityEngine;
using UnityEngine.UI;

namespace GodSeekerPlus.Modules;

[Hidden]
internal sealed class LocalizeMenu : Module {
	private protected override void Load() =>
		GameManager.instance.StartCoroutine(EditText());

	private IEnumerator EditText() {
		GameObject? btn = null;
		string btnName = $"{GodSeekerPlus.UnsafeInstance.GetName()}_Settings";

		yield return new WaitUntil(() => (btn = UIManager
			.instance
			.UICanvas
			.gameObject
			.ChildOrDefault(
				"ModListMenu",
				"Content",
				"ScrollMask",
				"ScrollingPane",
				btnName
			)) != null
		);

		btn!.Child("Label").GetComponent<Text>().text =
			"ModName".Localize() + ' ' + "Settings".Localize();

		btn!.Child("Description").GetComponent<Text>().text =
			'v' + GodSeekerPlus.UnsafeInstance.GetVersion();
	}
}
