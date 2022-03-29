namespace GodSeekerPlus.Modules.BossChallenge;

[Category(nameof(BossChallenge))]
[ToggleableLevel(ToggleableLevel.ChangeScene)]
internal sealed class P5Health : Module {
	private static readonly Type scalerType = typeof(HealthManager)
		.GetNestedType("HPScaleGG", BindingFlags.NonPublic);

	private static readonly FieldInfo scalerField = ReflectionHelper
		.GetFieldInfo(typeof(HealthManager), "hpScale");

	private static readonly FieldInfo lv1Field = ReflectionHelper
		.GetFieldInfo(scalerType, "level1");
	private static readonly FieldInfo lv2Field = ReflectionHelper
		.GetFieldInfo(scalerType, "level2");
	private static readonly FieldInfo lv3Field = ReflectionHelper
		.GetFieldInfo(scalerType, "level3");

	private protected override void Load() =>
		On.HealthManager.Start += NerfHP;

	private protected override void Unload() =>
		On.HealthManager.Start -= NerfHP;

	private void NerfHP(On.HealthManager.orig_Start orig, HealthManager self) {
		object scaler = scalerField.GetValue(self);

		object hp = lv1Field.GetValue(scaler);
		lv2Field.SetValue(scaler, hp);
		lv3Field.SetValue(scaler, hp);

		scalerField.SetValue(self, scaler);

		orig(self);
	}
}
