using LPContribMvc.PortableAreas;
using System;
using System.Collections.Generic;
using System.IO;

namespace LPContribMvc.UI.InputBuilder.ViewEngine
{
	public class AssemblyResourceStore
	{
		private Dictionary<string, string> resources;

		private Type typeToLocateAssembly;

		private string namespaceName;

		private PortableAreaMap map;

		private AssemblyResourceLocator resourceLocator;

		public string VirtualPath
		{
			get;
			private set;
		}

		public AssemblyResourceStore(Type typeToLocateAssembly, string virtualPath, string namespaceName)
		{
			Initialize(typeToLocateAssembly, virtualPath, namespaceName, null);
		}

		public AssemblyResourceStore(Type typeToLocateAssembly, string virtualPath, string namespaceName, PortableAreaMap map)
		{
			Initialize(typeToLocateAssembly, virtualPath, namespaceName, map);
		}

		private void Initialize(Type typeToLocateAssembly, string virtualPath, string namespaceName, PortableAreaMap map)
		{
			this.map = map;
			this.typeToLocateAssembly = typeToLocateAssembly;
			VirtualPath = virtualPath.ToLower();
			this.namespaceName = namespaceName.ToLower();
			string[] manifestResourceNames = this.typeToLocateAssembly.Assembly.GetManifestResourceNames();
			resources = new Dictionary<string, string>(manifestResourceNames.Length);
			string[] array = manifestResourceNames;
			foreach (string text in array)
			{
				resources.Add(text.ToLower(), text);
			}
			resourceLocator = new AssemblyResourceLocator(resources, this.namespaceName, VirtualPath);
		}

		public Stream GetResourceStream(string resourceName)
		{
			string resourceName2 = null;
			if (resourceLocator.TryGetActualResourceName(resourceName, out resourceName2))
			{
				Stream manifestResourceStream = typeToLocateAssembly.Assembly.GetManifestResourceStream(resourceName2);
				if (map != null && (resourceName.ToLower().EndsWith(".aspx") || resourceName.ToLower().EndsWith(".master")))
				{
					return map.Transform(manifestResourceStream);
				}
				return manifestResourceStream;
			}
			return null;
		}

		public string GetFullyQualifiedTypeFromPath(string path)
		{
			return resourceLocator.GetFullyQualifiedTypeFromPath(path);
		}

		public bool IsPathResourceStream(string path)
		{
			return resourceLocator.IsPathResourceStream(path);
		}
	}
}
