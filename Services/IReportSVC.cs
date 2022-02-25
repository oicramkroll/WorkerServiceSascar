using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public interface IReportSVC
    {
        void GenerateByDateInterval();
        void Generate(DateTime start, DateTime end);
        JObject GetConfigApp();
    }
}
