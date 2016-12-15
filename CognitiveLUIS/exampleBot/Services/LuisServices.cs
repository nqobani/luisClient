using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace exampleBot.Services
{
    public class LuisServices
    {
        LuisService_Base luisService_Base = new LuisService_Base();
        public void addIntent( string appId, string subscriptionKey, string intentName)
        {
            string body = "{\"Name\": \"" + intentName + "\"}";
            luisService_Base.luisAppManager(appId, subscriptionKey, "intents", body);
        }

        public void trainIntent(string appId, string subscriptionKey, string utterance,string selectedIntentName)
        {
            string body = "{\"ExampleText\": \"" + utterance + "\", \"SelectedIntentName\": \"" + selectedIntentName + "\"}";
            luisService_Base.luisAppManager(appId, subscriptionKey, "example", body);
        }

        public void createApp(string subscriptionKey, string cultureCode)
        {
            string body = "{\"Culture\": \"" + cultureCode + "\"}";
            luisService_Base.createLuisApp(subscriptionKey, "apps", body);
        }
        public void trainApplication(string appId, string subscriptionKey)
        {
            string body = "";
            luisService_Base.createLuisApp(subscriptionKey, "train", body);
        }
    }
}