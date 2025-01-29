using ClinicalTrials.Application.UseCases.ClinicalTrials.Commands.CreateClinicalTrialCommand;
using ClinicalTrials.Application.UseCases.ClinicalTrials.Queries.GetClinicalTrialById;
using ClinicalTrials.Application.UseCases.ClinicalTrials.Queries.GetClinicalTrialsFiltered;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ClinicalTrialsApi.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClinicalTrialsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClinicalTrialsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("upload-clinical-trial")]
        [SwaggerOperation(Summary = "Uploads a clinical trial file", Description = "Accepts a file and processes it into a clinical trial record.")]
        [SwaggerResponse(201, "Clinical trial successfully created.")]
        [SwaggerResponse(400, "Bad request, invalid input.")]
        public async Task<IActionResult> UploadClinicalTrial(IFormFile clinicalTrialFile)
        {
            var command = new CreateClinicalTrialCommand(clinicalTrialFile);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { Message = result.Error });
            }

            return CreatedAtAction(nameof(UploadClinicalTrial), result.Value);
        }


        [HttpGet("GetById/{id}")]
        [SwaggerOperation(Summary = "Fetch clinical trial by ID", Description = "Retrieves the details of a clinical trial using its unique identifier.")]
        [SwaggerResponse(200, "Successfully retrieved clinical trial.")]
        [SwaggerResponse(404, "Clinical trial not found.")]
        public async Task<IActionResult> GetById(string id)
        {
            var query = new GetClinicalTrialByIdQuery { TrialId = id };
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return NotFound(new { Message = result.Error });
            }

            return Ok(result.Value);
        }


        [HttpGet("GetFiltered")]
        [SwaggerOperation(Summary = "Fetch clinical trials with filters", Description = "Allows filtering clinical trials based on status, title, dates, and participant count.")]
        [SwaggerResponse(200, "Successfully retrieved filtered clinical trials.")]
        [SwaggerResponse(400, "Invalid request parameters.")]
        public async Task<IActionResult> GetFiltered(
            [FromQuery] string status,
            [FromQuery] string title,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int? participants)
        {
            var query = new GetFilteredClinicalTrialsQuery
            {
                Title = title,
                Status = status,
                StartDate = startDate,
                EndDate = endDate,
                Participants = participants
            };

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { Message = result.Error });
            }

            return Ok(result.Value);
        }
    }
}
