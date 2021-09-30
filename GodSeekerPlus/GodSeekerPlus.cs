using System.Reflection;
using Modding;

namespace GodSeekerPlus {
	public class GodSeekerPlus : Mod {
		public static GodSeekerPlus LoadedInstance { get; private set; }
		public override string GetVersion() => "0.1.0";


		public override void Initialize() {
			if (LoadedInstance != null) {
				return;
			}

			LoadedInstance = this;

			Hook();
		}

		public void Unload() {
			LoadedInstance = null;

			UnHook();
		}


		private void Hook() {}

		private void UnHook() {}
	}
}
