using System.Diagnostics.CodeAnalysis;

using Modding.Utils;

namespace GodSeekerPlus;

internal static class ModuleManager {
	private static readonly Lazy<Dictionary<string, Module>> modules = new(() => Assembly
		.GetExecutingAssembly()
		.GetTypesSafely()
		.Filter(type => type.IsSubclassOf(typeof(Module)) && !type.IsAbstract)
		.OrderBy(type => type.FullName)
#if DEBUG
		.Filter(type => {
			if (type.GetConstructor(Type.EmptyTypes) == null) {
				Logger.LogError($"Default constructor not found on module type {type.FullName}");
				return false;
			}

			if (!type.IsSealed) {
				Logger.LogWarn($"Module type {type.FullName} is not sealed");
			}

			return true;
		})
		.Map(type => {
			try {
				return (Activator.CreateInstance(type) as Module)!;
			} catch {
				Logger.LogError($"Failed to initialize module {type.FullName}");
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

	public static bool TryGetModule<T>([NotNullWhen(true)] out T? module) where T : Module {
		bool ret = TryGetModule(typeof(T).Name, out Module? m);
		module = m as T;
		return ret;
	}

	public static bool TryGetModule(Type type, [NotNullWhen(true)] out Module? module) =>
		TryGetModule(type.Name, out module);

	public static bool TryGetModule(string name, [NotNullWhen(true)] out Module? module) {
		module = Modules.TryGetValue(name, out Module? m) ? m : null;
		return module != null;
	}

	public static bool TryGetEnabledModule<T>([NotNullWhen(true)] out T? module) where T : Module {
		bool ret = TryGetEnabledModule(typeof(T).Name, out Module? m);
		module = m as T;
		return ret;
	}

	public static bool TryGetEnabledModule(Type type, [NotNullWhen(true)] out Module? module) =>
		TryGetEnabledModule(type.Name, out module);

	public static bool TryGetEnabledModule(string name, [NotNullWhen(true)] out Module? module) {
		module = Modules.TryGetValue(name, out Module? m) && m.Enabled ? m : null;
		return module != null;
	}

	public static bool IsModuleEnabled<T>() => IsModuleEnabled(typeof(T).Name);

	public static bool IsModuleEnabled(Type type) => IsModuleEnabled(type.Name);

	public static bool IsModuleEnabled(string name) => Modules.TryGetValue(name, out Module? m) && m.Enabled;
}
