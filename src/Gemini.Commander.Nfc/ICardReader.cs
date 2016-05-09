using System;

namespace Gemini.Commander.Nfc
{
    public interface ICardReader<TIdentity>
    {
        Func<CardTransaction<TIdentity>, CardTransaction<TIdentity>> CreateLog { get; set; }
        Action<CardTransaction<TIdentity>> UpdateLog { get; set; }

        void Dispose();
        void Initialize();
    }
}