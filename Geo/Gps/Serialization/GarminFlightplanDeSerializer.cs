﻿using System.Linq;
using Geo.Geometries;
using Geo.Gps.Serialization.Xml;
using Geo.Gps.Serialization.Xml.Garmin;

namespace Geo.Gps.Serialization
{
    public class GarminFlightplanDeSerializer : GpsXmlDeSerializer<GarminFlightplan>
    {
        public override GpsFileFormat[] FileFormats
        {
            get
            {
                return new[]
                           {
                               new GpsFileFormat("fpl", "Garmin Flightplan"),
                           };
            }
        }

        public override GpsFeatures SupportedFeatures
        {
            get { return GpsFeatures.Routes; }
        }

        protected override GpsData DeSerialize(GarminFlightplan xml)
        {
            var data = new GpsData();
            foreach (var route in xml.route)
            {
                var rte = new Route();
                rte.Metadata.Attribute(x => x.Name, route.routename);
                foreach (var point in route.routepoint)
                {
                    var wp = xml.waypointtable.Single(x => x.identifier == point.waypointidentifier);
                    rte.LineString.Add(new Point(wp.lat, wp.lon));
                }
                data.Routes.Add(rte);
            }
            return data;
        }
    }
}
