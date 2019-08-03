using iRacingExporter.ClassLibrary.DataExporter;
using iRacingSdkWrapper;
using iRacingSdkWrapper.Bitfields;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRacingExporter.ClassLibrary
{
    public class Exporter
    {
        private int _fps;
        public int Fps
        {
            get { return _fps; }
            set
            {
                if (1 <= value && value <= 60) _fps = value;
                else _fps = 30;
            }
        }
        public List<string> Filters { get; set; }

        private ISdkWrapper _sdk;
        private IDataExporter _dataExporter;
        private readonly object _lock = new object();

        public Exporter(IDataExporter dataExporter)
        {
            _dataExporter = dataExporter;
            //_sdk = new SdkWrapper();
            _sdk = new iRacingMock.ClassLibrary.Mock();

            _sdk.Connected += SdkOnConnected;
            _sdk.Disconnected += SdkOnDisconnected;
            _sdk.TelemetryUpdated += SdkOnTelemetryUpdated;
            _sdk.SessionInfoUpdated += SdkOnSessionInfoUpdated;
        }

        public void Start()
        {
            _sdk.Start();

            Console.WriteLine($"\tiRacingSDK started, waiting for connection");
        }

        public void Save()
        {
            Console.WriteLine($"\tExporting data");

            lock (_lock)
            {                
                _dataExporter.Export();
                Cache.TelemetryInfos.Clear();
            }

            Console.WriteLine($"\tData exported");
        }

        private void SdkOnConnected(object sender, EventArgs e)
        {
            Console.WriteLine($"\tiRacingSDK connected.");
        }

        private void SdkOnDisconnected(object sender, EventArgs e)
        {
            Console.WriteLine($"\tiRacingSDK disconnected.");

            Save();

            _sdk.Stop();
        }

        private void SdkOnTelemetryUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            lock (_lock)
            {
                var telemetryInfos = new List<object>();

                foreach (var filter in Filters)
                {
                    var telemetryInfo = _sdk.GetData(filter);
                    telemetryInfos.Add(telemetryInfo);
                }

                Cache.TelemetryInfos.Add(telemetryInfos);
            }
        }

        private void SdkOnSessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
        }
    }
}
