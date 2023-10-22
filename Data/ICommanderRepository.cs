using Commander.BasicAuth;
using Commander.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commander.Data
{
    public interface ICommanderRepository
    {
        bool SaveChanges();

        //  give me a list of all our command objects/resource
        IEnumerable<Command> GetAllCommands();
        //^ return type <of type Command> ...Get method name 

        // return a single command back to user based on user provided Id
        Command GetCommandById(int id);
        //^ return type Command...Get method name (passing int id)

        void CreateCommand(Command cmd);

        void UpdateCommand(Command cmd);

        void DeleteCommand(Command cmd);
    }
}
