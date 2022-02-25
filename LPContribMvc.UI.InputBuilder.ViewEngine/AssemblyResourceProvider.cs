using LPContribMvc.PortableAreas;
using System;
using System.Collections;
using System.Linq;
using System.Web.Caching;
using System.Web.Hosting;

namespace LPContribMvc.UI.InputBuilder.ViewEngine
{
	public class AssemblyResourceProvider : VirtualPathProvider
	{
		public override bool FileExists(string virtualPath)
		{
			bool flag = base.FileExists(virtualPath);
			if (!flag)
			{
				return AssemblyResourceManager.IsEmbeddedViewResourcePath(virtualPath);
			}
			return flag;
		}

		public override VirtualFile GetFile(string virtualPath)
		{
			if (AssemblyResourceManager.IsEmbeddedViewResourcePath(virtualPath) && !base.FileExists(virtualPath))
			{
				AssemblyResourceStore resourceStoreFromVirtualPath = AssemblyResourceManager.GetResourceStoreFromVirtualPath(virtualPath);
				return new AssemblyResourceVirtualFile(virtualPath, resourceStoreFromVirtualPath);
			}
			return base.GetFile(virtualPath);
		}

		public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
		{
			if (AssemblyResourceManager.IsEmbeddedViewResourcePath(virtualPath))
			{
				return null;
			}
			string[] virtualPathDependencies2 = (from s in virtualPathDependencies.OfType<string>()
				where !s.ToLower().Contains("/views/inputbuilders")
				select s).ToArray();
			return base.GetCacheDependency(virtualPath, virtualPathDependencies2, utcStart);
		}

		public override string GetCacheKey(string virtualPath)
		{
			return null;
		}
	}
}
