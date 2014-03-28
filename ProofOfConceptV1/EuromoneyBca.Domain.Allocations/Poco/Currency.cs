namespace EuromoneyBca.Domain.Allocations.Poco
{
    public  class Currency
    {
        public int Id { get; set; }
        public string currency_uri { get; set; }
        public string currency_code { get; set; }
        public string currency_symbol { get; set; }
        public string currency_label { get; set; }
    }
}
