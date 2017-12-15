using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotFrameworkOverview.Cards
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new Dialogs.BookingDialog());
            }
            else
            {
                await HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task<Activity> HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
                if (message is IConversationUpdateActivity iConversationUpdated)
                {
                    ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));

                    foreach (var member in iConversationUpdated.MembersAdded ?? Array.Empty<ChannelAccount>())
                    {
                        // if the bot is added, then 
                        if (member.Id == iConversationUpdated.Recipient.Id)
                        {
                            var replyToConversation = message.CreateReply();
                            replyToConversation.Attachments = new List<Attachment>
                            {
                                CreateHeroCard()
                            };
                            await connector.Conversations.SendToConversationAsync(replyToConversation);
                        }
                    }
                }
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }


        private static Attachment CreateHeroCard()
        {
            var heroCard = new HeroCard
            {
                Title = "Flight Booking Bot",
                Subtitle = "I'm a Bot that does books flight",
                Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                Images = new List<CardImage> { new CardImage("https://placeholdit.imgix.net/~text?txtsize=35&txt=Flight+Booking+Bot&w=350&h=350") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "Know more about me", value: "https://www.google.com") }
            };

            return heroCard.ToAttachment();
        }

    }
}