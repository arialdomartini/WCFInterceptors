using System;
using System.Collections.Generic;
using WCFInterceptors;


namespace TheServer
{
    class Program
    {

      static void Main(string[] args)
      {

        var host = new Hostme();
        Console.WriteLine("Listening on 8889...");
        Console.ReadLine();
      }
    }
}
