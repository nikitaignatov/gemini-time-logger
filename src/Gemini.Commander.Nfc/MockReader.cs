using System;

namespace Gemini.Commander.Nfc
{
    public class MockReader : ICardReader
    {
        public Func<CardTransaction, CardTransaction> CreateLog { get; set; }
        public Action<CardTransaction> UpdateLog { get; set; }

        public void Dispose()
        {
        }

        public void Initialize()
        {
            var tx = new CardTransaction { Card = "NONE" };
            var swap = true;
            while (true)
            {
                if (swap)
                {
                    tx = CardReader.CreateDefaultTransaction();
                    Console.WriteLine("Type in the card name:");
                    tx.Card = Console.ReadLine();
                    tx = CreateLog(tx);
                }
                else
                {
                    Console.WriteLine("Press enter when card should be removed.");
                    Console.ReadLine();
                    tx.Ended = DateTime.Now;
                    UpdateLog(tx);
                }
                swap = !swap;
            }
        }
    }
}