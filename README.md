# [Godseeker +](https://github.com/Clazex/HollowKnight.GodSeekerPlus)

[![Commitizen friendly](https://img.shields.io/badge/commitizen-friendly-brightgreen.svg)](http://commitizen.github.io/cz-cli/)

A Hollow Knight mod to enhance your Godhome experience

Compatible with `Hollow Knight` 1.5.
**`Satchel` is required for in-game menu.**

## Features

- **Boss Challenge**:
  + **Activate Fury**: Activate Fury of the Fallen on entry of boss scenes by setting health to 1.
  + **Add Lifeblood**: Add configured amount of lifeblood on entry of Hall of Gods boss fight.
  + **Add Soul**: Add configured amount of soul on entry of Hall of Gods boss fight.
  + **Force Arrive Animation**: Force the arrive animation to be played when fighting Vengefly King, Xero, Hive Knight and Enraged Guardian in Hall of Gods.
  + **Halve Damage**: Halve all the damage taken.
  + **Infinite Radiance Climbing**: Start Absolute Radiance boss fight from the end of Phase 2 and reset when starting final phase.
  + **P5 Health**: Reduce boss health in Ascended and Radiant difficulty to match with Attuned difficulty.

- **New Save Quickstart**:
  + **Unlock Eternal Ordeal**: Auto unlock the Eternal Ordeal.
  + **Unlock Pantheons**: Auto unlock Pantheon of Knight and the Pantheon of Hallownest when in Godseeker mode, also activates some objects in the Godhome Atrium Roof.
  + **Unlock Radiance**: Auto unlock the Radiance in Hall of Gods when in Godseeker mode.
  + **Unlock Radiant**: Auto unlock Radiant difficulty for all boss statues.

- **Quality of Life**:
  + **Carefree Melody Fix**: Remove the Grimmchild mistakenly spawned when equipping Carefree Melody not acquired in normal means. The effect is permanent.
  + **Complete Lower Difficulty**: Auto complete lower difficulties for boss statues if higher ones are completed. Triggers when opening challenge UI.
  + **Door Default Begin**: Default selecting the Begin button when opening Pantheon Door UI.
  + **Fast Dream Warping**: Remove dream warping charge time when in boss scenes. This decrease the total warping time from 2.25s to 0.25s.
  + **Fast Super Dash**: Buff Super Dash speed in Hall of Gods.
  + **Force Grey Prince Enter Type**: Force select enter type that Grey Prince use.
  + **Grey Prince Toggle**: Add a Dream Nail toggle beside Grey Prince statue to show and control whether Grey Prince will appear in the Pantheons or not when in Godseeker mode.
  + **Memorize Bindings**: Memorize the Bindings selected last time, like difficulties in the Hall of Gods. Recommend using with Door Default Begin.
  + **P5 Teleport**: Allow using the Dream Nail towards the lever which is used to unlock Godhome Atrium Roof to teleport to beside the Pantheon of Hallownest.
  + **Short Death Animation**: Skip part of the death animation when in boss scenes.

- **Restrictions**:
  + **Create Lag**: Create a lag in every in-game frame.
  + **Force Overcharm**: Force entering the overcharmed state.
  + **No Nail Attack**: Disable normal nail attack.
  + **No Nail Damage**: Remove damage from nail attacks, including nail arts.
  + **No Spell Damage**: Remove damage from spells, including Sharp Shadow.

- **Visual**:
  + **No Fury Effect**: Remove the red effect around the screen when the Fury of Fallen is activated.
  + **No Low Health Effect**: Remove the black effect around the screen when you have only one health.

- **Miscellaneous**
  + **Aggressive GC**: Force resource unloading and garbage collecting on every scene trasition in the pantheons, may reduce memory usage but slow down loading.
  + **Unlock All Modes**: Auto unlock Steel Soul mode and Godseeker mode.

All features can be toggled in-game.

## Configuration

- `features` (`Object`): Whether to enable specified features.
- `fastSuperDashSpeedMultiplier` (`Float`): Fast Super Dash speed multiplier. Ranges from `1` to `2`, defaults to `1.5`.
- `gpzEnterType` (`Bool`): Name the enter type Grey Prince use. `false` for long enter, `true` for short enter.
- `lagTime` (`Integer`): Lag time span in millisecond. Note that setting this to `0` does not mean zero lag. Ranges from `0` to `1000`, defaults to `50`.
- `lifebloodAmount` (`Integer`): The amount of lifeblood to be added. Ranges from `0` to `35`, defaults to `5`.
- `soulAmount` (`Integer`): The amount of soul to be added. Ranges from `0` to `198`, defaults to `99`.

## Statistics

![Repobeats analytics image](https://repobeats.axiom.co/api/embed/65e526723e20438fd78f8e117dee0a55cca44715.svg)
