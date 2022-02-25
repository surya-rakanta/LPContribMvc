using LPContribMvc.UI.InputBuilder;
using LPContribMvc.UI.InputBuilder.ViewEngine;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace LPContribMvc.PortableAreas
{
	public abstract class PortableAreaRegistration : AreaRegistration
	{
		public static Action RegisterEmbeddedViewEngine = delegate
		{
			InputBuilder.BootStrap();
		};

		public static Action CheckAreasWebConfigExists = delegate
		{
			EnsureAreasWebConfigExists();
		};

		public virtual string AreaRoutePrefix => ((AreaRegistration)this).AreaName;

		public virtual PortableAreaMap GetMap()
		{
			return null;
		}

		public virtual void RegisterArea(AreaRegistrationContext context, IApplicationBus bus)
		{
			bus.Send(new PortableAreaStartupMessage(((AreaRegistration)this).AreaName));
			RegisterDefaultRoutes(context);
			RegisterAreaEmbeddedResources();
		}

		public void CreateStaticResourceRoute(AreaRegistrationContext context, string SubfolderName)
		{
			context.MapRoute(((AreaRegistration)this).AreaName + "-" + SubfolderName, AreaRoutePrefix + "/" + SubfolderName + "/{resourceName}", (object)new
			{
				controller = "EmbeddedResource",
				action = "Index",
				resourcePath = "Content." + SubfolderName
			}, (object)null, new string[1]
			{
				"LPContribMvc.PortableAreas"
			});
		}

		public void RegisterDefaultRoutes(AreaRegistrationContext context)
		{
			CreateStaticResourceRoute(context, "Images");
			CreateStaticResourceRoute(context, "Styles");
			CreateStaticResourceRoute(context, "Scripts");
			context.MapRoute(((AreaRegistration)this).AreaName + "-Default", AreaRoutePrefix + "/{controller}/{action}", (object)new
			{
				controller = "default",
				action = "index"
			});
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			RegisterArea(context, Bus.Instance);
			RegisterEmbeddedViewEngine();
			CheckAreasWebConfigExists();
		}

		public void RegisterAreaEmbeddedResources()
		{
			Type type = base.GetType();
			AssemblyResourceStore assemblyResourceStore = new AssemblyResourceStore(type, "/areas/" + ((AreaRegistration)this).AreaName.ToLower(), type.Namespace, GetMap());
			AssemblyResourceManager.RegisterAreaResources(assemblyResourceStore);
		}

		private static void EnsureAreasWebConfigExists()
		{
			string path = HttpContext.Current.Server.MapPath("~/areas/web.config");
			if (!File.Exists(path))
			{
				throw new Exception("Portable Areas require a ~/Areas/Web.config file in your host application. Copy the config from ~/views/web.config into a ~/Areas/ folder.");
			}
		}

		protected PortableAreaRegistration() : base()
		{
		}
	}
}
