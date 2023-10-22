using Commander.BasicAuth;
using Commander.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commander.Data
{
    public class CommanderContext : DbContext
    {
        public CommanderContext(DbContextOptions<CommanderContext> opt) : base(opt)
        {

        }

        //  DbSet of type Command, name: Commands {}
        public DbSet <Command> Commands { get; set; }
        //                     ^linked to the Migrations name:
    }
}
