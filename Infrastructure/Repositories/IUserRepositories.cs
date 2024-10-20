using Domain.Entities;

namespace Domain.Repositories;
public interface IUserRepositories
{
    public void Create(User user);
    public void Delete(User user);
}
