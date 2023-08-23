using ChatMicrosoft.DataBase;
using ChatMicrosoft.Models;
using Newtonsoft.Json;
using OpenAI_API;
using OpenAI_API.Embedding;
using OpenAI_API.Models;
using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Text;


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

        public async Task<List<string>> GetSimilarFilesByContent(string userQuery)
        {
            float[] userEmbedding = await CalculateEmbedding(userQuery);
            double similarityThreshold = 0.8;

            var allFileEmbeddings = await databaseService.GetAllFileEmbeddings();

            List<string> matchingFiles = new List<string>();

            foreach (var fileEmbedding in allFileEmbeddings)
            {
                float[] fileEmbeddingArray = await databaseService.GetFileEmbedding(fileEmbedding.file_name);

                if (fileEmbeddingArray != null)
                {
                    float similarity = CalculateSimilarity(userEmbedding, fileEmbeddingArray);

                    if (similarity >= similarityThreshold)
                    {
                        matchingFiles.Add(fileEmbedding.file_name);
                    }
                }
            }

            return matchingFiles;
        }




        public float CalculateSimilarity(float[] embedding1, float[] embedding2)
        {
            float dotProduct = 0;
            float normEmbedding1 = 0;
            float normEmbedding2 = 0;

            int minLength = Math.Min(embedding1.Length, embedding2.Length);

            for (int i = 0; i < minLength; i++)
            {
                dotProduct += embedding1[i] * embedding2[i];
                normEmbedding1 += embedding1[i] * embedding1[i];
                normEmbedding2 += embedding2[i] * embedding2[i];
            }

            if (normEmbedding1 == 0 || normEmbedding2 == 0)
            {
                return 0; // Evita divisione per zero
            }

            float similarity = dotProduct / (float)(Math.Sqrt(normEmbedding1) * Math.Sqrt(normEmbedding2));

            return similarity;
        }
    }
}



