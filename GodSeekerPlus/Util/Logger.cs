namespace GodSeekerPlus.Util {
	internal static class Logger {
		private static GodSeekerPlus Instance => GodSeekerPlus.Instance;

		internal static void Log(string message) => Instance?.Log(message);
		internal static void Log(object message) => Instance?.Log(message);

#if DEBUG
		internal static void LogDebug(string message) => Instance?.Log(message);
		internal static void LogDebug(object message) => Instance?.Log(message);
#else
		internal static void LogDebug(string message) => Instance?.LogDebug(message);
		internal static void LogDebug(object message) => Instance?.LogDebug(message);
#endif

		internal static void LogError(string message) => Instance?.LogError(message);
		internal static void LogError(object message) => Instance?.LogError(message);


#if DEBUG
		internal static void LogFine(string message) => Instance?.LogDebug(message);
		internal static void LogFine(object message) => Instance?.LogDebug(message);
#else
		internal static void LogFine(string message) => Instance?.LogFine(message);
		internal static void LogFine(object message) => Instance?.LogFine(message);
#endif


		internal static void LogWarn(string message) => Instance?.LogWarn(message);
		internal static void LogWarn(object message) => Instance?.LogWarn(message);
	}
}
