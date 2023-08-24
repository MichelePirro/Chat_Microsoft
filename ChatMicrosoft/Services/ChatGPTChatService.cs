using ChatMicrosoft.Models;
using OpenAI_API;
using OpenAI_API.Chat;


namespace ChatMicrosoft.Data
{
    public class ChatGPTChatService
    {
        private readonly ChatGPTConnectionService connectionService;
        private readonly List<MessageGPT> chatMessages = new List<MessageGPT>();
        private readonly OpenAIAPI api;
        private readonly Conversation conversation;

        public ChatGPTChatService(ChatGPTConnectionService connectionService)
        {

            this.connectionService = connectionService ?? throw new ArgumentNullException(nameof(connectionService));
            this.api = connectionService.GetAPI();
            this.conversation = api.Chat.CreateConversation();
            Allena();
        }

        public async Task<string> GetResponse(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new NullReferenceException(nameof(query));
            }

            conversation.AppendUserInput(query);
            var aiResponse = await conversation.GetResponseFromChatbotAsync();
            return aiResponse;

        }

        private void Allena()
        {
            // give instruction as System
            conversation.AppendSystemMessage("You are James T. Kirk. Admiral of the star fleet and commander dell'Enterprise star ship");

            //// give a few examples as user and assistant
            conversation.AppendUserInput("Chi sei?");
            conversation.AppendExampleChatbotOutput("Sono il capitano James T. Kirk. della nave spaziale StarTrek");

            conversation.AppendUserInput("Chi sono i membri dell'equipaggio dell'Enterprise?");
            conversation.AppendExampleChatbotOutput("Ci sono diversi componenti dell'equipaggio tra qui: Mr. Spok, amico e il secondo ufficiale proveniente dal pianeta Vulcano, la signorina Hulu che si occupa del centralino, il maggiore Jackson e molti altri.");

            conversation.AppendUserInput("A che velocità può viaggiare l'Enterprise?");
            conversation.AppendExampleChatbotOutput("La nave spaziale Enterprise é tra le più moderne ed equipaggiate. Può viaggiare a velocità prossime a quelle della luce quando entra in modalità curvatura");
        }

        public async Task AddRequest(string UserInput)
        {
            chatMessages.Add(new MessageGPT { Content = UserInput, IsAIResponse = false, UserImage = "/user.png", date = DateTime.Now });
            await Task.CompletedTask;
        }

        public async Task AddBotResponse(string response)
        {
            chatMessages.Add(new MessageGPT { Content = response, IsAIResponse = true, AiImage = "/ai.png", date = DateTime.Now });
            await Task.CompletedTask;
        }

        public async Task AddBotError(string response)
        {
            chatMessages.Add(new MessageGPT { Content = response, IsAIResponse = true, AiImage = "/error.png", date = DateTime.Now });
            await Task.CompletedTask;
        }

        public async Task AddFileMessage(MessageGPT message)
        {
            chatMessages.Add(message);
            await Task.CompletedTask;
        }

        public List<MessageGPT> GetMessages()
        {
            return chatMessages;
        }

        public async Task CaricaContenuto(string content)
        {
            conversation.AppendSystemMessage(content);
            await Task.CompletedTask;
        }
    }
}