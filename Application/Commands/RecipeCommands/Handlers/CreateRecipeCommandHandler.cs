using Application.Models.Result;
using Domain.Entities.RecipeEntities;
using Domain.Interfaces.RecipeInterfaces;
using MediatR;

namespace Application.Commands.RecipeCommands.Handlers;

public class CreateRecipeCommandHandler : IRequestHandler<CreateRecipeCommand, RecipeResult>
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IIngridientRepository _ingridientRepository;
    private readonly IStepRepository _stepRepository;
    private readonly ITagRepository _tagRepository;

    public CreateRecipeCommandHandler(
        IRecipeRepository recipeRepository,
        IIngridientRepository ingridientRepository,
        IStepRepository stepRepository,
        ITagRepository tagRepository )
    {
        _recipeRepository = recipeRepository;
        _ingridientRepository = ingridientRepository;
        _stepRepository = stepRepository;
        _tagRepository = tagRepository;
    }

    public async Task<RecipeResult> Handle( CreateRecipeCommand request, CancellationToken cancellationToken )
    {
        if ( string.IsNullOrWhiteSpace( request.Name ) ||
        string.IsNullOrWhiteSpace( request.ShortDescription ) ||
        string.IsNullOrWhiteSpace( request.PhotoUrl ) ||
        request.NumberOfPersons <= 0 ||
        request.TimeCosts == 0 ||
        request.Ingridients == null ||
        request.Steps == null ||
        request.Tags == null )
        {
            return new RecipeResult( null, "Ошибка: Все поля обязательны для заполнения." );
        }

        Recipe newRecipe = new Recipe
        {
            Name = request.Name,
            ShortDescription = request.ShortDescription,
            PhotoUrl = request.PhotoUrl,
            IdAuthor = request.IdAuthor,
            AuthorName = request.AuthorName,
            TimeCosts = request.TimeCosts,
            NumberOfPersons = request.NumberOfPersons
        };

        foreach ( var tag in request.Tags )
        {
            var tagInDb = await _tagRepository.GetByNameAsync( tag.Name, cancellationToken );
            if ( tagInDb == null )
            {
                return new RecipeResult( null, "Ошибка: Несуществующий тег!" );
            }
            newRecipe.Tags.Add( tagInDb );
        }

        Recipe recipe = await _recipeRepository.CreateAsync( newRecipe, cancellationToken );

        foreach ( var ingridient in request.Ingridients )
        {
            var newIngridient = new Ingridient
            {
                Title = ingridient.Title,
                Description = ingridient.Description,
                IdRecipe = recipe.Id
            };

            await _ingridientRepository.CreateAsync( newIngridient, cancellationToken );
        }

        int numberOfStep = 0;
        foreach ( var step in request.Steps )
        {
            numberOfStep++;
            var newStep = new Step
            {
                Description = step.Description,
                NumberOfStep = numberOfStep,
                IdRecipe = recipe.Id
            };

            await _stepRepository.CreateAsync( newStep, cancellationToken );
        }

        return new RecipeResult( newRecipe, "Рецепт успешно создан!" );

    }
}
