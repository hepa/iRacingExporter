using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRacingExporter.Console
{
    class Options
    {
        [Option('f', "filter", Required = false, HelpText = "Set the filter file which includes the telemetry info names to be saved.")]
        public string Filter { get; set; }

        [Option('o', "output", Required = true, HelpText = "Set the the output path where the exported file will be saved.")]
        public string Output { get; set; }
    }
}
