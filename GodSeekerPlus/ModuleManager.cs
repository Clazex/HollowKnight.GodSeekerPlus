namespace GodSeekerPlus;

internal static class ModuleManager {
	internal static Dictionary<string, Module> Modules { get; private set; } = ModuleHelper
		.FindModules()
		.Map(ModuleHelper.ConstructModule)
		.ToDictionary(module => module.Name);

	internal static void Load() =>
		Modules.Values.ForEach(m => m.Enable());

	internal static void Unload() =>
		Modules.Values.ForEach(m => m.Disable());


	internal static bool ModuleEnabled<T>() where T : Module
		=> Modules[typeof(T).Name].Active;
}
