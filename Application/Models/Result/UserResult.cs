using Domain.Entities;

namespace Application.Models.Result;
public class UserResult
{
    public User User { get; set; }
    public string Message { get; set; }

    public UserResult( User user, string message )
    {
        User = user;
        Message = message;
    }
}
