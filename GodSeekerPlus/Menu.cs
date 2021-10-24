using System.Collections.Generic;
using System.Linq;
using GodSeekerPlus.Util;
using Modding;
using static Modding.IMenuMod;

namespace GodSeekerPlus {
	public sealed partial class GodSeekerPlus : IMenuMod {
		bool IMenuMod.ToggleButtonInsideMenu => false;

		List<MenuEntry> IMenuMod.GetMenuData(MenuEntry? _) {
			string[] states = {
				Language.Language.Get("MOH_OFF", "MainMenu"),
				Language.Language.Get("MOH_ON", "MainMenu")
			};

			return ModuleHelper
				.GetToggleableModuleNames()
				.Map<string, MenuEntry>(name => new(
					name,
					states,
					"",
					(val) => {
						Instance.GlobalSettings.modules[name] = val != 0;
						moduleManager.Modules[name].Update();
					},
					() => Instance.GlobalSettings.modules[name] ? 1 : 0
				))
				.ToList();
		}
	}
}
