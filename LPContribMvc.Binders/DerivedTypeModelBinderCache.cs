using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace LPContribMvc.Binders
{
	public static class DerivedTypeModelBinderCache
	{
		private static readonly ThreadSafeDictionary<Type, IEnumerable<Type>> _typeCache = new ThreadSafeDictionary<Type, IEnumerable<Type>>();

		private static readonly ConcurrentDictionary<string, Type> _hashToTypeDictionary = new ConcurrentDictionary<string, Type>();

		private static readonly ConcurrentDictionary<Type, string> _typeToHashDictionary = new ConcurrentDictionary<Type, string>();

		private static string _typeStampFieldName = "_xTypeStampx_";

		private static readonly byte[] _originalEncryptionSalt = new byte[16]
		{
			197,
			16,
			83,
			227,
			196,
			23,
			71,
			201,
			133,
			93,
			241,
			98,
			115,
			148,
			18,
			158
		};

		private static byte[] _activeEncryptionSalt = _originalEncryptionSalt;

		public static string TypeStampFieldName
		{
			get
			{
				return _typeStampFieldName;
			}
			set
			{
				_typeStampFieldName = value;
			}
		}

		public static bool RegisterDerivedTypes(Type baseType, IEnumerable<Type> derivedTypes)
		{
			try
			{
				_typeCache.Add(baseType, derivedTypes);
			}
			catch (ArgumentException)
			{
				return false;
			}
			foreach (Type derivedType in derivedTypes)
			{
				Type currentItem = derivedType;
				string encryptedName = currentItem.FullName.EncryptStringToBase64();
				_hashToTypeDictionary.AddOrUpdate(encryptedName, (string name) => currentItem, (string name, Type itemValue) => currentItem);
				_typeToHashDictionary.AddOrUpdate(currentItem, (Type type) => encryptedName, (Type type, string name) => encryptedName);
			}
			ModelBinders.Binders.Add(baseType, (IModelBinder)(object)new DerivedTypeModelBinder());
			return true;
		}

		public static void SetTypeStampSaltValue(Guid guid)
		{
			_activeEncryptionSalt = guid.ToByteArray();
		}

		private static string EncryptStringToBase64(this string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return value;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(value);
			HMACSHA256 hMACSHA = new HMACSHA256(_activeEncryptionSalt);
			byte[] inArray = hMACSHA.ComputeHash(bytes);
			return Convert.ToBase64String(inArray);
		}

		public static Type GetDerivedType(string typeValue)
		{
			if (!_hashToTypeDictionary.TryGetValue(typeValue, out Type value))
			{
				return null;
			}
			return value;
		}

		public static void Reset()
		{
			foreach (KeyValuePair<Type, IEnumerable<Type>> item in _typeCache)
			{
				ModelBinders.Binders.Remove(item.Key);
			}
			_hashToTypeDictionary.Clear();
			_typeToHashDictionary.Clear();
			_typeCache.Clear();
			_activeEncryptionSalt = _originalEncryptionSalt;
		}

		public static string GetTypeName(Type type)
		{
			try
			{
				return _typeToHashDictionary[type];
			}
			catch (KeyNotFoundException innerException)
			{
				throw new KeyNotFoundException($"Type {type.Name} is not registered with the DerivedTypeModelBinder", innerException);
			}
		}
	}
}
