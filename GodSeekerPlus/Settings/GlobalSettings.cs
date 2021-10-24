using Newtonsoft.Json;

namespace GodSeekerPlus {
	public sealed class GlobalSettings {
		public bool carefreeMelodyFix = true;
		public bool fastDreamWarp = true;
		public bool fastSuperDash = true;
		public bool frameRateLimit = false;
		public bool halveDamage = false;
		public bool memorizeBindings = true;

		[JsonIgnore]
		public float fastSuperDashSpeedMultiplier = 1.5f;
		[JsonIgnore]
		public int frameRateLimitMultiplier = 5;

		[JsonProperty(PropertyName = "fastSuperDashSpeedMultiplier")]
		public float FastSuperDashSpeedMultiplier {
			get => fastSuperDashSpeedMultiplier;
			set => fastSuperDashSpeedMultiplier = value < 1f ? 1f : (value > 2f ? 2f : value);
		}

		[JsonProperty(PropertyName = "frameRateLimitMultiplier")]
		public int FrameRateLimitMultiplier {
			get => frameRateLimitMultiplier;
			set => frameRateLimitMultiplier = value < 0 ? 0 : (value > 10 ? 10 : value);
		}
	}
}
