namespace GodSeekerPlus.Utils;

internal static class Logger {
	private static readonly SimpleLogger logger = new(nameof(GodSeekerPlus));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void Log(string message) => logger.Log(message);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void LogDebug(string message) =>
#if DEBUG
		logger.Log(message);
#else
		logger.LogDebug(message);
#endif

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void LogError(string message) => logger.LogError(message);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void LogFine(string message) =>
#if DEBUG
		logger.Log(message);
#else
		logger.LogFine(message);
#endif

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void LogWarn(string message) =>
#if DEBUG
		logger.LogError(message);
#else
		logger.LogWarn(message);
#endif
}
