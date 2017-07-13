using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nfield.Models
{
    /// <summary>
    /// Model for an email invitation template
    /// </summary>
    public class InvitationTemplateModel
    {
        /// <summary>
        /// Invitation template id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Invitation type.
        /// 1 = Invitation.
        /// 2 = Reminder.
        /// </summary>
        public int InvitationType { get; set; }

        /// <summary>
        /// Name of the invitation template
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Subject of the invitation
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Body of the invitation
        /// </summary>
        public string Body { get; set; }
    }
}
