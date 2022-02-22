using System.IO;
using System.Security.Cryptography;

namespace GodSeekerPlus.Util;

internal static class VersionUtil {
	internal static readonly Lazy<string> Version = new(() => Assembly
		.GetExecutingAssembly()
		.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
		.InformationalVersion
#if DEBUG
		+ "-dev"
#endif
	);

	internal static readonly Lazy<string> VersionWithHash = new(() => {
		using var hasher = SHA1.Create();
		byte[] bytes = File.ReadAllBytes(Assembly.GetExecutingAssembly().Location);
		return Version.Value + '+' + BitConverter.ToString(hasher.ComputeHash(bytes), 0, 4)
			.Substring(0, 10).Replace("-", "").ToLowerInvariant();
	});
}
