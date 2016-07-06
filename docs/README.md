# QSwag

This is another Swagger v2 generator for .net. Why use this one?

First, this is specific for core.net. Core.net introduced a lot of new functionality and specs - in special attributes and return types. Most of current implementations are still using old MVC conventions.

This library is build as core.net library as well, which allows to include it in new projects right away.

The other reason it was build from ground up to be very fast and slim. No UI included and JSON  served from controller just like any other API call. It even allows you to parameter-ize calls so you can retrieve spec for controller you want only.
