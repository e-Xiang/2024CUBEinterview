namespace CubeInterviewAPI.Models
{
    public class CoindeskResponse
    {
        public Time Time { get; set; }
        public Bpi Bpi { get; set; }
    }

    public class Time
    {
        public string Updated { get; set; }
    }

    public class Bpi
    {
        public CurrencyDetail USD { get; set; }
        public CurrencyDetail GBP { get; set; }
        public CurrencyDetail EUR { get; set; }
    }

    public class CurrencyDetail
    {
        public string Code { get; set; }
        public string Rate { get; set; }
    }
}