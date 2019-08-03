using CommandLine;
using iRacingExporter.ClassLibrary;
using iRacingExporter.ClassLibrary.DataExporter.Implementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace iRacingExporter.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Exporter exporter = null;

            new Thread(() =>
            {
                while (true)
                {
                    var cki = System.Console.ReadKey(true);
                    if (cki.Key == ConsoleKey.S)
                        exporter?.Save();

                    Thread.Sleep(1);
                }
            }).Start();


            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       //StartExport(o.Filter, o.Output);

                       var filters = File.ReadAllLines(o.Filter).ToList();
                       exporter = new Exporter(new CsvDataExporter(o.Output, filters));
                       exporter.Fps = 30;
                       exporter.Filters = filters;

                       System.Console.WriteLine("iRacingExporter started");
                       System.Console.WriteLine($"\tFps set to {exporter.Fps}");
                       System.Console.WriteLine($"\tFilter path set to {o.Filter}");
                       System.Console.WriteLine($"\t\tFiltering for [{string.Join(",", filters)}]");

                       exporter.Start();
                   });
        }

        private static void StartExport(string filterPath, string outputPath)
        {
            var filters = File.ReadAllLines(filterPath).ToList();

            var exporter = new Exporter(new CsvDataExporter(outputPath, filters));
            exporter.Fps = 30;
            exporter.Filters = filters;

            System.Console.WriteLine("iRacingExporter started");
            System.Console.WriteLine($"\tFps set to {exporter.Fps}");
            System.Console.WriteLine($"\tFilter path set to {filterPath}");
            System.Console.WriteLine($"\t\tFiltering for [{string.Join(",", filters)}]");

            exporter.Start();
        }
    }
}
