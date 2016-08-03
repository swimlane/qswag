namespace QSwagWebApi.Models
{
    /// <summary>
    ///     Address Info
    /// </summary>
    public class Address
    {
        /// <summary>
        ///     Gets or sets the street.
        /// </summary>
        /// <value>
        ///     The street.
        /// </value>
        public string Street { get; set; }

        /// <summary>
        ///     Gets or sets the city.
        /// </summary>
        /// <value>
        ///     The city.
        /// </value>
        public string City { get; set; }

        /// <summary>
        ///     Gets or sets the state.
        /// </summary>
        /// <value>
        ///     The state.
        /// </value>
        public string State { get; set; }

        /// <summary>
        ///     Gets or sets the zip.
        /// </summary>
        /// <value>
        ///     The zip.
        /// </value>
        public string Zip { get; set; }

        /// <summary>
        ///     Gets or sets the type.
        /// </summary>
        /// <value>
        ///     The type.
        /// </value>
        public AddressType Type { get; set; }

        /// <summary>
        ///     Gets or sets the type of the nullable.
        /// </summary>
        /// <value>
        ///     The type of the nullable.
        /// </value>
        public AddressType? NullableType { get; set; }
    }
}