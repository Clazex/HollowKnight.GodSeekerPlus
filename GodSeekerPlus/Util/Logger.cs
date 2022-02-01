namespace GodSeekerPlus.Util;

internal static class Logger {
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void Log(string message) => Ref.GSP.Log(message);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if DEBUG
	internal static void LogDebug(string message) => Ref.GSP.Log(message);
#else
	internal static void LogDebug(string message) => Ref.GSP.LogDebug(message);
#endif

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void LogError(string message) => Ref.GSP.LogError(message);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if DEBUG
	internal static void LogFine(string message) => Ref.GSP.LogDebug(message);
#else
	internal static void LogFine(string message) => Ref.GSP.LogFine(message);
#endif

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void LogWarn(string message) => Ref.GSP.LogWarn(message);
}
