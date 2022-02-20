namespace GodSeekerPlus.Modules;

internal abstract class Module {
	#region Attribute Cache

	private Type? type = null;
	private string? name = null;
	private string? category = null;
	private ToggleableLevel? toggleableLevel = null;
	private bool? hidden = null;

	#endregion

	#region Attribute Getters

	internal Type Type => type ??= GetType();

	internal string Name => name ??= Type.Name;

	internal string Category => category ??=
		Type.GetCustomAttribute<CategoryAttribute>()?.Name
		?? nameof(Misc);

	internal ToggleableLevel ToggleableLevel => toggleableLevel ??=
		Type.GetCustomAttribute<ToggleableLevelAttribute>()?.ToggleableLevel
		?? ToggleableLevel.AnyTime;

	internal bool Hidden => hidden ??= Type.GetCustomAttribute<HiddenAttribute>() != null;

	#endregion


	internal Module() {
		if (Hidden) {
			Activate();
		}
	}

	private bool Loaded { get; set; } = false;

	#region Enabled State

	// Controls whether this module is "silent"
	internal bool Enabled { get; set; } = false;

	internal void Enable() {
		if (!Enabled) {
			Enabled = true;
			Update();
		}
	}

	internal void Disable() {
		if (Enabled) {
			Enabled = false;
			Deactivate();
		}
	}

	#endregion

	#region Activation State

	// Controls whether this module is "online", is a two-way data binding to settings
	internal bool Active {
		get => Hidden || Ref.GS.modules[Name];
		set {
			if (!Hidden) {
				Ref.GS.modules[Name] = value;
				Update();
			}
		}
	}

	private void Activate() {
		if (!Loaded) {
			Load();
			Logger.LogDebug($"Activated module {Name}");
			Loaded = true;
		}
	}

	private void Deactivate() {
		if (Loaded) {
			Unload();
			Logger.LogDebug($"Deactivated module {Name}");
			Loaded = false;
		}
	}

	internal void Update() {
		if (!Enabled || Hidden) {
			return;
		}

		if (Active) {
			Activate();
		} else {
			Deactivate();
		}
	}

	#endregion

	private protected virtual void Load() { }

	private protected virtual void Unload() { }
}
