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
  + **Carefree Melody Reset**: Reset Carefree Melody hit counter to maximum when entering Godhome non-boss scenes.
  + **Force Arrive Animation**: Force the arrive animation to be played when fighting Vengefly King, Xero, Hive Knight and Enraged Guardian in Hall of Gods.
  + **Force Grey Prince Enter Type**: Force select enter type that Grey Prince use.
  + **Halve Damage (HoG Ascended or above)**: Halve all the damage taken when challenging at Ascended difficulty or above in Hall of Gods.
  + **Halve Damage (HoG Attuned)**: Halve all the damage taken when challenging at Attuned difficulty in Hall of Gods.
  + **Halve Damage (Other Place)**: Halve all the damage taken when not in Godhome boss scenes.
  + **Halve Damage (Pantheons)**: Halve all the damage taken when in pantheons.
  + **Infinite Challenge**: Immediately restart Hall of Gods boss fights upon death. Can be configured to restart on success as well. Can use Dream Warp to return.
  + **Infinite Grimm Pufferfish**: When fighting in the Hall of Gods, make Troupe Master Grimm and Nightmare King Grimm only do pufferfish attack.
  + **Infinite Radiance Climbing**: When fighting in the Hall of Gods, starts Absolute Radiance boss fight from the end of Phase 2 and reset when starting final phase.
  + **P5 Health**: Reduce boss health in Ascended and Radiant difficulty to match with Attuned difficulty.
  + **Segmented P5**: Add a pantheon entry to the left of the entrance of the Land of Storms, at which you can select a segment of the Pantheon of Hallownest to challenge.

- **Bugfix**:
  + **Camera Keep Rumbling**: Prevent camera from keeping rumbling after scene transitions in Godhome.
  + **Crossing Ooma**: Prevent Ooma from spawning explosive jellies across scenes.
  + **God Tamer Beast Roll Collider**: Prevent God Tamer's Beast's collider from not restoring after rolling.
  + **Grey Prince Nail Slam Collider**: Prevent Grey Prince's nail slam collider from not disappearing when staggered.
  + **HUD Display Cheker**: Prevent the HUD from disappearing sometimes.
  + **Transition Death**: Prevent from continuing pantheon challenges after death upon scene transitions.

- **Cosmetic**:
  + **More Pantheon Caps**: Enable the unused no hit cap for Pantheons 1 to 4, and add a new special effect for all pantheons indicating completed once no hit with all bindings.
  + **No Defender's Crest Effect**: Remove Defender's Crest's trail effect, optionally including clouds.
  + **No Fury Effect**: Remove the red effect around the screen when the Fury of Fallen is activated.
  + **No Low Health Effect**: Remove all low health related effects.
  + **Use Own Music (WIP)**: Let bosses in pantheons use their own music as in Hall of Gods. (WIP, P1 only)

- **New Save Quickstart**:
  + **Unlock Eternal Ordeal**: Auto unlock the Eternal Ordeal.
  + **Unlock Pantheons**: Auto unlock Pantheon of Knight and the Pantheon of Hallownest when in Godseeker mode, also activates some objects in the Godhome Atrium Roof.
  + **Unlock Radiance**: Auto unlock the Radiance in Hall of Gods when in Godseeker mode.
  + **Unlock Radiant**: Auto unlock Radiant difficulty for all boss statues.

- **Quality of Life**:
  + **Complete Lower Difficulty**: Auto complete lower difficulties for boss statues if higher ones are completed. Triggers when opening challenge UI.
  + **Correct Radiance HP**: Correct Absolute Radiance's health value to make her die at zero health.
  + **Door Default Begin**: Default selecting the Begin button when opening Pantheon Door UI.
  + **Eternal Ordeal Platform**: Add a platform at the entrance of the Eternal Ordeal.
  + **Fast Dash**: Remove dash cooldown when in Hall of Gods.
  + **Fast Dream Warp**: Remove dream warping charging time and particles when in boss scenes. Optionally allows skipping preparation time for immediate warping when up key is pressed not later than dream nail key.
  + **Fast Super Dash**: Buff Super Dash speed in Hall of Gods. Optionally removes charging, cancellation cooldown and inertia.
  + **Grey Prince Toggle**: Add a Dream Nail toggle beside Grey Prince statue to show and control whether Grey Prince will appear in the Pantheons or not when in Godseeker mode.
  + **Keep Cocoon Lifeblood**: Keep lifeblood from cocoon when benching in the Pantheons.
  + **Memorize Bindings**: Memorize the Bindings selected last time, like difficulties in the Hall of Gods. Recommend using with Door Default Begin.
  + **P5 Teleport**: Allow using the Dream Nail towards the lever which is used to unlock Godhome Atrium Roof to teleport to beside the Pantheon of Hallownest.
  + **Short Death Animation**: Skip part of the death animation when in boss scenes.

- **Restrictions**:
  + **Create Lag**: Create a lag in every in-game frame.
  + **No Dive Invincibility**: Remove invincibility frames from Desolate Dive and Descending Dark.
  + **No Nail Attack**: Disable normal nail attack.
  + **No Nail Damage**: Remove damage from nail attacks, including nail arts.
  + **No Spell Damage**: Remove damage from spells, including Sharp Shadow.

- **Miscellaneous**
  + **Aggressive GC**: Force resource unloading and garbage collecting on every scene trasition, may reduce memory usage but slow down loading.
  + **Unlock All Modes**: Auto unlock Steel Soul mode and Godseeker mode.

All options can be tweaked in-game.

## Statistics

![Repobeats analytics image](https://repobeats.axiom.co/api/embed/65e526723e20438fd78f8e117dee0a55cca44715.svg)
