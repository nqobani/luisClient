using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Microsoft.Cognitive.LUIS;
using System.Web;
using System.Text;
using System.Net.Http.Headers;
using exampleBot.Services;

namespace exampleBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        /// 
        private const string appId = "1ad9e321-b4b9-4f2d-8754-07a34503fe26";
        private const string subscriptionKey = "8f91111e54f2423b80e8000273f1ccd6";
        LuisClient luisClient = new LuisClient(appId, subscriptionKey, false);

        LuisServices luisServices_client = new LuisServices();
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                // calculate something for us to return
                int length = (activity.Text ?? string.Empty).Length;

                var luisResponse = await luisClient.Predict(activity.Text);
                string toUser = "";
                if(luisResponse.Intents.Length > 0)
                {
                    switch (luisResponse.Intents[0].Name)
                    {
                        case "book a flight":
                                toUser = "Top Intent Name: " + luisResponse.Intents[0].Name + " \n Score: " + luisResponse.Intents[0].Score;
                                luisServices_client.trainIntent(appId, subscriptionKey, "whats your name", "intent.name");
                            break;
                        case "intent.openApp":
                                toUser = "Top Intent Name: " + luisResponse.Intents[0].Name + " \n Score: " + luisResponse.Intents[0].Score;
                            break;
                        case "intent.greeting":
                                toUser = "Top Intent Name: " + luisResponse.Intents[0].Name + " \n Score: " + luisResponse.Intents[0].Score;
                                luisServices_client.addIntent(appId, subscriptionKey, "intent.name");
                            break;
                        case "None":
                                toUser = "Top Intent Name: " + luisResponse.Intents[0].Name + " \n Score: " + luisResponse.Intents[0].Score;
                                
                            break;
                    }
                }
                else
                {
                    toUser = "The app has no intents";
                }
                
               

                // return our reply to the user
                Activity reply = activity.CreateReply($"{toUser}");
                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
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
    }
}