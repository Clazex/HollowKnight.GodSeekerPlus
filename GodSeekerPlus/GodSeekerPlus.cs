using System.Reflection;
using Modding;

namespace GodSeekerPlus {
	public sealed partial class GodSeekerPlus : Mod {
		public static GodSeekerPlus Instance { get; private set; }

		public override string GetVersion() => Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

		public override void Initialize() {
			if (Instance != null) {
				return;
			}

			Instance = this;

			ModuleManager.LoadModules();
		}

		public void Unload() {
			ModuleManager.UnloadModules();

			Instance = null;
		}
	}
}
