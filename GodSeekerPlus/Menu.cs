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
		internal static bool dirty = true;
		private static Menu? menu = null;

		static ModMenu() => On.Language.Language.DoSwitch += (orig, self) => {
			dirty = true;
			orig(self);
		};



		private static string[] StateStrings => new string[] {
			Lang.Get("MOH_OFF", "MainMenu"),
			Lang.Get("MOH_ON", "MainMenu")
		};

		internal static MenuScreen GetSatchelMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates) {
			if (menu != null && !dirty) {
				return menu.GetMenuScreen(modListMenu);
			}

			menu = new("ModName".Localize(), new[] {
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
				.OrderBy(group => group.Key == nameof(Modules.Misc))
				.ThenBy(group => group.Key)
				.Map(group => Blueprints.NavigateToMenu(
					$"Categories/{group.Key}".Localize(),
					"",
					() => {
						Menu subMenu = new(
							$"Categories/{group.Key}".Localize(),
							group.Map(module => new HorizontalOption(
								$"Modules/{module.Name}".Localize(),
								$"ToggleableLevel/{module.ToggleableLevel}".Localize(),
								StateStrings,
								(val) => module.Active = Convert.ToBoolean(val),
								() => Convert.ToInt32(module.Active)
							))
							.ToArray()
						);

						if (Setting.Global.boolFields.TryGetValue(group.Key, out Dictionary<string, (FieldInfo fi, Func<bool> getter, Action<bool> setter, bool isOption)> boolFields)) {
							foreach (KeyValuePair<string, (FieldInfo fi, Func<bool> getter, Action<bool> setter, bool isOption)> pair in boolFields) {
								(string name, (FieldInfo fi, Func<bool> getter, Action<bool> setter, bool isOption)) = pair;

								if (!isOption) {
									continue;
								}

								BoolOptionAttribute optionAttr = fi.GetCustomAttribute<BoolOptionAttribute>();

								subMenu.AddElement(Blueprints.HorizontalBoolOption(
									$"Settings/{name}".Localize(),
									$"Modules/{fi.DeclaringType.Name}".Localize(),
									setter,
									getter,
									optionAttr.CustomTrueText ? $"Settings/{name}/True".Localize() : StateStrings[1],
									optionAttr.CustomFalseText ? $"Settings/{name}/False".Localize() : StateStrings[0]
								));
							}
						}

						if (Setting.Global.intFields.TryGetValue(group.Key, out Dictionary<string, (FieldInfo fi, Func<int> getter, Action<int> setter, bool isOption)> intFields)) {
							foreach (KeyValuePair<string, (FieldInfo fi, Func<int> getter, Action<int> setter, bool isOption)> pair in intFields) {
								(string name, (FieldInfo fi, Func<int> getter, Action<int> setter, bool isOption)) = pair;

								if (!isOption) {
									continue;
								}

								subMenu.AddElement(Blueprints.GenericHorizontalOption(
									$"Settings/{name}".Localize(),
									$"Modules/{fi.DeclaringType.Name}".Localize(),
									fi.GetCustomAttribute<IntOptionAttribute>().Options,
									setter,
									getter
								));
							}
						}

						if (Setting.Global.floatFields.TryGetValue(group.Key, out Dictionary<string, (FieldInfo fi, Func<float> getter, Action<float> setter, bool isOption)> floatFields)) {
							foreach (KeyValuePair<string, (FieldInfo fi, Func<float> getter, Action<float> setter, bool isOption)> pair in floatFields) {
								(string name, (FieldInfo fi, Func<float> getter, Action<float> setter, bool isOption)) = pair;

								if (!isOption) {
									continue;
								}

								subMenu.AddElement(Blueprints.GenericHorizontalOption(
									$"Settings/{name}".Localize(),
									$"Modules/{fi.DeclaringType.Name}".Localize(),
									fi.GetCustomAttribute<FloatOptionAttribute>().Options,
									setter,
									getter
								));
							}
						}

						return subMenu.GetMenuScreen(menu.menuScreen);
					}
				))
				.ForEach(menu.AddElement);

			menu.AddElement(new Satchel.BetterMenus.MenuButton(
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
							Options = StateStrings,
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
