using System;
using System.Collections.Generic;
using System.Linq;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Additional;
using Dexpa.Core.Services;

namespace Dexpa.Core.Utils
{
    public static class TaximeterHelper
    {
        public static double GetMinCost(Address addressFrom, Address addressTo, Tariff tariff,
            IGeocoderService geocoderService, RegionBinaryStorage regionStorage)
        {
            var fromRegionId = GetRegionId(addressFrom, regionStorage, geocoderService);
            var toRegionId = GetRegionId(addressTo, regionStorage, geocoderService);
            var regionCost = tariff.RegionsCosts.FirstOrDefault(c => c.RegionFromId == fromRegionId && c.RegionToId == toRegionId);
            if (regionCost != null && regionCost.Cost > 0)
            {
                return regionCost.Cost;
            }
            return 0.0;
        }

        private static long? GetRegionId(Address address, RegionBinaryStorage regionStorage, IGeocoderService geocoderService)
        {
            GeoPoint point;
            if (address.Latitude.HasValue && address.Longitude.HasValue)
            {
                point = new GeoPoint
                {
                    Latitude = address.Latitude.Value,
                    Longitude = address.Longitude.Value
                };
            }
            else
            {
                point = geocoderService.Geocoding(address.FullName);
            }
            var regionPoints = regionStorage.GetAllPoints();
            var pointsByRegion = regionPoints
                .GroupBy(rp => rp.RegionId)
                .ToList();
            foreach (var region in pointsByRegion)
            {
                var regionId = region.Key;
                if (IsPointInPolygon(point, region.ToList()))
                {
                    return regionId;
                }
            }
            return null;
        }

        public static Region GetRegion(string addressFrom, IRegionService regionService, IGeocoderService geocoderService)
        {
            var point = geocoderService.Geocoding(addressFrom);
            var regions = regionService.GetRegions();
            foreach (var region in regions)
            {
                if (IsPointInPolygon(point, region.Points))
                {
                    return region;
                }
            }
            return null;
        }

        public static double GetOrderCost(OrderPathWithTariff orderPath, Tariff tariff, IEnumerable<IGrouping<long,RegionPoint>> regionPoints)
        {
            var cost = 0.0;

            cost += tariff.MinimumCost;

            var freeTime = (double)tariff.IncludeMinutes;
            var freeKm = (double)tariff.IncludeKilometers;

            var inMkad = regionPoints.LastOrDefault();
            if (inMkad == null)
            {
                return 0.0;
            }

            var inMKADpoints = inMkad.ToList();

            var tariffZoneInCity = tariff.TariffZones.FirstOrDefault(t => t.TariffZoneType == TariffZoneType.City);
            var tariffZoneOutCity = tariff.TariffZones.FirstOrDefault(t => t.TariffZoneType == TariffZoneType.OutCity);

            foreach (var segment in orderPath.Segments)
            {
                var point = new GeoPoint()
                {
                    Latitude = segment.Latitude,
                    Longitude = segment.Longitude
                };
                TariffZone tariffZone;
                if (IsPointInPolygon(point, inMKADpoints))
                {
                    tariffZone = tariffZoneInCity;
                }
                else
                {
                    tariffZone = tariffZoneOutCity;
                }

                if (tariff.IncludeMinutesAndKilometers)
                {
                    if (freeTime > 0 || freeKm > 0)
                    {
                        if (freeTime > 0)
                        {
                            freeTime = freeTime - SecToMin(segment.Time);
                        }
                        else
                        {
                            freeKm = freeKm - MetToKm(segment.SegmentLength);
                        }
                    }
                    else
                    {
                        cost += GetSegmentCost(segment, tariffZone);
                    }
                }
                else
                {
                    if (freeTime > 0 && freeKm > 0)
                    {
                        freeTime = freeTime - SecToMin(segment.Time);
                        freeKm = freeKm - MetToKm(segment.SegmentLength);
                    }
                    else
                    {
                        cost += GetSegmentCost(segment, tariffZone);
                    }
                }
            }
            return cost;
        }

        private static double GetSegmentCost(OrderPathSegment segment, TariffZone tariffZone)
        {
            var dist = MetToKm(segment.SegmentLength);
            var timeH = SecToHour(segment.Time);
            var timeM = SecToMin(segment.Time);
            double speed = dist / timeH;

            if (speed > tariffZone.MinVelocity)
            {
                return tariffZone.KilometerCost * dist;
            }
            else
            {
                return tariffZone.MinuteCost * timeM;
            }
        }

        public static bool IsPointInPolygon(GeoPoint point, List<RegionPoint> points)
        {
            // ray casting alogrithm http://rosettacode.org/wiki/Ray-casting_algorithm
            var crossings = 0;
            // for each edge
            for (int i = 0; i < points.Count; i++)
            {
                var a = points[i];
                var j = i + 1;
                if (j >= points.Count)
                {
                    j = 0;
                }
                var b = points[j];
                if (point != null && RayCrossesSegment(point, a, b))
                {
                    crossings++;
                }
            }

            // odd number of crossings?
            return (crossings % 2 == 1);
        }

        private static bool RayCrossesSegment(GeoPoint point, RegionPoint a, RegionPoint b)
        {
            double px = point.Longitude,
                py = point.Latitude,
                ax = a.Lng,
                ay = a.Lat,
                bx = b.Lng,
                by = b.Lat;
            if (ay > by)
            {
                ax = b.Lng;
                ay = b.Lat;
                bx = a.Lng;
                by = a.Lat;
            }
            // alter longitude to cater for 180 degree crossings
            if (px < 0)
            {
                px += 360;
            }
            if (ax < 0)
            {
                ax += 360;
            }
            if (bx < 0)
            {
                bx += 360;
            }

            if (IsCoordEquals(py, ay) || IsCoordEquals(py, by)) py += 0.00000001;
            if ((py > by || py < ay) || (px > Math.Max(ax, bx))) return false;
            if (px < Math.Min(ax, bx)) return true;

            var red = !IsCoordEquals(ax, bx) ? ((by - ay) / (bx - ax)) : double.PositiveInfinity;
            var blue = !IsCoordEquals(ax, px) ? ((py - ay) / (px - ax)) : double.PositiveInfinity;
            return (blue >= red);
        }

        private static bool IsCoordEquals(double a, double b)
        {
            return Math.Abs(a - b) <= double.Epsilon;
        }

        public static double SecToHour(double secs)
        {
            return secs / 60.0 / 60.0;
        }

        public static double SecToMin(double secs)
        {
            return secs / 60.0;
        }

        public static double MetToKm(double met)
        {
            return met / 1000.0;
        }
    }
}
