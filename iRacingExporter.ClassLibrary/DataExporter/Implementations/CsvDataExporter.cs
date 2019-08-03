using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRacingExporter.ClassLibrary.DataExporter.Implementations
{
    public class CsvDataExporter : IDataExporter
    {
        private const string SEPARATOR = ",";

        private List<string> _names;
        private string _path;
        private List<List<object>> _telemetryInfos;

        public CsvDataExporter(string path, List<string> names)
        {
            _path = path;
            _names = names;
            _telemetryInfos = Cache.TelemetryInfos;
        }

        public CsvDataExporter(string path, List<string> names, List<List<object>> telemetryInfos)
        {
            _path = path;
            _names = names;
            _telemetryInfos = telemetryInfos;
        }

        public void Export()
        {
            var csvFile = new StringBuilder();

            csvFile.AppendLine(CreateHeader());
            csvFile.AppendLine(CreateBody());

            File.WriteAllText(_path + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv", csvFile.ToString());
        }

        private string CreateHeader()
        {
            return string.Join(SEPARATOR, _names);
        }

        private string CreateBody()
        {
            var body = new StringBuilder();

            foreach (var telemetyInfo in _telemetryInfos)
            {
                body.AppendLine(string.Join(SEPARATOR, telemetyInfo));
            }

            return body.ToString();
        }
    }
}
