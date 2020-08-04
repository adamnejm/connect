using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Connect
{
    public static class Program
    {

        static void Main(string[] args)
        {
            var parameters = new Dictionary<string, int>()
            {
                { "-w", 7 },
                { "-h", 6 },
                { "-needed", 4 }
            };
            
            var flags = new Dictionary<string, bool>()
            {
                { "-noclear", false },
                { "-startrandom", false },
            };

            for (int i = 0; i < args.Length; i++)
            {
                if (parameters.ContainsKey(args[i]) && i < args.Length - 1)
                {
                    if (int.TryParse(args[i + 1], out int paramValue))
                        parameters[args[i]] = paramValue;
                }
                else if (flags.ContainsKey(args[i]))
                {
                    flags[args[i]] = true;
                }
            }

            Connect connect = new Connect();
            connect.noClear = flags["-noclear"];
            connect.startRandom = flags["-startrandom"];
            connect.NewGame(parameters["-w"], parameters["-h"], parameters["-needed"]);
        }

    }
}
