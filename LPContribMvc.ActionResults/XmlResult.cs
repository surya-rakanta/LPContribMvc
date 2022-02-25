using System.Web.Mvc;
using System.Xml.Serialization;

namespace LPContribMvc.ActionResults
{
	public class XmlResult : ActionResult
	{
		private object _objectToSerialize;

		private XmlAttributeOverrides _xmlAttribueOverrides;

		public object ObjectToSerialize => _objectToSerialize;

		public XmlResult(object objectToSerialize) : base()
		{
			_objectToSerialize = objectToSerialize;
		}

		public XmlResult(object objectToSerialize, XmlAttributeOverrides xmlAttributeOverrides) : base()
		{
			_objectToSerialize = objectToSerialize;
			_xmlAttribueOverrides = xmlAttributeOverrides;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			if (_objectToSerialize != null)
			{
				XmlSerializer xmlSerializer = (_xmlAttribueOverrides == null) ? new XmlSerializer(_objectToSerialize.GetType()) : new XmlSerializer(_objectToSerialize.GetType(), _xmlAttribueOverrides);
				context.HttpContext.Response.ContentType = "text/xml";
				xmlSerializer.Serialize(context.HttpContext.Response.Output, _objectToSerialize);
			}
		}
	}
}
