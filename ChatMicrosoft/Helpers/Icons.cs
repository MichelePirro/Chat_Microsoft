namespace ChatMicrosoft.Helpers
{
    public static class Icons
    {
        public static string GetFileIconUrl(string fileExtension)
        {
            Dictionary<string, string> fileIcons = new Dictionary<string, string>
            {
                { ".txt", "/image/icon-txt.png" },
                { ".pdf", "/image/icon-pdf.png" },
                { ".doc", "/image/icon-doc.png" },
                { ".docx", "/image/icon-docx.png" }
            };

            if (fileIcons.TryGetValue(fileExtension.ToLower(), out string iconUrl))
            {
                return iconUrl;
            }

            return "/image/icons/default-icon.png";
        }

    }
}