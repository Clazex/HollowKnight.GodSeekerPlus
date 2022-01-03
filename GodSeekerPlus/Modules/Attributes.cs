namespace GodSeekerPlus.Modules;

[AttributeUsage(AttributeTargets.Class)]
internal sealed class CategoryAttribute : Attribute {
	public string Name { get; private init; }

	public CategoryAttribute(string name) =>
		Name = name;
}

[AttributeUsage(AttributeTargets.Class)]
internal sealed class ToggleableLevelAttribute : Attribute {
	public ToggleableLevel ToggleableLevel { get; private init; }

	public ToggleableLevelAttribute(ToggleableLevel toggleableLevel) =>
		ToggleableLevel = toggleableLevel;
}

[AttributeUsage(AttributeTargets.Class)]
internal sealed class DefaultEnabledAttribute : Attribute {
}

[AttributeUsage(AttributeTargets.Class)]
internal sealed class HiddenAttribute : Attribute {
}

internal enum ToggleableLevel {
	AnyTime,
	ChangeScene,
	ReloadSave,
	RestartGame
}
