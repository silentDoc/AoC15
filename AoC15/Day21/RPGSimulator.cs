using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC15.Day21
{
    enum ShopItemType
    { 
        Weapon = 0,     
        Armor = 1,
        Ring = 2
    }

    class ShopItem
    {
        public string name;
        public ShopItemType itemType;
        public int cost;
        public int damage;
        public int armor;

        public ShopItem(string name, ShopItemType itemType, int cost, int damage, int armor)
        {
            this.name = name;
            this.itemType = itemType;
            this.cost = cost;
            this.damage = damage;
            this.armor = armor;
        }
    }

    class Setup
    {
        List<ShopItem> items = new();
        public bool playerWins = false;
        public void AddItem(ShopItem item)
            => items.Add(item);

        public int damage
            => items.Sum(x => x.damage);
        public int armor
            => items.Sum(x => x.armor);
        public int cost
            => items.Sum(x => x.cost);
    }

    internal class RPGSimulator
    {
        List<ShopItem> availableItems = new();
        int EnemyHitPoints = -1;
        int EnemyDamage = -1;
        int EnemyArmor = -1;


        public void ParseInput(List<string> lines)
        {
            EnemyHitPoints = int.Parse(lines[0].Replace("Hit Points: ", ""));
            EnemyDamage = int.Parse(lines[1].Replace("Damage: ", ""));
            EnemyArmor = int.Parse(lines[2].Replace("Armor: ", ""));
        }

        void PrepData()
        {
            availableItems.Clear();
            availableItems.Add(new ShopItem("Dagger", ShopItemType.Weapon, 8, 4, 0));
            availableItems.Add(new ShopItem("ShortSword", ShopItemType.Weapon, 10, 5, 0));
            availableItems.Add(new ShopItem("Warhammer", ShopItemType.Weapon, 25, 6, 0));
            availableItems.Add(new ShopItem("LongSword", ShopItemType.Weapon, 40, 7, 0));
            availableItems.Add(new ShopItem("GreatAxe", ShopItemType.Weapon, 74, 8, 0));

            availableItems.Add(new ShopItem("Leather", ShopItemType.Armor, 13, 0, 1));
            availableItems.Add(new ShopItem("Chainmail", ShopItemType.Armor, 31, 0, 2));
            availableItems.Add(new ShopItem("Splintmail", ShopItemType.Armor, 53, 0, 3));
            availableItems.Add(new ShopItem("Bandedmail", ShopItemType.Armor, 75, 0, 4));
            availableItems.Add(new ShopItem("Platemail", ShopItemType.Armor, 102, 0, 5));
            availableItems.Add(new ShopItem("No_Armor", ShopItemType.Armor, 0, 0, 0));     // Armor is optional

            availableItems.Add(new ShopItem("Damage_1", ShopItemType.Ring, 25, 1, 0));
            availableItems.Add(new ShopItem("Damage_2", ShopItemType.Ring, 50, 2, 0));
            availableItems.Add(new ShopItem("Damage_3", ShopItemType.Ring, 100, 3, 0));
            availableItems.Add(new ShopItem("Defense_1", ShopItemType.Ring, 20, 0, 1));
            availableItems.Add(new ShopItem("Defense_2", ShopItemType.Ring, 40, 0, 2));
            availableItems.Add(new ShopItem("Defense_3", ShopItemType.Ring, 80, 0, 3));
            availableItems.Add(new ShopItem("No_Ring_1", ShopItemType.Ring, 0, 0, 0));    // We can wear 0, 1 or 2 
            availableItems.Add(new ShopItem("No_Ring_2", ShopItemType.Ring, 0, 0, 0));    // rings
        }


        Setup GetSetup(List<ShopItem> inventory)
        {
            Setup setup = new();

            inventory.ForEach(x => setup.AddItem(x));
            return setup;
        }

        IEnumerable<List<ShopItem>> GenerateItemCombination(List<ShopItem> available, List<ShopItem> used)
        {
            // Similar to what we did with the fridge filler puzzle, but we have some knowledge about 
            // the rules of setup confection - so we can reduce the number of items to be considered
            // at each step when doing the setup
            for (int n = 0; n < available.Count; n++)
            {
                var nextItem = available[n];
                var newList = used.ToList();
                newList.Add(nextItem);

                var numWeapons = used.Count(x => x.itemType == ShopItemType.Weapon);
                var numArmors = used.Count(x => x.itemType == ShopItemType.Armor);
                var numRings = used.Count(x => x.itemType == ShopItemType.Ring);

                _ = nextItem.itemType switch
                {
                    ShopItemType.Weapon => numWeapons++,
                    ShopItemType.Armor => numArmors++,
                    ShopItemType.Ring => numRings++,
                    _ => throw new Exception("Invalid item type")
                };

                if (numWeapons == 1 && numArmors == 1 && numRings == 2)
                    yield return newList.ToList();
                else
                {
                    string ringName = "";
                    if (numRings == 1)
                        ringName = nextItem.name;


                    List<ShopItem> nextAvailable = (numArmors, numRings) switch
                    {
                        (0, _) => availableItems.Where(x => x.itemType == ShopItemType.Armor).ToList(),
                        (_, 0) => availableItems.Where(x => x.itemType == ShopItemType.Ring).ToList(),
                        (_, 1) => availableItems.Where(x => x.itemType == ShopItemType.Ring && x.name != ringName).ToList(),
                        (_, _) => throw new Exception("Invalid combination")
                    };

                    foreach (var d in GenerateItemCombination(nextAvailable, newList))
                    {
                        yield return d.ToList();
                    }
                }
            }
        }

        void SimCombat(Setup setup, int myHitPoints)
        {
            int enemyHitPoints = EnemyHitPoints;
            int enemyDamage = EnemyDamage;
            int enemyArmor = EnemyArmor;
            int hitPoints = myHitPoints;
            bool playerRound = true;

            while (hitPoints > 0 && enemyHitPoints > 0)
            { 
                if(playerRound)
                    enemyHitPoints = enemyHitPoints - Math.Max(1, setup.damage - enemyArmor);
                else
                    hitPoints = hitPoints - Math.Max(1, enemyDamage - setup.armor);
                playerRound = !playerRound;
            }

            setup.playerWins = (hitPoints > 0);
        }

        int CheaperWinningSetup()
        {
            List<Setup> setups = new List<Setup>();

            PrepData();
            var startingOptions = availableItems.Where(x => x.itemType == ShopItemType.Weapon).ToList();
            var combs = GenerateItemCombination(startingOptions, new List<ShopItem>()).ToList();
            combs.ForEach(listItems => setups.Add(GetSetup(listItems)));

            int myHitPoints = 100;
            setups.ForEach(x => SimCombat(x, myHitPoints));

            return setups.Where(x => x.playerWins).Min(x => x.cost);
        }

        public int Solve(int part = 1)
            => (part == 1) ? CheaperWinningSetup() : 0;
    }
}
