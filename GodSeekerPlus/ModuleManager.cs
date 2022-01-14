namespace GodSeekerPlus;

internal sealed class ModuleManager : IDisposable {
	internal Dictionary<string, Module> Modules { get; private init; }

	public ModuleManager() => Modules = ModuleHelper
		.FindModules()
		.Map(ModuleHelper.ConstructModule)
		.ToDictionary(module => module.Name);

	public void Dispose() {
		Modules.Values.ForEach(m => m.Dispose());
		Modules.Clear();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal bool ModuleEnabled<T>() where T : Module
		=> Modules[typeof(T).Name].Enabled;
}
