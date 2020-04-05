# HotChocolate.Types.GeoLibrary

Simple extension to the original HotChocolate Type system to `Point` and `Polygon` types from [`GeoLibrary`](https://www.nuget.org/packages/GeoLibrary/).

[Nuget package](https://www.nuget.org/packages/HotChocolate.Types.GeoLibrary).

To use, install package using ``dotnet add package HotChocolate.Types.GeoLibrary`` then add to your schema builder:

    services.AddGraphQL(SchemaBuilder.New()
        .AddQueryType<YourQuery>()
        .AddGeoPointScalar()
        .AddGeoPolygonScalar()
        .Create());
