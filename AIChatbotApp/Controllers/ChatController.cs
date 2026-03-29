using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AIChatbotApp.Models;
using AIChatbotApp.Services;
using AIChatbotApp.Models;

namespace AIChatbotApp.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly GeminiService _geminiService;

        public ChatController(GeminiService geminiService)
        {
            _geminiService = geminiService;
        }

        public IActionResult Index(string? sessionId = null)
        {
            var sessions = GetAllSessions();
            if (string.IsNullOrEmpty(sessionId))
                sessionId = HttpContext.Session.GetString("ActiveSession") ?? CreateNewSession();

            HttpContext.Session.SetString("ActiveSession", sessionId);
            var history = GetSessionHistory(sessionId);

            ViewBag.SessionId = sessionId;
            ViewBag.Sessions = sessions;
            ViewBag.UserName = User.Identity?.Name ?? "User";

            return View(history);
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody] SendMessageRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
                return BadRequest(new { error = "Message cannot be empty." });

            if (request.Message.Length > 1000)
                return BadRequest(new { error = "Message too long (max 1000 characters)." });

            var sessionId = request.SessionId ?? HttpContext.Session.GetString("ActiveSession") ?? CreateNewSession();
            var history = GetSessionHistory(sessionId);

            history.Add(new ChatMessage { Role = "user", Content = request.Message.Trim() });

            try
            {
                var reply = await _geminiService.GetReplyAsync(history);
                history.Add(new ChatMessage { Role = "assistant", Content = reply });
                SaveSessionHistory(sessionId, history);
                UpdateSessionTitle(sessionId, request.Message.Trim());
                return Ok(new { reply, sessionId });
            }
            catch (Exception ex)
            {
                history.RemoveAt(history.Count - 1);
                SaveSessionHistory(sessionId, history);
                return StatusCode(500, new { error = $"AI Error: {ex.Message}" });
            }
        }

        [HttpPost]
        public IActionResult NewChat()
        {
            var sessionId = CreateNewSession();
            HttpContext.Session.SetString("ActiveSession", sessionId);
            return Ok(new { sessionId });
        }

        [HttpPost]
        public IActionResult DeleteSession([FromBody] SessionRequest request)
        {
            var sessions = GetAllSessions();
            sessions.RemoveAll(s => s.Id == request.SessionId);
            SaveAllSessions(sessions);
            HttpContext.Session.Remove($"Chat_{request.SessionId}");
            var nextSession = sessions.FirstOrDefault()?.Id ?? CreateNewSession();
            HttpContext.Session.SetString("ActiveSession", nextSession);
            return Ok(new { nextSessionId = nextSession });
        }

        [HttpPost]
        public IActionResult SwitchSession([FromBody] SessionRequest request)
        {
            HttpContext.Session.SetString("ActiveSession", request.SessionId);
            var history = GetSessionHistory(request.SessionId);
            return Ok(new { messages = history });
        }

        // ── PRIVATE HELPERS ──

        private string UserKey => User.Identity?.Name ?? "guest";

        private string CreateNewSession()
        {
            var sessionId = Guid.NewGuid().ToString("N")[..8];
            var sessions = GetAllSessions();
            sessions.Insert(0, new ChatSession
            {
                Id = sessionId,
                Title = "New Chat",
                CreatedAt = DateTime.Now
            });
            SaveAllSessions(sessions);
            return sessionId;
        }

        private List<ChatSession> GetAllSessions()
        {
            var json = HttpContext.Session.GetString($"Sessions_{UserKey}");
            if (string.IsNullOrEmpty(json)) return new List<ChatSession>();
            return System.Text.Json.JsonSerializer.Deserialize<List<ChatSession>>(json) ?? new();
        }

        private void SaveAllSessions(List<ChatSession> sessions)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(sessions);
            HttpContext.Session.SetString($"Sessions_{UserKey}", json);
        }

        private List<ChatMessage> GetSessionHistory(string sessionId)
        {
            var json = HttpContext.Session.GetString($"Chat_{sessionId}");
            if (string.IsNullOrEmpty(json)) return new List<ChatMessage>();
            return System.Text.Json.JsonSerializer.Deserialize<List<ChatMessage>>(json) ?? new();
        }

        private void SaveSessionHistory(string sessionId, List<ChatMessage> history)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(history);
            HttpContext.Session.SetString($"Chat_{sessionId}", json);
        }

        private void UpdateSessionTitle(string sessionId, string firstMessage)
        {
            var sessions = GetAllSessions();
            var session = sessions.FirstOrDefault(s => s.Id == sessionId);
            if (session != null && session.Title == "New Chat")
            {
                session.Title = firstMessage.Length > 30
                    ? firstMessage[..30] + "..."
                    : firstMessage;
                SaveAllSessions(sessions);
            }
        }
    }

    // ── REQUEST MODELS (outside controller class, inside namespace) ──

    public class SendMessageRequest
    {
        public string Message { get; set; } = "";
        public string? SessionId { get; set; }
    }

    public class SessionRequest
    {
        public string SessionId { get; set; } = "";
    }
}