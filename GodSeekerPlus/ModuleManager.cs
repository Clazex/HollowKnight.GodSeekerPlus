using System.Collections.Generic;
using System.Linq;
using GodSeekerPlus.Modules;
using GodSeekerPlus.Util;

namespace GodSeekerPlus {
	internal sealed class ModuleManager {
		internal Dictionary<string, Module> Modules { get; private set; } = new();

		internal void LoadModules() => Modules = ModuleHelper
			.FindModules()
			.Map(ModuleHelper.ConstructModule)
			.ToDictionary(module => module.Name);

		internal void UnloadModules() => Modules.Clear();
	}
}
