using Microsoft.AspNetCore.Mvc;
using PolicyManagement.Core.Interfaces.Services;
using PolicyManagement.Core.Models.Requests.Policies;
using PolicyManagement.Core.Models.Responses.Common;
using PolicyManagement.Core.Models.Responses.Policies;

namespace PolicyManagement.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PoliciesController : ControllerBase
    {
        private readonly IPolicyService _policyService;

        public PoliciesController(IPolicyService policyService)
        {
            _policyService = policyService;
        }

        [HttpGet]
        [ProducesResponseType(
            typeof(PagedResponse<PolicyResponse>),
            StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResponse<PolicyResponse>>> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var result = await _policyService.GetPagedAsync(
                pageNumber,
                pageSize,
                cancellationToken);

            return Ok(result);
        }

        [HttpGet("{guid:guid}")]
        [ProducesResponseType(
            typeof(PolicyDetailsResponse),
            StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PolicyDetailsResponse>> GetById(
            Guid guid,
            CancellationToken cancellationToken = default)
        {
            var policy = await _policyService.GetByIdAsync(
                guid,
                cancellationToken);

            if (policy is null)
            {
                return NotFound(new
                {
                    message = "Policy not found."
                });
            }

            return Ok(policy);
        }

        [HttpPost]
        [ProducesResponseType(
            typeof(PolicyDetailsResponse),
            StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PolicyDetailsResponse>> Create(
            [FromBody] CreatePolicyRequest request,
            CancellationToken cancellationToken = default)
        {
            var policy = await _policyService.CreateAsync(
                request,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { guid = policy.RecordGuid },
                policy);
        }

        [HttpPut("{guid:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            Guid guid,
            [FromBody] UpdatePolicyRequest request,
            CancellationToken cancellationToken = default)
        {
            var updated = await _policyService.UpdateAsync(
                guid,
                request,
                cancellationToken);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Policy not found."
                });
            }

            return NoContent();
        }

        [HttpDelete("{guid:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            Guid guid,
            CancellationToken cancellationToken = default)
        {
            var deleted = await _policyService.DeleteAsync(
                guid,
                cancellationToken);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Policy not found."
                });
            }

            return NoContent();
        }
    }
}
