using LPContribMvc.UI.InputBuilder.ViewEngine;
using System.Collections.Generic;
using System.Web;

namespace LPContribMvc.PortableAreas
{
	public static class AssemblyResourceManager
	{
		private static Dictionary<string, AssemblyResourceStore> assemblyResourceStores = InitializeAssemblyResourceStores();

		private static Dictionary<string, AssemblyResourceStore> InitializeAssemblyResourceStores()
		{
			Dictionary<string, AssemblyResourceStore> dictionary = new Dictionary<string, AssemblyResourceStore>();
			AssemblyResourceStore assemblyResourceStore = new AssemblyResourceStore(typeof(AssemblyResourceProvider), "/views/inputbuilders", "LPContribMvc.UI.InputBuilder.Views.InputBuilders");
			dictionary.Add(assemblyResourceStore.VirtualPath, assemblyResourceStore);
			return dictionary;
		}

		public static AssemblyResourceStore GetResourceStoreForArea(string areaName)
		{
			return assemblyResourceStores["/areas/" + areaName.ToLower()];
		}

		public static AssemblyResourceStore GetResourceStoreFromVirtualPath(string virtualPath)
		{
			string text = VirtualPathUtility.ToAppRelative(virtualPath).ToLower();
			foreach (KeyValuePair<string, AssemblyResourceStore> assemblyResourceStore in assemblyResourceStores)
			{
				if (text.Contains(assemblyResourceStore.Key) && assemblyResourceStore.Value.IsPathResourceStream(text))
				{
					return assemblyResourceStore.Value;
				}
			}
			return null;
		}

		public static bool IsEmbeddedViewResourcePath(string virtualPath)
		{
			AssemblyResourceStore resourceStoreFromVirtualPath = GetResourceStoreFromVirtualPath(virtualPath);
			return resourceStoreFromVirtualPath != null;
		}

		public static void RegisterAreaResources(AssemblyResourceStore assemblyResourceStore)
		{
			assemblyResourceStores.Add(assemblyResourceStore.VirtualPath, assemblyResourceStore);
		}
	}
}
