namespace GodSeekerPlus.Settings;

[MeansImplicitUse]
[PublicAPI]
[AttributeUsage(AttributeTargets.Field)]
public sealed class GlobalSettingAttribute : Attribute {
}

[MeansImplicitUse]
[PublicAPI]
[AttributeUsage(AttributeTargets.Field)]
public sealed class LocalSettingAttribute : Attribute {
}

[PublicAPI]
[AttributeUsage(AttributeTargets.Field)]
internal abstract class OptionAttribute : Attribute {
}

	[PublicAPI]
[AttributeUsage(AttributeTargets.Field)]
internal sealed class BoolOptionAttribute : OptionAttribute {
	internal bool CustomFalseText { get; private init; }
	internal bool CustomTrueText { get; private init; }

	internal BoolOptionAttribute(bool customFalseText = false, bool customTrueText = false) {
		CustomFalseText = customFalseText;
		CustomTrueText = customTrueText;
	}
}

[PublicAPI]
[AttributeUsage(AttributeTargets.Field)]
internal sealed class IntOptionAttribute : OptionAttribute {
	internal int[] Options { get; private init; }

	internal IntOptionAttribute(int start, int stop, int step = 1) {
		List<int> options = new();

		for (int i = start; i < stop; i += step) {
			options.Add(i);
		}

		options.Add(stop);

		Options = options.ToArray();
	}

	internal IntOptionAttribute(params int[] options) => Options = options;
}

[PublicAPI]
[AttributeUsage(AttributeTargets.Field)]
internal sealed class FloatOptionAttribute : OptionAttribute {
	internal float[] Options { get; private init; }

	internal FloatOptionAttribute(float start, float stop, float step) {
		List<float> options = new();

		for (float i = start; i < stop; i += step) {
			options.Add(i);
		}

		options.Add(stop);

		Options = options.ToArray();
	}

	internal FloatOptionAttribute(params float[] options) => Options = options;
}
