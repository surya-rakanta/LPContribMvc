using LPContribMvc.PortableAreas;
using LPContribMvc.UI.InputBuilder.Conventions;
using LPContribMvc.UI.InputBuilder.InputSpecification;
using LPContribMvc.UI.InputBuilder.ViewEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;

namespace LPContribMvc.UI.InputBuilder
{
	public class InputBuilder
	{
		private static Func<IList<IPropertyViewModelFactory>> _propertyConventionProvider = () => new DefaultPropertyConventionsFactory();

		private static Func<IList<ITypeViewModelFactory>> _typeConventionProvider = () => new DefaultTypeConventionsFactory();

		public static Action<VirtualPathProvider> RegisterPathProvider = HostingEnvironment.RegisterVirtualPathProvider;

		public static IList<IPropertyViewModelFactory> Conventions => _propertyConventionProvider();

		public static IList<ITypeViewModelFactory> TypeConventions => _typeConventionProvider();

		public static void BootStrap()
		{
			if (!((IEnumerable<IViewEngine>)ViewEngines.Engines).Any((IViewEngine engine) => ((object)engine).GetType().Equals(typeof(InputBuilderViewEngine))))
			{
				VirtualPathProvider obj = new AssemblyResourceProvider();
				RegisterPathProvider(obj);
				AssemblyResourceStore assemblyResourceStore = new AssemblyResourceStore(typeof(PortableAreaRegistration), "/areas", typeof(PortableAreaRegistration).Namespace);
				AssemblyResourceManager.RegisterAreaResources(assemblyResourceStore);
				((Collection<IViewEngine>)(object)ViewEngines.Engines).Add((IViewEngine)(object)new InputBuilderViewEngine(new string[0]));
			}
		}

		public static void SetPropertyConvention(Func<IList<IPropertyViewModelFactory>> conventionProvider)
		{
			_propertyConventionProvider = conventionProvider;
		}

		public static void SetTypeConventions(Func<IList<ITypeViewModelFactory>> conventionProvider)
		{
			_typeConventionProvider = conventionProvider;
		}
	}
}
