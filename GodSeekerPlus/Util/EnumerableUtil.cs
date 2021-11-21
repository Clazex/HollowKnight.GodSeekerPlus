using Enumerable = System.Linq.Enumerable;

namespace GodSeekerPlus.Util {
	internal static class IEnumerableUtil {
		internal static IEnumerable<U> Map<T, U>(this IEnumerable<T> self, Func<T, U> f) =>
			Enumerable.Select(self, f);

		internal static IEnumerable<T> Filter<T>(this IEnumerable<T> self, Func<T, bool> f) =>
			Enumerable.Where(self, f);

		internal static U Reduce<T, U>(this IEnumerable<T> self, Func<U, T, U> f, U init) {
			U acc = init;
			foreach (T i in self) {
				acc = f(acc, i);
			}
			return acc;
		}

		internal static void ForEach<T>(this IEnumerable<T> self, Action<T> f) {
			foreach (T i in self) {
				f(i);
			}
		}
	}
}
