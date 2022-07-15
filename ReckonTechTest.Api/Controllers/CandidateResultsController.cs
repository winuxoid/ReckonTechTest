using Microsoft.AspNetCore.Mvc;
using ReckonTechTest.Api.Services;
using ReckonTechTest.Shared.Models;

namespace ReckonTest.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidateResultsController : ControllerBase
    {
        private readonly ICandidateResultsService _candidate;

        public CandidateResultsController(ICandidateResultsService candidate)
        {
            _candidate = candidate;
        }

        /// <summary>
        /// Finds all the occurrences of a particular set of characters in a string from supplied endpoints as reference data
        /// </summary>
        /// <returns>CandidateModel</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CandidateModel>> GetCandidateResults()
        {
            try
            {
                var results = new List<ResultModel>();
                var candidate = await _candidate.GetCandidateAsync<CandidateModel>("test2/textToSearch");
                var subText = await _candidate.GetCandidateAsync<SubTextModel>("test2/subTexts");

                _candidate.FindTextOccurences(candidate, subText);
                candidate.Candidate = "Alwin Bombita";
                return Ok(candidate);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
        }

        /// <summary>
        /// Finds all the occurrences of a particular set of characters in a string from supplied endpoints as reference data
        /// and saves the result to an external API
        /// </summary>
        /// <returns>CandidateModel</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> SaveCandidateResults()
        {
            try
            {
                var results = new List<ResultModel>();
                var candidate = await _candidate.GetCandidateAsync<CandidateModel>("test2/textToSearch");
                var subText = await _candidate.GetCandidateAsync<SubTextModel>("test2/subTexts");

                _candidate.FindTextOccurences(candidate, subText);
                candidate.Candidate = "Alwin Bombita";
                var response = _candidate.SaveCandidateAsync("test2/submitResults", candidate);
                var responseBody = await response.Result.Content.ReadAsStringAsync();

                return Ok(responseBody);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
        }
    }
}