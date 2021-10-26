using System;
using System.Collections.Generic;
using System.IO;

namespace GodSeekerPlus.Util {
	internal static class MiscUtil {
		internal static bool EnclosedWith(this string self, string start, string end) =>
			self.StartsWith(start) && self.EndsWith(end);

		internal static string StripStart(this string self, string val) =>
			self.StartsWith(val) ? self.Substring(val.Length) : self;

		internal static string StripEnd(this string self, string val) =>
			self.EndsWith(val) ? self.Substring(0, self.Length - val.Length) : self;


		internal static string ReadToString(this Stream self) =>
			new StreamReader(self).ReadToEnd();



		internal static T Try<T>(Func<T> f, T @default) {
			try {
				return f();
			} catch {
				return @default;
			}
		}


		internal static void Set(this List<PersistentBoolData> self, string sceneName, string id, bool activated, bool semiPersistent = false) {
			IEnumerable<PersistentBoolData> items = self
				.Filter(item => item.sceneName == sceneName && item.id == id);

			if (items.Any()) {
				items.ForEach(item => {
					item.activated = true;
					item.semiPersistent = semiPersistent;
				});
			} else {
				self.Add(new() {
					sceneName = sceneName,
					id = id,
					activated = activated,
					semiPersistent = semiPersistent
				});
			}
		}
	}
}
