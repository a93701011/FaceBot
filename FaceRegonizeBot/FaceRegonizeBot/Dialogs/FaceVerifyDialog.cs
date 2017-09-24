using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Net.Http;


namespace FaceRegonizeBot.Dialogs
{
    [Serializable]
    public class FaceVerifyDialog : IDialog<Object>
    {

        private string Guid1, Guid2;

        private FaceVerify faceverify = new FaceVerify();

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Upload a picture");
            context.Wait(MessageReceivedAsync);
            //return Task.CompletedTask;
        }
        

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            Guid1 = await faceverify.GetCaptionAsync(activity, connector);
            //await context.PostAsync(message);
            await context.PostAsync("Upload another picture");
            context.Wait(MessageReceive2dAsync);
        }

        private async Task MessageReceive2dAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            Guid2 = await faceverify.GetCaptionAsync(activity, connector);
            //await context.PostAsync(message);
            string message;
            if (Guid1 != null | Guid2 != null)
            {
                string  res = await faceverify.GetVerifyAsync(Guid1, Guid2);
                if (res == "True")
                {
                    message = String.Format("These two picture are idential");
                }
                else {
                    message = String.Format("These two picture are two person");
                }
               
            }
            else
            {
                message = "Sorry! Guid1 or Guid2 is null";
            }
            await context.PostAsync(message);

            context.Done(true);
        }   
    }
}