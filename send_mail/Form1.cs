﻿using System;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace send_mail
{
    public partial class Form1 : Form
    {

        #region Constructor

        public Form1()
        {
            InitializeComponent();
        }

        #endregion

        #region Metodos

        private void enviarMail(
                                string pCorreoOrigen,
                                string pContrasenaOrigen,
                                string pServSMTP,
                                int pPuertoSMPT,
                                string pCorreoDestino,
                                string pAsunto,
                                string pAdjunto,
                                string pMensaje,
                                bool pHtml)
        {
            try
            {

                MailMessage lMensaje = new MailMessage(pCorreoOrigen, pCorreoDestino)
                {
                    Subject = pAsunto,
                    IsBodyHtml = pHtml,
                    Body = pMensaje,
                };
                if (pAdjunto != null)
                {
                    lMensaje.Attachments.Add(new Attachment(pAdjunto));
                }

                SmtpClient lCorreo = new SmtpClient()
                {
                    Port = pPuertoSMPT,
                    Host = pServSMTP,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(pCorreoOrigen, pContrasenaOrigen)
                };

                lCorreo.Send(lMensaje);

                MessageBox.Show("Listo");
            }
            catch (Exception lExcp)
            {
                MessageBox.Show(lExcp.Message);
            }
        }

        public static bool verificarConexion()
        {
            Uri Url = new Uri("http://www.microsoft.com");

            WebRequest lSolicitud;
            WebResponse lRespuesta;
            lSolicitud = WebRequest.Create(Url);

            try
            {
                lRespuesta = lSolicitud.GetResponse();
                lRespuesta.Close();
                lSolicitud = null;
                return true;
            }
            catch
            {
                lSolicitud = null;
                return false;
            }
        }

        public static bool verificarConexion2()
        {
            try
            {
                using (var lClienteWeb = new WebClient())
                using (lClienteWeb.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool verificarConexion3()
        {

            try
            {
                string Url = "www.google.com";

                Ping ping = new Ping();

                PingReply respuesta = ping.Send(Url, 3000);

                return (respuesta.Status == IPStatus.Success);
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Botones

        private void btnAdjunto_Click(object sender, EventArgs e)
        {
            OpenFileDialog lArchivo = new OpenFileDialog()
            {
                InitialDirectory = Environment.SpecialFolder.Desktop.ToString(),
                Multiselect = false
            };

            DialogResult lResult = lArchivo.ShowDialog();

            if (lResult != DialogResult.Cancel)
            {
                txtAdjunto.Text = lArchivo.FileName;
            }
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            if (verificarConexion3())
            {
                string lCorreoOrigen = txtOrigen.Text.Trim();
                string lContrasenaOrigen = txtPassword.Text.Trim();
                string lServSMTP = txtServidor.Text.Trim();
                int lPuertoSMPT = Convert.ToInt32(txtPuerto.Value);
                string lCorreoDestino = txtDestino.Text.Trim();
                string lAsunto = txtAsunto.Text.Trim();
                string lAdjunto = txtAdjunto.Text.Length > 0 ? txtAdjunto.Text : null;
                string lMensaje = txtMensaje.Text;
                bool lHtml = chkHtml.Checked;


                Action act = () =>
                {
                    enviarMail(
                        lCorreoOrigen,
                        lContrasenaOrigen,
                        lServSMTP,
                        lPuertoSMPT,
                        lCorreoDestino,
                        lAsunto,
                        lAdjunto,
                        lMensaje,
                        lHtml);
                };

                waitForm enviar = new waitForm(act, "Enviando Correo Electrónico . . . ");
                enviar.ShowDialog();

            }
            else
            {
                MessageBox.Show("No hay conexión a internet\nNo se puede enviar el correo", "Atención");
            }
        }

        #endregion

    }
}