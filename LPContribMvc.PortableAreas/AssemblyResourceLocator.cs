using System;
using System.Collections.Generic;
using System.Linq;

namespace LPContribMvc.PortableAreas
{
	internal class AssemblyResourceLocator
	{
		private readonly Dictionary<string, string> resources;

		private readonly string namespaceName;

		private readonly string virtualPath;

		public AssemblyResourceLocator(Dictionary<string, string> resources, string namespaceName, string virtualPath)
		{
			this.virtualPath = virtualPath;
			this.resources = resources;
			this.namespaceName = namespaceName;
		}

		public bool TryGetActualResourceName(string path, out string resourceName)
		{
			resourceName = GetFullyQualifiedTypeFromPath(path);
			bool flag = ResourcesContainsType(resourceName) || TryGetPartialResourceName(path, ref resourceName);
			if (flag)
			{
				resourceName = resources[resourceName];
			}
			return flag;
		}

		public string GetFullyQualifiedTypeFromPath(string path)
		{
			string text = path.ToLower().Replace("~", namespaceName);
			if (!string.IsNullOrEmpty(virtualPath))
			{
				text = text.Replace(virtualPath, "");
			}
			return text.Replace("/", ".");
		}

		public bool IsPathResourceStream(string path)
		{
			if (!ResourcesContainsFullyQualifiedType(path))
			{
				return ResourcesContainsPartiallyQualifiedType(path);
			}
			return true;
		}

		private bool ResourcesContainsFullyQualifiedType(string path)
		{
			return ResourcesContainsType(GetFullyQualifiedTypeFromPath(path));
		}

		private bool TryGetPartialResourceName(string path, ref string resourceName)
		{
			bool keepGoing = true;
			int num = 0;
			bool flag = false;
			string text = "";
			while (keepGoing && !flag)
			{
				text = GetPartiallyQualifiedTypeFromPath(path, num, ref keepGoing);
				flag = ResourcesContainsType(text);
				num++;
			}
			if (flag)
			{
				resourceName = text;
			}
			return flag;
		}

		private bool ResourcesContainsPartiallyQualifiedType(string path)
		{
			string resourceName = "";
			if (TryGetPartialResourceName(path, ref resourceName))
			{
				return ResourcesContainsType(resourceName);
			}
			return false;
		}

		private bool ResourcesContainsType(string typeString)
		{
			return resources.ContainsKey(typeString);
		}

		private string GetPartiallyQualifiedTypeFromPath(string path, int rootLevel, ref bool keepGoing)
		{
			string fullyQualifiedTypeFromPath = GetFullyQualifiedTypeFromPath(path);
			return RemoveIntermediatePathFrom(fullyQualifiedTypeFromPath, rootLevel, ref keepGoing);
		}

		private string RemoveIntermediatePathFrom(string fullPath, int rootLevel, ref bool keepGoing)
		{
			string text = namespaceName;
			string[] array = text.Split('.');
			keepGoing = (array.Length - 1 > rootLevel);
			if (keepGoing)
			{
				text = array.Take(array.Length - rootLevel).Aggregate(JoinNamespaces());
			}
			return JoinNamespaces(text, GetFileNameOnly(fullPath));
		}

		private string GetFileNameOnly(string fullPath)
		{
			IEnumerable<string> source = from s in fullPath.Split('.').Reverse()
				select (s);
			return source.Take(2).Reverse().Aggregate(JoinNamespaces());
		}

		private Func<string, string, string> JoinNamespaces()
		{
			return (string x, string y) => JoinNamespaces(x, y);
		}

		private string JoinNamespaces(string x, string y)
		{
			return x + "." + y;
		}
	}
}
