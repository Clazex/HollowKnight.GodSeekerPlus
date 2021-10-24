using System.Reflection;
using GodSeekerPlus.Util;

namespace GodSeekerPlus.Modules {
	internal abstract class Module {
		internal Module() {
			Update();
		}

		~Module() {
			DoUnload();
		}

		internal void Update() {
			if (ShouldLoad) {
				DoLoad();
			} else {
				DoUnload();
			}
		}

		private bool Loaded { get; set; } = false;

		internal bool Toggleable => GetType().GetCustomAttribute<ModuleAttribute>().toggleable;

		private bool ShouldLoad => GodSeekerPlus.Instance.GlobalSettings.modules[GetType().Name];

		private void DoLoad() {
			if (!Loaded) {
				Load();
				Logger.LogDebug($"Loaded module {GetType().Name}");
				Loaded = true;
			}
		}

		private void DoUnload() {
			if (Loaded) {
				Unload();
				Logger.LogDebug($"Unloaded module {GetType().Name}");
				Loaded = false;
			}
		}


		private protected abstract void Load();

		private protected abstract void Unload();
	}
}
