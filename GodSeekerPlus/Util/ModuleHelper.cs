namespace GodSeekerPlus.Util;

internal static class ModuleHelper {
	internal static IEnumerable<Type> FindModules() => Assembly
		.GetExecutingAssembly()
		.GetTypes()
		.Filter(type => type.IsSubclassOf(typeof(Module)))
		.Filter(HasDefaultConstructor)
		.OrderBy(type => type.Name);

	internal static Dictionary<string, bool> GetDefaultModuleStateDict() => FindModules()
		.Filter(type => type.GetCustomAttribute<HiddenAttribute>() == null)
		.ToDictionary(type => type.Name, type => type.GetCustomAttribute<DefaultEnabledAttribute>() != null);

	internal static Module ConstructModule(Type type) => (Module) Activator.CreateInstance(type);


	private static bool HasDefaultConstructor(Type type) {
		if (type.GetConstructor(Type.EmptyTypes) == null) {
			Logger.LogError($"Default constructor not found on module type {type.Name}");
			return false;
		}

		return true;
	}
}
