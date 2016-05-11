using System;

namespace Gemini.Commander.Nfc
{
    public class CardTransaction
    {
        /// <summary>
        /// UID of the card
        /// </summary>
        public string Card { get; set; }
        public DateTime Started { get; set; }
        public DateTime? Ended { get; set; }
        public Guid TransactionId { get; set; }
        public TimeSpan Duration => Ended.HasValue ? Ended.Value - Started : default(TimeSpan);
        public bool IsEnded => Ended.HasValue;
    }
}
