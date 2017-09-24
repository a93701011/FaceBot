using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Threading;

namespace FaceRegonizeBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {

        private const string FaceAge = "FaceAge";

        private const string FaceVerify = "FaceVerify";

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }
        
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result;

            if (activity.Text.Contains("help"))
            {
                await context.Forward(new SupportDialog(), this.ResumeAfterSupportDialog, activity, CancellationToken.None);
            }
            else
            {
                this.ShowOptions(context);
            }
        }
        private void ShowOptions(IDialogContext context)
         {
            PromptDialog.Choice(context, this.OnOptionSelected, new List<string>() { FaceAge, FaceVerify }, "Which function you want to use today?", "Not a valid option", 3);     
         }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;

                switch (optionSelected)
                {
                    case FaceAge:
                        context.Call(new FaceDialog(), this.ResumeAfterOptionDialog);
                        break;

                    case FaceVerify:
                        context.Call(new FaceVerifyDialog(), this.ResumeAfterOptionDialog);
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");

                context.Wait(this.MessageReceivedAsync);
            }
        }
        private async Task ResumeAfterSupportDialog(IDialogContext context, IAwaitable<int> result)
        {
            var ticketNumber = await result;

            await context.PostAsync($"Thanks for contacting our support team. Your ticket number is {ticketNumber}.");
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var message = await result;
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Failed with message: {ex.Message}");
            }
            finally
            {
                context.Wait(this.MessageReceivedAsync);
            }
        }
    }
}