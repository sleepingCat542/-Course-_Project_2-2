using Barista1.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barista1
{
    public class UnitOfWork /*: IDisposable*/
    {
        private BaristaContext db = new BaristaContext();
        private SQLUsersRepository usersRepository;
        private SQLAnswersRepository answersRepository;
        private SQLCoctailsRepository coctailsRepository;
        private SQLTestsRepository testsRepository;
        private SQLTopicsRepository topicsRepository;
        private SQLIngrAnswersRepository ingrAnswersRepository;
        private SQLRecipeTestsRepository recipeTestsRepository;
        private SQLQuestionsRepository questionsRepository;
        private SQLVideosRepository videosRepository;

        public SQLUsersRepository Users
        {
            get
            {
                if (usersRepository == null)
                    usersRepository = new SQLUsersRepository();
                return usersRepository;
            }
        }

        public SQLAnswersRepository Answers
        {
            get
            {
                if (answersRepository == null)
                    answersRepository = new SQLAnswersRepository();
                return answersRepository;
            }
        }

        public SQLTopicsRepository Topics
        {
            get
            {
                if (topicsRepository == null)
                    topicsRepository = new SQLTopicsRepository();
                return topicsRepository;
            }
        }

        public SQLCoctailsRepository Coctails
        {
            get
            {
                if (coctailsRepository == null)
                    coctailsRepository = new SQLCoctailsRepository();
                return coctailsRepository;
            }
        }

        public SQLVideosRepository Videos
        {
            get
            {
                if (videosRepository == null)
                    videosRepository = new SQLVideosRepository();
                return videosRepository;
            }
        }

        public SQLTestsRepository Tests
        {
            get
            {
                if (testsRepository == null)
                    testsRepository = new SQLTestsRepository();
                return testsRepository;
            }
        }

        public SQLRecipeTestsRepository RecipeTests
        {
            get
            {
                if (recipeTestsRepository == null)
                    recipeTestsRepository = new SQLRecipeTestsRepository();
                return recipeTestsRepository;
            }
        }

        public SQLQuestionsRepository Questions
        {
            get
            {
                if (questionsRepository == null)
                    questionsRepository = new SQLQuestionsRepository();
                return questionsRepository;
            }
        }

        public SQLIngrAnswersRepository IngrAnswers
        {
            get
            {
                if (ingrAnswersRepository == null)
                    ingrAnswersRepository = new SQLIngrAnswersRepository();
                return ingrAnswersRepository;
            }
        }

        public void Close()
        {
            db.connection.Close();
        }

    }
}
