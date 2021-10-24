using GodSeekerPlus.Util;

namespace GodSeekerPlus.Modules {
	internal abstract class Module {
		internal Module() {
			if (ShouldLoad()) {
				Load();
				Logger.LogDebug($"Loaded module {GetType().Name}");
				Loaded = true;
			}
		}

		~Module() {
			if (Loaded) {
				Unload();
				Logger.LogDebug($"Unloaded module {GetType().Name}");
				Loaded = false;
			}
		}

		private bool Loaded { get; set; } = false;

		private protected abstract void Load();

		private protected abstract void Unload();

		private protected abstract bool ShouldLoad();
	}
}
