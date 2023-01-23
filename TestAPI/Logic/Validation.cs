using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebAPI.Models;

namespace WebAPI.Logic
{
    public static class Validation
    {
        private static int MinNameLength = 5;
        private static int MaxNameLength = 25;
        public static void Test()
        {
            throw new Exception("aboba");
        }

        public static void ValidateList<T>(DbSet<T> list) where T : class
        {
            if (list.Count() == 0)
                throw new Exception("Table contains no elements");
        }

        public static void ValidateGameID(DbSet<Game> list, int id)
        {
            ValidateList(list);

            if (list.Where(x => x.ID == id).FirstOrDefault() == null)
                throw new Exception($"Game with ID {id} do not exist");
        }

        public static void ValidateDeveloperID(DbSet<Developer> list, int id)
        {
            ValidateList(list);

            if (list.Where(x => x.ID == id).FirstOrDefault() == null)
                throw new Exception($"Developer with ID {id} do not exist");
        }

        public static void ValidateName(string name)
        {
            if(name.Length < MinNameLength)
                throw new Exception($"Name should be longer than {MinNameLength} symbols");

            if (name.Length > MaxNameLength)
                throw new Exception($"Name should be shorter than {MaxNameLength} symbols");
        }



    }
}
