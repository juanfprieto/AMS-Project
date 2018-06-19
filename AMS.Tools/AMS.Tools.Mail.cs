using System;
using System.Net.Mail;
using System.Text;
using System.Net;
using System.Configuration;

namespace AMS.Tools
{
	/// <summary>
	/// Objeto para envio de correos electrónicos
	/// </summary>
	public class Mail
	{
		private string mensajes;

		public string Mensajes{get{return mensajes;}}

		public Mail()
		{
			mensajes="";
		}

		/// <summary>
		/// Función que permite enviar un correo
		/// </summary>
		/// <param name="from">Correo Origen</param>
		/// <param name="to">Correo Destino</param>
		/// <param name="subject">Asunto</param>
		/// <param name="body">Cuerpo del correo</param>
		/// <param name="bodyFormat">Formato del Cuerpo, puede tomar valores de la enumeración TipoCorreo</param>
		/// <param name="SMTPserver">Servidor de Correo</param>
		/// <param name="rutaArchivo">Ruta de archivo adjunto (Si existe)</param>
		/// <param name="URLBase">URL Base para aplicación de objetos embebidos y hojas de estilo</param>
		/// <param name="password">Password del correo desde el cual se envia</param>
		/// <returns>bool true si el envio se hizo satisfactoriamente; false si ocurrio algun error durante el envio</returns>
		public bool EnviarMail(string from, string to, string subject, string body, TipoCorreo bodyFormat, string SMTPserver, string rutaArchivo, string URLBase, string password)
		{
			bool error = false;
			try
			{
                SmtpClient client = new SmtpClient(SMTPserver, 587);
                //SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(from, password);
				MailMessage oMsg = new MailMessage(from, to, subject, body);
                oMsg.IsBodyHtml = bodyFormat == TipoCorreo.HTML;
				
                if(rutaArchivo != "")
					oMsg.Attachments.Add(new Attachment(rutaArchivo));
				if(URLBase != "")
                    oMsg.Headers.Add("Content-Base", URLBase);

				client.Send(oMsg);
                client.Dispose();
			}
			catch(Exception e)
			{
				error = true;
                mensajes = e.ToString() + "\n" + SMTPserver;
			}
			return error;
		}
        public int EnviarMail(string to, string subject, string body, TipoCorreo bodyFormat, string rutaArchivo)
        {
            string from = ConfigurationManager.AppSettings["EmailFrom"];
            string SMTPserver = ConfigurationManager.AppSettings["MailServer"];
            int SMTPport = Convert.ToInt32(ConfigurationManager.AppSettings["MailServerPort"]);
            string password = ConfigurationManager.AppSettings["PasswordEMail"];
            int resultado = 0;

            try
            {
                SmtpClient client = new SmtpClient(SMTPserver, SMTPport);
                client.EnableSsl = true; //comentar para caso normal. quitar comment para gmail.
                client.UseDefaultCredentials = false; //comentar caso normal. quitar comment para gmail.
                client.Credentials = new NetworkCredential(from, password);
                //client.Timeout = 10000;
                //client.DeliveryMethod = SmtpDeliveryMethod.Network;

                MailMessage oMsg = new MailMessage(from, to, subject, body);
                oMsg.IsBodyHtml = bodyFormat == TipoCorreo.HTML;
                oMsg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                if (rutaArchivo != "")
                    oMsg.Attachments.Add(new Attachment(rutaArchivo));

                client.Send(oMsg);
                client.Dispose();
                resultado = 1;

            }
            catch (Exception e)
            {
                throw new Exception(String.Format("Error al enviar el correo [From:{0};To:{1};SMTP:{2}:{3}] excepcion: {4}",
                    from, to, SMTPserver, SMTPport, e.Message));
            }

            return resultado;

        }
    }

	/// <summary>
	/// Enumeración donde se manejan los tipos de correo necesario
	/// </summary>
	public enum TipoCorreo
	{
		HTML,
		TEXT
	};
}
