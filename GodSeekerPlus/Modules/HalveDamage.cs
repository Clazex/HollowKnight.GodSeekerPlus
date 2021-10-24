using System;
using Modding;

namespace GodSeekerPlus.Modules {
	[Module(toggleable = true, defaultState = false)]
	internal sealed class HalveDamage : Module {
		private protected override void Load() => ModHooks.TakeHealthHook += MakeDamageHalved;

		private protected override void Unload() => ModHooks.TakeHealthHook -= MakeDamageHalved;

		private protected override bool ShouldLoad => GodSeekerPlus.Instance.GlobalSettings.halveDamage;

		private int MakeDamageHalved(int damage) => (int) Math.Ceiling(damage / 2d);
	}
}
