using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace Barista1.Classes
{
    public partial class BaristaContext : DbContext
    {
        public SqlConnection connection { get; set; }
        public BaristaContext(): base("DefaultConnection")
        {
           connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        
    }
        //public DbSet<User> Users { get; set; }
        //public DbSet<Topic> Topics { get; set; }
        //public DbSet<Coctail> Coctails { get; set; }
        //public DbSet<Test> Tests { get; set; }
        //public DbSet<RecipesTest> RecipesTests { get; set; }
        //public DbSet<Answer> Answers { get; set; }
        //public DbSet<Question> Questions { get; set; }
        //public DbSet<IngrAnswer> IngrAnswers { get; set; }
    }
}
