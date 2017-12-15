using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Linq;

namespace BotFrameworkOverview.ComputerVision.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        // URL: https://www.customvision.ai/
        private const string _customVisionUrl = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.0/Prediction/62b6f4f3-1855-43ec-978c-8a5ee8a9eaa6/url";
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Prediction-Key", "687677d8866a493a8a97f5268d29d60a");

                var body = new CustomVisionBody { Url = activity.Text };
                var response = await client.PostAsJsonAsync(_customVisionUrl, body);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                if (response.IsSuccessStatusCode)
                {
                    var res = await response.Content.ReadAsAsync<CustomVisionResponse>();

                    // gets the prediction with highest confidence level
                    var tagResult = res.Predictions.FirstOrDefault().Tag;
                    await context.PostAsync($"I think it's {tagResult}");
                }
                else
                {
                    await context.PostAsync($"Something went wrong :(");
                }
            }

            context.Wait(MessageReceivedAsync);
        }
    }
}