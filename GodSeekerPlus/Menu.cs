using Satchel.BetterMenus;

using Lang = Language.Language;

namespace GodSeekerPlus;

public sealed partial class GodSeekerPlus : ICustomMenuMod {
	bool ICustomMenuMod.ToggleButtonInsideMenu => true;

	private static string[] States => new string[] {
		Lang.Get("MOH_OFF", "MainMenu"),
		Lang.Get("MOH_ON", "MainMenu")
	};

	public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates) {
		var menu = new Menu("ModName".Localize(), new[] {
			toggleDelegates!.Value.CreateToggle(
				"ModName".Localize(),
				"ToggleButtonDesc".Localize(),
				States[1],
				States[0]
			)
		});

		ModuleManager!
			.Modules
			.Values
			.Filter(module => !module.Hidden)
			.GroupBy(module => module.Category)
			.OrderBy(group => group.Key)
			.Map(group => Blueprints.NavigateToMenu(
				$"Categories/{group.Key}".Localize(),
				"",
				() => BuildSubMenu(menu.menuScreen, group.Key, group)
			))
			.ForEach(menu.AddElement);

		return menu.GetMenuScreen(modListMenu);
	}

	private MenuScreen BuildSubMenu(MenuScreen parent, string name, IEnumerable<Module> modules) => new Menu(
		$"Categories/{name}".Localize(),
		modules.Map(module => new HorizontalOption(
			$"Modules/{module.Name}".Localize(),
			$"ToggleableLevel/{module.ToggleableLevel}".Localize(),
			States,
			(val) => module.Enabled = Convert.ToBoolean(val),
			() => Convert.ToInt32(module.Enabled)
		))
		.ToArray()
	).GetMenuScreen(parent);
}
