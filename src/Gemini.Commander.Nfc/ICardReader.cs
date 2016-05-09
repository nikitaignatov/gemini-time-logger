using System;

namespace Gemini.Commander.Nfc
{
    public interface ICardReader
    {
        Func<CardTransaction, CardTransaction> CreateLog { get; set; }
        Action<CardTransaction> UpdateLog { get; set; }

        void Dispose();
        void Initialize();
    }
}