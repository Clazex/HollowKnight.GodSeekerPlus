namespace GodSeekerPlus.Util;

internal static class ModuleHelper {
	internal static IEnumerable<Type> FindModules() => Assembly
		.GetExecutingAssembly()
		.GetTypes()
		.Filter(type => type.IsSubclassOf(typeof(Module)))
		.Filter(HasModuleAttribute)
		.Filter(HasDefaultConstructor);

	internal static IEnumerable<string> GetModuleNames() =>
		FindModules().Map(type => type.Name);

	internal static Dictionary<string, bool> GetDefaultModuleStateDict() => FindModules()
		.Reduce((dict, type) => {
			dict[type.Name] = type.GetCustomAttribute<ModuleAttribute>().defaultEnabled;
			return dict;
		}, new Dictionary<string, bool>());

	internal static Module ConstructModule(Type type) => (Module) type.GetConstructor(Type.EmptyTypes).Invoke(null);

	private static bool HasModuleAttribute(Type type) {
		if (type.GetCustomAttribute<ModuleAttribute>() == null) {
			Logger.LogError($"Module attribute not found on module type {type.Name}");
			return false;
		}

		return true;
	}

	private static bool HasDefaultConstructor(Type type) {
		if (type.GetConstructor(Type.EmptyTypes) == null) {
			Logger.LogError($"Default constructor not found on module type {type.Name}");
			return false;
		}

		return true;
	}
}
