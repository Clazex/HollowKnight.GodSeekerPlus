namespace GodSeekerPlus {
	public class GlobalSettings {
		public bool fastDreamWarp = true;
		public bool frameRateLimit = false;
		public int frameRateLimitMultiplier = 5;

		public void Coerce() {
			if (frameRateLimitMultiplier < 0) {
				frameRateLimitMultiplier = 0;
			} else if (frameRateLimitMultiplier > 10) {
				frameRateLimitMultiplier = 10;
			}
		}
	}
}
