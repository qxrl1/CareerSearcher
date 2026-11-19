using Asp.Versioning;
using CareerSearcher.Application.UseCases.Jobs.Queries.GetJobs;
using CareerSearcher.Domain.Common;
using CareerSearcher.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CareerSearcher.API.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("api/v{v:apiVersion}/[controller]")]
public class JobsController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(Pagination<Job>), StatusCodes.Status200OK)]
    public async Task<ActionResult<Pagination<Job>>> GetJobs(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        var response = await _mediator.Send(new GetJobsCommand(page, pageSize, search));
        
        if (response.IsFailure)
            return BadRequest();

        return Ok(response.Value);
    }
}
