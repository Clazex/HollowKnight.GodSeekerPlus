# [寻神者+](https://github.com/Clazex/HollowKnight.GodSeekerPlus)

一个提升神居体验的《空洞骑士》模组

适用于 `空洞骑士` 1.5。
**需要 `Vasi`。**

## 功能

- **无忧旋律修复**\*：移除在装备非正常途径获得的无忧旋律时错误生成的格林之子。效果是永久的。
- **快速梦门传送**：当在神居 Boss 战房间中时，移除梦之门传送充能时间。这将总传送时间从 2.25 秒减少到 0.25 秒。
- **快速超级冲刺**：提升在诸神堂中的超级冲刺速度。
- **帧率限制**\*：在游戏内的每帧添加卡顿。
- **伤害减半**\*: 将受到的所有伤害减半。
- **记住束缚**\*：记住上次选择的束缚，效果与诸神堂中的难度选择一样。
- **解锁无尽折磨**\*: 自动解锁无尽折磨。
- **解锁辐光**\*: 在寻神者模式中，自动在诸神堂解锁辐光。

标 `*` 的功能可在游戏中开启或关闭。

## 配置

- `features` (`Object`): 是否启用指定的功能。
  + `CarefreeMelodyFix` (`Boolean`)：是否启用 `无忧旋律修复` 功能。默认为 `true`。
  + `FastDreamWarp` (`Boolean`)：是否启用 `快速梦门传送` 功能。默认为 `true`。
  + `FastSuperDash` (`Boolean`)：是否启用 `快速超级冲刺` 功能。默认为 `true`。
  + `FrameRateLimit` (`Boolean`)：是否启用 `帧率限制` 功能。默认为 `false`。
  + `HalveDamage` (`Boolean`)：是否启用 `伤害减半` 功能。默认为 `false`。
  + `MemorizeBindings` (`Boolean`)：是否启用 `记住束缚` 功能。默认为 `true`。
  + `UnlockEternalOrdeal` (`Boolean`)：是否启用 `解锁无尽折磨` 功能。默认为 `true`。
  + `UnlockRadiance` (`Boolean`)：是否启用 `解锁辐光` 功能。默认为 `true`。
- `fastSuperDashSpeedMultiplier` (`Float`)：快速超冲速度倍率。范围从 `1` 到 `2`，默认为 `1.5`。
- `frameRateLimitMultiplier` (`Integer`)：帧率限制卡顿时间倍率。最终卡顿时间是 10ms 乘以这个值。注意将其设置为 `0` 并不意味着零卡顿。范围从 `0` 到 `10`，默认为 `5`。

## 贡献

1. 克隆存储库
2. 将环境变量 `HKRefs` 设置为游戏文件中的 `Managed` 文件夹
