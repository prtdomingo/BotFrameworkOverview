using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;

namespace BotFrameworkOverview.Cards.Dialogs
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
                .Build();
        }

        private async Task ResumeAfterBooking(IDialogContext context, IAwaitable<Booking> result)
        {
            var response = await result;
            try
            {
                var receiptCard = new ReceiptCard
                {
                    Title = "Ticket Itinerary",
                    Facts = new List<Fact>
                    {
                        new Fact("Flight Number", "WK 2200"),
                        new Fact("From", $"{response.Origin} - {response.DepartureDate}"),
                        new Fact("To", $"{response.Destination} - {response.ReturnDate}"),
                        new Fact("Payment Method", "PayPal")
                    },
                    Items = new List<ReceiptItem>
                    {
                        new ReceiptItem("Meals", price: "PHP 539.00"),
                        new ReceiptItem("Number of Passengers", quantity: $"{response.AdultPax + response.ChildrenPax}", price: "PHP 1000.00"),

                    },
                    Tax = "PHP 300.00",
                    Total = "PHP 1839.00"
                };

                var message = context.MakeMessage();
                message.Attachments.Add(receiptCard.ToAttachment());
                await context.PostAsync(message);
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