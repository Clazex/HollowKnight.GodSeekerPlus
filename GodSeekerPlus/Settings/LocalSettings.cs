namespace GodSeekerPlus.Settings;

public sealed class LocalSettings {
	public bool boundNail = false;
	public bool boundHeart = false;
	public bool boundCharms = false;
	public bool boundSoul = false;

	[JsonIgnore]
	private int rabCompletion = 0;

	[JsonProperty(PropertyName = nameof(rabCompletion))]
	public int RABCompletion {
		get => rabCompletion;
		set => rabCompletion = ((value & 0b11111) == value) ? value : 0;
	}


	#region RAB Completions Getter/Setter

	public bool GetRABCompletion(int num) => num >= 1 && num <= 5
		? (RABCompletion & (1 << (num - 1))) != 0
		: throw new ArgumentOutOfRangeException(nameof(num));

	internal void SetRABCompletion(int num, bool completed) {
		if (num < 1 || num > 5) {
			throw new ArgumentOutOfRangeException(nameof(num));
		}

		int mask = 1 << (num - 1);
		if (completed) {
			RABCompletion |= mask;
		} else {
			RABCompletion &= ~mask;
		}
	}

	#endregion
}
