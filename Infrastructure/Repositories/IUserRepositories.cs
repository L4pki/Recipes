using Domain.Entities;
public interface IUserRepositories
{
    public void Create( User user );
    public void Delete( User user );
}
