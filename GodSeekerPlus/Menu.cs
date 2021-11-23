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
			moduleManager.Modules[name].Toggleable ? "" : L11nUtil.Localize("RequireRestart"),
			(val) => moduleManager.Modules[name].Enabled = Convert.ToBoolean(val),
			() => Convert.ToInt32(moduleManager.Modules[name].Enabled)
		))
		.ToList();
}
