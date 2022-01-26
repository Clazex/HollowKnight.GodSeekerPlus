using System.IO;

namespace GodSeekerPlus.Util;

internal static class MiscUtil {
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static int Clamp(int val, int min, int max) =>
		val < min ? min : (val > max ? max : val);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static float Clamp(float val, float min, float max) =>
		val < min ? min : (val > max ? max : val);


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static bool EnclosedWith(this string self, string start, string end) =>
		self.StartsWith(start) && self.EndsWith(end);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static string StripStart(this string self, string val) =>
		self.StartsWith(val) ? self.Substring(val.Length) : self;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static string StripEnd(this string self, string val) =>
		self.EndsWith(val) ? self.Substring(0, self.Length - val.Length) : self;


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static string ReadToString(this Stream self) =>
		new StreamReader(self).ReadToEnd();



	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static T Try<T>(Func<T> f, T @default) {
		try {
			return f();
		} catch {
			return @default;
		}
	}



	internal static GameObject? Child(this GameObject self, params string[] path) =>
		self.transform.Find(path.Aggregate((a, b) => $"{a}/{b}"))?.gameObject;
}
