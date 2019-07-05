using Barista1.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barista1
{
    

    public class SQLUsersRepository : IRepository<User>
    {

        BaristaContext db;
              
        public SQLUsersRepository()
        {
            this.db = new BaristaContext();
        }

        public SqlCommand GetList()
        {
            if (db.connection.State == ConnectionState.Open) { db.connection.Close(); }
            db.connection.Open();
            string sqlExpression = "SELECT * FROM Users";
            SqlCommand command = new SqlCommand(sqlExpression, db.connection);
            return command;
        }

        public void Add(params object[] list)
        {
            if (db.connection.State == ConnectionState.Open)
            {
                db.connection.Close();
            }
            db.connection.Open();
            string sqlExpression = $"INSERT INTO Users ([Login], [Password], [E-mail]) VALUES (N'{list[0]}', N'{list[1]}', N'{list[2]}')";
            SqlCommand command = new SqlCommand(sqlExpression, db.connection);
            command.ExecuteNonQuery();
            db.connection.Close();
        }

        public void UpdateImage(string fileName, object user)
        {
            if (db.connection.State == ConnectionState.Open){db.connection.Close();}
            db.connection.Open();
            string sqlExpression = $@"UPDATE USERS SET ImageDate = N'{fileName}' WHERE IdUser = {user}";
            SqlCommand command = new SqlCommand(sqlExpression, db.connection);
            command.ExecuteNonQuery();
            db.connection.Close();
        }
    }

    public class SQLAnswersRepository : IRepository<Answer>
    {

        BaristaContext db;

        public SQLAnswersRepository()
        {
            this.db = new BaristaContext();
        }

        public SqlCommand GetList()
        {
            if (db.connection.State == ConnectionState.Open)
            {
                db.connection.Close();
            }
            db.connection.Open();
            string sqlExpression = "SELECT * FROM Answer";
            SqlCommand command = new SqlCommand(sqlExpression, db.connection);
            return command;
        }

        public void Add(params object[] list)
        {
            if (db.connection.State == ConnectionState.Open)
            {
                db.connection.Close();
            }
            db.connection.Open();
            //
            string sqlExpression = $"INSERT INTO Users ([Login], [Password], [E-mail]) VALUES (N'{list[0]}', N'{list[1]}', N'{list[2]}')";
            SqlCommand command = new SqlCommand(sqlExpression, db.connection);
            command.ExecuteNonQuery();
            db.connection.Close();
        }
    }

    public class SQLCoctailsRepository : IRepository<Coctail>
    {

        BaristaContext db;

        public SQLCoctailsRepository()
        {
            this.db = new BaristaContext();
        }

        public SqlCommand GetList()
        {
            if (db.connection.State == ConnectionState.Open) { db.connection.Close(); }
            db.connection.Open();
            string sqlExpression = "SELECT * FROM ForRecipes";
            SqlCommand command = new SqlCommand(sqlExpression, db.connection);
            return command;
        }

        public void Add(params object[] list)
        {
            if (db.connection.State == ConnectionState.Open)
            {
                db.connection.Close();
            }
            db.connection.Open();
            string sqlExpression = $"INSERT INTO ForRecipes ([NameForRecipes], [RightIngredients], [Recipe]) VALUES (N'{list[0]}'," +
                                            $" N'{list[1]}', N'{list[2]}')";
            SqlCommand command = new SqlCommand(sqlExpression, db.connection);
            command.ExecuteNonQuery();
            db.connection.Close();
        }

        public void Delete(params object[] list)
        {
            if (db.connection.State == ConnectionState.Open){db.connection.Close();}
            db.connection.Open();
            string sqlExpression = $"Delete from ForRecipes where NameForRecipes=N'{list[0]}'";
            SqlCommand command = new SqlCommand(sqlExpression, db.connection);
            command.ExecuteNonQuery();
            db.connection.Close();
        }
    }

    public class SQLIngrAnswersRepository : IRepository<IngrAnswer>
    {

        BaristaContext db;

        public SQLIngrAnswersRepository()
        {
            this.db = new BaristaContext();
        }

        public SqlCommand GetList()
        {
            if (db.connection.State == ConnectionState.Open) { db.connection.Close(); }
            db.connection.Open();
            string sqlExpression = "SELECT * from RecipeAnswer";
            SqlCommand command = new SqlCommand(sqlExpression, db.connection);
            return command;
        }

        public SqlCommand GetListName(params object[] list)
        {
            if (db.connection.State == ConnectionState.Open) { db.connection.Close(); }
            db.connection.Open();
            string sqlExpression = $"SELECT * FROM TestRecipes where TestName=N'{list[0]}'";
            SqlCommand command = new SqlCommand(sqlExpression, db.connection);
            return command;
        }

        public SqlCommand GetListNumber(params object[] list)
        {
            if (db.connection.State == ConnectionState.Open) { db.connection.Close(); }
            db.connection.Open();
            string sqlExpression = $"SELECT * FROM RecipeAnswer where TestNumber={list[0]}";
            SqlCommand command = new SqlCommand(sqlExpression, db.connection);
            return command;
        }

        public void Add(params object[] list)
        {
            if (db.connection.State == ConnectionState.Open)
            {
                db.connection.Close();
            }
            db.connection.Open();
            string sqlExpression = $"INSERT INTO RecipeAnswer (Answer, IsRight, TestNumber) VALUES (N'{list[0]}', N'{list[1]}', {list[2]})";
            SqlCommand command = new SqlCommand(sqlExpression, db.connection);
            command.ExecuteNonQuery();
            db.connection.Close();
        }

        
    }

    public class SQLQuestionsRepository : IRepository<Question>
    {

        BaristaContext db;

        public SQLQuestionsRepository()
        {
            this.db = new BaristaContext();
        }

        public SqlCommand GetList()
        {
            if (db.connection.State == ConnectionState.Open) { db.connection.Close(); }
            db.connection.Open();
            string sqlExpression = "SELECT * FROM Questions";
            SqlCommand command = new SqlCommand(sqlExpression, db.connection);
            return command;
        }

        public void Add(params object[] list)
        {
            if (db.connection.State == ConnectionState.Open)
            {
                db.connection.Close();
            }
            db.connection.Open();
            //
            string sqlExpression = $"INSERT INTO Users ([Login], [Password], [E-mail]) VALUES (N'{list[0]}', N'{list[1]}', N'{list[2]}')";
            SqlCommand command = new SqlCommand(sqlExpression, db.connection);
            command.ExecuteNonQuery();
            db.connection.Close();
        }
    }

    public class SQLRecipeTestsRepository : IRepository<RecipesTest>
    {

        BaristaContext db;

        public SQLRecipeTestsRepository()
        {
            this.db = new BaristaContext();
        }

        public SqlCommand GetList()
        {
            if (db.connection.State == ConnectionState.Open) { db.connection.Close(); }
            db.connection.Open();
            string sqlExpression = "SELECT * FROM TestRecipes";
            SqlCommand command = new SqlCommand(sqlExpression, db.connection);
            return command;
        }

        public void Add(params object[] list)
        {
            if (db.connection.State == ConnectionState.Open)
            {
                db.connection.Close();
            }
            db.connection.Open();
            string sqlExpression = $"INSERT INTO TestRecipes (TestName) VALUES (N'{list[0]}')";
            SqlCommand command = new SqlCommand(sqlExpression, db.connection);
            command.ExecuteNonQuery();
            db.connection.Close();
        }

        public int GetNumber(params object[] list)
        {
            if (db.connection.State == ConnectionState.Open)
            {
                db.connection.Close();
            }
            db.connection.Open();
            string sqlExpression = $"Select TestNumber from TestRecipes where TestName = N'{list[0]}'";
            SqlCommand command = new SqlCommand(sqlExpression, db.connection);
            return Convert.ToInt32(command.ExecuteScalar());
        }

        public void Delete(params object[] list)
        {
            if (db.connection.State == ConnectionState.Open)
            {
                db.connection.Close();
            }
            db.connection.Open();
            string sqlExpression = $"Delete from TestRecipes where TestName=N'{list[0]}'";
            SqlCommand command = new SqlCommand(sqlExpression, db.connection);
            command.ExecuteNonQuery();
            db.connection.Close();
        }

        
    }

    public class SQLTestsRepository : IRepository<Test>
    {

        BaristaContext db;

        public SQLTestsRepository()
        {
            this.db = new BaristaContext();
        }

        public SqlCommand GetList()
        {
            if (db.connection.State == ConnectionState.Open) { db.connection.Close(); }
            db.connection.Open();
            string sqlExpression = "SELECT * FROM Tests";
            SqlCommand command = new SqlCommand(sqlExpression, db.connection);
            return command;
        }

        public void Add(params object[] list)
        {
            if (db.connection.State == ConnectionState.Open)
            {
                db.connection.Close();
            }
            db.connection.Open();
            //
            string sqlExpression = $"INSERT INTO Users ([Login], [Password], [E-mail]) VALUES (N'{list[0]}', N'{list[1]}', N'{list[2]}')";
            SqlCommand command = new SqlCommand(sqlExpression, db.connection);
            command.ExecuteNonQuery();
            db.connection.Close();
        }
    }

    public class SQLTopicsRepository : IRepository<Topic>
    {

        BaristaContext db;

        public SQLTopicsRepository()
        {
            this.db = new BaristaContext();
        }

        public SqlCommand GetList()
        {
            if (db.connection.State == ConnectionState.Open) { db.connection.Close(); }
            db.connection.Open();
            string sqlExpression = "SELECT * FROM Topic";
            SqlCommand command = new SqlCommand(sqlExpression, db.connection);
            return command;
        }

        public void Add(params object[] list)
        {
            if (db.connection.State == ConnectionState.Open){db.connection.Close();}
            db.connection.Open();            
            string sqlExpression = $"INSERT INTO Topic ([Text], [Name]) VALUES (N'{list[0]}', N'{list[1]}')";
            SqlCommand command = new SqlCommand(sqlExpression, db.connection);
            command.ExecuteNonQuery();
            db.connection.Close();
        }

        public void Delete(params object[] list)
        {
            if (db.connection.State == ConnectionState.Open) { db.connection.Close(); }
            db.connection.Open();
            string sqlExpression = $"Delete from Topic where Name=N'{list[0]}'";
            SqlCommand command = new SqlCommand(sqlExpression, db.connection);
            command.ExecuteNonQuery();
            db.connection.Close();
        }
    }

    public class SQLVideosRepository : IRepository<Video>
    {

        BaristaContext db;

        public SQLVideosRepository()
        {
            this.db = new BaristaContext();
        }

        public SqlCommand GetList()
        {
            if (db.connection.State == ConnectionState.Open) { db.connection.Close(); }
            db.connection.Open();
            string sqlExpression = "SELECT * FROM Video";
            SqlCommand command = new SqlCommand(sqlExpression, db.connection);
            return command;
        }

        public void Add(params object[] list)
        {
            if (db.connection.State == ConnectionState.Open){db.connection.Close();}
            db.connection.Open();
            string sqlExpression = $"INSERT INTO Video ([Name], [Source]) VALUES (N'{list[0]}', N'{list[1]}')";
            SqlCommand command = new SqlCommand(sqlExpression, db.connection);
            command.ExecuteNonQuery();
            db.connection.Close();
        }
    }

}
