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

	internal static readonly Dictionary<int, (string suppressor, Module[] modules)> suppressions = [];
	private static int lastSuppressionHandle = 0;

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


	public static int SuppressModules(string suppressor, params Module[] modules) {
		int handle = ++lastSuppressionHandle;

		suppressions.Add(handle, (suppressor, modules));

		foreach (Module module in modules) {
			module.suppressorMap.Add(handle, suppressor);
			module.UpdateStatus();
		}

		Log(suppressor + " starts to suppress modules " + modules.Map(m => m.Name).Join(", ") + " with handle " + handle);

		return handle;
	}

	public static int SuppressModule<T>(string suppressor) where T : Module =>
		SuppressModules(suppressor, GetModule<T>());

	public static int SuppressModules(string suppressor, params string[] modules) =>
		SuppressModules(suppressor, modules.Map(name => TryGetModule(name, out Module? m)
			? m
			: throw new InvalidOperationException("Unknown module " + name)
		).ToArray());

	public static void CancelSuppression(int handle) {
		if (!suppressions.TryGetValue(handle, out (string suppressor, Module[] modules) suppression)) {
			LogError("Failed attempt to end unknown suppresion with handle " + handle);
			return;
		}

		_ = suppressions.Remove(handle);
		(string suppressor, Module[] modules) = suppression;

		foreach (Module module in modules) {
			_ = module.suppressorMap.Remove(handle);
			module.UpdateStatus();
		}

		Log(suppressor + " end to suppress modules " + modules.Map(m => m.Name).Join(", ") + " with handle " + handle);
	}
}
