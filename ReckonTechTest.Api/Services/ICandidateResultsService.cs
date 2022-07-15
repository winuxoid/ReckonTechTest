using ReckonTechTest.Shared.Models;

namespace ReckonTechTest.Api.Services
{
    public interface ICandidateResultsService
    {
        Task<T> GetCandidateAsync<T>(string path);
        Task<HttpResponseMessage> SaveCandidateAsync(string path, CandidateModel model);
        void FindTextOccurences(CandidateModel candidate, SubTextModel subText);
    }
}
