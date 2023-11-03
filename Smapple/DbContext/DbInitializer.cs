using Smapple.Extensions;
using Smapple.Models;

namespace Smapple.DbContext;

public static class DbInitializer
{
    public static void Initialize(SmappleDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Users.Any())
        {
            return;   // DB has been seeded
        }

        context.Users.Add(new User()
        {
            Email = "admin@admin.com",
            Password = "test".GetHash(),
            Role = RoleEnum.Admin,
            IsVerified = true,
            NickName = "admin",
        });
        context.SaveChanges();
    }

    public static void CreateDbIfNotExists(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            //Resolve dbcontext with DI help
            var dbcontext = (SmappleDbContext)scope.ServiceProvider.GetService(typeof(SmappleDbContext));

            //call your static method herer

            Initialize(dbcontext);
        }
}
}