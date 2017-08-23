namespace Nfield.Models
{
    public class EmailSettingsEditable
    {
        /// <summary>
        /// The 'from' email address
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        /// Name of the sender
        /// </summary>
        public string FromName { get; set; }

        /// <summary>
        /// The 'reply to' address
        /// </summary>
        public string ReplyToAddress { get; set; }

        /// <summary>
        /// The physical address
        /// </summary>
        public string PostalAddress { get; set; }
    }
}