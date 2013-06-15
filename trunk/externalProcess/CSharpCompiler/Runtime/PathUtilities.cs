using System.IO;

namespace CSharpCompiler.Runtime
{
	public class PathUtilities
	{
		internal static string MakeTempPath(string fileName)
		{
			return Path.Combine(Path.GetTempPath(), fileName);
		}
	}
}