# [GodSeekerPlus](https://github.com/Clazex/HollowKnight.GodSeekerPlus)

A Hollow Knight mod to enhance your Godhome experience

Compatible with `Hollow Knight` 1.5.
**`Vasi` is required.**

## Features

- **Carefree Melody Fix**: Remove the Grimmchild mistakenly spawned when equipping Carefree Melody not acquired in normal means. The effect is permanent.
- **Fast Dream Warping**: Remove dream warping charge time when in Godhome boss fight rooms. This decrease the total warping time from 2.25s to 0.25s.
- **Fast Super Dash**: Buff Super Dash speed in Hall of Gods.
- **Frame Rate Limit**: Create a lag in every in-game frame.
- **Halve Damage**: Halve all the damage taken.
- **Memorize Bindings**: Memorize the Bindings selected last time, like difficulties in the Hall of Gods.

## Configuration

- `carefreeMelodyFix` (`Boolean`): Whether to enable the Carefree Melody Fix feature. Defaults to `true`.
- `fastDreamWarp` (`Boolean`): Whether to enable the Fast Dream Warping feature. Defaults to `true`.
- `fastSuperDash` (`Boolean`): Whether to enable the Fast Super Dash feature. Defaults to `true`.
- `fastSuperDashSpeedMultiplier` (`Float`): Fast Super Dash speed multiplier. Ranges from `1` to `2`, defaults to `1.5`.
- `frameRateLimit` (`Boolean`): Whether to enable the Frame Rate Limit feature. Defaults to `false`.
- `frameRateLimitMultiplier` (`Integer`): Frame rate limit time span multiplier. Final lag time is 10ms multiplied by this value. Note that setting this to `0` does not mean zero lag. Ranges from `0` to `10`, defaults to `5`.
- `halveDamage` (`Boolean`): Whether to enable the Halve Damage feature. Defaults to `false`.
- `memorizeBindings` (`Boolean`): Whether to enable the Memorize Bindings feature. Defaults to `true`.

## Contributing

1. Clone the repository
2. Set environment variable `HKRefs` to your `Managed` folder in HK installation
