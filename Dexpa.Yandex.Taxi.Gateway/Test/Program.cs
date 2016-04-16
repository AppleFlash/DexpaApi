using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Yandex.Taxi.Gateway.Contracts;
using Yandex.Taxi.Gateway.Contracts.Tracks;
using Yandex.Taxi.Gateway.Core;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            TracksGateway tracksGateway = new TracksGateway("http://tst.extjams.maps.yandex.net/taxi_collect/1.x/", "1956789018");
            Gateway gateway = new Gateway("https://ymsh.taxi-partners-test.mobile.yandex.net", "1956789018", "3efab07c5fda49dd9a6794c3198f06c6");

            gateway.SendDriversStatus(new DriverStatus("2", Status.Free));
            tracksGateway.SendTracks(new Tracks());

            string sOrderId = "032d3fa68b904371aa351871d5b3ffa7";

            //gateway.SendReadyToProceedOrder("2", sOrderId);

            //gateway.SendOrderUpdate(sOrderId, OrderStatus.Driving, "2");
            //gateway.SendOrderUpdate(sOrderId, OrderStatus.Waiting);
            //gateway.SendOrderUpdate(sOrderId, OrderStatus.Transporting);
            //gateway.SendOrderUpdate(sOrderId, OrderStatus.Complete, "1500");
        }

        private class Tracks : ITracks
        {
            IEnumerable<ITrack> ITracks.Tracks
            {
                get
                {
                    yield return new Track();
                }
            }

            private class Track : ITrack
            {
                public string Uuid
                {
                    get { return "2"; }
                }

                public IEnumerable<Yandex.Taxi.Gateway.Contracts.Tracks.Point> Points
                {
                    get
                    {
                        yield return new Yandex.Taxi.Gateway.Contracts.Tracks.Point(55.782882M, 37.565518M, 0, 0, DateTime.UtcNow);
                    }
                }
            }
        }
    }
}
