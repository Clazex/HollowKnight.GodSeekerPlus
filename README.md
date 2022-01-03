# [God Seeker +](https://github.com/Clazex/HollowKnight.GodSeekerPlus)

[![Commitizen friendly](https://img.shields.io/badge/commitizen-friendly-brightgreen.svg)](http://commitizen.github.io/cz-cli/)

A Hollow Knight mod to enhance your Godhome experience

Compatible with `Hollow Knight` 1.5.
**`Satchel` is required.**

## Features

- **Boss Challenge**:
  + **Activate Fury**: Activate Fury of the Fallen on entry of boss scenes by setting health to 1.
  + **Add Lifeblood**: Add configured amount of lifeblood on entry of Hall of Gods boss fight.
  + **Add Soul**: Add configured amount of soul on entry of Hall of Gods boss fight.
  + **Force Arrive Animation**: Force the arrive animation to be played when fighting Vengefly King, Xero, Hive Knight and Enraged Guardian in Hall of Gods.
  + **Halve Damage**: Halve all the damage taken.
  + **P5 Health**: Reduce boss health in Ascended and Radiant difficulty to match with Attuned difficulty.

- **New Save Quickstart**:
  + **Unlock Eternal Ordeal**: Auto unlock the Eternal Ordeal.
  + **Unlock Radiance**: Auto unlock the Radiance in Hall of Gods when in God Seeker mode.
  + **Unlock Radiant**: Auto unlock Radiant difficulty for all boss statues.

- **Quality of Life**:
  + **Carefree Melody Fix**: Remove the Grimmchild mistakenly spawned when equipping Carefree Melody not acquired in normal means. The effect is permanent.
  + **Complete Lower Difficulty**: Auto complete lower difficulties for boss statues if higher ones are completed. Triggers when opening challenge UI.
  + **Door Default Begin**: Default selecting the Begin button when opening Pantheon Door UI.
  + **Fast Dream Warping**: Remove dream warping charge time when in boss scenes. This decrease the total warping time from 2.25s to 0.25s.
  + **Fast Super Dash**: Buff Super Dash speed in Hall of Gods.
  + **Grey Prince Enter Short**: Force Grey Prince to use short enter, as when defeated last time.
  + **Memorize Bindings**: Memorize the Bindings selected last time, like difficulties in the Hall of Gods. Recommend using with Door Default Begin.

- **Restrictions**:
  + **Force Overcharm**: Force entering the overcharmed state.
  + **Frame Rate Limit**: Create a lag in every in-game frame.

All features can be toggled in-game.

## Configuration

- `features` (`Object`): Whether to enable specified features.
  + `ActivateFury` (`Boolean`): Whether to enable the Activate Fury feature. Defaults to `false`.
  + `AddLifeblood` (`Boolean`): Whether to enable the Add Lifeblood feature. Defaults to `false`.
  + `AddSoul` (`Boolean`): Whether to enable the Add Soul feature. Defaults to `false`.
  + `CarefreeMelodyFix` (`Boolean`): Whether to enable the Carefree Melody Fix feature. Defaults to `true`.
  + `CompleteLowerDifficulty` (`Boolean`): Whether to enable the Complete Lower Difficulty feature. Defaults to `true`.
  + `DoorDefaultBegin` (`Boolean`): Whether to enable the Door Default Begin feature. Defaults to `true`.
  + `FastDreamWarp` (`Boolean`): Whether to enable the Fast Dream Warping feature. Defaults to `true`.
  + `FastSuperDash` (`Boolean`): Whether to enable the Fast Super Dash feature. Defaults to `true`.
  + `ForceArriveAnimation` (`Boolean`): Whether to enable the Force Arrive Animation feature. Defaults to `false`.
  + `ForceOvercharm` (`Boolean`): Whether to enable the Force Overcharm feature. Defaults to `false`.
  + `FrameRateLimit` (`Boolean`): Whether to enable the Frame Rate Limit feature. Defaults to `false`.
  + `GreyPrinceEnterShort` (`Boolean`): Whether to enable the Grey Prince Enter Short feature. Defaults to `true`.
  + `HalveDamage` (`Boolean`): Whether to enable the Halve Damage feature. Defaults to `false`.
  + `MemorizeBindings` (`Boolean`): Whether to enable the Memorize Bindings feature. Defaults to `true`.
  + `P5Health` (`Boolean`): Whether to enable the P5 Health feature. Defaults to `false`.
  + `UnlockEternalOrdeal` (`Boolean`): Whether to enable the Unlock Eternal Ordeal feature. Defaults to `true`.
  + `UnlockRadiance` (`Boolean`): Whether to enable the Unlock Radiance feature. Defaults to `true`.
  + `UnlockRadiant` (`Boolean`): Whether to enable the Unlock Radiant feature. Defaults to `true`.
- `fastSuperDashSpeedMultiplier` (`Float`): Fast Super Dash speed multiplier. Ranges from `1` to `2`, defaults to `1.5`.
- `frameRateLimitMultiplier` (`Integer`): Frame rate limit time span multiplier. Final lag time is 10ms multiplied by this value. Note that setting this to `0` does not mean zero lag. Ranges from `0` to `10`, defaults to `5`.
- `LifebloodAmount` (`Integer`): The amount of lifeblood to be added. Ranges from `0` to `35`, defaults to `5`.
- `SoulAmount` (`Integer`): The amount of soul to be added. Ranges from `0` to `198`, defaults to `99`.

## Contributing

1. Clone the repository
2. Set environment variable `HKRefs` to your `Managed` folder in HK installation
