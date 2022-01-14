namespace GodSeekerPlus.Util;

internal static class IEnumerableUtil {
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static IEnumerable<U> Map<T, U>(this IEnumerable<T> self, Func<T, U> f) =>
		self.Select(f);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static IEnumerable<T> Filter<T>(this IEnumerable<T> self, Func<T, bool> f) =>
		self.Where(f);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static U Reduce<T, U>(this IEnumerable<T> self, Func<U, T, U> f, U init) {
		U acc = init;
		foreach (T i in self) {
			acc = f(acc, i);
		}
		return acc;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void ForEach<T>(this IEnumerable<T> self, Action<T> f) {
		foreach (T i in self) {
			f(i);
		}
	}
}
