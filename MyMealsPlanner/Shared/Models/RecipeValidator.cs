using FluentValidation;

namespace MyMealsPlanner.Shared.Models
{
    public class RecipeValidator : AbstractValidator<Recipe>
    {
        public RecipeValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(recipe => recipe.Name).NotEmpty().WithMessage("Nazwa jest wymagana")
                .Length(5, 50).WithMessage("Nazwa musi mieć miedzy 5 a 50 znaków");
            RuleFor(recipe => recipe.Ingredients).NotEmpty().WithMessage("Składniki są wymagane");
            RuleFor(recipe => recipe.Description).NotEmpty().WithMessage("Opis jest wymagany");
            RuleForEach(organization => organization.Ingredients).SetValidator(new IngredientValidator());
        }
    }

    public class IngredientValidator : AbstractValidator<Ingredient>
    {
        public IngredientValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(ingredient => ingredient.Name).NotEmpty().WithMessage("Nazwa jest wymagana");
            RuleFor(ingredient => ingredient.Quantity).NotEmpty().WithMessage("Ilość jest wymagana");
        }
    }

}
