using System.Reflection;
using GodSeekerPlus.Util;

namespace GodSeekerPlus.Modules {
	internal abstract class Module {
		internal Module() {
			Update();
		}

		~Module() {
			Disable();
		}

		private bool Loaded { get; set; } = false;

		internal bool Toggleable => GetType().GetCustomAttribute<ModuleAttribute>().toggleable;

		internal bool Enabled => GodSeekerPlus.Instance.GlobalSettings.modules[GetType().Name];

		private void Enable() {
			if (!Loaded) {
				Load();
				Logger.LogDebug($"Loaded module {GetType().Name}");
				Loaded = true;
			}
		}

		private void Disable() {
			if (Loaded) {
				Unload();
				Logger.LogDebug($"Unloaded module {GetType().Name}");
				Loaded = false;
			}
		}

		internal void Update() {
			if (Enabled) {
				Enable();
			} else {
				Disable();
			}
		}



		private protected abstract void Load();

		private protected abstract void Unload();
	}
}
