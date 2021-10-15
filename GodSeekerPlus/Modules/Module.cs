namespace GodSeekerPlus.Modules {
	public abstract class Module {
		internal Module() {
			if (ShouldLoad()) {
				Load();
				GodSeekerPlus.Instance.Log($"Loaded module {GetType().Name}");
				Loaded = true;
			}
		}

		~Module() {
			if (Loaded) {
				Unload();
				GodSeekerPlus.Instance.Log($"Unloaded module {GetType().Name}");
				Loaded = false;
			}
		}

		public bool Loaded { get; protected set; }

		public abstract void Load();

		public abstract void Unload();

		public abstract bool ShouldLoad();
	}
}
