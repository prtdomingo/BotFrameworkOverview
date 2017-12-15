using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotFrameworkOverview.Cards
{
    [Serializable]
    public class Booking
    {
        [Prompt("What's your Name?")]
        public string Name { get; set; }

        [Prompt("Where's your Origin?")]
        public string Origin { get; set; }

        [Prompt("Where's your Destination?")]
        public string Destination { get; set; }

        public DateTime DepartureDate { get; set; }

        public DateTime ReturnDate { get; set; }

        [Prompt("How many Adults?")]
        public int AdultPax { get; set; }

        public int ChildrenPax { get; set; }

        public FlightType? FlightTypeOption { get; set; }

        public enum FlightType
        {
            RoundTrip = 1, OneWay = 2
        }
    }
}