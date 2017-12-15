using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotFrameworkOverview.Dialogs.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private string[] _mathOperations = new string[] { "Add", "Subtract", "Multiplication" };

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(PromptUserToSelectOperation);

            return Task.CompletedTask;
        }

        private async Task PromptUserToSelectOperation(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result;

            PromptDialog.Choice(context, SelectMathOperation, _mathOperations, "Please select a Math operation", "That's an invalid operation, please select the valid operation");
        }

        private async Task SelectMathOperation(IDialogContext context, IAwaitable<string> result)
        {
            var message = await result;
            switch(message.ToLower().Trim())
            {
                case "add":
                    context.Call(new AdditionDialog(), EndMessageAsync);
                    break;
                case "subtract":
                    context.Call(new SubtractionDialog(), EndMessageAsync);
                    break;
                case "multiplication":
                    context.Call(new MultiplicationDialog(), EndMessageAsync);
                    break;
            }
        }

        private Task EndMessageAsync(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(PromptUserToSelectOperation);
            return Task.CompletedTask;
        }
    }
}