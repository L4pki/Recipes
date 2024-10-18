namespace Application.Models.Result;
public class TagResult
{
    public string Name { get; set; }
    public string Message { get; set; }

    public TagResult( string name, string message )
    {
        Name = name;
        Message = message;
    }
}
