namespace GodSeekerPlus.Modules;

internal abstract class Module : IDisposable {
	#region Attribute Cache

	private Type? type = null;
	private string? name = null;
	private string? category = null;
	private ToggleableLevel? toggleableLevel = null;
	private bool? hidden = null;

	#endregion


	internal Module() => Update();

	public void Dispose() => Disable();

	private bool Loaded { get; set; } = false;


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


	#region State Transitions

	internal bool Enabled {
		get => Hidden || GodSeekerPlus.UnsafeInstance.GlobalSettings.modules[Name];
		set {
			if (!Hidden) {
				GodSeekerPlus.UnsafeInstance.GlobalSettings.modules[Name] = value;
				Update();
			}
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
