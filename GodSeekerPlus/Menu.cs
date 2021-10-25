using System.Collections.Generic;
using System.Linq;
using GodSeekerPlus.Util;
using Modding;
using static Modding.IMenuMod;
using Lang = Language.Language;

namespace GodSeekerPlus {
	public sealed partial class GodSeekerPlus : IMenuMod {
		bool IMenuMod.ToggleButtonInsideMenu => false;

		private static string[] States { get; } = new string[] {
			Lang.Get("MOH_OFF", "MainMenu"),
			Lang.Get("MOH_ON", "MainMenu")
		};

		private string GetLocalizedName(string name) {
			string localizedName = Lang.Get(name, GetName());
			return localizedName.Any() ? localizedName : name;
		}

		List<MenuEntry> IMenuMod.GetMenuData(MenuEntry? _) => ModuleHelper
			.GetToggleableModuleNames()
			.Map(name => new MenuEntry(
				GetLocalizedName(name),
				States,
				"",
				(val) => moduleManager.Modules[name].Enabled = val != 0,
				() => moduleManager.Modules[name].Enabled ? 1 : 0
			))
			.ToList();
	}
}
