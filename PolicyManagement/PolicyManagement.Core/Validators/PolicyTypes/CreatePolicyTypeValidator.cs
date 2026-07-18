using FluentValidation;
using PolicyManagement.Core.Models.Requests.PolicyTypes;

namespace PolicyManagement.Core.Validators.PolicyTypes
{
    public class CreatePolicyTypeRequestValidator
       : AbstractValidator<CreatePolicyTypeRequest>
    {
        public CreatePolicyTypeRequestValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty()
                .WithMessage("Policy name is required.")
                .MaximumLength(200)
                .WithMessage(
                    "Policy name must not exceed 200 characters.");

            RuleFor(request => request.Description)
                .MaximumLength(1000)
                .WithMessage(
                    "Description must not exceed 1000 characters.")
                .When(request =>
                    !string.IsNullOrWhiteSpace(request.Description));
        }
    }
}
