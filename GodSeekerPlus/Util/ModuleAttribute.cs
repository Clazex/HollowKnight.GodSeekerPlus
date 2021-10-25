using System;

namespace GodSeekerPlus.Modules {
	[AttributeUsage(AttributeTargets.Class)]
	internal sealed class ModuleAttribute : Attribute {
		public bool toggleable;
		public bool defaultEnabled;
	}
}
