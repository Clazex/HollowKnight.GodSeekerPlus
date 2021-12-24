namespace GodSeekerPlus.Modules;

[AttributeUsage(AttributeTargets.Class)]
internal sealed class ModuleAttribute : Attribute {
	public ToggleableLevel toggleableLevel;
	public bool defaultEnabled;
	public bool hidden = false;
}

internal enum ToggleableLevel {
	AnyTime,
	ReloadSave,
	RestartGame
}
