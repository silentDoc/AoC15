using System.Runtime.CompilerServices;

namespace AoC15
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Program _instance = new Program();
            string input = "";
            int result = -1;

            int day = 6;
            int part = 2;
            bool test = false;

            input = "./Input/day" + day.ToString() + "_1";
            input += (test) ? "_test.txt" : ".txt";

            Console.WriteLine("AoC 2015 - Day {0} , Part {1} - Test Data {2}", day, part, test);
            switch (day)
            {
                case 1:
                    result = _instance.Day1(input, part);
                    Console.WriteLine("Result : {0}", result);
                    break;
                case 2:
                    result = _instance.Day2(input, part);
                    Console.WriteLine("Result : {0}", result);
                    break;
                case 3:
                    result = _instance.Day3(input, part);
                    Console.WriteLine("Result : {0}", result);
                    break;
                case 4:
                    result = _instance.Day4(input, part);
                    Console.WriteLine("Result : {0}", result);
                    break;
                case 5:
                    if (part == 2 && test)
                        input = "./Input/day5_2_test.txt";
                    result = _instance.Day5(input, part);
                    Console.WriteLine("Result : {0}", result);
                    break;
                case 6:
                    result = _instance.Day6(input, part);
                    Console.WriteLine("Result : {0}", result);
                    break;
                default:
                    break;
            }
            Console.WriteLine("Key to exit");
            Console.ReadLine();
        }

        int Day1(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            Elevator elevator =  new(lines[0]);

            return part == 1 ? elevator.FinalFloor : elevator.BasementEntry();
        }

        int Day2(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            var boxes = lines.Select(x => new PresentBox(x)).ToList();

            return part == 1 ? boxes.Sum(x => x.WrapArea) : boxes.Sum(x => x.RibbonLength);
        }

        int Day3(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            var routes = lines.Select(x => new PresentRoute(x, part)).ToList();

            return routes.Sum(x=>x.VisitedHouses);
        }

        int Day4(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            var miner = new AdventCoinMiner(lines[0]);
            
            return miner.Mine(part);
        }

        int Day5(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            var niceLines = lines.Select(x => new StringChecker(x, part).IsNice).ToList();
            return niceLines.Where(x => x == true).Count();
        }
        int Day6(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            LightManager lightManager = new(part);

            foreach (var line in lines)
                lightManager.DoInstruction(line);

            return lightManager.CountLights();
        }
    }
}