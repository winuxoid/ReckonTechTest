using ReckonTechTest.Shared.Models;

namespace ReckonTechTest.Api.Services
{
    public class CandidateResultsService : ICandidateResultsService
    {
        private readonly HttpClient _httpClient;

        public CandidateResultsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<T> GetCandidateAsync<T>(string path)
        {
            try
            {
                var res = await _httpClient.GetFromJsonAsync<T>(path);
                return res;
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        public async Task<HttpResponseMessage> SaveCandidateAsync(string path, CandidateModel data)
        {
            try
            {
                var res = await _httpClient.PostAsJsonAsync(path, data);
                return res;
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }


        public void FindTextOccurences(CandidateModel candidate, SubTextModel subText)
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

        public void AddIndexOfOccurences(CandidateModel candidate, string strTarget, List<string> arrStringResult)
        {
            candidate.Results.Add(new ResultModel
            {
                SubText = strTarget,
                Result = arrStringResult.Any() ? String.Join(", ", arrStringResult) : "<No Output>"
            });
        }
    }
}
