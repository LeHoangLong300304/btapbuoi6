using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace btapb6.Models
{
    public partial class studentDBContext : DbContext
    {
        public studentDBContext()
            : base("name=studentDBContext")
        {
        }

        public virtual DbSet<Faculty> Faculties{ get; set; }
        public virtual DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
