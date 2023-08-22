using OpenAI_API;

namespace ChatMicrosoft.Data
{
    public class ChatGPTConnectionService
    {
        private readonly OpenAIAPI api;
       
        public ChatGPTConnectionService(IConfiguration config)
        {
            var apiUrl = config.GetValue<string>("ChatGPTSettings:ApiURL");
            var apiKey = config.GetValue<string>("ChatGPTSettings:ApiKey");
            api = new OpenAIAPI(apiKey);
        }
          
        public OpenAIAPI GetAPI()
        {
            return api;
        }
    }
}

