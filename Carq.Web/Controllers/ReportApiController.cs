namespace Carq.Ops.Controllers
{
    using Carq.Data;
    using Carq.Service;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    [Authorize]
    //[ClaimRequired(ClaimTypes.Role, Roles.CarqUser)]
    [Route("api/report")]
    public class ReportApiController : BaseController
    {
        private ReportService _report;
        public ReportApiController(IMemoryCache cache, ReportService report) =>
            (_cache, _report) = (cache, report);

        [HttpGet("{regn}")]
        public async Task<IActionResult> GetServiceReport(string regn)
        {
            if (string.IsNullOrEmpty(regn)) return BadRequest();
            regn = regn.Replace("-", "").ToUpperInvariant();
            var report = await WithCache($"Report-{regn}", 300, async () =>
            {
                try
                {
                    var exists = await _report.GetOne(regn);
                    if (exists != null) return exists;
                }
                catch { }
                var result = await GetReportFromVendor(regn);
                await _report.Create(result);
                return result;
            });

            return Json(report);
        }

        private async Task<Report> GetReportFromVendor(string regn)
        {
            string rto, svc = "";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("rc_no", regn),
                new KeyValuePair<string, string>("consent", "Y")
            });

            rto = await GetApi("https://uat.aadrila.com/api/v1/rc-proplus", content);

            try
            {
                dynamic json = JsonConvert.DeserializeObject(rto);
                json = JsonConvert.DeserializeObject(json.data.ToString());
                var maker = json.rc_maker_desc.ToString();
                if (maker.IndexOf("MARUTI") > 0)
                    svc = await GetApi("https://uat.aadrila.com/api/v1/service-history-maruti", content);
                else if (maker.IndexOf("HYUNDAI") > 0)
                    svc = await GetApi("https://uat.aadrila.com/api/v1/service-history-hyundai", content);
            }
            catch { }

            return new Report
            {
                PartitionKey = regn.Substring(0, 6),
                RowKey = regn,
                RtoDetails = rto,
                ServiceDetails = svc
            };
        }

        private async Task<string> GetApi(string uri, FormUrlEncodedContent content)
        {
            var http = new HttpClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "3266a00234807bf0c280c55f16c6c632");
            var request = new HttpRequestMessage()
            {
                Content = content,
                Method = HttpMethod.Post,
                RequestUri = new Uri(uri)
            };

            using HttpResponseMessage response = await http.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

    }
}