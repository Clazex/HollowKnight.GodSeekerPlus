using static Modding.IMenuMod;

using Lang = Language.Language;

namespace GodSeekerPlus;

public sealed partial class GodSeekerPlus : IMenuMod {
	bool IMenuMod.ToggleButtonInsideMenu => true;

	private static string[] States => new string[] {
		Lang.Get("MOH_OFF", "MainMenu"),
		Lang.Get("MOH_ON", "MainMenu")
	};

	List<MenuEntry> IMenuMod.GetMenuData(MenuEntry? toggleButton) => ModuleHelper
		.GetModuleNames()
		.Map(name => new MenuEntry(
			$"Modules/{name}".Localize(),
			States,
			$"ToggleableLevel/{ModuleManager!.Modules[name].ToggleableLevel}".Localize(),
			(val) => ModuleManager!.Modules[name].Enabled = Convert.ToBoolean(val),
			() => Convert.ToInt32(ModuleManager!.Modules[name].Enabled)
		))
		.Prepend((MenuEntry) toggleButton! with {
			Name = "ModName".Localize(),
			Values = States,
			Description = "ToggleButtonDesc".Localize()
		})
		.ToList();
}
