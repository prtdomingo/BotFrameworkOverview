using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;

namespace BotFrameworkOverview.FormFlows.Dialogs
{
    [Serializable]
    public class BookingDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var formsDialog = FormDialog.FromForm(BuildBookingsForm, FormOptions.PromptInStart);
            context.Call(formsDialog, ResumeAfterBooking);
            return Task.CompletedTask;
        }

        private IForm<Booking> BuildBookingsForm()
        {
            return new FormBuilder<Booking>()
                .Message("Welcome to the Booking Helper Bot!")
                .Build();
        }

        private async Task ResumeAfterBooking(IDialogContext context, IAwaitable<Booking> result)
        {
            var response = await result;
            try
            {
                await context.PostAsync($"Your flight going from {response.Origin} to {response.Origin} is now booked {response.Name}!");
            }
            catch(FormCanceledException ex)
            {
                string reply = ex.InnerException == null ? "You have canceled the operation." : $"Something went wrong: {ex.InnerException.Message}";

                await context.PostAsync(reply);
            }
            finally
            {
                context.Done<object>(null);
            }
        }
    }
}