namespace GodSeekerPlus.Util;

internal static class ModuleHelper {
	internal static IEnumerable<Type> FindModules() => Assembly
		.GetExecutingAssembly()
		.GetTypes()
		.Filter(type => type.IsSubclassOf(typeof(Module)))
		.Filter(HasModuleAttribute)
		.Filter(HasDefaultConstructor);

	internal static IEnumerable<string> GetModuleNames() => FindModules()
		.Filter(type => !type.GetCustomAttribute<ModuleAttribute>().hidden)
		.Map(type => type.Name);

	internal static Dictionary<string, bool> GetDefaultModuleStateDict() => FindModules()
		.Reduce((dict, type) => {
			ModuleAttribute attr = type.GetCustomAttribute<ModuleAttribute>();
			dict[type.Name] = attr.defaultEnabled || attr.hidden;
			return dict;
		}, new Dictionary<string, bool>());

	internal static Module ConstructModule(Type type) => (Module) Activator.CreateInstance(type);

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
