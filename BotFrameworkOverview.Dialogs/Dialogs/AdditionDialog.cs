using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotFrameworkOverview.Dialogs.Dialogs
{
    [Serializable]
    public class AdditionDialog : IDialog<object>
    {
        private double _number1 { get; set; }

        public Task StartAsync(IDialogContext context)
        {
            PromptDialog.Number(context, SecondQuestion, "Enter the first number:", "That is an invalid number");
            return Task.CompletedTask;
        }

        //private Task FirstQuestion(IDialogContext context, IAwaitable<object> result)
        //{
        //    PromptDialog.Number(context, SecondQuestion, "Enter the first number:");
        //    return Task.CompletedTask;
        //}

        private async Task SecondQuestion(IDialogContext context, IAwaitable<double> result)
        {
            var answer = await result;
            _number1 = answer;
            PromptDialog.Number(context, AddNumbers, "Then the second number:", "That is an invalid number");
        }

        private async Task AddNumbers(IDialogContext context, IAwaitable<double> result)
        {
            var answer = await result;
            await context.PostAsync($"{_number1} plus {answer} is {_number1 + answer}");

            context.Done<object>(null);
        }
    }
}