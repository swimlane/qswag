namespace SwaggerSchema
{
    public class Info
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; }
        public string TermsOfService { get; set; }
        public Contact Contact { get; set; }
        public License License { get; set; }
        public string Version { get; set; } = string.Empty;
    }
}