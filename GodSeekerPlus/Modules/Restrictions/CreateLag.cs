using System.Threading;

namespace GodSeekerPlus.Modules.Restrictions;

[Category(nameof(Restrictions))]
internal sealed class CreateLag : Module {
	private protected override void Load() =>
		ModHooks.HeroUpdateHook += ThreadSleep;

	private protected override void Unload() =>
		ModHooks.HeroUpdateHook -= ThreadSleep;

	private void ThreadSleep() {
		if (Ref.GM.IsGamePaused()) {
			return;
		}

		Thread.Sleep(Ref.GS.lagTime);
	}
}
