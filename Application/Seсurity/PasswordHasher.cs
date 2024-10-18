using System.Security.Cryptography;
using System.Text;

namespace Application.Seсurity;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword( string password )
    {
        using ( var sha256 = SHA256.Create() )
        {
            byte[] bytes = sha256.ComputeHash( Encoding.UTF8.GetBytes( password ) );
            StringBuilder builder = new StringBuilder();
            foreach ( byte b in bytes )
            {
                builder.Append( b.ToString( "x2" ) );
            }
            return builder.ToString();
        }
    }
}
