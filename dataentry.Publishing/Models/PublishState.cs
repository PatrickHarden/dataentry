using System;
using System.Collections.Generic;
using System.Text;

namespace dataentry.Publishing.Models
{
    /// <summary>
    /// The listing published states defined by the data entry app
    /// </summary>
    public enum PublishState
    {
        Publishing,
        Published,
        PublishFailed,
        Unpublishing,
        Unpublished,
        UnpublishFailed
    }
}
