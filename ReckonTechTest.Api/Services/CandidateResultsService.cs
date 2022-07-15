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
    }
}
