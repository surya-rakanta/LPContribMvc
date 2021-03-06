#define TRACE
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.Hosting;

namespace LPContribMvc.UI.InputBuilder.ViewEngine
{
	public class AssemblyResourceVirtualFile : VirtualFile
	{
		private readonly AssemblyResourceStore resourceStore;

		private readonly string path;

		public AssemblyResourceVirtualFile(string virtualPath, AssemblyResourceStore resourceStore)
			: base(virtualPath)
		{
			this.resourceStore = resourceStore;
			path = VirtualPathUtility.ToAppRelative(virtualPath);
		}

		public override Stream Open()
		{
			Trace.WriteLine("Opening " + path);
			return resourceStore.GetResourceStream(path);
		}
	}
}
