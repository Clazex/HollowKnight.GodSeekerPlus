using System.Diagnostics.CodeAnalysis;

using Modding.Utils;

namespace GodSeekerPlus;

public static class ModuleManager {
	private static readonly Lazy<Dictionary<string, Module>> modules = new(() => Assembly
		.GetExecutingAssembly()
		.GetTypesSafely()
		.Filter(type => type.IsSubclassOf(typeof(Module)) && !type.IsAbstract)
		.OrderBy(type => type.FullName)
#if DEBUG
		.Filter(type => {
			if (type.GetConstructor(Type.EmptyTypes) == null) {
				LogError($"Default constructor not found on module type {type.FullName}");
				return false;
			}

			if (!type.IsSealed) {
				LogWarn($"Module type {type.FullName} is not sealed");
			}

			return true;
		})
		.Map(type => {
			try {
				return (Activator.CreateInstance(type) as Module)!;
			} catch {
				LogError($"Failed to initialize module {type.FullName}");
				return null!;
			}
		})
#else
		.Map(type => (Activator.CreateInstance(type) as Module)!)
#endif
		.ToDictionary(module => module.Name)
	);

	internal static Dictionary<string, Module> Modules => modules.Value;

	internal static void Load() => Modules.Values.ForEach(module => module.Active = true);

	internal static void Unload() => Modules.Values.ForEach(module => module.Active = false);

	public static Module GetModule<T>() where T : Module {
		_ = TryGetModule(typeof(T).Name, out Module? m);
		return m!;
	}

	public static bool TryGetModule(Type type, [NotNullWhen(true)] out Module? module) =>
		TryGetModule(type.Name, out module);

	public static bool TryGetModule(string name, [NotNullWhen(true)] out Module? module) {
		module = Modules.TryGetValue(name, out Module? m) ? m : null;
		return module != null;
	}

	public static bool TryGetLoadedModule<T>([NotNullWhen(true)] out T? module) where T : Module {
		bool ret = TryGetLoadedModule(typeof(T).Name, out Module? m);
		module = m as T;
		return ret;
	}

	public static bool TryGetLoadedModule(Type type, [NotNullWhen(true)] out Module? module) =>
		TryGetLoadedModule(type.Name, out module);

	public static bool TryGetLoadedModule(string name, [NotNullWhen(true)] out Module? module) {
		module = Modules.TryGetValue(name, out Module? m) && m.Loaded ? m : null;
		return module != null;
	}

	public static bool IsModuleLoaded<T>() where T : Module => TryGetLoadedModule<T>(out _);

	public static bool IsModuleLoaded(Type type) => TryGetLoadedModule(type, out _);

	public static bool IsModuleLoaded(string name) => TryGetLoadedModule(name, out _);
}
