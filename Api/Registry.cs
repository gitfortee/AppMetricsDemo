using App.Metrics;
using App.Metrics.Histogram;

namespace Api
{
    public static class Registry
    {
        public static HistogramOptions One = new HistogramOptions
        {
            Name = "test1",
            MeasurementUnit = Unit.Bytes,
            Context = "test"
        };

        public static HistogramOptions Three = new HistogramOptions
        {
            Name = "test3",
            MeasurementUnit = Unit.Bytes,
            Context = "test"
        };

        public static HistogramOptions Two = new HistogramOptions
        {
            Name = "test2",
            MeasurementUnit = Unit.Bytes,
            Context = "test"
        };
    }
}
