using System;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;

namespace LPContribMvc.Services
{
	public class EmailTemplateService : IEmailTemplateService
	{
		private class EmailDetails
		{
			public string Subject
			{
				get;
				set;
			}

			public string Body
			{
				get;
				set;
			}
		}

		private class ViewStreamReader : IViewStreamReader
		{
			public Stream GetViewStream(string viewName, object model, ControllerContext context)
			{
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0046: Expected O, but got Unknown
				//IL_0046: Expected O, but got Unknown
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Expected O, but got Unknown
				IView view = ViewEngines.Engines.FindPartialView(context, viewName).View;
				if (view == null)
				{
					throw new InvalidOperationException($"Could not find a view named '{viewName}'");
				}
				StringBuilder stringBuilder = new StringBuilder();
				using (StringWriter stringWriter = new StringWriter(stringBuilder))
				{
					ViewContext val = (ViewContext)(object)new ViewContext(context, view, (ViewDataDictionary)(object)new ViewDataDictionary(model), (TempDataDictionary)(object)new TempDataDictionary(), (TextWriter)stringWriter);
					view.Render(val, (TextWriter)stringWriter);
					stringWriter.Flush();
				}
				return new MemoryStream(Encoding.UTF8.GetBytes(stringBuilder.ToString()));
			}
		}

		private readonly IViewStreamReader _viewReader;

		public EmailTemplateService()
		{
			_viewReader = new ViewStreamReader();
		}

		public EmailTemplateService(IViewStreamReader viewReader)
		{
			_viewReader = viewReader;
		}

		public MailMessage RenderMessage(string viewName, EmailMetadata metadata, ControllerContext context)
		{
			EmailDetails emailDetails = GetEmailDetails(viewName, metadata, context);
			MailMessage result = new MailMessage
			{
				From = metadata.From,
				Subject = emailDetails.Subject,
				Body = emailDetails.Body,
				IsBodyHtml = metadata.IsHtmlEmail
			};
			metadata.To.ForEach(delegate(MailAddress x)
			{
				result.To.Add(x);
			});
			metadata.Cc.ForEach(delegate(MailAddress x)
			{
				result.CC.Add(x);
			});
			metadata.Bcc.ForEach(delegate(MailAddress x)
			{
				result.Bcc.Add(x);
			});
			return result;
		}

		private EmailDetails GetEmailDetails(string viewName, EmailMetadata metadata, ControllerContext context)
		{
			using (Stream stream = _viewReader.GetViewStream(viewName, metadata, context))
			{
				string subject = "";
				string text = "";
				using (StreamReader streamReader = new StreamReader(stream))
				{
					bool flag = false;
					string text2;
					while ((text2 = streamReader.ReadLine()) != null)
					{
						if (!flag)
						{
							if (!string.IsNullOrEmpty(text2))
							{
								subject = text2;
								flag = true;
							}
						}
						else
						{
							text += text2;
						}
					}
				}
				EmailDetails emailDetails = new EmailDetails();
				emailDetails.Body = text;
				emailDetails.Subject = subject;
				return emailDetails;
			}
		}
	}
}
