namespace ChatMicrosoft.Models
{
    public class MessageGPT
    {

        public string Content { get; set; }
        public bool IsAIResponse { get; set; }
        public string UserImage { get; set; }
        public string AiImage { get; set; }
        public DateTime date { get; set; }

        public string FileIconUrl { get; set; }

        public string AiResponse { get; set; }

    }
}
