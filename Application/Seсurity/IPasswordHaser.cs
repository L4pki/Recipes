namespace Application.Seсurity;

public interface IPasswordHasher
{
    string HashPassword( string password );
}
