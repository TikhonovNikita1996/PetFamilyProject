using CSharpFunctionalExtensions;
using FluentValidation;
using Pet.Family.SharedKernel;

namespace PetFamily.Core.Validation;

public static class CustomValidation
{
    public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
        this IRuleBuilder<T, TElement> ruleBuilder,
        Func<TElement, Result<TValueObject, CustomError>> factoryMethod)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            Result<TValueObject, CustomError> result = factoryMethod(value);

            if (result.IsSuccess)
                return;

            context.AddFailure(result.Error.Serialize());
        });
    }

    public static IRuleBuilderOptions<T, TProp> WithError <T, TProp>(
        this IRuleBuilderOptions<T, TProp> rule, CustomError error)
    {
        return rule.WithMessage(error.Serialize());
    }
}