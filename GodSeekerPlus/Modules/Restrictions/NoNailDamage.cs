namespace GodSeekerPlus.Modules.Restrictions;

[Category(nameof(Restrictions))]
internal sealed class NoNailDamage : Module {
	private protected override void Load() =>
		On.HealthManager.TakeDamage += AddHealth;

	private protected override void Unload() =>
		On.HealthManager.TakeDamage -= AddHealth;

	private void AddHealth(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance) {
		if (hitInstance.AttackType == AttackTypes.Nail) {
			self.hp += hitInstance.DamageDealt;
		}

		orig(self, hitInstance);
	}
}
