using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PolicyManagement.Core.Interfaces.Services;
using PolicyManagement.Core.Models.Requests.PolicyTypes;
using PolicyManagement.Core.Models.Responses.PolicyTypes;

namespace PolicyManagement.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PolicyTypeController : ControllerBase
    {
        private readonly IPolicyTypeService _policyTypeService;
        private readonly IValidator<CreatePolicyTypeRequest> _createPolicyTypeValidator;

        public PolicyTypeController(IPolicyTypeService policyTypeService,
    IValidator<CreatePolicyTypeRequest> createPolicyTypeValidator)
        {
            _policyTypeService = policyTypeService;
            _createPolicyTypeValidator = createPolicyTypeValidator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PolicyTypeResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PolicyTypeResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var policyTypes = await _policyTypeService.GetAllAsync(cancellationToken);
            return Ok(policyTypes);
        }

        [HttpPost]
        [ProducesResponseType(
            typeof(PolicyTypeResponse),
            StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PolicyTypeResponse>> Create(
            [FromBody] CreatePolicyTypeRequest request,
            CancellationToken cancellationToken = default)
        {
            await _createPolicyTypeValidator.ValidateAndThrowAsync(request, cancellationToken);

            var policyType =
            await _policyTypeService.CreateAsync(
                request,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetAll),
                new { recordGuid = policyType.RecordGuid },
                policyType);
        }
    }
}
