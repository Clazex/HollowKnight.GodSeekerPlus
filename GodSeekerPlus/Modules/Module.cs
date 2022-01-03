namespace GodSeekerPlus.Modules;

internal abstract class Module {
	#region Attribute Cache

	private Type type = null;
	private string name = null;
	private string category = null;
	private ToggleableLevel? toggleableLevel = null;

	#endregion


	internal Module() => Update();

	~Module() {
		Disable();
	}

	private bool Loaded { get; set; } = false;


	#region Attribute Getters

	internal Type Type => type ??= GetType();

	internal string Name => name ??= Type.Name;

	internal string Category => category ??=
		Type.GetCustomAttribute<CategoryAttribute>()?.Name
		?? "Misc";

	internal ToggleableLevel ToggleableLevel => toggleableLevel ??=
		Type.GetCustomAttribute<ToggleableLevelAttribute>()?.ToggleableLevel
		?? ToggleableLevel.AnyTime;

	#endregion


	#region State Transitions

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

	#endregion


	private protected virtual void Load() { }

	private protected virtual void Unload() { }
}
