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
public enum OptionType {
	Option,
	Slider,
}

[PublicAPI]
[AttributeUsage(AttributeTargets.Field)]
internal abstract class OptionAttribute : Attribute {
}

[PublicAPI]
[AttributeUsage(AttributeTargets.Field)]
internal sealed class BoolOptionAttribute : OptionAttribute {
}

[PublicAPI]
[AttributeUsage(AttributeTargets.Field)]
internal sealed class IntOptionAttribute : OptionAttribute {
	internal int[] Options { get; private init; }
	internal OptionType Type { get; private init; } = OptionType.Option;

	internal IntOptionAttribute(int start, int stop, int step = 1) {
		List<int> options = new();

		for (int i = start; i < stop; i += step) {
			options.Add(i);
		}

		options.Add(stop);

		Options = options.ToArray();
	}

	internal IntOptionAttribute(int start, int stop, OptionType type) {
		Options = new[] { start, stop };
		Type = type;
	}

	internal IntOptionAttribute(params int[] options) => Options = options;
}

[PublicAPI]
[AttributeUsage(AttributeTargets.Field)]
internal sealed class FloatOptionAttribute : OptionAttribute {
	internal float[] Options { get; private init; }
	internal OptionType Type { get; private init; } = OptionType.Option;

	internal FloatOptionAttribute(float start, float stop, float step) {
		List<float> options = new();

		decimal decimalStop = (decimal) stop, decimalStep = (decimal) step;
		for (decimal i = (decimal) start; i < decimalStop; i += decimalStep) {
			options.Add(decimal.ToSingle(i));
		}

		options.Add(stop);

		Options = options.ToArray();
	}

	internal FloatOptionAttribute(float start, float stop, OptionType type) {
		Options = new[] { start, stop };
		Type = type;
	}

	internal FloatOptionAttribute(params float[] options) => Options = options;
}

[PublicAPI]
[AttributeUsage(AttributeTargets.Field)]
internal sealed class EnumOptionAttribute : OptionAttribute {
}

[PublicAPI]
[AttributeUsage(AttributeTargets.Field)]
internal sealed class ReloadOnUpdateAttribute : Attribute {
}
