namespace GodSeekerPlus.Util;

internal static class Logger {
	private static GodSeekerPlus Instance => GodSeekerPlus.UnsafeInstance;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void Log(string message) => Instance.Log(message);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if DEBUG
	internal static void LogDebug(string message) => Instance.Log(message);
#else
	internal static void LogDebug(string message) => Instance.LogDebug(message);
#endif

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void LogError(string message) => Instance.LogError(message);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if DEBUG
	internal static void LogFine(string message) => Instance.LogDebug(message);
#else
	internal static void LogFine(string message) => Instance.LogFine(message);
#endif

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void LogWarn(string message) => Instance.LogWarn(message);
}
