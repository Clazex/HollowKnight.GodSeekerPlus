using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Vasi;
using UnityEngine;
namespace GodSeekerPlus.Modules {
	internal sealed class FastSuperDash : Module {
		private protected override void Load() => UnityEngine.SceneManagement.SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;

		private void SceneManager_activeSceneChanged(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.Scene arg1) {
			PlayMakerFSM superdash = GameObject.Find("Knight").LocateMyFSM("Superdash");
			if(superdash!=null) {
				if (arg1.name == "GG_Workshop") {

					superdash.Fsm.GetFsmFloat("Superdash Speed").Value = 30 * GodSeekerPlus.Instance.GlobalSetting.fastSuperDashSpeedMultiplier;
					superdash.Fsm.GetFsmFloat("Superdash Speed neg").Value = -30 * GodSeekerPlus.Instance.GlobalSetting.fastSuperDashSpeedMultiplier;
				}
				else {
					superdash.Fsm.GetFsmFloat("Superdash Speed").Value = 30;
					superdash.Fsm.GetFsmFloat("Superdash Speed neg").Value = -30;
				}
			}
		}

		private protected override void Unload() => UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;

		private protected override bool ShouldLoad() => GodSeekerPlus.Instance.GlobalSetting.fastSuperDash;

		
	}
}
