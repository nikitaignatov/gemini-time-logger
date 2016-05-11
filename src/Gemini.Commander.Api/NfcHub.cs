using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Countersoft.Gemini.Commons.Entity;
using Gemini.Commander.Commands;
using Gemini.Commander.Core;
using Gemini.Commander.Nfc;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Gemini.Commander.Api
{
    public class NfcHub : Hub
    {
        public void Register(string card, string name)
        {
            TimeTracker.users.TryAdd(card, name);
        }

        private string Some(string value) => string.IsNullOrWhiteSpace(value) ? null : value;
        private TimeType? Some(TimeType value) => value == TimeType.None ? (TimeType?)null : value;

        public void CommentOn(Guid card, string message, string ticket, TimeType type)
        {
            TimeTracker.data[card].Message = Some(TimeTracker.data[card].Message ?? message);
            TimeTracker.data[card].Ticket = Some(TimeTracker.data[card].Ticket ?? ticket);
            TimeTracker.data[card].Type = Some(TimeTracker.data[card].Type ?? type);
            TimeTracker.StoreHistory(TimeTracker.data);
            Clients.All.update(TimeTracker.Convert(TimeTracker.data));
        }

        public void SubmitTimeEntry(Guid card)
        {
            try
            {
                var log = SubmitToGemini(card);

                TimeTracker.data[card].TimeEntryId = log.Id.ToString();
                TimeTracker.StoreHistory(TimeTracker.data);
                Clients.All.update(TimeTracker.Convert(TimeTracker.data));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);

                Clients.All.log(e.Message, e.StackTrace);
            }
        }

        private static IssueTimeTracking SubmitToGemini(Guid card)
        {
            var svc = CommandsController.LoadService();
            var session = TimeTracker.data[card];
            var user = svc.Item.WhoAmI();
            var ticket = Convert.ToInt32(session.Ticket);
            var issue = svc.Item.Get(ticket);
            var log = new IssueTimeTracking
            {
                IssueId = ticket,
                TimeTypeId = 30,
                Hours = session.Transaction.Duration.Hours,
                Minutes = session.Transaction.Duration.Minutes,
                Comment = session.Message,
                EntryDate = session.Transaction.Started
            };

            log.UserId = user.Entity.Id;
            log.Active = true;
            log.Archived = false;
            log.Deleted = false;
            log.Created = DateTime.Now;
            log.ProjectId = issue.Project.Id;

            svc.Item.LogTime(log);
            return log;
        }

        public void Remove(Guid card)
        {
            TimeTracker.data.Remove(card);
            TimeTracker.StoreHistory(TimeTracker.data);
            Clients.All.update(TimeTracker.Convert(TimeTracker.data));
        }

        /// <summary>
        /// Called when the connection connects to this hub instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task"/>
        /// </returns>
        public override Task OnConnected()
        {
            Clients.All.update(TimeTracker.Convert(TimeTracker.data));
            return base.OnConnected();
        }
    }
}