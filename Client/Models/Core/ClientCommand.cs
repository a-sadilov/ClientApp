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
        /*public string Cmd
        {
            get { return command.ToString();}
            set { command = Enum.Parse(Command, value);}
        }*/
    }
}
