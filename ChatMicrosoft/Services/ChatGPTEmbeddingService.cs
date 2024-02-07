using ChatMicrosoft.Helpers;
using ChatMicrosoft.Models;
using ChatMicrosoft.Services;
using OpenAI_API;
using OpenAI_API.Embedding;
using OpenAI_API.Models;


namespace ChatMicrosoft.Data
{
    public class ChatGPTEmbeddingService
    {
        private readonly OpenAIAPI api;
        private readonly ChatGPTConnectionService chatService;
        private readonly DatabaseService databaseService;


        public ChatGPTEmbeddingService(ChatGPTConnectionService chatService, DatabaseService databaseService)
        {
            this.chatService = chatService ?? throw new ArgumentNullException(nameof(chatService));
            this.databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            api = new OpenAIAPI();
        }

        public async Task<float[]> CalculateEmbedding(string text)
        {
            var apiInstance = chatService.GetAPI();
            var response = await apiInstance.Embeddings.CreateEmbeddingAsync(new EmbeddingRequest(Model.AdaTextEmbedding, text));
            var data = (float[])response;

            if ((data?.Length ?? 0) == 0)
            {
                throw new Exception("Errore nel calcolo dell'embedding o nella risposta dell'API.");
            }

            return data;
        }

        public async Task<List<FileEmbedding>> GetSimilarFilesByContent(string userQuery)
        {
            float[] userEmbedding = await CalculateEmbedding(userQuery);
            double similarityThreshold = 0.75;

            var allFileEmbeddings = await databaseService.GetAllFileEmbeddings();

            var matchingFiles = new List<FileEmbedding>();

            foreach (var fileEmbedding in allFileEmbeddings)
            {
                float[] fileEmbeddingArray = fileEmbedding.file_embedding;

                float similarity = Vectors.ConsineSimilarity(userEmbedding, fileEmbeddingArray);

                if (similarity >= similarityThreshold)
                {
                    matchingFiles.Add(fileEmbedding);
                }
            }

            return matchingFiles;
        }




       
    }
}



