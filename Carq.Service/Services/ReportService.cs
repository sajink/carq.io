namespace Carq.Service
{
    using Az.Storage;
    using Carq.Data;

    public class ReportService : AzDataServiceBase<Report>
    {
        public ReportService(AzureStorageContext context) : base(context)
            => _split = 6;
    }
}
