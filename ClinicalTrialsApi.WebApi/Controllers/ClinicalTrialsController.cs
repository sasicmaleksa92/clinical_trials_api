using ClinicalTrials.Application.UseCases.ClinicalTrials.Commands.CreateClinicalTrialCommand;
using ClinicalTrials.Application.UseCases.ClinicalTrials.Queries.GetClinicalTrialById;
using ClinicalTrials.Application.UseCases.ClinicalTrials.Queries.GetClinicalTrialsFiltered;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> UploadClinicalTrial(IFormFile clinicalTrialFile)
        {
            var command = new CreateClinicalTrialCommand(clinicalTrialFile);
            var result = await _mediator.Send(command);

            if(!result.IsSuccess)
            {
                return BadRequest(new { Message = result.Error });
            }

            return CreatedAtAction(nameof(UploadClinicalTrial), result.Value);
        }

        [Route("GetById/{id}")]
        [HttpGet]
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

        [Route("GetFiltered")]
        [HttpGet]
        public async Task<IActionResult> GetFiltered([FromQuery] string status, [FromQuery] string title, [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate, [FromQuery] int? participants)
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
