using System;

namespace Gemini.Commander.Nfc
{
    public class CardTransaction
    {
        public string CardId { get; set; }
        public DateTime Started { get; set; }
        public DateTime Ended { get; set; }
        public Guid TransactionId { get; set; }
        public string Message { get; set; }
        public int Id { get; set; }
        public ContactType ContacType { get; set; }
        public TimeSpan Duration => Ended - Started;
    }

    public enum ContactType
    {
        Random = 0,
        Question = 1,
        Consulting = 2,
        Discussion = 3,
        NonWorkRelated = 4,
    }
}
