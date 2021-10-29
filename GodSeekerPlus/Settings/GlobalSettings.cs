using System.Collections.Generic;
using GodSeekerPlus.Util;
using Newtonsoft.Json;

namespace GodSeekerPlus {
	public sealed class GlobalSettings {
		[JsonIgnore]
		public Dictionary<string, bool> modules =
			ModuleHelper.GetDefaultModuleStateDict();

		[JsonIgnore]
		public float fastSuperDashSpeedMultiplier = 1.5f;

		[JsonIgnore]
		public int frameRateLimitMultiplier = 5;


		[JsonProperty(PropertyName = "features")]
		public Dictionary<string, bool> Modules {
			get => modules;
			set {
				foreach (KeyValuePair<string, bool> pair in value) {
					if (modules.ContainsKey(pair.Key)) {
						modules[pair.Key] = pair.Value;
					}
				}
			}
		}

		[JsonProperty(PropertyName = "fastSuperDashSpeedMultiplier")]
		public float FastSuperDashSpeedMultiplier {
			get => fastSuperDashSpeedMultiplier;
			set => fastSuperDashSpeedMultiplier = MiscUtil.ForceInRange(value, 1f, 2f);
		}

		[JsonProperty(PropertyName = "frameRateLimitMultiplier")]
		public int FrameRateLimitMultiplier {
			get => frameRateLimitMultiplier;
			set => frameRateLimitMultiplier = MiscUtil.ForceInRange(value, 0, 10);
		}
	}
}
