using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LPContribMvc.ActionResults
{
	public class HeadResult : ActionResult
	{
		public HttpStatusCode StatusCode
		{
			get;
			private set;
		}

		public HeadResult(HttpStatusCode statusCode) : base()
		{
			StatusCode = statusCode;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			HttpResponseBase response = context.RequestContext.HttpContext.Response;
			response.StatusCode = (int)StatusCode;
		}
	}
}
