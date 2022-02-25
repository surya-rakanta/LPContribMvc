using System;
using System.IO;
using System.Web.Mvc;

namespace LPContribMvc.UI.MenuBuilder
{
	public class MenuItem
	{
		protected bool? internalDisabled;

		protected bool itemSelected;

		public string Title
		{
			get;
			set;
		}

		public string Icon
		{
			get;
			set;
		}

		public string HelpText
		{
			get;
			set;
		}

		public string ActionUrl
		{
			get;
			set;
		}

		public string AnchorClass
		{
			get;
			set;
		}

		public string IconClass
		{
			get;
			set;
		}

		public string ItemClass
		{
			get;
			set;
		}

		public string IconDirectory
		{
			get;
			set;
		}

		public bool ShowWhenDisabled
		{
			get;
			set;
		}

		public string DisabledClass
		{
			get;
			set;
		}

		public bool Disabled
		{
			get;
			set;
		}

		public string SelectedClass
		{
			get;
			set;
		}

		protected bool ItemDisabled => internalDisabled ?? Disabled;

		public bool HideItem
		{
			get
			{
				if (ItemDisabled)
				{
					return !ShowWhenDisabled;
				}
				return false;
			}
		}

		protected bool Prepared
		{
			get;
			set;
		}

		public MenuItem()
		{
			Prepared = false;
		}

		protected virtual string RenderLink()
		{
			CleanTagBuilder cleanTagBuilder = new CleanTagBuilder("a");
			if (ItemDisabled)
			{
				((TagBuilder)cleanTagBuilder).AddCssClass(DisabledClass);
			}
			else
			{
				((TagBuilder)cleanTagBuilder).Attributes["href"] = ActionUrl;
			}
			if (IsItemSelected())
			{
				((TagBuilder)cleanTagBuilder).AddCssClass(SelectedClass);
			}
			((TagBuilder)cleanTagBuilder).Attributes["title"] = HelpText;
			((TagBuilder)cleanTagBuilder).InnerHtml = ((TagBuilder)cleanTagBuilder).InnerHtml + RenderIcon() + RenderTitle();
			return cleanTagBuilder.ToString((TagRenderMode)0);
		}

		public virtual bool IsItemSelected()
		{
			return itemSelected;
		}

		protected virtual string RenderIcon()
		{
			if (!string.IsNullOrEmpty(Icon))
			{
				string value = (IconDirectory ?? "") + Icon;
				CleanTagBuilder cleanTagBuilder = new CleanTagBuilder("img");
				((TagBuilder)cleanTagBuilder).Attributes["border"] = "0";
				((TagBuilder)cleanTagBuilder).Attributes["src"] = value;
				((TagBuilder)cleanTagBuilder).Attributes["alt"] = Title;
				((TagBuilder)cleanTagBuilder).AddCssClass(IconClass);
				return cleanTagBuilder.ToString((TagRenderMode)3);
			}
			return string.Empty;
		}

		protected virtual string RenderTitle()
		{
			if (!string.IsNullOrEmpty(Title))
			{
				return Title;
			}
			return string.Empty;
		}

		public virtual void RenderHtml(ControllerContext requestContext, TextWriter writer)
		{
			Prepare(requestContext);
			writer.Write(RenderHtml());
		}

		public virtual string RenderHtml()
		{
			if (!Prepared)
			{
				throw new InvalidOperationException("Must call Prepare before RenderHtml() or call RenderHtml(RequestContext, TextWriter)");
			}
			if (HideItem)
			{
				return string.Empty;
			}
			CleanTagBuilder cleanTagBuilder = new CleanTagBuilder("li");
			((TagBuilder)cleanTagBuilder).AddCssClass(ItemClass);
			((TagBuilder)cleanTagBuilder).InnerHtml = RenderLink();
			return cleanTagBuilder.ToString(TagRenderMode.Normal);
		}

		public virtual void Prepare(ControllerContext controllerContext)
		{
			Prepared = true;
			if (controllerContext.RequestContext.HttpContext.Request.Path == ActionUrl)
			{
				itemSelected = true;
			}
		}

		public MenuItem SetTitle(string title)
		{
			Title = title;
			return this;
		}

		public MenuItem SetIcon(string icon)
		{
			Icon = icon;
			return this;
		}

		public MenuItem SetHelpText(string helpText)
		{
			HelpText = helpText;
			return this;
		}

		public MenuItem SetActionUrl(string actionUrl)
		{
			ActionUrl = actionUrl;
			return this;
		}

		public MenuItem SetAnchorClass(string anchorClass)
		{
			AnchorClass = anchorClass;
			return this;
		}

		public MenuItem SetIconClass(string iconClas)
		{
			IconClass = iconClas;
			return this;
		}

		public MenuItem SetItemClass(string itemClass)
		{
			ItemClass = itemClass;
			return this;
		}

		public MenuItem SetIconDirectory(string iconDirectory)
		{
			IconDirectory = iconDirectory;
			return this;
		}

		public MenuItem SetDisabled(bool disabled)
		{
			Disabled = disabled;
			return this;
		}

		public MenuItem SetSelectedClass(string selectedClass)
		{
			SelectedClass = selectedClass;
			return this;
		}

		public MenuItem SetDisabledMenuItemClass(string itemClass)
		{
			DisabledClass = itemClass;
			return this;
		}

		public MenuItem SetShowWhenDisabled(bool hide)
		{
			ShowWhenDisabled = hide;
			return this;
		}
	}
}
