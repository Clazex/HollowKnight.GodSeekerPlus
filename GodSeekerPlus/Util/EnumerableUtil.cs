using System;
using System.Collections.Generic;

namespace GodSeekerPlus.Util {
	internal static class IEnumerableUtil {
		internal static bool Any<T>(this IEnumerable<T> self) =>
			System.Linq.Enumerable.Any(self);

		internal static IEnumerable<U> Map<T, U>(this IEnumerable<T> self, Func<T, U> f) {
			foreach (T i in self) {
				yield return f(i);
			}
		}

		internal static IEnumerable<T> Filter<T>(this IEnumerable<T> self, Predicate<T> predicate) {
			foreach (T i in self) {
				if (predicate(i)) {
					yield return i;
				}
			}
		}

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
