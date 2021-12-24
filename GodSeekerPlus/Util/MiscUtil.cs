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

	public static bool isGodHomeBossScene(string currentScene) {
		string[] exclusions = new string[]
		{
			"Waterways",
			"Atrium",
			"Lurker",
			"Pipeway",
			"Spa",
			"Unlock",
			"Workshop",
			"End_Sequence",
			"Atrium_Roof",
			"Blue_Room",
			"Engine",
			"Engine_Prime",
			"Engine_Root",
			"Entrance_Cutscene",
			"Land_of_Storms",
			"Boss_Door_Entrance",
			"Wyrm",
			"Unn",
			"Door_5_Finale",
			"Unlock_Wastes"
		};

		string[] sceneNameParts = currentScene.Split(new[] { '_' },2 );
		string scenePrefix = sceneNameParts[0];
		string sceneSuffix = sceneNameParts[1];

		return scenePrefix == "GG" && !exclusions.Contains(sceneSuffix);
	}



	internal static string GetVersion() {
		var asm = Assembly.GetExecutingAssembly();

		string version = asm
			.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
			.InformationalVersion;

#if DEBUG
		version += '+';
		using var hash = SHA1.Create();
		using FileStream stream = File.OpenRead(asm.Location);
		Math.Ceiling(stream.Length / 2f);
		version += BitConverter.ToString(hash.ComputeHash(stream), 0, 4)
			.Replace("-", "").Substring(0, 7).ToLowerInvariant();
#endif

		return version;
	}
}
