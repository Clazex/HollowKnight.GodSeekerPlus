using Newtonsoft.Json;

namespace GodSeekerPlus.Settings;

public sealed class GlobalSettings {
	[JsonIgnore]
	public Dictionary<string, bool> modules =
		ModuleHelper.GetDefaultModuleStateDict();

	[JsonIgnore]
	public float fastSuperDashSpeedMultiplier = 1.5f;

	[JsonIgnore]
	public int frameRateLimitMultiplier = 5;

	[JsonIgnore]
	public int lifebloodAmount = 5;

	[JsonIgnore]
	public int soulAmount = 99;


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

	[JsonProperty(PropertyName = "lifebloodAmount")]
	public int LifebloodAmount {
		get => lifebloodAmount;
		set => lifebloodAmount = MiscUtil.ForceInRange(value, 0, 35);
	}

	[JsonProperty(PropertyName = "soulAmount")]
	public int SoulAmount {
		get => soulAmount;
		set => soulAmount = MiscUtil.ForceInRange(value, 0, 198);
	}
}
