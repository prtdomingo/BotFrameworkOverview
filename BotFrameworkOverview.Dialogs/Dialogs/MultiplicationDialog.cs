using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotFrameworkOverview.Dialogs.Dialogs
{
    [Serializable]
    public class MultiplicationDialog : IDialog<object>
    {
        private double _number1 { get; set; }

        public Task StartAsync(IDialogContext context)
        {
            PromptDialog.Number(context, SecondQuestion, "Enter the first number:", "Please include a valid number");
            return Task.CompletedTask;
        }

        private async Task SecondQuestion(IDialogContext context, IAwaitable<double> result)
        {
            _number1 = await result;
            PromptDialog.Number(context, GetProduct, "Enter the second number:", "Please include a valid number");
        }

        private async Task GetProduct(IDialogContext context, IAwaitable<double> result)
        {
            var answer = await result;
            await context.PostAsync($"{_number1} times {answer} is {_number1 * answer}");

            context.Done<object>(null);
        }
    }
}