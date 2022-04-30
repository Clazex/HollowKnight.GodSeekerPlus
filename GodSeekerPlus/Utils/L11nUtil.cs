namespace GodSeekerPlus.Utils;

internal static class L11nUtil {
	internal static readonly Dict dict = new(typeof(GodSeekerPlus).Assembly, "Resources.Lang");

	internal static string Localize(this string key) =>
		dict.Localize(key);
}
