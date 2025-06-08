namespace InvestControl.Producer.Model
{
    public class CotacaoItem
    {
        public string Symbol { get; set; } = "";
        public decimal RegularMarketPrice { get; set; }
        public DateTime RegularMarketTime { get; set; }
    }
}
