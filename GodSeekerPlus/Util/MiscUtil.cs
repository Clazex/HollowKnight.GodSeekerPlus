using System.IO;

#if DEBUG
using System.Security.Cryptography;
#endif

namespace GodSeekerPlus.Util;

internal static class MiscUtil {
	internal static int ForceInRange(int val, int min, int max) =>
		val < min ? min : (val > max ? max : val);

	internal static float ForceInRange(float val, float min, float max) =>
		val < min ? min : (val > max ? max : val);


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
				item.activated = activated;
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

	internal static BossStatue.Completion GetStatueCompletion(BossStatue statue) =>
		statue.UsingDreamVersion ? statue.DreamStatueState : statue.StatueState;

	internal static void SetStatueCompletion(BossStatue statue, BossStatue.Completion completion) {
		if (statue.UsingDreamVersion) {
			statue.DreamStatueState = completion;
		} else {
			statue.StatueState = completion;
		}
	}



	internal static string GetVersion() {
		var asm = Assembly.GetExecutingAssembly();

		string version = asm
			.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
			.InformationalVersion;

#if DEBUG
		version += '+';
		using (var hash = SHA1.Create()) {
			using FileStream stream = File.OpenRead(asm.Location);
			version += BitConverter.ToString(hash.ComputeHash(stream), 0, 3)
				.Replace("-", "").ToLowerInvariant();
		}
#endif

		return version;
	}
}
