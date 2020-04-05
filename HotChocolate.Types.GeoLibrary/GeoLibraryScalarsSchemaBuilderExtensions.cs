using System;
using HotChocolate.Types.GeoLibrary;

namespace HotChocolate
{
    public static class GeoLibraryScalarsSchemaBuilderExtensions
    {
        /// <summary>
        /// Add the <see cref="GeoPointType"/> scalar to the schema builder.
        /// </summary>
        /// <param name="builder">The schema builder.</param>
        /// <returns>The schema builder.</returns>
        public static ISchemaBuilder AddGeoPointScalar(this ISchemaBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddType(typeof(GeoPointType));

            return builder;
        }

        /// <summary>
        /// Add the <see cref="GeoPolygonType"/> scalar to the schema builder.
        /// </summary>
        /// <param name="builder">The schema builder.</param>
        /// <returns>The schema builder.</returns>
        public static ISchemaBuilder AddGeoPolygonScalar(this ISchemaBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddType(typeof(GeoPointType));

            return builder;
        }
    }
}
