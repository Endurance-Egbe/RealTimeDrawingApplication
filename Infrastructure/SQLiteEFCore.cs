using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.Infrastructure
{
    public class SQLiteEFCore : UserDbContext
    {
        public SQLiteEFCore() : base()
        {

        }

        protected override void InitialiseDatabase(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite
                ("data source=DrawingShapeApplication.db");
        }

    }
}
