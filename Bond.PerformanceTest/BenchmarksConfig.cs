using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;

namespace Bond.PerformanceTest
{
    public class BenchmarksConfig : ManualConfig
    {
        public BenchmarksConfig()
        {
            Add(RankColumn.Arabic);
            Add(StatisticColumn.OperationsPerSecond);
            Add(CsvMeasurementsExporter.Default);
            Add(RPlotExporter.Default);
        }
    }
}