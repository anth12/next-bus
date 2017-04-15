using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator.Abstractions;

namespace NextBus.Utilities.Extensions
{
    public static class PositionExtensions
    {
        public static double DistanceFrom(this Position baseCoordinates, Position targetCoordinates, UnitOfLength unitOfLength)
        {
            var baseRad = Math.PI * baseCoordinates.Latitude / 180;
            var targetRad = Math.PI * targetCoordinates.Latitude / 180;
            var theta = baseCoordinates.Longitude - targetCoordinates.Longitude;
            var thetaRad = Math.PI * theta / 180;

            double dist =
                Math.Sin(baseRad) * Math.Sin(targetRad) + Math.Cos(baseRad) *
                Math.Cos(targetRad) * Math.Cos(thetaRad);
            dist = Math.Acos(dist);

            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            return unitOfLength.ConvertFromMiles(dist);
        }

        public class UnitOfLength
        {
            public static UnitOfLength Kilometers = new UnitOfLength(1.609344);
            public static UnitOfLength NauticalMiles = new UnitOfLength(0.8684);
            public static UnitOfLength Miles = new UnitOfLength(1);

            private readonly double _fromMilesFactor;

            private UnitOfLength(double fromMilesFactor)
            {
                _fromMilesFactor = fromMilesFactor;
            }

            public double ConvertFromMiles(double input)
            {
                return input * _fromMilesFactor;
            }
        }

    }
}
