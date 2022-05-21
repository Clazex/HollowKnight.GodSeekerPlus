namespace GodSeekerPlus.Modules.Restrictions;

internal sealed class NoSpellDamage : Module {
	private protected override void Load() =>
		On.HealthManager.TakeDamage += AddHealth;

	private protected override void Unload() =>
		On.HealthManager.TakeDamage -= AddHealth;

	private void AddHealth(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance) {
		AttackTypes type = hitInstance.AttackType;
		if (type is AttackTypes.Spell or AttackTypes.SharpShadow) {
			self.hp += hitInstance.DamageDealt;
		}

		orig(self, hitInstance);
	}
}
