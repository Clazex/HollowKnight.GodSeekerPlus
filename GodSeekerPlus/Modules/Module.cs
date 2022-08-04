namespace GodSeekerPlus.Modules;

[MeansImplicitUse]
[UsedImplicitly]
internal abstract class Module {
	#region Attribute Cache

	private Type? type = null;
	private string? name = null;
	private string? category = null;
	private ToggleableLevel? toggleableLevel = null;
	private bool? defaultEnabled = null;
	private bool? hidden = null;

	#endregion

	#region Attribute Getters

	internal Type Type => type ??= GetType();

	internal string Name => name ??= Type.Name;

	internal string Category => category ??=
		Type.FullName
			.StripStart(nameof(GodSeekerPlus) + '.' + nameof(Modules) + '.')
			.StripEnd('.' + Name)
		?? nameof(Misc);

	internal ToggleableLevel ToggleableLevel => toggleableLevel ??=
		Type.GetCustomAttribute<ToggleableLevelAttribute>()?.ToggleableLevel
		?? ToggleableLevel.AnyTime;

	internal bool DefaultEnabled => defaultEnabled ??= Attribute.IsDefined(Type, typeof(DefaultEnabledAttribute));

	internal bool Hidden => hidden ??= Attribute.IsDefined(Type, typeof(HiddenAttribute));

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
		get => Hidden || Setting.Global.Modules[Name];
		set {
			if (!Hidden) {
				Setting.Global.Modules[Name] = value;
				Update();
			}
		}
	}

	private void Activate() {
		if (!Loaded) {
			try {
				Load();
				Logger.LogDebug($"Activated module {Name}");
				Loaded = true;
			} catch (Exception e) {
				Logger.LogError($"Failed to activate module {Name}!");
				Logger.LogError(e.ToString());
			}
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
