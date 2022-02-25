using System.IO;
using System.Web.Mvc;

namespace LPContribMvc.Services
{
	public interface IViewStreamReader
	{
		Stream GetViewStream(string viewName, object model, ControllerContext context);
	}
}
