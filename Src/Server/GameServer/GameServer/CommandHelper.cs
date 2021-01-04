using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    class CommandHelper
    {
        public static void Run()
        {
            bool run = true;
            while (run)
            {
                Console.Write(">");
                string line = Console.ReadLine();
                switch (line.ToLower().Trim())
                {
                    case "exit":
                        run = false;
                        break;
                    case "clear":
                        Services.DBService.Instance.Entities.Characters.RemoveRange(Services.DBService.Instance.Entities.Characters);
                        Services.DBService.Instance.Entities.SaveChanges();
                        break;
                    default:
                        Help();
                        break;
                }
            }
        }

        public static void Help()
        {
            Console.Write(@"
Help:
    exit    Exit Game Server
    help    Show Help
    clear   Clear character
");
        }
    }
}
