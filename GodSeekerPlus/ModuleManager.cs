namespace GodSeekerPlus;

internal static class ModuleManager {
	internal static Dictionary<string, Module>? modules = null;

	internal static Dictionary<string, Module> Modules =>
		modules ??= InitModules();

	internal static void Load() =>
		Modules.Values.ForEach(m => m.Enable());

	internal static void Unload() =>
		Modules.Values.ForEach(m => m.Disable());

	internal static bool ModuleEnabled<T>() where T : Module
		=> Modules[typeof(T).Name].Active;


	internal static IEnumerable<Type> FindModules() => Assembly
		.GetExecutingAssembly()
		.GetTypes()
		.Filter(type => type.IsSubclassOf(typeof(Module)))
		.OrderBy(type => type.Name);

	private static Dictionary<string, Module> InitModules() => FindModules()
#if DEBUG
		.Filter(type => {
			if (type.IsAbstract) {
				Logger.LogError($"Module type {type.FullName} is abstract");
				return false;
			}

			if (type.GetConstructor(Type.EmptyTypes) == null) {
				Logger.LogError($"Default constructor not found on module type {type.FullName}");
				return false;
			}

			if (!type.IsSealed) {
				Logger.LogWarn($"Module type {type.FullName} is not sealed");
			}

			return true;
		})
#endif
		.Map(type => (Activator.CreateInstance(type) as Module)!)
		.ToDictionary(module => module.Name);
}
