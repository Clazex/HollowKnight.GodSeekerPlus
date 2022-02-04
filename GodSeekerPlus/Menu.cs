using Modding.Menu;
using Modding.Menu.Config;

using Satchel.BetterMenus;

namespace GodSeekerPlus;

public sealed partial class GodSeekerPlus : ICustomMenuMod {
	bool ICustomMenuMod.ToggleButtonInsideMenu => true;

	public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates) =>
		satchelPresent
			? ModMenu.GetSatchelMenuScreen(modListMenu, toggleDelegates)
			: ModMenu.GetFallbackMenuScreen(modListMenu, toggleDelegates);

	private static class ModMenu {
		private static string[] StateStrings => new string[] {
			Lang.Get("MOH_OFF", "MainMenu"),
			Lang.Get("MOH_ON", "MainMenu")
		};

		internal static MenuScreen GetSatchelMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates) {
			Menu menu = new("ModName".Localize(), new[] {
				toggleDelegates!.Value.CreateToggle(
					"ModName".Localize(),
					"ToggleButtonDesc".Localize(),
					StateStrings[1],
					StateStrings[0]
				)
			});

			ModuleManager
				.Modules
				.Values
				.Filter(module => !module.Hidden)
				.GroupBy(module => module.Category)
				.OrderBy(group => group.Key)
				.OrderBy(group => group.Key == nameof(Modules.Misc))
				.Map(group => Blueprints.NavigateToMenu(
					$"Categories/{group.Key}".Localize(),
					"",
					() => new Menu(
						$"Categories/{group.Key}".Localize(),
						group.Map(module => new HorizontalOption(
							$"Modules/{module.Name}".Localize(),
							$"ToggleableLevel/{module.ToggleableLevel}".Localize(),
							StateStrings,
							(val) => module.Active = Convert.ToBoolean(val),
							() => Convert.ToInt32(module.Active)
						))
						.ToArray()
					).GetMenuScreen(menu.menuScreen)
				))
				.ForEach(menu.AddElement);

			return menu.GetMenuScreen(modListMenu);
		}

		internal static MenuScreen GetFallbackMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates) {
			Logger.LogWarn("Satchel not found, using fallback menu");

			MenuBuilder builder = MenuUtils.CreateMenuBuilderWithBackButton("ModName".Localize(), modListMenu, out _);
			var toggler = (ModToggleDelegates) toggleDelegates!;

			builder.AddContent(
				RegularGridLayout.CreateVerticalLayout(105f),
				c => {
					c.AddHorizontalOption(
						"ModName".Localize(),
						new() {
							ApplySetting = (_, i) => toggler.SetModEnabled(Convert.ToBoolean(i)),
							RefreshSetting = (settings, _) =>
								settings.optionList.SetOptionTo(Convert.ToInt32(toggler.GetModEnabled())),
							CancelAction = _ => UIManager.instance.GoToDynamicMenu(modListMenu),
							Description = null,
							Label = "ModName".Localize(),
							Options = StateStrings,
							Style = HorizontalOptionStyle.VanillaStyle
						},
						out UnityEngine.UI.MenuOptionHorizontal toggle
					);
					toggle.menuSetting.RefreshValueFromGameSettings();

					c.AddTextPanel(
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
