namespace GodSeekerPlus.Utils;

internal static class Logger {
	private static readonly SimpleLogger logger = new(nameof(GodSeekerPlus));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void Log(string message) => logger.Log(message);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if DEBUG
	internal static void LogDebug(string message) => logger.Log(message);
#else
	internal static void LogDebug(string message) => logger.LogDebug(message);
#endif

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void LogError(string message) => logger.LogError(message);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if DEBUG
	internal static void LogFine(string message) => logger.LogDebug(message);
#else
	internal static void LogFine(string message) => logger.LogFine(message);
#endif

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void LogWarn(string message) => logger.LogWarn(message);
}
