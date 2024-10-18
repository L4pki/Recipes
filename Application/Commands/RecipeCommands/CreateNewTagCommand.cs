using Application.Models.Result;
using MediatR;

namespace Application.Commands.RecipeCommands;
public class CreateNewTagCommand : IRequest<TagResult>
{
    public string Name { get; set; }

    public CreateNewTagCommand( string name )
    {
        Name = name;
    }
}

