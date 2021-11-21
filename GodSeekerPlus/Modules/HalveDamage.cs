namespace GodSeekerPlus.Modules {
	[Module(toggleable = true, defaultEnabled = false)]
	internal sealed class HalveDamage : Module {
		private protected override void Load() =>
			ModHooks.TakeHealthHook += MakeDamageHalved;

		private protected override void Unload() =>
			ModHooks.TakeHealthHook -= MakeDamageHalved;

		private int MakeDamageHalved(int damage) =>
			(int) Math.Ceiling(damage / 2d);
	}
}
