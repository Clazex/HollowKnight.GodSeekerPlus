using Modding.Menu;
using Modding.Menu.Config;

using Satchel.BetterMenus;

using UnityEngine.UI;

using MenuButton = Satchel.BetterMenus.MenuButton;

namespace GodSeekerPlus;

public sealed partial class GodSeekerPlus : ICustomMenuMod {
	bool ICustomMenuMod.ToggleButtonInsideMenu => true;

	public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates) =>
		satchelPresent
			? ModMenu.GetSatchelMenuScreen(modListMenu, toggleDelegates)
			: ModMenu.GetFallbackMenuScreen(modListMenu, toggleDelegates);

	private static class ModMenu {
		internal static bool dirty = true;
		private static Menu? menu = null;

		static ModMenu() => On.Language.Language.DoSwitch += (orig, self) => {
			dirty = true;
			orig(self);
		};

		internal static MenuScreen GetSatchelMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates) {
			if (menu != null && !dirty) {
				return menu.GetMenuScreen(modListMenu);
			}

			menu = new("ModName".Localize(), new[] {
				toggleDelegates!.Value.CreateToggle(
					"ModName".Localize(),
					"ToggleButtonDesc".Localize()
				)
			});

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
							$"ToggleableLevel/{module.ToggleableLevel}".Localize(),
							(val) => module.Active = val,
							() => module.Active
						))
						.Concat(Setting.Global.GetMenuOptions(group.Key))
						.ToArray()
					).GetMenuScreen(menu.menuScreen)
				))
				.ForEach(menu.AddElement);

			menu.AddElement(new MenuButton(
				"ResetModules".Localize(),
				string.Empty,
				btn => ModuleManager.Modules.Values.ForEach(
					module => module.Active = module.DefaultEnabled
				),
				true
			));

			dirty = false;
			return menu.GetMenuScreen(modListMenu);
		}

		internal static MenuScreen GetFallbackMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates) {
			Logger.LogWarn("Satchel not found, using fallback menu");

			MenuBuilder builder = MenuUtils.CreateMenuBuilderWithBackButton("ModName".Localize(), modListMenu, out _);
			var toggler = (ModToggleDelegates) toggleDelegates!;

			_ = builder.AddContent(
				RegularGridLayout.CreateVerticalLayout(105f),
				c => {
					_ = c.AddHorizontalOption(
						"ModName".Localize(),
						new() {
							ApplySetting = (_, i) => toggler.SetModEnabled(Convert.ToBoolean(i)),
							RefreshSetting = (settings, _) =>
								settings.optionList.SetOptionTo(Convert.ToInt32(toggler.GetModEnabled())),
							CancelAction = _ => UIManager.instance.GoToDynamicMenu(modListMenu),
							Description = new() {
								Text = "ToggleButtonDesc".Localize()
							},
							Label = "ModName".Localize(),
							Options = new string[] {
								Lang.Get("MOH_OFF", "MainMenu"),
								Lang.Get("MOH_ON", "MainMenu")
							},
							Style = HorizontalOptionStyle.VanillaStyle
						},
						out MenuOptionHorizontal toggle
					);
					toggle.menuSetting.RefreshValueFromGameSettings();

					_ = c.AddTextPanel(
						"SatchelNotFoundPrompt",
						new RelVector2(new Vector2(1500f, 105f)),
						new() {
							Text = "SatchelNotFoundPrompt".Localize(),
							Anchor = TextAnchor.MiddleCenter,
							Font = TextPanelConfig.TextFont.TrajanBold,
							Size = 46,
						}
					);
				}
			);

			return builder.Build();
		}
	}
}
