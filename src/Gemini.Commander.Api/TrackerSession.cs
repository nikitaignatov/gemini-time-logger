using System.Linq;
using Gemini.Commander.Nfc;

namespace Gemini.Commander.Api
{
    public class TrackerSession
    {
        public string TimeEntryId { get; set; }
        public string Name { get; set; }
        public CardTransaction Transaction { get; set; }
        public string Ticket { get; set; }
        public TimeType? Type { get; set; }
        public string Message { get; set; }

        public bool IsMissingTimeEntryId => string.IsNullOrWhiteSpace(TimeEntryId);
        public bool IsMissingTicket => string.IsNullOrWhiteSpace(Ticket);
        public bool IsMissingMessage => string.IsNullOrWhiteSpace(Message);
        public bool IsMissingName => string.IsNullOrWhiteSpace(Name);
        public bool IsValid => new[] { IsMissingTicket, IsMissingMessage, IsMissingName }.All(x => !x);
        public bool IsSubmitted => !IsMissingTimeEntryId;
    }
}