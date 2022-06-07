using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models.Core
{
    class ClientCommand
    {
        public Command command;
        public ClientCommand(Command cmd)
        {
            command = cmd;
        }
    }
}
