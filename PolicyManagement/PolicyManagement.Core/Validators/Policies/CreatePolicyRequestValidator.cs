using FluentValidation;
using PolicyManagement.Core.Models.Requests.Policies;

namespace PolicyManagement.Core.Validators.Policies
{

    public class CreatePolicyRequestValidator
        : AbstractValidator<CreatePolicyRequest>
    {
        public CreatePolicyRequestValidator()
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

            RuleFor(request => request.PolicyTypeGuid)
                .NotEmpty()
                .WithMessage("Policy type is required.");

            RuleFor(request => request.EffectiveDate)
                .NotEmpty()
                .WithMessage("Effective date is required.");

            RuleFor(request => request.ExpiryDate)
                .NotEmpty()
                .WithMessage("Expiry date is required.")
                .GreaterThan(request => request.EffectiveDate)
                .WithMessage(
                    "Expiry date must be later than the effective date.");
        }
    }
}
