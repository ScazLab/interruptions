
using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class mono_gmail : MonoBehaviour
{
    private MainGameController gameController;
    private SmtpClient smtpServer;
    private MailMessage mail;
    private bool sendMail = true;


    void Start()
    {
        Debug.Log("End of the game");
        gameController = GameObject.Find("MainGameController").GetComponent<MainGameController>();

        mail = new MailMessage();

        mail.From = new MailAddress("interruptionstraining@gmail.com");
        mail.To.Add("interruptionstraining@gmail.com");
        //mail.Subject = gameController.part_id;
        mail.Body = gameController.sb.ToString();

        smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential("interruptionstraining@gmail.com", "keepondancing") as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
        //smtpServer.Send(mail);
        //Debug.Log("success");

    }

    void Update()
    {
      print(gameController.part_id);
      if(gameController.part_id!=""&&sendMail)
      {
        mail.Subject = gameController.part_id;
        smtpServer.Send(mail);

        Debug.Log("success");
        sendMail = false;
      }
    }
}
