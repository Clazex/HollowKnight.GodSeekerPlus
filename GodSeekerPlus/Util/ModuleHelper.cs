namespace GodSeekerPlus.Util;

internal static class ModuleHelper {
	internal static IEnumerable<Type> FindModules() => Assembly
		.GetExecutingAssembly()
		.GetTypes()
		.Filter(type => type.IsSubclassOf(typeof(Module)))
		.Filter(HasDefaultConstructor);

	internal static IEnumerable<string> GetModuleNames() => FindModules()
		.Filter(type => type.GetCustomAttribute<HiddenAttribute>() == null)
		.Map(type => type.Name);

	internal static Dictionary<string, bool> GetDefaultModuleStateDict() => FindModules()
		.Reduce((dict, type) => {
			if (type.GetCustomAttribute<HiddenAttribute>() == null) {
				dict[type.Name] = type.GetCustomAttribute<DefaultEnabledAttribute>() != null;
			}

			return dict;
		}, new Dictionary<string, bool>());

	internal static Module ConstructModule(Type type) => (Module) Activator.CreateInstance(type);


	private static bool HasDefaultConstructor(Type type) {
		if (type.GetConstructor(Type.EmptyTypes) == null) {
			Logger.LogError($"Default constructor not found on module type {type.Name}");
			return false;
		}

		return true;
	}
}
