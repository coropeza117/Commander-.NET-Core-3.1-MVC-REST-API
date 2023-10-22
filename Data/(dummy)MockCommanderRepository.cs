using Commander.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commander.Data
{
    public class MockCommanderRepository : ICommanderRepository
    {
        public void CreateCommand(Command cmd)
        {
            throw new NotImplementedException();
        }

        public void DeleteCommand(Command cmd)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Command> GetAllCommands()
        {
            //  return a list of mock command objects back 
            var commands = new List<Command>
            {
                new Command{ Id = 0, HowTo = "Bad-Joke data Here", Line = "Bad-another Joke", Platform = "Bad-more jokes" },
                new Command{ Id = 1, HowTo = "Good-Joke data Here", Line = "Good-another Joke", Platform = "Good-more jokes" },
                new Command{ Id = 2, HowTo = "Great-Joke data Here", Line = "Great-another Joke", Platform = "Great-more jokes" }
            };

            return commands;
        }

        public Command GetCommandById(int id)
        {
            return new Command { Id = 0,HowTo = "Joke data Here",Line = "another Joke",Platform = "more jokes" };
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void UpdateCommand(Command cmd)
        {
            throw new NotImplementedException();
        }
    }
}
