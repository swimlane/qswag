# Basic

The most basic way to get swagger definition for controller is to pass the type to the method it will return a json string back.
```
using QSwagGenerator;
using QSwagSchema;
```

`WebApiToSwagger.GenerateForController(typeof(ApplicationController),null)`

The first parameter is a controller type and the second+ are the excluded method names. Helpful if you want to include swagger endpoint in the controller itself. This way it won't get documented.

`WebApiToSwagger.GenerateForController(typeof(ApplicationController),"GetSwagger","TestGet")`


If you want to generate document for multiple controllers you can send the list of types to the method 

`WebApiToSwagger.GenerateForControllers(new []{typeof(ApplicationController),typeof(PersonController)}, null,"GetSwagger");`

The second parameter in this case is a settings object which I will describe in second part.

These methods and their overloads return json string. Core.net will not return string correctly if your make it the output of the method. It will escape it. I suggest you do the following.

```
[AllowAnonymous]
[HttpGet("/swagger/Application")]
public ActionResult GetSwagger()
{
    var result = WebApiToSwagger.GenerateForController(typeof(ApplicationController),null);
    return new FileContentResult(Encoding.UTF8.GetBytes(generateForControllers), "application/json");
}
```