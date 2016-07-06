# Using settings

Settings object allow you to modify the behavior of the parser, and also inject additional data in your swagger definition.
Following is complex example that I will discuss in more detail.

``` CSharp
var httpRequest = HttpContext?.Request;
var generatorSettings = new GeneratorSettings(httpRequest)
{
    DefaultUrlTemplate = "/[controller]/{id?}",
    IgnoreObsolete = true,
    Info = new Info() { Title = "Your API", Version = "3.0" },
    XmlDocPath = Path.ChangeExtension(Assembly.GetEntryAssembly().Location, "xml"),
    SecurityDefinitions = new Dictionary<string, SecurityDefinition>()
    {
        {
            "jwt_token",
            new SecurityDefinition("Authorization", SecuritySchemeType.ApiKey) {In = Location.Header}
        }
    },
    JsonSchemaLicense = "Your Json.Schema license"
};
generatorSettings.Security.Add(new SecurityRequirement("jwt_token"));            
```

Let's start from the first line. This is where we getting request object. It is nullable for unit tests.
We want to pass it to GeneratorSettings constructor so it can automatically assign Host and Port for swagger spec. If you want to do it manually, pass null to the constructor and set Host and Port properties.
`var genSettings= new GeneratorSettings(null){Host="myhost.com",Port=443};`

DefaultUrlTemplate sets the names and routes for the controllers without routing or Http attributes. This is no longer recommended way by Microsoft. Even default ones generated have Http attribute now. This is another change by Microsoft you can define route on HttpGet attribute for instance, so it's really not that difficult to do.

