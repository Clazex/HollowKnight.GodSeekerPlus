using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GodSeekerPlus.Util;
using Module = GodSeekerPlus.Modules.Module;

namespace GodSeekerPlus {
	internal sealed class ModuleManager {
		private readonly List<Module> modules = new();

		internal void LoadModules() => modules.InsertRange(0, Assembly
			.GetExecutingAssembly()
			.GetTypes()
			.Where(type => type.IsSubclassOf(typeof(Module)))
			.Select(type => {
				Logger.LogDebug($"Found module {type.Name}");
				return type;
			})
			.Select(type => type.GetConstructor(Type.EmptyTypes))
			.Select(ctor => ctor.Invoke(null))
			.Cast<Module>()
		);

		internal void UnloadModules() => modules.Clear();
	}
}
