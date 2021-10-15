namespace GodSeekerPlus {
	internal static class Logger {
		public static void Log(string message) => GodSeekerPlus.Instance?.Log(message);
		public static void Log(object message) => GodSeekerPlus.Instance?.Log(message);
		public static void LogDebug(string message) => GodSeekerPlus.Instance?.LogDebug(message);
		public static void LogDebug(object message) => GodSeekerPlus.Instance?.LogDebug(message);
		public static void LogError(string message) => GodSeekerPlus.Instance?.LogError(message);
		public static void LogError(object message) => GodSeekerPlus.Instance?.LogError(message);
		public static void LogFine(string message) => GodSeekerPlus.Instance?.LogFine(message);
		public static void LogFine(object message) => GodSeekerPlus.Instance?.LogFine(message);
		public static void LogWarn(string message) => GodSeekerPlus.Instance?.LogWarn(message);
		public static void LogWarn(object message) => GodSeekerPlus.Instance?.LogWarn(message);
	}
}
