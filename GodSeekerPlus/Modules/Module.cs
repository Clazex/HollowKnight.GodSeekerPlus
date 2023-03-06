namespace GodSeekerPlus.Modules;

[MeansImplicitUse]
[UsedImplicitly]
public abstract class Module {
	public Type Type { get; private init; }

	public string Name { get; private init; }

	public string Category { get; private init; }

	public virtual bool DefaultEnabled => false;

	public virtual ToggleableLevel ToggleableLevel => ToggleableLevel.AnyTime;

	public virtual bool Hidden => false;

	public bool Loaded { get; private set; }

	private bool enabled;

	private bool active;


	internal Module() {
		Type = GetType();
		Name = Type.Name;
		Category = Type.FullName
			.StripStart($"{nameof(GodSeekerPlus)}.{nameof(Modules)}.")
			.StripEnd($".{Name}")
		?? nameof(Misc);
		enabled = DefaultEnabled;
	}

	public bool Enabled {
		get => enabled || Hidden;
		internal set {
			enabled = Setting.Global.Modules[Name] = value;
			UpdateStatus();
		}
	}

	internal bool Active {
		get => active;
		set {
			active = value;
			UpdateStatus();
		}
	}

	private void UpdateStatus() {
		if (Active && Enabled) {
			if (!Loaded) {
				try {
					Load();
					LogDebug($"Activated module {Name}");
					Loaded = true;
				} catch (Exception e) {
					LogError($"Failed to activate module {Name} - {e}");
				}
			}
		} else {
			if (Loaded) {
				Unload();
				LogDebug($"Deactivated module {Name}");
				Loaded = false;
			}
		}
	}

	private protected virtual void Load() { }

	private protected virtual void Unload() { }
}
