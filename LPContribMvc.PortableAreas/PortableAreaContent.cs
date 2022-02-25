using System;
using System.Collections.Generic;

namespace LPContribMvc.PortableAreas
{
	public class PortableAreaContent
	{
		private static PortableAreaMap _defaultMap;

		private static Dictionary<Type, PortableAreaMap> _maps;

		public static Dictionary<Type, PortableAreaMap> Maps
		{
			get
			{
				if (_maps == null)
				{
					_maps = new Dictionary<Type, PortableAreaMap>();
				}
				return _maps;
			}
			set
			{
				_maps = value;
			}
		}

		public static PortableAreaMap MapAll()
		{
			_defaultMap = new PortableAreaMap();
			return _defaultMap;
		}

		public static T Map<T>() where T : PortableAreaMap
		{
			PortableAreaMap value = null;
			if (!Maps.TryGetValue(typeof(T), out value))
			{
				value = CreateMapWithDefault<T>(value);
				Maps.Add(typeof(T), value);
			}
			return value as T;
		}

		private static PortableAreaMap CreateMapWithDefault<T>(PortableAreaMap map) where T : PortableAreaMap
		{
			map = Activator.CreateInstance<T>();
			if (_defaultMap != null)
			{
				map.Master(_defaultMap.MasterPageLocation).Title(_defaultMap.Title).Body(_defaultMap.Body);
			}
			return map;
		}
	}
}
