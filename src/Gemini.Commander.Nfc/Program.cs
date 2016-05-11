using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;

namespace Gemini.Commander.Nfc
{
    public class Program
    {
        public static void Main()
        {
            var reader = new CardReader();

            reader.CreateLog = x =>
            {
                Console.WriteLine($"Begin: { x.Card}");
                return x;
            };

            reader.UpdateLog = x =>
            {
                Console.WriteLine($"End  : {x.Card}");
            };

            reader.Initialize();
            Console.ReadKey();
        }
    }
}