using Microsoft.EntityFrameworkCore;

namespace WebAPI.Logic
{
    public static class Validation
    {
        private static int MinNameLength = 5;
        private static int MaxNameLength = 25;
        private static float MaxGamePrice = 999;
        private static float MaxAchCount = 9999;

        public static void ValidateList<T>(DbSet<T> list) where T : class
        {
            if (list.Count() == 0)
                throw new Exception("Table contains no elements");
        }

        public static void ValidateGameID(int id)
        {
            ValidateList(new ApplicationContext().Games);

            if (new ApplicationContext().Games.Where(x => x.ID == id).FirstOrDefault() == null)
                throw new Exception($"Game with ID {id} do not exist");
        }

        public static void ValidateDeveloperID(int id)
        {
            ValidateList(new ApplicationContext().Developers);

            if (new ApplicationContext().Developers.Where(x => x.ID == id).FirstOrDefault() == null)
                throw new Exception($"Developer with ID {id} do not exist");
        }

        public static void ValidateDeveloperName(string name)
        {
            ValidateNameLength(name);

            if (new ApplicationContext().Developers.Where(x => x.Name.ToLower() == name.ToLower()).Any())
                throw new Exception($"Developer with name {name} already exists");
        }

        public static void ValidateGameName(string name)
        {
            ValidateNameLength(name);

            if (new ApplicationContext().Games.Where(x => x.Name.ToLower() == name.ToLower()).Any())
                throw new Exception($"Game with name {name} already exists");
        }

        public static void ValidateNameLength(string name)
        {
            if (name.Length < MinNameLength)
                throw new Exception($"Name should be at least {MinNameLength} symbols long");

            if (name.Length > MaxNameLength)
                throw new Exception($"Name should be shorter than {MaxNameLength} symbols");
        }

        public static void ValidateGamePrice(float price)
        {
            if (price < 0)
                throw new Exception($"Game price can't be negative");

            if (price > MaxGamePrice)
                throw new Exception($"Game price can't be more than {MaxGamePrice}");
        }

        public static void ValidateAchievementsCount(int achCount)
        {
            if (achCount < 0)
                throw new Exception($"Achievements count can't be negative");

            if (achCount > MaxAchCount)
                throw new Exception($"Achievements count can't be more than {MaxAchCount}");

        }

    }
}
