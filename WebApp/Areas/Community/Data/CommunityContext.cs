using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Community.Data.Entities;

namespace WebApp.Areas.Community.Data
{

    public class CommunityContext : DbContext
    {
        #region DBSet

        public DbSet<Config> Config { get; set; }
        #endregion
        public CommunityContext(DbContextOptions<CommunityContext> options) : base(options)
        {

        }
    }
}
