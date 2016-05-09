using System;
using System.Linq;
using PCSC;

namespace Gemini.Commander.Nfc
{
    public class CardReader : IDisposable, ICardReader<int>
    {
        public Func<CardTransaction<int>, CardTransaction<int>> CreateLog { get; set; }
        public Action<CardTransaction<int>> UpdateLog { get; set; }

        private readonly IContextFactory _contextFactory = ContextFactory.Instance;
        private bool go = true;
        private CardTransaction<int> transaction = new CardTransaction<int> { CardId = "NONE" };

        private void AttachEvents(ISCardMonitor monitor, ISCardContext context, string reader)
        {
            monitor.CardInserted += (sender, args) => InsertCard(context, reader);
            monitor.CardRemoved += (sender, args) => RemoveCard();
        }

        private void InsertCard(ISCardContext context, string reader)
        {
            lock (transaction)
            {
                if (transaction != null && (DateTime.Now - transaction.Started).TotalSeconds < 30) return;
                transaction = transaction ?? new CardTransaction<int>();
                transaction.Started = DateTime.Now;
                transaction.TransactionId = Guid.NewGuid();

                transaction.CardId = context.ReadCardUid(reader);
                transaction = CreateLog(transaction);
            }
        }

        private void RemoveCard()
        {
            lock (transaction)
            {
                try
                {
                    UpdateLog?.Invoke(transaction);
                }
                finally
                {
                    transaction = new CardTransaction<int>();
                }
            }
        }

        public void Dispose() => go = false;

        public void Initialize()
        {
            using (var context = new SCardContext())
            using (var monitor = new SCardMonitor(_contextFactory, SCardScope.System))
            {
                context.Establish(SCardScope.System);

                var readers = context.GetReaders();
                if (readers == null || readers.Length != 1) throw new Exception("Only one reader is supported");

                var reader = readers.FirstOrDefault();
                if (reader == null) throw new Exception("Reader was not connected");

                AttachEvents(monitor, context, reader);

                monitor.Start(readers);

                while (go)
                {
                    var key = Console.ReadKey();
                    if (monitor.Monitoring)
                    {
                        monitor.Cancel();
                    }
                    else
                    {
                        monitor.Start(readers);
                    }
                }
            }
        }
    }
}