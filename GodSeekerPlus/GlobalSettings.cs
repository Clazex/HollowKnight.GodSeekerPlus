namespace GodSeekerPlus {
	public class GlobalSettings {
		public bool fastDreamWarp = true;
		public bool fastSuperDash = true;
		public float fastSuperDashSpeedMultiplier = 1.5f;
		public bool frameRateLimit = false;
		public int frameRateLimitMultiplier = 5;

		public void Coerce() {
			if (fastSuperDashSpeedMultiplier < 1f) {
				fastSuperDashSpeedMultiplier = 1f;
			} else if (fastSuperDashSpeedMultiplier > 2f) {
				fastSuperDashSpeedMultiplier = 2f;
			}

			if (frameRateLimitMultiplier < 0) {
				frameRateLimitMultiplier = 0;
			} else if (frameRateLimitMultiplier > 10) {
				frameRateLimitMultiplier = 10;
			}
		}
	}
}
