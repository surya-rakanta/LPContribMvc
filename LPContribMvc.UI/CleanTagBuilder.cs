using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LPContribMvc.UI
{
	public class CleanTagBuilder : TagBuilder
	{
		public CleanTagBuilder(string tagName) : base(tagName)
		{

		}

		private void TrimAttributes()
		{
			List<KeyValuePair<string, string>> list = ((TagBuilder)this).Attributes.ToList();
			foreach (KeyValuePair<string, string> item in list)
			{
				if (string.IsNullOrEmpty(item.Value))
				{
					((TagBuilder)this).Attributes.Remove(item);
				}
				else
				{
					((TagBuilder)this).Attributes[item.Key] = item.Value.Trim();
				}
			}
		}

		public string ToString(TagRenderMode renderMode)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			TrimAttributes();
			return ((TagBuilder)this).ToString(renderMode);
		}

		public override string ToString()
		{
			TrimAttributes();
			return ((TagBuilder)this).ToString();
		}
	}
}
