namespace GodSeekerPlus.Modules.Restrictions;

internal sealed class NoDiveInvincibility : Module {
	private PlayMakerFSM? spellControl = null;

	public NoDiveInvincibility() =>
		On.HeroController.Awake += GetSpellControl;

	private protected override void Load() {
		On.HeroController.Start += ModifySpellControlFSM;

		Apply(true);
	}

	private protected override void Unload() {
		On.HeroController.Start -= ModifySpellControlFSM;

		Apply(false);
	}

	private void GetSpellControl(On.HeroController.orig_Awake orig, HeroController self) {
		spellControl = self.gameObject.LocateMyFSM("Spell Control");

		orig(self);
	}

	private void ModifySpellControlFSM(On.HeroController.orig_Start orig, HeroController self) {
		orig(self);

		Apply(ModuleManager.TryGetActiveModule<NoDiveInvincibility>(out _));
	}

	private void Apply(bool active) {
		if (spellControl == null) {
			return;
		}

		spellControl.GetAction<CallMethodProper>("Quake1 Down", 4).Enabled = !active;
		spellControl.GetAction<CallMethodProper>("Quake2 Down", 4).Enabled = !active;
	}
}
