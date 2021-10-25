using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace GodSeekerPlus.Util {
	internal static class LocalizationUtil {
		private static Dictionary<string, Dictionary<string, string>> ReadLangs() {
			string path = Assembly.GetExecutingAssembly().Location;
			path = Path.GetFullPath(path);
			path = Path.GetDirectoryName(path);
			path = Path.Combine(path, "lang");

			return Directory.GetFiles(path)
				.Filter(path => Path.GetExtension(path) == ".json")
				.Map(path => (Path.GetFileNameWithoutExtension(path), new StreamReader(path).ReadToEnd()))
				.Map(tuple => (tuple.Item1,
					(Dictionary<string, string>) JsonConvert.DeserializeObject(tuple.Item2, typeof(Dictionary<string, string>))
				))
				.Reduce(
					(dict, tuple) => {
						dict[tuple.Item1] = tuple.Item2;
						return dict;
					},
					new Dictionary<string, Dictionary<string, string>>()
				);
		}

		private static Dictionary<string, Dictionary<string, string>> Dict { get; set; } = ReadLangs();

		internal static string TryLocalize(string key) {
			try {
				return Dict[Language.Language.CurrentLanguage().ToString().ToLower().Replace('_', '-')][key];
			} catch {
				return key;
			}
		}
	}
}
