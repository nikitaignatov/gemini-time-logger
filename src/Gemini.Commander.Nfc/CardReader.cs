using System;
using System.Linq;
using PCSC;

namespace Gemini.Commander.Nfc
{
    public class CardReader<TIdentity> : IDisposable, ICardReader<TIdentity>
    {
        public Func<CardTransaction<TIdentity>, CardTransaction<TIdentity>> CreateLog { get; set; }
        public Action<CardTransaction<TIdentity>> UpdateLog { get; set; }
        private CardTransaction<TIdentity> transaction = new CardTransaction<TIdentity> { CardId = "NONE" };

        public void InsertCard(ISCardContext context, string reader)
        {
            lock (transaction)
            {
                if (transaction != null && (DateTime.Now - transaction.Started).TotalSeconds < 30) return;
                transaction = transaction ?? new CardTransaction<TIdentity>();
                transaction.Started = DateTime.Now;
                transaction.TransactionId = Guid.NewGuid();

                transaction.CardId = context.ReadCardUid(reader);
                transaction = CreateLog(transaction);
            }
        }

        public void RemoveCard()
        {
            lock (transaction)
            {
                try
                {
                    transaction.Ended = DateTime.Now;
                    UpdateLog?.Invoke(transaction);
                }
                finally
                {
                    transaction = new CardTransaction<TIdentity>();
                }
            }
        }

        private bool go = true;
        private readonly IContextFactory _contextFactory = ContextFactory.Instance;
        public void Dispose() => go = false;

        private void AttachEvents(ISCardMonitor monitor, ISCardContext context, string reader)
        {
            monitor.CardInserted += (sender, args) => InsertCard(context, reader);
            monitor.CardRemoved += (sender, args) => RemoveCard();
        }

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