using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace LPContribMvc.PortableAreas
{
	public class PortableAreaMap
	{
		public const string MasterPageTemplate = "MasterPageFile=\"{0}\"";

		public const string ContentPlaceHolderTemplate = "ContentPlaceHolderID=\"{0}\"";

		public const string ContentPlaceHolderPattern = "<asp:Content .*ContentPlaceHolderID=\"{0}.*>";

		protected Dictionary<string, string> _mappings = new Dictionary<string, string>();

		public string DefaultMasterPageLocation
		{
			get;
			set;
		}

		public string DefaultTitleID
		{
			get;
			set;
		}

		public string DefaultBodyID
		{
			get;
			set;
		}

		public string MasterPageLocation
		{
			get;
			set;
		}

		public string Title
		{
			get
			{
				if (_mappings.ContainsKey(DefaultTitleID))
				{
					return _mappings[DefaultTitleID];
				}
				return null;
			}
		}

		public string Body
		{
			get
			{
				if (_mappings.ContainsKey(DefaultBodyID))
				{
					return _mappings[DefaultBodyID];
				}
				return null;
			}
		}

		public PortableAreaMap()
		{
			DefaultMasterPageLocation = "~/Views/Shared/Site.Master";
			DefaultTitleID = "TitleContent";
			DefaultBodyID = "MainContent";
		}

		public Stream Transform(Stream stream)
		{
			string text = string.Empty;
			using (StreamReader streamReader = new StreamReader(stream))
			{
				text = TransformMarkup(streamReader.ReadToEnd());
			}
			Stream stream2 = new MemoryStream(text.Length);
			StreamWriter streamWriter = new StreamWriter(stream2);
			streamWriter.Write(text, 0, text.Length);
			streamWriter.Flush();
			stream2.Position = 0L;
			return stream2;
		}

		protected string TransformMarkup(string input)
		{
			string text = ReplaceMasterPage(input);
			using (Dictionary<string, string>.Enumerator enumerator = _mappings.GetEnumerator())
			{
				KeyValuePair<string, string> pair;
				while (enumerator.MoveNext())
				{
					pair = enumerator.Current;
					KeyValuePair<string, string> keyValuePair = pair;
					string pattern = $"<asp:Content .*ContentPlaceHolderID=\"{keyValuePair.Key}.*>";
					text = Regex.Replace(text, pattern, delegate(Match m)
					{
						KeyValuePair<string, string> keyValuePair2 = pair;
						string oldValue = $"ContentPlaceHolderID=\"{keyValuePair2.Key}\"";
						KeyValuePair<string, string> keyValuePair3 = pair;
						string newValue = $"ContentPlaceHolderID=\"{keyValuePair3.Value}\"";
						return m.Value.Replace(oldValue, newValue);
					});
				}
				return text;
			}
		}

		private string ReplaceMasterPage(string input)
		{
			string oldValue = $"MasterPageFile=\"{DefaultMasterPageLocation}\"";
			string newValue = $"MasterPageFile=\"{MasterPageLocation}\"";
			if (string.IsNullOrEmpty(MasterPageLocation))
			{
				return input;
			}
			return input.Replace(oldValue, newValue);
		}

		public void Add(string defaultID, string newID)
		{
			if (_mappings.ContainsKey(defaultID))
			{
				_mappings[defaultID] = newID;
			}
			else
			{
				_mappings.Add(defaultID, newID);
			}
		}
	}
}
