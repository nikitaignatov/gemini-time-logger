﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Countersoft.Gemini.Commons.Dto;
using Countersoft.Gemini.Commons.Entity;

namespace BaconTime.Terminal.Extensions
{
    public static class IssueTimeTrackingExtensions
    {
        public static int Minutes(this IssueTimeTracking time) => (time.Minutes + (time.Hours * 60));
        public static int Minutes(this IssueTimeTrackingDto time) => time.Entity.Minutes();
        public static int Minutes(this IEnumerable<IssueTimeTrackingDto> times) => times.Select(Minutes).Sum();
        public static int Minutes(this IEnumerable<IssueTimeTracking> times) => times.Select(Minutes).Sum();
    }
}
