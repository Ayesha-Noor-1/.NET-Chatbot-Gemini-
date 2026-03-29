namespace AIChatbotApp.Models
{
    // One chat message
    public class ChatMessage
    {
        public string Role { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }

    // One chat session (conversation)
    public class ChatSession
    {
        public string Id { get; set; } = "";
        public string Title { get; set; } = "New Chat";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    // Gemini API request models
    public class GeminiRequest
    {
        public List<GeminiContent> contents { get; set; } = new();
    }

    public class GeminiContent
    {
        public string role { get; set; } = "";
        public List<GeminiPart> parts { get; set; } = new();
    }

    public class GeminiPart
    {
        public string text { get; set; } = "";
    }

    public class GeminiResponse
    {
        public List<GeminiCandidate>? candidates { get; set; }
    }

    public class GeminiCandidate
    {
        public GeminiContent? content { get; set; }
    }

    // Auth models
    public class UserMessageRequest
    {
        public string Message { get; set; } = "";
    }
}