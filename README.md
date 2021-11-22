# [God Seeker +](https://github.com/Clazex/HollowKnight.GodSeekerPlus)

A Hollow Knight mod to enhance your Godhome experience

Compatible with `Hollow Knight` 1.5.
**`Vasi` is required.**

## Features

- **Carefree Melody Fix**: Remove the Grimmchild mistakenly spawned when equipping Carefree Melody not acquired in normal means. The effect is permanent.
- **Complete Lower Difficulty**: Auto complete lower difficulties for boss statues if higher ones are completed. Triggers when opening challenge UI.
- **Door Default Begin**: Default selecting the Begin button when opening Pantheon Door UI.
- **Fast Dream Warping**: Remove dream warping charge time when in boss scenes. This decrease the total warping time from 2.25s to 0.25s.
- **Fast Super Dash**: Buff Super Dash speed in Hall of Gods.
- **Force Overcharm**: Force entering the overcharmed state.
- **Frame Rate Limit**: Create a lag in every in-game frame.
- **Grey Prince Enter Short**: Force Grey Prince to use short enter, as when defeated last time.
- **Halve Damage**: Halve all the damage taken.
- **Memorize Bindings**: Memorize the Bindings selected last time, like difficulties in the Hall of Gods. Recommend using with Door Default Begin.
- **Unlock Eternal Ordeal**: Auto unlock the Eternal Ordeal.
- **Unlock Radiance**: Auto unlock the Radiance in Hall of Gods when in God Seeker mode.
- **Unlock Radiant**: Auto unlock Radiant difficulty for all boss statues.

All features can be toggled in-game.

## Configuration

- `features` (`Object`): Whether to enable specified features.
  + `CarefreeMelodyFix` (`Boolean`): Whether to enable the Carefree Melody Fix feature. Defaults to `true`.
  + `CompleteLowerDifficulty` (`Boolean`): Whether to enable the Complete Lower Difficulty feature. Defaults to `true`.
  + `DoorDefaultBegin` (`Boolean`): Whether to enable the Door Default Begin feature. Defaults to `true`.
  + `FastDreamWarp` (`Boolean`): Whether to enable the Fast Dream Warping feature. Defaults to `true`.
  + `FastSuperDash` (`Boolean`): Whether to enable the Fast Super Dash feature. Defaults to `true`.
  + `ForceOvercharm` (`Boolean`): Whether to enable the Force Overcharm feature. Defaults to `false`.
  + `FrameRateLimit` (`Boolean`): Whether to enable the Frame Rate Limit feature. Defaults to `false`.
  + `GreyPrinceEnterShort` (`Boolean`): Whether to enable the Grey Prince Enter Short feature. Defaults to `true`.
  + `HalveDamage` (`Boolean`): Whether to enable the Halve Damage feature. Defaults to `false`.
  + `MemorizeBindings` (`Boolean`): Whether to enable the Memorize Bindings feature. Defaults to `true`.
  + `UnlockEternalOrdeal` (`Boolean`): Whether to enable the Unlock Eternal Ordeal feature. Defaults to `true`.
  + `UnlockRadiance` (`Boolean`): Whether to enable the Unlock Radiance feature. Defaults to `true`.
  + `UnlockRadiant` (`Boolean`): Whether to enable the Unlock Radiant feature. Defaults to `true`.
- `fastSuperDashSpeedMultiplier` (`Float`): Fast Super Dash speed multiplier. Ranges from `1` to `2`, defaults to `1.5`.
- `frameRateLimitMultiplier` (`Integer`): Frame rate limit time span multiplier. Final lag time is 10ms multiplied by this value. Note that setting this to `0` does not mean zero lag. Ranges from `0` to `10`, defaults to `5`.

## Contributing

1. Clone the repository
2. Set environment variable `HKRefs` to your `Managed` folder in HK installation
