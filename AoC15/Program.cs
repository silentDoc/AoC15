using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace AoC15
{
    internal class Program
    {
        static void Main()
        {
            int day = 15;
            int part = 2;
            bool test = false;

            string input = "./Input/day" + day.ToString() + "_1";
            input += (test) ? "_test.txt" : ".txt";

            Console.WriteLine("AoC 2015 - Day {0} , Part {1} - Test Data {2}", day, part, test);

            string result = day switch
            {
                1 => Program.Day1(input, part).ToString(),
                2 => Program.Day2(input, part).ToString(),
                3 => Program.Day3(input, part).ToString(),
                4 => Program.Day4(input, part).ToString(),
                5 => Program.Day5((part == 2 && test) ? "./Input/day5_2_test.txt" : input, part).ToString(),
                6 => Program.Day6(input, part).ToString(),
                7 => Program.Day7(input, part).ToString(),
                8 => Program.Day8(input, part).ToString(),
                9 => Program.Day9(input, part).ToString(),
                10 => Program.Day10(input, part).ToString(),
                11 => Program.Day11(input, part).ToString(),
                12 => Program.Day12(input, part).ToString(),
                13 => Program.Day13(input, part).ToString(),
                14 => Program.Day14(input, part).ToString(),
                15 => Program.Day15(input, part).ToString(),
                _ => throw new ArgumentException("Wrong day number - unimplemented"),
            };
            Console.WriteLine("Result : {0}", result);
        }

        static int Day1(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            Day1.Elevator elevator =  new(lines[0]);

            return part == 1 ? elevator.FinalFloor : elevator.BasementEntry();
        }

        static int Day2(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            var boxes = lines.Select(x => new Day2.PresentBox(x)).ToList();

            return part == 1 ? boxes.Sum(x => x.WrapArea) : boxes.Sum(x => x.RibbonLength);
        }

        static int Day3(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            var routes = lines.Select(x => new Day3.PresentRoute(x, part)).ToList();

            return routes.Sum(x=>x.VisitedHouses);
        }

        static int Day4(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            var miner = new Day4.AdventCoinMiner(lines[0]);
            
            return miner.Mine(part);
        }

        static int Day5(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            var niceLines = lines.Select(x => new Day5.StringChecker(x, part).IsNice).ToList();
            return niceLines.Where(x => x == true).Count();
        }

        static int Day6(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            Day6.LightManager lightManager = new(part);

            foreach (var line in lines)
                lightManager.DoInstruction(line);

            return lightManager.CountLights();
        }

        static int Day7(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();

            Day7.CircuitManager cm = new(part);
            cm.BuildCircuit(lines);
            
            if(part ==1) 
                return cm.GetWireValue("a");

            // Functional version for part 2
            Day7.CircuitManagerFunctional cmf= new();
            cmf.BuildCircuit(lines);
            var signal = cmf.GetWireValue("a");
            
            
            cm.OverrideWire("b", AoC15.Day7.WireOperations.assign, signal, "", "");
            return cm.GetWireValue("a");
        }

        static int Day8(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            Day8.StringMemory stringMemo = new();
            int value = (part == 1) ? stringMemo.Process(lines)
                                    : stringMemo.ProcessP2(lines);
            return value;
        }

        static int Day9(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            var tp = new Day9.TripPlanner(lines);

            return tp.GetRoute(part);
        }

        static int Day10(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            var lookAndSay = new Day10.LookAndSay();


            return (part == 1) ? lookAndSay.PlayTimes(lines[0],40)
                               : lookAndSay.PlayTimes(lines[0],50);
        }

        static string Day11(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            Day11.PasswordGenerator pg = new();
            var firstPass = pg.FindNextPass(lines[0]);
            return (part == 1) ? firstPass
                               : pg.FindNextPass(firstPass);
        }

        static int Day12(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            Day12.JSONHelper jh = new();

            //return jh.GetSum(lines[0]);   // Part 1
            return jh.GetSumJson(lines[0], part);
        }

        static int Day13(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            Day13.DinnerTable dt = new(lines);
            if (part == 2)
                dt.AddMyself();
            return dt.GetHappiness(part);
        }

        static int Day14(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            Day14.ReindeerRacer racer = new(lines);

            return (part == 1) ? racer.Race(2503)
                               : racer.RaceNewSystem(2503);
        }

        static long Day15(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            Day15.RecipeManager rm = new();
            Console.WriteLine(rm.ParseIngredients(lines));
            rm.FindBestCookieBruteForce();

            return rm.FindBestCookieBruteForce(part); 

        }
    }
}