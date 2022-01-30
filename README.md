# [Godseeker +](https://github.com/Clazex/HollowKnight.GodSeekerPlus)

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
  + **Unlock Pantheons**: Auto unlock Pantheon of Knight and the Pantheon of Hallownest when in Godseeker mode, also activates some objects in the Godhome Atrium Roof.
  + **Unlock Radiance**: Auto unlock the Radiance in Hall of Gods when in Godseeker mode.
  + **Unlock Radiant**: Auto unlock Radiant difficulty for all boss statues.

- **Quality of Life**:
  + **Carefree Melody Fix**: Remove the Grimmchild mistakenly spawned when equipping Carefree Melody not acquired in normal means. The effect is permanent.
  + **Complete Lower Difficulty**: Auto complete lower difficulties for boss statues if higher ones are completed. Triggers when opening challenge UI.
  + **Door Default Begin**: Default selecting the Begin button when opening Pantheon Door UI.
  + **Fast Dream Warping**: Remove dream warping charge time when in boss scenes. This decrease the total warping time from 2.25s to 0.25s.
  + **Fast Super Dash**: Buff Super Dash speed in Hall of Gods.
  + **Grey Prince Enter Short**: Force Grey Prince to use short enter, as when defeated last time.
  + **Memorize Bindings**: Memorize the Bindings selected last time, like difficulties in the Hall of Gods. Recommend using with Door Default Begin.
  + **P5 Teleport**: Allow using the Dream Nail towards the lever which is used to unlock Godhome Atrium Roof to teleport to beside the Pantheon of Hallownest.
  + **Short Death Animation**: Skip part of the death animation when in boss scenes.

- **Restrictions**:
  + **Force Overcharm**: Force entering the overcharmed state.
  + **Frame Rate Limit**: Create a lag in every in-game frame.
  + **No Nail Attack**: Disable normal nail attack.
  + **No Nail Damage**: Remove damage from nail attacks, including nail arts.
  + **No Spell Damage**: Remove damage from spells, including Sharp Shadow.

- **Visual**:
  + **No Fury Effect**: Remove the red effect around the screen when the Fury of Fallen is activated.
  + **No Low Health Effect**: Remove the black effect around the screen when you have only one health.

- **Miscellaneous**
  + **Unlock All Modes**: Auto unlock Steel Soul mode and Godseeker mode.

All features can be toggled in-game.

## Configuration

- `features` (`Object`): Whether to enable specified features.
- `fastSuperDashSpeedMultiplier` (`Float`): Fast Super Dash speed multiplier. Ranges from `1` to `2`, defaults to `1.5`.
- `frameRateLimitMultiplier` (`Integer`): Frame rate limit time span multiplier. Final lag time is 10ms multiplied by this value. Note that setting this to `0` does not mean zero lag. Ranges from `0` to `10`, defaults to `5`.
- `lifebloodAmount` (`Integer`): The amount of lifeblood to be added. Ranges from `0` to `35`, defaults to `5`.
- `soulAmount` (`Integer`): The amount of soul to be added. Ranges from `0` to `198`, defaults to `99`.
