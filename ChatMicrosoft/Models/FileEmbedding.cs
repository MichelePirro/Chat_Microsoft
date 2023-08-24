namespace ChatMicrosoft.Models
{
    public class FileEmbedding
    {
        public string file_name {  get; set; }
        public string text_file { get; set; }
        public float[] file_embedding { get; set; }

        public float Similarity { get; set; }

    }
}
