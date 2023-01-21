using ChattingApp.Hub;
using ChattingApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChattingApp.Controllers;

[Route("api/chat")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly ILogger<ChatController> _logger;

    public ChatController(IHubContext<ChatHub> hubContext, ILogger<ChatController> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendRequest([FromBody] MessageDto msg)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.SelectMany(x => 
                    x.Value!.Errors.SelectMany(error 
                        => error.ErrorMessage))
                .ToList();
            return BadRequest(new { message = "Validation failed", errors });
        }

        //Send message trough SignalR
        await _hubContext.Clients.All.SendAsync("ReceiveOne", msg.User, msg.MsgText);
        _logger.LogInformation($"Message sent from: {msg.User}, message: {msg.MsgText}");
        return Ok();
    }
}