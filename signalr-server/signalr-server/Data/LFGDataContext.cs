using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using signalr_server.Models;

namespace signalr_server.Data
{
    public class LFGDataContext : DbContext
    {
        public LFGDataContext (DbContextOptions<LFGDataContext> options)
            : base(options)
        {
        }

        public DbSet<signalr_server.Models.Computer> Computer { get; set; } = default!;
    }
}
