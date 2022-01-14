namespace GodSeekerPlus.Util;

internal static class ModuleHelper {
	internal static IEnumerable<Type> FindModules() => Assembly
		.GetExecutingAssembly()
		.GetTypes()
		.Filter(type => type.IsSubclassOf(typeof(Module)))
#if DEBUG
		.Filter(HasDefaultConstructor)
#endif
		.OrderBy(type => type.Name);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static Dictionary<string, bool> GetDefaultModuleStateDict() => FindModules()
		.Filter(type => type.GetCustomAttribute<HiddenAttribute>() == null)
		.ToDictionary(type => type.Name, type => type.GetCustomAttribute<DefaultEnabledAttribute>() != null);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static Module ConstructModule(Type type) => (Module) Activator.CreateInstance(type);

#if DEBUG
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool HasDefaultConstructor(Type type) {
		if (type.GetConstructor(Type.EmptyTypes) == null) {
			Logger.LogError($"Default constructor not found on module type {type.Name}");
			return false;
		}

		return true;
	}
#endif
}
