using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Linq;
using System.Net.Http;


namespace FaceRegonizeBot.Dialogs
{
    [Serializable]
    public class FaceDialog : IDialog<object>
    {
        //private readonly IFaceCaptionService captionService = new FaceApiService();
        private FaceMessage facemessage = new FaceMessage();
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("please upload pictur");
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {

            var activity = await result as Activity;
            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            string message;
  
            var imageAttachment = activity.Attachments?.FirstOrDefault(a => a.ContentType.Contains("image"));
            if (imageAttachment != null)
            {
                string reply = await facemessage.GetCaptionAsync(activity, connector);
                message = String.Format("This is a {0} age person", reply);
            }
            else
            {
                message = "Sorry, please upload picture";
            }
            await context.PostAsync(message);

            context.Done(true);
        }
    }
}