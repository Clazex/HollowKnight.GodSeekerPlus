namespace GodSeekerPlus.Modules;

[Category("BossChallenge")]
[ToggleableLevel(ToggleableLevel.ChangeScene)]
internal sealed class P5Health : Module {
	private static readonly Type scalerType = typeof(HealthManager)
		.GetNestedType("HPScaleGG", BindingFlags.NonPublic);

	private static readonly FieldInfo scalerField = typeof(HealthManager)
		.GetField("hpScale", BindingFlags.Instance | BindingFlags.NonPublic);

	private static readonly FieldInfo lv1Field = scalerType.GetField("level1");
	private static readonly FieldInfo lv2Field = scalerType.GetField("level2");
	private static readonly FieldInfo lv3Field = scalerType.GetField("level3");

	private protected override void Load() =>
		On.HealthManager.Start += NerfHP;

	private protected override void Unload() =>
		On.HealthManager.Start -= NerfHP;

	private void NerfHP(On.HealthManager.orig_Start orig, HealthManager self) {
		object scaler = scalerField.GetValue(self);

		int hp = (int) lv1Field.GetValue(scaler);
		lv2Field.SetValue(scaler, hp);
		lv3Field.SetValue(scaler, hp);

		scalerField.SetValue(self, scaler);

		orig(self);
	}
}
