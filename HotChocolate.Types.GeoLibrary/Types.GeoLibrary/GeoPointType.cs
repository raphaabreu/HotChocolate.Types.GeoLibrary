using System;
using System.Collections.Generic;
using System.Linq;
using GeoLibrary.Model;
using HotChocolate.Language;

namespace HotChocolate.Types.GeoLibrary
{
    /// <summary>
    /// The `GeoPoint` scalar type represents coordinates as an array of doubles where the first element is the longitude and the second is the latitude.
    /// </summary>
    public class GeoPointType : ScalarType
    {
        public GeoPointType() : base("GeoPoint")
        {
            Description = "The `GeoPoint` scalar type represents coordinates as an array of doubles where the first element is the longitude and the second is the latitude.";
        }

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

            return literal is ListValueNode list && list.Items.Count == 2 && list.Items.All(item => item is FloatValueNode || item is IntValueNode);
        }

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

            if (literal is ListValueNode listLiteral && listLiteral.Items.Count == 2 && listLiteral.Items.All(item => item is FloatValueNode || item is IntValueNode))
            {
                var lng = Convert.ToDouble(listLiteral.Items[0].Value);
                var lat = Convert.ToDouble(listLiteral.Items[1].Value);

                return new Point(lng, lat);
            }

            throw new ArgumentException(
                "The GeoPointType can only parse list literals containing exactly two number literals.",
                nameof(literal));
        }

        public override IValueNode ParseValue(object value)
        {
            if (value == null)
            {
                return new NullValueNode(null);
            }

            if (value is Point point)
            {
                return new ListValueNode(null, new[] { new FloatValueNode(point.Longitude), new FloatValueNode(point.Latitude) });
            }

            throw new ArgumentException(
                "The specified value has to be Point in order to be parsed by the GeoPointType.");
        }

        public override object Serialize(object value)
        {
            if (value == null)
            {
                return null;
            }

            if (value is Point t)
            {
                return new[] { t.Longitude, t.Latitude };
            }

            throw new ArgumentException(
                "The specified value cannot be serialized by the GeoPointType.");
        }

        public override bool TryDeserialize(object serialized, out object value)
        {
            if (serialized is null)
            {
                value = null;
                return true;
            }

            if (serialized is IList<object> list && list.Count == 2 && list.All(item => double.TryParse(Convert.ToString(item), out var _)))
            {
                value = new Point(Convert.ToDouble(list[0]), Convert.ToDouble(list[1]));
                return true;
            }

            value = null;
            return false;
        }

        public override Type ClrType => typeof(Point);
    }
}
