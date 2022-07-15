namespace ReckonTechTest.Shared.Models
{
    public class CandidateModel
    {
        public string Candidate { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public List<ResultModel> Results { get; set; } = new List<ResultModel>();

    }
}
