namespace WebAPI.Dto.UserDto;

public class CreateUserDto
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string About { get; set; }
}
