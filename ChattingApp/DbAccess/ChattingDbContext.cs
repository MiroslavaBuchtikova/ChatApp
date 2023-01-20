using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace ChattingApp.DbAccess
{
    public class ChattingDbContext : IdentityDbContext<IdentityUser>
    {
        //TODO: custom entities
        public ChattingDbContext(DbContextOptions<ChattingDbContext> options) : base(options)
        {
            
        }

        public DbSet<ChatMessage> ChatMessages { get; set; }
    }
}
