# [寻神者+](https://github.com/Clazex/HollowKnight.GodSeekerPlus)

一个提升神居体验的《空洞骑士》模组

适用于 `空洞骑士` 1.5。
**需要 `Vasi`。**

## 功能

- **无忧旋律修复**：移除在装备非正常途径获得的无忧旋律时错误生成的格林之子。效果是永久的。
- **完成较低难度**：当完成了 Boss 雕像的较高难度时，自动完成较低难度。打开挑战界面时触发。
- **门默认开始**：打开万神殿门界面时默认选中开始按钮。
- **快速梦门传送**：当在 Boss 场景中时，移除梦之门传送充能时间。这将总传送时间从 2.25 秒减少到 0.25 秒。
- **快速超级冲刺**：提升在诸神堂中的超级冲刺速度。
- **帧率限制**：在游戏内的每帧添加卡顿。
- **强制护符过载**：强制进入护符过载状态。
- **灰色王子短入场**：强制灰色王子使用短入场，如同上一次击败之的效果。
- **伤害减半**：将受到的所有伤害减半。
- **记住束缚**：记住上次选择的束缚，效果与诸神堂中的难度选择一样。建议与门默认开始一起使用。
- **解锁无尽折磨**：自动解锁无尽折磨。
- **解锁辐光**：在寻神者模式中，自动在诸神堂解锁辐光。
- **解锁辐辉**：自动为所有 Boss 雕像解锁辐辉难度。

所有功能均可在游戏中开启或关闭。

## 配置

- `features` (`Object`): 是否启用指定的功能。
  + `CarefreeMelodyFix` (`Boolean`)：是否启用 `无忧旋律修复` 功能。默认为 `true`。
  + `CompleteLowerDifficulty` (`Boolean`)：是否启用 `完成较低难度` 功能。默认为 `true`。
  + `DoorDefaultBegin` (`Boolean`)：是否启用 `门默认开始` 功能。默认为 `true`。
  + `FastDreamWarp` (`Boolean`)：是否启用 `快速梦门传送` 功能。默认为 `true`。
  + `FastSuperDash` (`Boolean`)：是否启用 `快速超级冲刺` 功能。默认为 `true`。
  + `FrameRateLimit` (`Boolean`)：是否启用 `帧率限制` 功能。默认为 `false`。
  + `ForceOvercharm` (`Boolean`)：是否启用 `强制护符过载` 功能。默认为 `false`。
  + `GreyPrinceEnterShort` (`Boolean`)：是否启用 `灰色王子短入场` 功能。默认为 `true`。
  + `HalveDamage` (`Boolean`)：是否启用 `伤害减半` 功能。默认为 `false`。
  + `MemorizeBindings` (`Boolean`)：是否启用 `记住束缚` 功能。默认为 `true`。
  + `UnlockEternalOrdeal` (`Boolean`)：是否启用 `解锁无尽折磨` 功能。默认为 `true`。
  + `UnlockRadiance` (`Boolean`)：是否启用 `解锁辐光` 功能。默认为 `true`。
  + `UnlockRadiant` (`Boolean`)：是否启用 `解锁辐辉` 功能。默认为 `true`。
- `fastSuperDashSpeedMultiplier` (`Float`)：快速超冲速度倍率。范围从 `1` 到 `2`，默认为 `1.5`。
- `frameRateLimitMultiplier` (`Integer`)：帧率限制卡顿时间倍率。最终卡顿时间是 10ms 乘以这个值。注意将其设置为 `0` 并不意味着零卡顿。范围从 `0` 到 `10`，默认为 `5`。

## 贡献

1. 克隆存储库
2. 将环境变量 `HKRefs` 设置为游戏文件中的 `Managed` 文件夹
