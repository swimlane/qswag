namespace SwaggerSchema
{
    public class Parameter : ItemsObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Location In { get; set; } = Location.Query;
        public bool Required { get; set; } = false;

        #region In=Body

        public SchemaObject Schema { get; set; }

        #endregion
    }
}