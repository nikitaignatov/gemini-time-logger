using System;
using System.Linq;
using System.Windows.Forms;
using FluentAssertions;
using Gemini.Commander.Nfc;
using NSubstitute;
using NUnit.Framework;
using PCSC;

namespace Gemini.Commander.Spec.Tests
{
    [TestFixture]
    public class CardReaderTests
    {
        private ISCardContext context;
        private ISCardReader reader;
        private CardReader svc;

        [SetUp]
        public void setup()
        {
            context = Substitute.For<ISCardContext>();
            reader = Substitute.For<ISCardReader>();
            svc = Substitute.For<CardReader>();


            svc.CreateLog = x => x;
            svc.UpdateLog = x => { };
            svc.ReaderFactory = () => reader;
        }

        [Test]
        public void trigger_create_log_when_card_is_inserted()
        {
            // arrage 
            reader.ActiveProtocol.Returns(SCardProtocol.T1);
            // act 
            svc.InsertCard(context, "reader");
            // assert
            svc.ReceivedWithAnyArgs(1).CreateLog(null);
        }

        [Test]
        public void trigger_update_log_when_card_is_removed()
        {
            // arrage 
            reader.ActiveProtocol.Returns(SCardProtocol.T1);
            // act 
            svc.InsertCard(context, "reader");
            svc.RemoveCard();
            // assert
            svc.ReceivedWithAnyArgs(1).UpdateLog(Arg.Is<CardTransaction>(x => x.Card != "NONE"));
        }

        [Test]
        public void when_card_is_removed_without_insert_detected_first_none_should_be_returned()
        {
            // arrage 
            // act 
            svc.RemoveCard();
            // assert
            svc.ReceivedWithAnyArgs(1).UpdateLog(Arg.Is<CardTransaction>(x => x.Card == "NONE"));
        }

        [Test]
        public void invalid_sc_should_throw_exception()
        {
            // arrage 
            var expected = "Some message";
            var type = typeof(SCardError);
            var errors = Enum.GetNames(type)
                   .Select(x => (SCardError)Enum.Parse(type, x))
                   .Where(x => x != SCardError.Success);

            // act 
            foreach (var error in errors)
            {
                Action result = () => error.Validate(expected);
                // assert
                result.ShouldThrow<Exception>().WithMessage(expected);
            }
        }

        [Test]
        public void success_should_not_throw()
        {
            // arrage 
            // act 
            Action result = () => SCardError.Success.Validate("");
            // assert
            result.ShouldNotThrow();
        }
    }
}