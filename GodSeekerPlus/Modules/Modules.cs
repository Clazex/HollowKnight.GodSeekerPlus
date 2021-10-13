namespace GodSeekerPlus.Modules {
	internal static class Modules {
		public static void LoadModules(this GodSeekerPlus self) {
			if (self.GlobalSettings.carefreeMelodyFix) {
				CarefreeMelodyFix.Load();
			}

			if (self.GlobalSettings.fastDreamWarp) {
				FastDreamWarp.Load();
			}

			if (self.GlobalSettings.fastSuperDash) {
				FastSuperDash.Load();
			}

			if (self.GlobalSettings.frameRateLimit) {
				FrameRateLimit.Load();
			}
		}

		public static void UnloadModules(this GodSeekerPlus _) {
			CarefreeMelodyFix.Unload();
			FastDreamWarp.Unload();
			FastSuperDash.Unload();
			FrameRateLimit.Unload();
		}
	}
}
