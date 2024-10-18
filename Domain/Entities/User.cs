using System.Collections.Generic;

namespace Domain.Entities;

public class User
{
    public int Id { get; private init; }

    private string Login { get; private init; }

    private string Password { get; private init; }

    public string Name { get; private set; }

    public string About { get; set; }



    public User( string login, string password )
    {
        Login = login;
        Password = password;
    }

    public List<Recipes> PersonalRecipes { get; set; } = new List<Recipes>();

    public List<Recipes> FavoriteRecipes { get; set; } = new List<Recipes>();

}
