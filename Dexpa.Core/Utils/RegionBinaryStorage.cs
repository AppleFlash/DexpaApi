using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dexpa.Core.Model;

namespace Dexpa.Core.Utils
{
    public class RegionBinaryStorage
    {
        private string mFilename;

        public RegionBinaryStorage(string filename)
        {
            mFilename = filename;
        }

        public void SetPoints(IList<RegionPoint> points)
        {
            using (var writer = new BinaryWriter(File.Create(mFilename)))
            {
                for (int i = 0; i < points.Count; i++)
                {
                    var point = points[i];
                    writer.Write(point.Id);
                    writer.Write(point.RegionId);
                    writer.Write(point.Lat);
                    writer.Write(point.Lng);
                }
            }
        }

        public IList<RegionPoint> GetAllPoints()
        {
            var points = new List<RegionPoint>(10000);
            using (var reader = new BinaryReader(File.OpenRead(mFilename)))
            {
                while (reader.BaseStream.Position <= reader.BaseStream.Length-1)
                {
                    var point = new RegionPoint
                    {
                        Id = reader.ReadInt64(),
                        RegionId = reader.ReadInt64(),
                        Lat = reader.ReadDouble(),
                        Lng = reader.ReadDouble()
                    };
                    points.Add(point);
                }
            }

            return points;
        }
    }
}
