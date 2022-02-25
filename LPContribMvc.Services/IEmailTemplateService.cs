using System.Net.Mail;
using System.Web.Mvc;

namespace LPContribMvc.Services
{
	public interface IEmailTemplateService
	{
		MailMessage RenderMessage(string viewName, EmailMetadata metadata, ControllerContext context);
	}
}
