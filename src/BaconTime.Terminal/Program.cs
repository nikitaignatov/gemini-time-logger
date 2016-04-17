using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using BaconTime.Terminal.Commands;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Entity;
using Fclp;
using SimpleInjector;

namespace BaconTime.Terminal
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new CommandRunner().Run(args);
            }
            catch (Exception e)
            {
                Console.WriteLine("\nError occured.");
                Console.WriteLine(e.Message);
            }
        }
    }
}
