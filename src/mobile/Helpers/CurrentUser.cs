using MauiChat.Models;

namespace MauiChat.Helpers
{
    public static class CurrentUser
    {
        public static User user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "User" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6)
        };
    }
}
