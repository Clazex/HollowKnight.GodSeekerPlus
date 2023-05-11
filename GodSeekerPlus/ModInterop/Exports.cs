using MonoMod.ModInterop;

using GodSeekerPlus.Modules.BossChallenge;
using GodSeekerPlus.Modules.QoL;

namespace GodSeekerPlus.ModInterop;

[ModExportName(nameof(GodSeekerPlus))]
public static class Exports {
	public static int SuppressModules(string suppressor, params string[] modules) =>
		ModuleManager.SuppressModules(suppressor, modules);

	public static void CancelSuppression(int handle) =>
		ModuleManager.CancelSuppression(handle);

	public static void AddFastDashPredicate(Func<Scene, bool> predicate) =>
		FastDash.predicates.Add(predicate);

	public static void AddInfiniteChallengeReturnScenePredicate(Func<GameManager.SceneLoadInfo, bool> predicate) =>
		InfiniteChallenge.returnScenePredicates.Add(predicate);
}
