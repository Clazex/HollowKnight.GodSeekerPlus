using System.Reflection;
using GodSeekerPlus.Util;
using Modding;

namespace GodSeekerPlus {
	public sealed partial class GodSeekerPlus : Mod {
		public static GodSeekerPlus Instance { get; private set; }

		public GodSeekerPlus() : base(L11nUtil.Localize("GodSeekerPlus")) { }


		public override string GetVersion() => Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

		private readonly ModuleManager moduleManager = new();

		public override void Initialize() {
			if (Instance != null) {
				return;
			}

			Instance = this;

			moduleManager.LoadModules();
		}

		public void Unload() {
			moduleManager.UnloadModules();

			Instance = null;
		}
	}
}
