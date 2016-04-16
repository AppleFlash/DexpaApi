using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dexpa.Core.Model;

namespace Dexpa.Core.Services
{
    public static class AggregatedPointService
    {
        public const double PI = Math.PI;
        public static string DirectionToString(double dir)
        {
            string direction = "";
            if (dir > 20 && dir < 60) direction = "СВ";
            else if (dir >= 110 && dir <= 160) direction = "ЮВ";
            else if (dir >= 200 && dir <= 250) direction = "ЮЗ";
            else if (dir >= 290 && dir <= 340) direction = "СЗ";
            else if (dir >= 0 && dir < 20 || dir <= 360 && dir > 340) direction = "С";
            else if (dir > 60 && dir < 110) direction = "В";
            else if (dir > 160 && dir < 200) direction = "Ю";
            else if (dir > 250 && dir < 290) direction = "З";
            return direction;
        }

        public static double CalculateAngle(double x1, double y1, double x2, double y2)
        {
            double cos = (x1 * x2 + y1 * y2) / (Math.Pow((x1 * x1 + y1 * y1), 0.5) * Math.Pow(x2 * x2 + y2 * y2, 0.5));
            double arccos = Math.Acos(cos);
            double toDegrees = arccos * 180 / PI;
            return toDegrees;
        }

       /* public static bool AreaPoints(TrackPoint point, IList<TrackPoint> points, ref int current, ref double dir)
        {
            double lat1Point = point.Latitude;
            double long1Point = point.Longitude;
            double lat2Point, long2Point;
            double averageSpeed = 0;
            double sumDist = 0,
                averageDist = 0;
            const double constDist = 0.15;
            double diagonal = -1;
            double averDir;
            int count = 0;

            dir = AverageDirection(points, current, ref diagonal);

            while (averageDist <= constDist)
            {
                averDir = dir;
                if (current != points.Count - 2)
                {
                    lat2Point = points[current + 2].Latitude;
                    long2Point = points[current + 2].Longitude;
                    var distance = Utils.Utils.GetDistance(lat1Point, long1Point, lat2Point, long2Point);
                    if (distance > constDist) break;
                    sumDist += (double)distance;
                    count++;
                    averageSpeed += points[current + 1].Speed;
                    dir = AverageDirection(points, current, ref diagonal, averDir);
                    current++;
                    if (count > 0) averageDist = sumDist / count;
                }
                else break;
            }
            averageSpeed /= count;
            if (count > 5 && averageSpeed < 20) return true;
            else return false;
        }*/
        public static TrackPoint AreaPoints(IList<TrackPoint> points, int i, out int count)
        {
            double lat1Point = points[i].Latitude;
            double long1Point = points[i].Longitude;
            double lat2Point, long2Point;
            double averageSpeed = 0;
            double sumDist = 0,
                averageDist = 0;
            const double constDist = 0.15;
            double diagonal = -1;
            double averDir;

            string direction = DirectionToString(points[i].Direction);

            count = 0;

            double dir = AverageDirection(points, i, ref diagonal);

            while (averageDist <= constDist)
            {
                averDir = dir;
                if (i != points.Count - 1)
                {
                    lat2Point = points[i + 1].Latitude;
                    long2Point = points[i + 1].Longitude;
                    var distance = Utils.Utils.GetDistance(lat1Point, long1Point, lat2Point, long2Point);
                    if (distance > constDist) break;
                    sumDist += (double)distance;
                    count++;
                    averageSpeed += points[i + 1].Speed;
                    dir = AverageDirection(points, i, ref diagonal, averDir);
                    i++;
                    if (count > 0) averageDist = sumDist / count;
                }
                else break;
            }
            averageSpeed /= count;

            string averageDirection = DirectionToString(dir);


            if (count > 5 && averageSpeed < 20 && direction != averageDirection)
                return points[i-count];
            else return null;
        }

        public static double AverageDirection(IList<TrackPoint> points, int i, ref double diagonal, double dir = -1)
        {
            //double sA = 4;
            //double sB = 4;
            double sA = (diagonal == -1) ? points[i].Speed : diagonal;
            double sB = points[i + 1].Speed;
            double a = (dir == -1) ? points[i].Direction : dir;
            double b = points[i + 1].Direction;
            double side1,
                   side2,
                   side1Ready,
                   side2Ready,
                   degree,
                   diagonalAngle,
                   readyAngle = 0;
            bool flag = false;

            if ((a < 180 && b < 180) || (a > 180 && b > 180))
            {
                degree = Math.Abs(a - b);
                diagonal = CalculateDiagonal(degree, sA, sB);
                diagonalAngle = CalculateDegree(sA, sB, diagonal);
                if (degree > 165)
                    readyAngle = degree / 2;
                else if (a > b)
                    readyAngle = b + diagonalAngle;
                else if (a < b)
                    readyAngle = b - diagonalAngle;
                else if (a == b)
                    readyAngle = a;
            }
            else if (a > 270 && a < 360 || b > 270 && b < 360)
            {
                side1 = ((180 - a) < 0) ? a : b;
                side1Ready = 360 - side1;
                side2Ready = (side1 == a) ? b : a;
                degree = Math.Abs(side1Ready + side2Ready);
                diagonal = CalculateDiagonal(degree, sA, sB);
                diagonalAngle = CalculateDegree(sA, sB, diagonal);
                if (degree > 170)
                    readyAngle = degree / 2;
                else if (side1Ready > side2Ready)
                    readyAngle = a + diagonalAngle;
                else if (a == b)
                    readyAngle = a;
                else
                    readyAngle = b - diagonalAngle;
            }
            else if (a > 180 && a < 270 || b > 180 && b < 270)
            {
                side1 = ((180 - a) < 0) ? a : b;
                side1Ready = Math.Abs(180 - side1);
                side2 = (side1 == a) ? b : a;
                side2Ready = Math.Abs(180 - side2);
                degree = Math.Abs(side1Ready + side2Ready);
                if (degree > 180)
                {
                    flag = true;
                    degree = 360 - degree;
                }
                diagonal = CalculateDiagonal(degree, sA, sB);
                diagonalAngle = CalculateDegree(sA, sB, diagonal);
                if (degree > 170)
                {
                    if (flag)
                        readyAngle = (degree / 2) + 180;
                    else
                        readyAngle = degree / 2;
                }
                else if (side1Ready > side2Ready)
                    readyAngle = side1 - diagonalAngle;
                else if (a == b)
                    readyAngle = a;
                else
                {
                    if (flag)
                    {
                        readyAngle = side1 + diagonalAngle;
                        if (readyAngle > 360)
                        {
                            double val = Math.Min(side1, side2);
                            readyAngle = val + 360 - diagonalAngle;
                        }
                    }
                    else
                        readyAngle = side2 + diagonalAngle;
                    if (readyAngle > side1Ready && readyAngle < side2Ready)
                        readyAngle += 180;
                }
            }
            return readyAngle;
        }

        public static double CalculateDegree(double sA, double sB, double diagonal)
        {
            double alpha = Math.Acos((sB * sB + diagonal * diagonal - sA * sA) / (2 * sB * diagonal));
            return alpha * 180 / PI;
        }

        public static double CalculateDiagonal(double degree, double sA, double sB)
        {
            double degInRad = degree * PI / 180;
            return Math.Pow((sA * sA + sB * sB + 2 * sA * sB * Math.Cos(degInRad)), 0.5);
        }
    }
}
