using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Yandex.Taxi.Gateway.Contracts.Tracks;
using Yandex.Taxi.Model.Tracks;
using YAXLib;

namespace Yandex.Taxi.Gateway.Core
{
    public class TracksGateway : ITracksGateway
    {
        private readonly Uri mTracksURI;

        private readonly string mClid;

        private readonly YAXSerializer mSerializer;

        private readonly Logger mLogger = LogManager.GetCurrentClassLogger();

        public TracksGateway(string sTracksURI, string sClid)
        {
            this.mTracksURI = new Uri(sTracksURI);
            this.mClid = sClid;
            this.mSerializer = new YAXSerializer(typeof(Tracks));
        }

        public void SendTracks(ITracks tracks)
        {
            string sTracks = mSerializer.Serialize(Map(tracks, mClid));

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.ExpectContinue = false;

                // TODO: Compression
                var content = new FormUrlEncodedContent(new[] { 
                new KeyValuePair<string, string>("compressed", "0"),
                new KeyValuePair<string, string>("data", sTracks)
                    });

                //mLogger.Debug("SendTracks: {0}\r\nContent:{1}", mTracksURI, sTracks);
                // TODO: Reliable POST
                var result = client.PostAsync(mTracksURI, content).Result;
                //if (!result.IsSuccessStatusCode)
                //{
                //    throw new Exception("Send tracks failed: status code: " + result.StatusCode);
                //}
            }
        }

        private static Tracks Map(ITracks tracks, string sClid)
        {
            Tracks result = new Tracks();
            result.Clid = sClid;
            result.Items.AddRange(Map(tracks.Tracks));
            return result;
        }

        private static IEnumerable<Track> Map(IEnumerable<ITrack> tracks)
        {
            List<Track> result = new List<Track>();
            foreach (var track in tracks)
            {
                result.Add(Map(track));
            }
            return result;
        }

        private static Track Map(ITrack track)
        {
            Track result = new Track();
            result.Uuid = track.Uuid;
            result.Points.AddRange(Map(track.Points));
            return result;
        }

        private static IEnumerable<Model.Tracks.Point> Map(IEnumerable<Contracts.Tracks.Point> points)
        {
            List<Model.Tracks.Point> result = new List<Model.Tracks.Point>();
            foreach (var point in points)
            {
                result.Add(Map(point));
            }
            return result;
        }

        private static Model.Tracks.Point Map(Contracts.Tracks.Point point)
        {
            Model.Tracks.Point result = new Model.Tracks.Point();
            result.AverageSpeed = point.AverageSpeed;
            result.Direction = point.Direction;
            result.Latitude = point.Latitude;
            result.Longitude = point.Longitude;
            result.Time = point.Time;
            return result;
        }
    }
}
