using ReckonTechTest.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using ReckonTechTest.Api.Services;

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

                FindTextOccurences(candidate, subText);
                candidate.Candidate = "Alwin Bombita";
                return Ok(candidate);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
        }

        private static void FindTextOccurences(CandidateModel candidate, SubTextModel subText)
        {
            foreach (var strTarget in subText.SubTexts)
            {
                List<string> arrStringResult = new List<string>();
                var strIndex = 0;
                foreach (string str in candidate.Text.Split(' '))
                {
                    var curStr = str.Trim();
                    var strLen = 0;
                    foreach (char c in curStr)
                    {
                        strIndex++;
                        if (char.ToLower(c) == char.ToLower(strTarget[strLen]))
                            strLen++;

                        if (strLen == strTarget.Trim().Length)
                        {
                            arrStringResult.Add((strIndex - (strTarget.Trim().Length - 1)).ToString());
                            strLen = 0;
                        }
                    }

                    strIndex++;

                }

                AddIndexOfOccurences(candidate, strTarget, arrStringResult);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CandidateModel>> SaveCandidateResults()
        {
            try
            {
                var results = new List<ResultModel>();
                var candidate = await _candidate.GetCandidateAsync<CandidateModel>("test2/textToSearch");
                var subText = await _candidate.GetCandidateAsync<SubTextModel>("test2/subTexts");

                FindTextOccurences(candidate, subText);
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

        private static void AddIndexOfOccurences(CandidateModel candidate, string strTarget, List<string> arrStringResult)
        {
            candidate.Results.Add(new ResultModel
            {
                SubText = strTarget,
                Result = arrStringResult.Any() ? String.Join(", ", arrStringResult) : "<No Output>"
            });
        }
    }
}