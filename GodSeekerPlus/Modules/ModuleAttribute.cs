namespace GodSeekerPlus.Modules;

[AttributeUsage(AttributeTargets.Class)]
internal sealed class ModuleAttribute : Attribute {
	public ToggleableLevel toggleableLevel;
	public bool defaultEnabled;
}

internal enum ToggleableLevel {
	AnyTime,
	ReloadSave,
	RestartGame
}
