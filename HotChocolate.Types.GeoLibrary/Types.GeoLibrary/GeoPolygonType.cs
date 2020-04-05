using System;
using System.Collections.Generic;
using System.Linq;
using GeoLibrary.Model;
using HotChocolate.Language;

namespace HotChocolate.Types.GeoLibrary
{
    /// <summary>
    /// The `GeoPolygon` scalar type represents polygons as an array of line strings where each line string contains points as an array of longitude/latitude pairs.
    /// </summary>
    public class GeoPolygonType : ScalarType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoPolygonType"/> class.
        /// </summary>
        public GeoPolygonType() : base("GeoPolygon")
        {
            Description =
                "The `GeoPolygon` scalar type represents polygons as an array of line strings where each line string contains points as an array of longitude/latitude pairs.";
        }

        /// <inheritdoc />
        public override bool IsInstanceOfType(IValueNode literal)
        {
            if (literal == null)
            {
                throw new ArgumentNullException(nameof(literal));
            }

            if (literal is NullValueNode)
            {
                return true;
            }

            return literal is ListValueNode list &&
                   list.Items.All(item => item is ListValueNode listNode &&
                                          listNode.Items.All(subitem => subitem is ListValueNode subitemList &&
                                                                        subitemList.Items.Count == 2));
        }

        /// <inheritdoc />
        public override object ParseLiteral(IValueNode literal)
        {
            if (literal == null)
            {
                throw new ArgumentNullException(nameof(literal));
            }

            if (literal is NullValueNode)
            {
                return null;
            }

            if (literal is ListValueNode segmentsLiteral &&
                segmentsLiteral.Items.All(item => item is ListValueNode listNode &&
                                                  listNode.Items.All(subitem => subitem is ListValueNode subitemList &&
                                                                                subitemList.Items.Count == 2)))
            {
                var linesStrings = new List<LineString>();

                foreach (var segmentLiteral in segmentsLiteral.Items)
                {
                    var segment = new List<Point>();
                    foreach (var segmentItem in ((ListValueNode)segmentLiteral).Items)
                    {
                        var lng = Convert.ToDouble(((ListValueNode)segmentItem).Items[0].Value);
                        var lat = Convert.ToDouble(((ListValueNode)segmentItem).Items[1].Value);

                        segment.Add(new Point(lng, lat));
                    }
                    linesStrings.Add(new LineString(segment));
                }

                return new Polygon(linesStrings);
            }

            throw new ArgumentException(
                "Invalid input for GeoPolygon.",
                nameof(literal));
        }

        /// <inheritdoc />
        public override IValueNode ParseValue(object value)
        {
            if (value == null)
            {
                return new NullValueNode(null);
            }

            if (value is Polygon polygon)
            {
                var lineStrings = polygon.LineStrings.Select(ls => new ListValueNode(null, ls.Coordinates.Select(point =>
                    new ListValueNode(null,
                        new[] { new FloatValueNode(point.Longitude), new FloatValueNode(point.Latitude) })).ToArray()));

                return new ListValueNode(null, lineStrings.ToArray());
            }

            throw new ArgumentException(
                "The specified value cannot be parsed to GeoPolygon.");
        }

        /// <inheritdoc />
        public override object Serialize(object value)
        {
            if (value == null)
            {
                return null;
            }

            if (value is Polygon polygon)
            {
                var lineStrings = polygon.LineStrings.Select(ls =>
                    ls.Coordinates.Select(point => new[] { point.Longitude, point.Latitude }).ToArray()).ToArray();

                return lineStrings;
            }

            throw new ArgumentException(
                "The specified value cannot be serialized by the GeoPolygon.");
        }

        /// <inheritdoc />
        public override bool TryDeserialize(object serialized, out object value)
        {
            if (serialized is null)
            {
                value = null;
                return true;
            }

            if (serialized is IList<object> list && list.All(item => item is IList<object> sublist &&
                                                                     sublist.All(subitem => subitem is IList<object> subitemList &&
                                                                                            subitemList.Count == 2)))
            {
                var lineStrings = list.Select(ls =>
                    new LineString(
                        ((IList<object>)ls).Select(point => new Point(Convert.ToDouble(((IList<object>)point)[0]), Convert.ToDouble(((IList<object>)point)[1])))));

                value = new Polygon(lineStrings);
                return true;
            }

            value = null;
            return false;
        }

        /// <inheritdoc />
        public override Type ClrType => typeof(Polygon);
    }
}
