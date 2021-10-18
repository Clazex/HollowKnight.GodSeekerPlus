namespace GodSeekerPlus {
	internal static class Logger {
		private static GodSeekerPlus Instance() => GodSeekerPlus.Instance;

		public static void Log(string message) => Instance()?.Log(message);
		public static void Log(object message) => Instance()?.Log(message);

#if DEBUG
		public static void LogDebug(string message) => Instance()?.Log(message);
		public static void LogDebug(object message) => Instance()?.Log(message);
#else
		public static void LogDebug(string message) => Instance()?.LogDebug(message);
		public static void LogDebug(object message) => Instance()?.LogDebug(message);
#endif

		public static void LogError(string message) => Instance()?.LogError(message);
		public static void LogError(object message) => Instance()?.LogError(message);


#if DEBUG
		public static void LogFine(string message) => Instance()?.LogDebug(message);
		public static void LogFine(object message) => Instance()?.LogDebug(message);
#else
		public static void LogFine(string message) => Instance()?.LogFine(message);
		public static void LogFine(object message) => Instance()?.LogFine(message);
#endif


		public static void LogWarn(string message) => Instance()?.LogWarn(message);
		public static void LogWarn(object message) => Instance()?.LogWarn(message);
	}
}
