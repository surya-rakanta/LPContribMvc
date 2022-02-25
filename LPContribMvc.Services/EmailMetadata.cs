using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Mail;

namespace LPContribMvc.Services
{
	public class EmailMetadata
	{
		public MailAddress From
		{
			get;
			set;
		}

		public MailAddress ReplyTo
		{
			get;
			set;
		}

		public List<MailAddress> To
		{
			get;
			private set;
		}

		public List<MailAddress> Cc
		{
			get;
			private set;
		}

		public List<MailAddress> Bcc
		{
			get;
			private set;
		}

		public bool IsHtmlEmail
		{
			get;
			set;
		}

		public NameValueCollection Headers
		{
			get;
			private set;
		}

		public EmailMetadata()
		{
			To = new List<MailAddress>();
			Cc = new List<MailAddress>();
			Bcc = new List<MailAddress>();
			Headers = new NameValueCollection();
			IsHtmlEmail = true;
		}

		public EmailMetadata(MailAddress from, MailAddress to)
			: this()
		{
			From = from;
			To.Add(to);
		}

		public EmailMetadata(string from, string to)
			: this()
		{
			From = new MailAddress(from);
			To.Add(new MailAddress(to));
		}
	}
}
