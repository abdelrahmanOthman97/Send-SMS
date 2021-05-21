using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vonage;
using Vonage.Request;
using Send_SMS_Task.Models;

namespace Send_SMS_Task.DAL
{
    public class SMSOP
    {
        Innovs_IT_TaskEntities db;
        public List<UserMessage> UserMessages;
        public Credentials credentials;
        public VonageClient VonageClient;
        public SMSOP()
        {
             credentials = Credentials.FromApiKeyAndSecret(
                    "07f67afd",
                    "BmBye4nt60cUIG58"
                    );
             VonageClient = new VonageClient(credentials);
            db = new Innovs_IT_TaskEntities();
            UserMessages = new List<UserMessage>();
        }
        public void Send(Message message)
        {
            var Users = db.Users.AsNoTracking().OrderBy(x => x.id);
            int UserCount = (int)Math.Ceiling((double)Users.Count() / 10);// In General
            for (int i = 0; i < UserCount; i++)
            {
                var _users = Users.Skip(i * 10).Take(10).ToList();

                foreach (var user in _users)
                {
                    try
                    {
                        var response = VonageClient.SmsClient.SendAnSms(new Vonage.Messaging.SendSmsRequest()
                        {
                            To = (user.mobile).ToString(),
                            From = "Innovs-IT",
                            Text = message.smsContent
                        });


                       UserMessages.Add(new UserMessage() { userId = user.id, messageId = message.id, });

                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
            db.UserMessages.AddRange(UserMessages);
            db.SaveChanges();
        }


        #region old
        //int n = 3,m=0;
        //while (n <= 4)
        //{
        //    users = db.Users.OrderBy(u => u.id).Skip(m).Take(n).ToList();
        //    for (int i = 0; i < 2; i++)
        //    {
        //        try
        //        {
        //            var response = VonageClient.SmsClient.SendAnSms(new Vonage.Messaging.SendSmsRequest()
        //            {
        //                To = (users[i].mobile).ToString(),
        //                From = "Innovs-IT",
        //                Text = message.smsContent
        //            });


        //            SendSuccess ss = new SendSuccess() { userId = users[i].id, messageId = message.id, };
        //            db.SendSuccesses.Add(ss);
        //        }
        //        catch (Exception e)
        //        {
        //            throw e;
        //        }
        //    }
        //    n += 2;
        //    m += 2;
        //}
        //db.SaveChanges();

        #endregion
    }
}