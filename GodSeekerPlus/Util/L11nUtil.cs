using System.IO;

using Language;

namespace GodSeekerPlus.Util;

internal static class L11nUtil {
	private const string resPrefix = "GodSeekerPlus.Resources.Lang.";
	private const string resPostfix = ".json";

	private static readonly string[] langs = Lang
		.GetLanguages()
		.Map(str => str.ToLower().Replace('_', '-'))
		.ToArray();


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static string ToIdentifier(this LanguageCode code) =>
		code.ToString().ToLower().Replace('_', '-');

	private static string CurrentLang =>
		Lang.CurrentLanguage().ToIdentifier();


	internal static readonly Lazy<Dictionary<string, Lazy<Dictionary<string, string>>>> Dict = new(() => Assembly
		.GetExecutingAssembly()
		.GetManifestResourceNames()
		.Filter(name => name.EnclosedWith(resPrefix, resPostfix))
		.Map(name => (
			lang: name.StripStart(resPrefix).StripEnd(resPostfix),
			path: name
		))
		.Filter(tuple => langs.Contains(tuple.lang))
		.ToDictionary(
			tuple => tuple.lang,
			tuple => new Lazy<Dictionary<string, string>>(() => {
				using Stream stream = Assembly
					.GetExecutingAssembly()
					.GetManifestResourceStream(tuple.path);
				using StreamReader reader = new(stream);

				string content = reader.ReadToEnd();
				Dictionary<string, string> table =
					JsonConvert.DeserializeObject<Dictionary<string, string>>(content)!;

				Logger.LogDebug($"Loaded localization for lang: {tuple.lang}");
				return table;
			})
		)
	);


	internal static string Localize(this string key) {
		if (Dict.Value.TryGetValue(CurrentLang, out Lazy<Dictionary<string, string>> table)) {
			if (table.Value.TryGetValue(key, out string value)) {
				return value;
			}
		}

		return key;
	}
}
