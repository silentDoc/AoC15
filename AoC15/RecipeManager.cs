using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC15.Day15
{
    class Ingredient
    {
        string name;
        int capacity;
        int durability;
        int flavor;
        int texture;
        int calories;

        public Ingredient(string name, int capacity, int durability, int flavor, int texture, int calories)
        {
            this.name = name;
            this.capacity = capacity;
            this.durability = durability;
            this.flavor = flavor;
            this.texture = texture;
            this.calories = calories;
        }

        public int[] Properties(int teaspoons)
            => new int[4] { capacity * teaspoons, durability * teaspoons, flavor * teaspoons, texture * teaspoons };

        public int Calories(int teaspoons)
            => calories * teaspoons;
    }


    internal class RecipeManager
    {
        List<Ingredient> listIngredients = new();

        public int ParseIngredients(List<string> ingredientList)
        {
            var regex = new Regex(@"^(\w+): capacity (-?[0-9]\d*), durability (-?[0-9]\d*), flavor (-?[0-9]\d*), texture (-?[0-9]\d*), calories (-?[0-9]\d*)");
            foreach (var entry in ingredientList)
            {
                var match = regex.Match(entry);
                if (match.Groups.Count < 7)
                    throw new Exception("Ingrdient entry in wrong format");

                var name = match.Groups[1].Value;
                var capacity = int.Parse(match.Groups[2].Value);
                var durability = int.Parse(match.Groups[3].Value);
                var flavor = int.Parse(match.Groups[4].Value);
                var texture = int.Parse(match.Groups[5].Value);
                var calories = int.Parse(match.Groups[6].Value);

                listIngredients.Add(new Ingredient(name, capacity, durability, flavor, texture, calories));
            }
            return listIngredients.Count;
        }

        public long FindBestCookieBruteForce(int part =1)
        {
            List<long> scores = new();
            List<int[]> proportions = new();

            var numIngredients = listIngredients.Count;
            var cycles = (int) Math.Pow(100, numIngredients);
            int[] teaspons = new int[numIngredients];


            for (int i = 0; i < cycles + 1; i++)
            {
                for (int j = 0; j < numIngredients; j++)
                {
                    var num = i % (int)Math.Pow(100, (j+1));
                    var den = (int)Math.Pow(100, j);
                    teaspons[j] = num / den;
                }

                if (teaspons.Sum() != 100)
                    continue;

                if(part==1)
                    scores.Add(score(teaspons));
                else 
                {
                    var cals = teaspons.Select((x, index) => listIngredients[index].Calories(x)).Sum();
                    if (cals == 500)
                        scores.Add(score(teaspons));
                }
            }

            long result = scores.Max();
            return result;
        }

        long score(int[] teaspoons)
        {
            List<int[]> properties = new();

            for (int i = 0; i < listIngredients.Count; i++)
                properties.Add(listIngredients[i].Properties(teaspoons[i]));

            var suma = new int[properties[0].Length];
            suma.Select(x => x = 0);

            for (int i = 0; i < listIngredients.Count; i++)
                for (int j = 0; j< properties[0].Length; j++)
                    suma[j] += properties[i][j];

            if(suma.Any(x => x<0))
                return 0;

            var score = suma.Aggregate(1, (acc, val) => acc * val);
            return (long)score;
        }
    }
}
