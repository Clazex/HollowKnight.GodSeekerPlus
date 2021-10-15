using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GodSeekerPlus.Modules {
	internal static class ModuleManager {
		private static readonly List<Module> modules = new();

		public static void LoadModules() => modules.InsertRange(0, Assembly
			.GetExecutingAssembly()
			.GetTypes()
			.Where(type => type.IsSubclassOf(typeof(Module)))
			.Select(type => {
				GodSeekerPlus.Instance.Log($"Found module {type.Name}");
				return type;
			})
			.Select(type => type.GetConstructor(Type.EmptyTypes))
			.Select(ctor => ctor.Invoke(null))
			.Cast<Module>()
		);

		public static void UnloadModules() => modules.Clear();
	}
}
