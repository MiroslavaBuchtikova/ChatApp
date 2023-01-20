using ChattingApp.DbAccess;
using ChattingApp.Hub;
using ChattingApp.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChattingApp.Controllers;

[Route("api/chat")]
[Authorize]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly ILogger<ChatController> _logger;
    private readonly IDbContextFactory<ChattingDbContext> _dbContextFactory;
    private readonly SignInManager<IdentityUser> _signInManager;

    public ChatController(IHubContext<ChatHub> hubContext, ILogger<ChatController> logger, IDbContextFactory<ChattingDbContext> dbContextFactory, SignInManager<IdentityUser> signInManager)
    {
        _hubContext = hubContext;
        _logger = logger;
        _dbContextFactory = dbContextFactory;
        _signInManager = signInManager;
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

        //Save message to DB
        var message = new ChatMessage
        {
            MessageText = msg.MsgText,
            UserId = (await _signInManager.UserManager.FindByEmailAsync(User.Claims.FirstOrDefault(x => x.Type.Contains("email"))?.Value)).Id,
            SentAt = DateTime.UtcNow
        };

        await using var context = await _dbContextFactory.CreateDbContextAsync();
        await context.ChatMessages.AddAsync(message);
        await context.SaveChangesAsync();

        //Send message trough SignalR
        await _hubContext.Clients.All.SendAsync("ReceiveOne", msg.User, msg.MsgText);
        _logger.LogInformation($"Message sent from: {msg.User}, message: {msg.MsgText}");
        return Ok();
    }

    [HttpGet]
    public IActionResult GetLastMessages([FromQuery] int limit = 10)
    {
        switch (limit)
        {
            case <= 0:
                return BadRequest("Limit has to be greater than 0");
            case > 1000:
                return BadRequest("Azure databases are expensive!");
        }

        using var dbContext = _dbContextFactory.CreateDbContext();
        var messages = dbContext.ChatMessages.OrderByDescending(x => x.SentAt).Take(limit).ToList();
        return Ok(messages);
    }
}