using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace mvc_app.DbContext
{
    public class UserContext:IdentityDbContext
    {
        public UserContext(DbContextOptions<UserContext> options): base(options) { }
    }
}
