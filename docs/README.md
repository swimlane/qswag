# QSwag

This is another Swagger v2 generator for .NET Core

This library was built for the new .NET Core framework. The new .NET framework
introduced a lot of new functionality and specs such special attributes and return types.
Most of current implementations are still using old conventions leaving us unhappy.

The library was built from the ground up to be very fast and slim. There is no UI included
and JSON is served from your controllers just like any other request. In addition, it allows you to
parameterize calls so you can retrieve specific specs for each endpoint.
