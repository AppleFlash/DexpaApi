using System.Collections.Generic;

namespace Yandex.Taxi.Model.Rates
{
    public class Intervals
    {
        public Intervals()
        {
            IntervalsList = new List<Interval>();
        }

        public RateChoice RateChoice { get; set; }

        public List<Interval> IntervalsList { get; private set; }
    }
}