using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Colors.API.Controllers
{
    /// <summary>
    /// An example controller for testing <code>multipart/form-data</code> submission
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(ILogger<StudentsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// View a form
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <param name="formId">Form ID</param>
        /// <returns></returns>
        [HttpGet("{id:int}/forms/{formId:int}")]
        [ProducesResponseType(typeof(FormSubmissionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<FormSubmissionResult> ViewForm(int id, int formId)
        {
            _logger.LogInformation($"viewing the form#{formId} for Student ID={id}");
            await Task.Delay(1000);
            return new FormSubmissionResult { FormId = formId, StudentId = id };
        }

        /// <summary>
        /// Submit a form which contains a key-value pair and a file.
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <param name="form">A form which contains the FormId and a file</param>
        /// <returns></returns>
        [HttpPost("{id:int}/forms")]
        [ProducesResponseType(typeof(FormSubmissionResult), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FormSubmissionResult>> SubmitForm(int id, [FromForm] StudentForm form)
        {
            _logger.LogInformation($"validating the form#{form.FormId} for Student ID={id}");
            _logger.LogInformation($"saving file [{form.StudentFile.FileName}]");
            await Task.Delay(1500);
            _logger.LogInformation("file saved.");
            var result = new FormSubmissionResult { FormId = form.FormId, StudentId = id };
            return CreatedAtAction(nameof(ViewForm), new { id, form.FormId }, result);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpDelete("{id:int}/forms/{formId:int}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<bool> Delete(int id, int formId)
        {
            _logger.LogInformation("This API is hidden in Swagger UI and Swagger JSON doc.");
            _logger.LogInformation($"deleting the form#{formId} for student ID=[{id}]");
            await Task.Delay(1500);
            return true;
        }

        /// <summary>
        /// Get students by residential type
        /// </summary>
        /// <param name="residentialType">Residential Type. **Default**: <code>InState</code>.</param>
        /// <returns></returns>
        [HttpGet("")]
        public ActionResult GetStudentsByResidentialType(ResidentialType residentialType = ResidentialType.InState)
        {
            _logger.LogInformation($"query {residentialType} students");
            if (residentialType == ResidentialType.International)
            {
                _logger.LogInformation("found 10000 students.");
            }
            return Ok();
        }
    }

    public class StudentForm
    {
        [Required] public int FormId { get; set; }
        [Required] public IFormFile StudentFile { get; set; }
    }

    public class FormSubmissionResult
    {
        public int StudentId { get; set; }
        public int FormId { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ResidentialType
    {
        InState,
        OutOfState,
        International
    }
}
