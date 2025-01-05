using Application.Models.Result;
using Domain.Entities.RecipeEntities;
using Domain.Interfaces.RecipeInterfaces;
using MediatR;

namespace Application.Commands.RecipeCommands.Handlers;

public class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommand, RecipeResult>
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IIngridientRepository _ingridientRepository;
    private readonly IStepRepository _stepRepository;
    private readonly ITagRepository _tagRepository;

    public UpdateRecipeCommandHandler(
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

    public async Task<RecipeResult> Handle( UpdateRecipeCommand request, CancellationToken cancellationToken )
    {
        /*if ( string.IsNullOrWhiteSpace( request.Name ) ||
         string.IsNullOrWhiteSpace( request.ShortDescription ) ||
         string.IsNullOrWhiteSpace( request.PhotoUrl ) ||
         request.NumberOfPersons == 0 ||
         request.TimeCosts == TimeSpan.Zero ||
         request.Ingridients.Length == 0 ||
         request.Steps.Length == 0 ||
         request.Tags.Length == 0 )
         {

             return new RecipeResult( null, "Ошибка: Все поля обязательны для заполнения." );
         }*/
        if ( string.IsNullOrWhiteSpace( request.Name ) )
        {
            return new RecipeResult( null, "Название рецепта обязательно для заполнения." );
        }

        if ( string.IsNullOrWhiteSpace( request.ShortDescription ) )
        {
            return new RecipeResult( null, "Краткое описание рецепта обязательно для заполнения." );
        }

        if ( string.IsNullOrWhiteSpace( request.PhotoUrl ) )
        {
            return new RecipeResult( null, "URL изображения обязателен для заполнения." );
        }

        if ( request.NumberOfPersons <= 0 )
        {
            return new RecipeResult( null, "Количество персон должно быть больше нуля." );
        }

        if ( request.TimeCosts == TimeSpan.Zero )
        {
            return new RecipeResult( null, "Время приготовления должно быть указано." );
        }

        if ( request.Ingridients == null )
        {
            return new RecipeResult( null, "Ингредиенты обязательны для заполнения." );
        }

        if ( request.Steps == null )
        {
             return new RecipeResult( null, "Шаги приготовления обязательны для заполнения." );
        }

        if ( request.Tags == null )
        {
            return new RecipeResult( null, "Теги обязательны для заполнения." );
        }

        Recipe newRecipe = await _recipeRepository.GetByIdAsync( request.IdRecipe, cancellationToken );

        if( newRecipe == null )
        {
            return new RecipeResult( null, "Ошибка: Рецепт не найден." );
        }

        newRecipe.Name = request.Name;
        newRecipe.ShortDescription = request.ShortDescription;
        newRecipe.PhotoUrl = request.PhotoUrl;
        newRecipe.IdAuthor = request.IdAuthor;
        newRecipe.TimeCosts = request.TimeCosts;
        newRecipe.NumberOfPersons = request.NumberOfPersons;

        Recipe recipe = await _recipeRepository.UpdateAsync( request.IdRecipe, newRecipe, cancellationToken );

        foreach ( var tag in request.Tags )
        {
            var tagInDb = await _tagRepository.GetByNameAsync( tag.Name, cancellationToken );
            if ( tagInDb == null )
            {
                return new RecipeResult( null, "Ошибка: Несуществующий тег!" );
            }
            recipe.Tags.Add( tagInDb );
        }

        var existingIngridients = await _ingridientRepository.GetByRecipeIdAsync( recipe.Id, cancellationToken );
        foreach ( var existingIngridient in existingIngridients )
        {
            await _ingridientRepository.DeleteAsync( existingIngridient, cancellationToken );
        }

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

        var existingSteps = await _stepRepository.GetByRecipeIdAsync( recipe.Id, cancellationToken );
        foreach ( var existingStep in existingSteps )
        {
            await _stepRepository.DeleteAsync( existingStep, cancellationToken );
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

        return new RecipeResult( recipe, "Рецепт успешно обновлен!" );

    }
}
