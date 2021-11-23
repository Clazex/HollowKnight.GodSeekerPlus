namespace GodSeekerPlus.Modules;

internal abstract class Module {
	internal Module() => Update();

	~Module() {
		Disable();
	}

	internal string Name => GetType().Name;

	private bool Loaded { get; set; } = false;

	internal ToggleableLevel ToggleableLevel =>
		GetType().GetCustomAttribute<ModuleAttribute>().toggleableLevel;

	internal bool Enabled {
		get => GodSeekerPlus.Instance.GlobalSettings.modules[Name];
		set {
			GodSeekerPlus.Instance.GlobalSettings.modules[Name] = value;
			Update();
		}
	}

	private void Enable() {
		if (!Loaded) {
			Load();
			Logger.LogDebug($"Loaded module {Name}");
			Loaded = true;
		}
	}

	private void Disable() {
		if (Loaded) {
			Unload();
			Logger.LogDebug($"Unloaded module {Name}");
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
