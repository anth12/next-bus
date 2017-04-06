using System;

namespace NextBus.Models
{
    public class Coordinates
    {

        /// <summary>
        /// The equator radius.
        /// </summary>
        public const int EquatorRadius = 6378137;

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        public double Longitude { get; set; }

        public DateTime TimeStamp { get; set; }

        public Coordinates()
        {
        }

        public Coordinates(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }
        public Coordinates(string latitude, string longitude)
        {
            this.Latitude = double.Parse(latitude);
            this.Longitude = double.Parse(longitude);
        }
        public Coordinates(decimal latitude, decimal longitude)
        {
            this.Latitude = (double) latitude;
            this.Longitude = (double)longitude;
        }

        /// <summary>
        /// Calculates distance between two locations.
        /// </summary>
        /// <returns>The <see cref="System.Double"/>The distance in meters</returns>
        /// <param name="a">Location a</param>
        /// <param name="b">Location b</param>
        public static double DistanceBetween(Coordinates a, Coordinates b)
        {
            double distance = Math.Acos(
                (Math.Sin(a.Latitude) * Math.Sin(b.Latitude)) +
                (Math.Cos(a.Latitude) * Math.Cos(b.Latitude))
                * Math.Cos(b.Longitude - a.Longitude));

            return EquatorRadius * distance;
        }

        /// <summary>
        /// Calculates bearing between start and stop.
        /// </summary>
        /// <returns>The <see cref="System.Double"/>.</returns>
        /// <param name="start">Start coordinates.</param>
        /// <param name="stop">Stop coordinates.</param>
        public static double BearingBetween(Coordinates start, Coordinates stop)
        {
            var deltaLon = stop.Longitude - start.Longitude;
            var cosStop = Math.Cos(stop.Latitude);
            return Math.Atan2(
                (Math.Cos(start.Latitude) * Math.Sin(stop.Latitude)) -
                (Math.Sin(start.Latitude) * cosStop * Math.Cos(deltaLon)),
                Math.Sin(deltaLon) * cosStop);
        }

        /// <summary>
        /// Calculates this locations distance to another coordicate.
        /// </summary>
        /// <returns>The distance to another coordicate</returns>
        /// <param name="other">Other coordinates.</param>
        public double DistanceFrom(Coordinates other)
        {
            return DistanceBetween(this, other);
        }

        /// <summary>
        /// Calculates this locations bearing to another coordicate.
        /// </summary>
        /// <returns>Bearing degree.</returns>
        /// <param name="other">Other coordinates.</param>
        public double BearingFrom(Coordinates other)
        {
            return BearingBetween(this, other);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return $"({Latitude:0.0000}, {Longitude:0.0000})";
        }
    }
}
