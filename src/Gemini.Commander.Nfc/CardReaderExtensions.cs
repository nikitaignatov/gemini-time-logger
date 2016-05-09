using System;
using PCSC;
using PCSC.Iso7816;

namespace Gemini.Commander.Nfc
{
    public static class CardReaderExtensions
    {
        public static string ReadCardUid(this string reader, Func<ISCardReader> factory)
        {
            using (var rfidReader = factory())
            {
                var sc = rfidReader.Connect(reader, SCardShareMode.Shared, SCardProtocol.Any);

                sc.Validate($"Could not connect to reader {reader}:\n{SCardHelper.StringifyError(sc)}");

                var apdu = CreateApdu(rfidReader);
                sc = rfidReader.BeginTransaction();
                sc.Validate("Could not begin transaction.");

                var receivePci = new SCardPCI();
                var sendPci = SCardPCI.GetPci(rfidReader.ActiveProtocol);

                var receiveBuffer = new byte[256];
                var command = apdu.ToArray();

                sc = rfidReader.Transmit(sendPci, command, receivePci, ref receiveBuffer);

                sc.Validate("Error: " + SCardHelper.StringifyError(sc));

                var responseApdu = new ResponseApdu(receiveBuffer, IsoCase.Case2Short, rfidReader.ActiveProtocol);

                rfidReader.EndTransaction(SCardReaderDisposition.Leave);

                if (responseApdu.HasData)
                    return BitConverter.ToString(responseApdu.GetData());

                throw new Exception("No uid received: " + string.Format("SW1: {0:X2}, SW2: {1:X2}\nUid: {2}", responseApdu.SW1, responseApdu.SW2));
            }
        }

        private static void Validate(this SCardError sc, string message)
        {
            if (sc != SCardError.Success) throw new Exception(message);
        }

        private static CommandApdu CreateApdu(this ISCardReader rfidReader)
            => new CommandApdu(IsoCase.Case2Short, rfidReader.ActiveProtocol)
            {
                CLA = 0xFF,
                Instruction = InstructionCode.GetData,
                P1 = 0x00,
                P2 = 0x00,
                Le = 0
            };

    }
}