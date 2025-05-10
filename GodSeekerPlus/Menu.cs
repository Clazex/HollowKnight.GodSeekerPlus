using Satchel.BetterMenus;

using MenuButton = Satchel.BetterMenus.MenuButton;

namespace GodSeekerPlus;

public sealed partial class GodSeekerPlus : ICustomMenuMod {
	bool ICustomMenuMod.ToggleButtonInsideMenu => true;

	public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates) =>
		ModMenu.GetMenuScreen(modListMenu, toggleDelegates);

	private static class ModMenu {
		private static bool dirty = true;
		private static Menu? menu = null;

		static ModMenu() => On.Language.Language.DoSwitch += (orig, self) => {
			dirty = true;
			orig(self);
		};

		internal static MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates) {
			if (menu != null && !dirty) {
				return menu.GetMenuScreen(modListMenu);
			}

			menu = new("ModName".Localize(), [
				toggleDelegates!.Value.CreateToggle(
					"ModName".Localize(),
					"ToggleButtonDesc".Localize()
				)
			]);

			ModuleManager
				.Modules
				.Values
				.Filter(module => !module.Hidden)
				.GroupBy(module => module.Category)
				.OrderBy(group => group.Key == nameof(Modules.Misc))
				.ThenBy(group => group.Key)
				.Map(group => Blueprints.NavigateToMenu(
					$"Categories/{group.Key}".Localize(),
					"",
					() => new Menu(
						$"Categories/{group.Key}".Localize(),
						group.Map(module => Blueprints.HorizontalBoolOption(
							$"Modules/{module.Name}".Localize(),
							module.Suppressed
								? string.Format(
									"Suppression".Localize(),
									module.suppressorMap.Values.Distinct().Join(", ")
								)
								: $"ToggleableLevel/{module.ToggleableLevel}".Localize(),
							(val) => module.Enabled = val,
							() => module.Enabled
						))
						.Concat(Setting.Global.GetMenuElements(group.Key))
						.ToArray()
					).GetMenuScreen(menu.menuScreen)
				))
				.ForEach(menu.AddElement);

			menu.AddElement(new MenuButton(
				"ResetModules".Localize(),
				string.Empty,
				btn => ModuleManager.Modules.Values.ForEach(
					module => module.Enabled = module.DefaultEnabled
				),
				true
			));

			dirty = false;
			return menu.GetMenuScreen(modListMenu);
		}

	}
}
