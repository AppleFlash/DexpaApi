using System;
using AutoMapper;
using Dexpa.Core.Model;

namespace Dexpa.Core
{
    class ObjectMapper
    {
        public static ObjectMapper Instance
        {
            get
            {
                return mLazyInstance.Value;
            }
        }

        private static readonly Lazy<ObjectMapper> mLazyInstance = new Lazy<ObjectMapper>(() => new ObjectMapper());

        private ObjectMapper()
        {
            Mapper.CreateMap<Driver, Driver>();
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }
    }
}
