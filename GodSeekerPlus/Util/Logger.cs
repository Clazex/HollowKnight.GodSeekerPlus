using L = Modding.Logger;

namespace GodSeekerPlus.Util;

internal static class Logger {
	private static readonly string prefix = $"[{nameof(GodSeekerPlus)}] - ";

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void Log(string message) => L.Log(prefix + message);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if DEBUG
	internal static void LogDebug(string message) => L.Log(prefix + message);
#else
	internal static void LogDebug(string message) => L.LogDebug(prefix + message);
#endif

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void LogError(string message) => L.LogError(prefix + message);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if DEBUG
	internal static void LogFine(string message) => L.LogDebug(prefix + message);
#else
	internal static void LogFine(string message) => L.LogFine(prefix + message);
#endif

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void LogWarn(string message) => L.LogWarn(prefix + message);
}
