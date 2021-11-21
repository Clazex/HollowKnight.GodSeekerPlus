
using Language;

using Newtonsoft.Json;

namespace GodSeekerPlus.Util;

internal static class L11nUtil {
	private const string resPrefix = "GodSeekerPlus.Lang.";
	private const string resPostfix = ".json";


	private static readonly List<string> langs = Language.Language
		.GetLanguages()
		.Map(str => str.ToLower().Replace('_', '-'))
		.ToList();

	private static string ToIdentifier(this LanguageCode code) =>
		code.ToString().ToLower().Replace('_', '-');

	private static string CurrentLang =>
		Language.Language.CurrentLanguage().ToIdentifier();


	private static Dictionary<string, Dictionary<string, string>> ReadLangs() => Assembly
		.GetExecutingAssembly()
		.GetManifestResourceNames()
		.Filter(name => name.EnclosedWith(resPrefix, resPostfix))
		.Map(name =>
			(lang: name.StripStart(resPrefix).StripEnd(resPostfix), path: name)
		)
		.Filter(tuple => langs.Contains(tuple.lang))
		.Map(tuple => (
			tuple.lang,
			stream: Assembly.GetExecutingAssembly().GetManifestResourceStream(tuple.path)
		))
		.Map(tuple => (tuple.lang, json: tuple.stream.ReadToString()))
		.Map(tuple => (
			tuple.lang,
			table: JsonConvert.DeserializeObject<Dictionary<string, string>>(tuple.json)
		))
		.Reduce(
			(dict, tuple) => {
				Logger.LogDebug($"Loaded localization for lang: {tuple.lang}");
				dict[tuple.lang] = tuple.table;
				return dict;
			},
			new Dictionary<string, Dictionary<string, string>>()
		);

	private static Dictionary<string, Dictionary<string, string>> Dict { get; } = ReadLangs();

	internal static string Localize(string key) =>
		MiscUtil.Try(() => Dict[CurrentLang][key], key);
}
