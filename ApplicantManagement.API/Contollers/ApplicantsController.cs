using ApplicantManagement.Application.DTOs;
using ApplicantManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApplicantManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ApplicantsController : ControllerBase
    {
        private readonly IApplicantService _applicantService;
        private readonly ILogger<ApplicantsController> _logger;

        public ApplicantsController(
            IApplicantService applicantService,
            ILogger<ApplicantsController> logger)
        {
            _applicantService = applicantService;
            _logger = logger;
        }


        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ApplicantDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Getting all applicants");
            var response = await _applicantService.GetAllApplicantsAsync();

            if (!response.Success)
            {
                _logger.LogError("Error getting applicants: {Message}", response.Message);
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ApplicantDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Getting applicant with ID: {Id}", id);
            var response = await _applicantService.GetApplicantByIdAsync(id);

            if (!response.Success)
            {
                _logger.LogWarning("Applicant with ID {Id} not found", id);
                return NotFound(response);
            }

            return Ok(response);
        }


        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ApplicantDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateApplicantDto createDto)
        {
            _logger.LogInformation("Creating new applicant");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _applicantService.CreateApplicantAsync(createDto);

            if (!response.Success)
            {
                _logger.LogError("Error creating applicant: {Message}", response.Message);
                return BadRequest(response);
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = response.Data?.ID },
                response);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ApplicantDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] ApplicantDto updateDto)
        {
            _logger.LogInformation("Updating applicant with ID: {Id}", id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _applicantService.UpdateApplicantAsync(id, updateDto);

            if (!response.Success)
            {
                _logger.LogWarning("Failed to update applicant with ID {Id}: {Message}", id, response.Message);
                return NotFound(response);
            }

            return Ok(response);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting applicant with ID: {Id}", id);

            var response = await _applicantService.DeleteApplicantAsync(id);

            if (!response.Success)
            {
                _logger.LogWarning("Failed to delete applicant with ID {Id}: {Message}", id, response.Message);
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}