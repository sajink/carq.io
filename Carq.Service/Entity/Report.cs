namespace Carq.Data
{
    using Microsoft.Azure.Cosmos.Table;

    public class Report : TableEntity
    {
        //PK - Connection
        //RK - Time (granularity depends on table)
        public string RtoDetails { get; set; }
        public string ServiceDetails { get; set; }
    }
}
