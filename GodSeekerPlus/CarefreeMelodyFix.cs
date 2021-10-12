using Modding;

namespace GodSeekerPlus {
	internal static class CarefreeMelodyFix {
		private const string entry = "destroyedNightmareLantern";

		public static void Hook() => ModHooks.CharmUpdateHook += OnCharmUpdate;

		public static void UnHook() => ModHooks.CharmUpdateHook -= OnCharmUpdate;

		public static void OnCharmUpdate(PlayerData data, HeroController controller) {
			if (controller.carefreeShieldEquipped && !data.GetBool(entry)) {
				data.SetBool(entry, true);

				GodSeekerPlus.Instance.Log("Carefree Melody fixed");
			}
		}
	}
}
