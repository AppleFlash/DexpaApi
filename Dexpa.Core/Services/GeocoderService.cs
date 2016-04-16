using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;
using Newtonsoft.Json;

namespace Dexpa.Core.Services
{
    public class GeocoderService : IGeocoderService
    {
        public List<SearchResult> ReverseGeocoding(string query)
        {
            List<SearchResult> searchResults = new List<SearchResult>();

            string uri = "http://geocode-maps.yandex.ru/1.x/?format=json&geocode=Москва, " + query;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            try
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    SearchResult searchResult = new SearchResult();
                    searchResult.MapObject = reader.ReadToEnd();
                    searchResults.Add(searchResult);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return searchResults;
        }

        public GeoPoint Geocoding(string address)
        {
            try
            {
                var response = GetResponse(address);
                dynamic result = JsonConvert.DeserializeObject(response);
                var resultCount = (int)result.response.GeoObjectCollection.metaDataProperty.GeocoderResponseMetaData.found;
                if (resultCount > 0)
                {
                    for (int i = 0; i < resultCount; i++)
                    {
                        var featureMember = result.response.GeoObjectCollection.featureMember[i];
                        string point = featureMember.GeoObject.Point.pos;
                        if (address.ToLower().Contains("аэропорт") && featureMember.GeoObject.metaDataProperty.GeocoderMetaData.kind.ToString() != "airport")
                        {
                            continue;
                        }
                        var pointParts = point.Split(' ');
                        return new GeoPoint
                        {
                            Longitude = double.Parse(pointParts[0].Replace(',', '.'), CultureInfo.InvariantCulture),
                            Latitude = double.Parse(pointParts[1].Replace(',', '.'), CultureInfo.InvariantCulture)
                        };
                    }
                }
            }
            catch
            {
                //suppressing all errors
            }

            return null;
        }

        public string GetResponse(string query)
        {
            //boundedBy: [[56.48, 36.18], [54.92, 39.10]],
            //ll=37.618920,55.756994&spn=0.552069,0.400552
            try
            {
                var uri = "http://geocode-maps.yandex.ru/1.x/?format=json&rspn=1&ll=37.622915,55.752890&spn=2.92,1.56&geocode= " + query;

                var request = (HttpWebRequest)WebRequest.Create(uri);
                var response = (HttpWebResponse)request.GetResponse();


                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
