using System.Text;

namespace GodSeekerPlus.Util;

internal static class GameObjectUtil {
	internal static GameObject? Child(this GameObject self, string name) =>
		self.transform.Find(name)?.gameObject;

	internal static GameObject? Child(this GameObject self, params string[] path) {
		using IEnumerator<string> enumerator = path.AsEnumerable().GetEnumerator();

		enumerator.MoveNext();
		StringBuilder builder = new(enumerator.Current);

		while (enumerator.MoveNext()) {
			builder.Append('/');
			builder.Append(enumerator.Current);
		}

		return self.transform.Find(builder.ToString())?.gameObject;
	}

	internal static IEnumerable<GameObject> GetChildren(this GameObject self) {
		foreach (Transform child in self.transform) {
			yield return child.gameObject;
		}
	}
}
