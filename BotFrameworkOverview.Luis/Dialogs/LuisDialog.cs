using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System.Linq;

namespace BotFrameworkOverview.Luis.Dialogs
{
    [LuisModel("daea4935-5119-4f1f-b73a-53dcd7ca241c", "42c38a98887f472d9b52982ee8d0177b")]
    [Serializable]
    public class LuisDialog : LuisDialog<object>
    {
        private const string _entityNumber = "builtin.number";

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Sorry, I wasn't able to understand {result.Query}");

            context.Wait(MessageReceived);
        }

        [LuisIntent("Operation.Addition")]
        public async Task Addition(IDialogContext context, LuisResult result)
        {
            double total = 0;
            var numbers = result.Entities.Where(e => e.Type == _entityNumber).ToList();

            foreach(var number in numbers)
            {
                total += Convert.ToDouble(number.Resolution.Values.FirstOrDefault() ?? 0);
            }

            await context.PostAsync($"The sum is: {total}");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Operation.Subtraction")]
        public async Task Subtraction(IDialogContext context, LuisResult result)
        {
            double total = 0;
            var numbers = result.Entities.Where(e => e.Type == _entityNumber).ToList();

            for(var i = 0; i < numbers.Count(); i++)
            {
                var value = Convert.ToDouble(numbers[i].Resolution.Values.FirstOrDefault() ?? 0);
                if (i == 0)
                    total = value;
                else
                    total -= value;
            }

            await context.PostAsync($"The difference is: {total}");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Operation.Multiplication")]
        public async Task Multiplication(IDialogContext context, LuisResult result)
        {
            double total = 0;
            var numbers = result.Entities.Where(e => e.Type == _entityNumber).ToList();

            for (var i = 0; i < numbers.Count(); i++)
            {
                var value = Convert.ToDouble(numbers[i].Resolution.Values.FirstOrDefault() ?? 0);
                if (i == 0)
                    total = value;
                else
                    total *= value;
            }

            await context.PostAsync($"The product is: {total}");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Operation.Division")]
        public async Task Division(IDialogContext context, LuisResult result)
        {
            double total = 0;
            var numbers = result.Entities.Where(e => e.Type == _entityNumber).ToList();

            for (var i = 0; i < numbers.Count(); i++)
            {
                var value = Convert.ToDouble(numbers[i].Resolution.Values.FirstOrDefault() ?? 0);
                if (i == 0)
                    total = value;
                else
                    total /= value;
            }

            await context.PostAsync($"The quotient is: {total}");
            context.Wait(MessageReceived);
        }
    }
}