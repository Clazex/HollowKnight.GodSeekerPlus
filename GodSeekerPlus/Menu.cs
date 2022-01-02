using static Modding.IMenuMod;

using Lang = Language.Language;

namespace GodSeekerPlus;

public sealed partial class GodSeekerPlus : IMenuMod {
	bool IMenuMod.ToggleButtonInsideMenu => false;

	private static string[] States => new string[] {
		Lang.Get("MOH_OFF", "MainMenu"),
		Lang.Get("MOH_ON", "MainMenu")
	};

	List<MenuEntry> IMenuMod.GetMenuData(MenuEntry? _) => ModuleHelper
		.GetModuleNames()
		.Map(name => new MenuEntry(
			L11nUtil.Localize($"Modules/{name}"),
			States,
			L11nUtil.Localize($"ToggleableLevel/{ModuleManager!.Modules[name].ToggleableLevel}"),
			(val) => ModuleManager!.Modules[name].Enabled = Convert.ToBoolean(val),
			() => Convert.ToInt32(ModuleManager!.Modules[name].Enabled)
		))
		.ToList();
}
