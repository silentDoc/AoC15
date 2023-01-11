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
            int day = 23;
            int part = 1;
            bool test = false;

            string input = "./Input/day" + day.ToString() + "_1";
            input += (test) ? "_test.txt" : ".txt";

            Console.WriteLine("AoC 2015 - Day {0} , Part {1} - Test Data {2}", day, part, test);

            string result = day switch
            {
                1 => day1(input, part).ToString(),
                2 => day2(input, part).ToString(),
                3 => day3(input, part).ToString(),
                4 => day4(input, part).ToString(),
                5 => day5((part == 2 && test) ? "./Input/day5_2_test.txt" : input, part).ToString(),
                6 => day6(input, part).ToString(),
                7 => day7(input, part).ToString(),
                8 => day8(input, part).ToString(),
                9 => day9(input, part).ToString(),
                10 => day10(input, part).ToString(),
                11 => day11(input, part).ToString(),
                12 => day12(input, part).ToString(),
                13 => day13(input, part).ToString(),
                14 => day14(input, part).ToString(),
                15 => day15(input, part).ToString(),
                16 => day16(input, part).ToString(),
                17 => day17(input, part).ToString(),
                18 => day18(input, part).ToString(),
                19 => day19(input, part).ToString(),
                20 => day20(part).ToString(),
                21 => day21(input, part).ToString(),
                22 => day22(input, part).ToString(),
                23 => day23(input, part).ToString(),
                _ => throw new ArgumentException("Wrong day number - unimplemented"),
            };
            Console.WriteLine("Result : {0}", result);
        }

        static int day1(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            Day01.Elevator elevator =  new(lines[0]);
            return part == 1 ? elevator.FinalFloor : elevator.BasementEntry();
        }

        static int day2(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            var boxes = lines.Select(x => new Day02.PresentBox(x)).ToList();
            return part == 1 ? boxes.Sum(x => x.WrapArea) : boxes.Sum(x => x.RibbonLength);
        }

        static int day3(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            var routes = lines.Select(x => new Day03.PresentRoute(x, part)).ToList();
            return routes.Sum(x=>x.VisitedHouses);
        }

        static int day4(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            var miner = new Day04.AdventCoinMiner(lines[0]);
            
            return miner.Mine(part);
        }

        static int day5(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            var niceLines = lines.Select(x => new Day05.StringChecker(x, part).IsNice).ToList();
            return niceLines.Where(x => x == true).Count();
        }

        static int day6(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            Day06.LightManager lightManager = new(part);

            foreach (var line in lines)
                lightManager.DoInstruction(line);

            return lightManager.CountLights();
        }

        static int day7(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();

            Day07.CircuitManager cm = new(part);
            cm.BuildCircuit(lines);
            
            if(part ==1) 
                return cm.GetWireValue("a");

            // Functional version for part 2
            Day07.CircuitManagerFunctional cmf= new();
            cmf.BuildCircuit(lines);
            var signal = cmf.GetWireValue("a");
            
            
            cm.OverrideWire("b", Day07.WireOperations.assign, signal, "", "");
            return cm.GetWireValue("a");
        }

        static int day8(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            Day08.StringMemory stringMemo = new();
            int value = (part == 1) ? stringMemo.Process(lines)
                                    : stringMemo.ProcessP2(lines);
            return value;
        }

        static int day9(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            var tp = new Day09.TripPlanner(lines);

            return tp.GetRoute(part);
        }

        static int day10(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            var lookAndSay = new Day10.LookAndSay();


            return (part == 1) ? lookAndSay.PlayTimes(lines[0],40)
                               : lookAndSay.PlayTimes(lines[0],50);
        }

        static string day11(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            Day11.PasswordGenerator pg = new();
            var firstPass = pg.FindNextPass(lines[0]);
            return (part == 1) ? firstPass
                               : pg.FindNextPass(firstPass);
        }

        static int day12(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            Day12.JSONHelper jh = new();

            //return jh.GetSum(lines[0]);   // Part 1
            return jh.GetSumJson(lines[0], part);
        }

        static int day13(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            Day13.DinnerTable dt = new(lines);
            if (part == 2)
                dt.AddMyself();
            return dt.GetHappiness(part);
        }

        static int day14(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            Day14.ReindeerRacer racer = new(lines);

            return (part == 1) ? racer.Race(2503)
                               : racer.RaceNewSystem(2503);
        }

        static long day15(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            Day15.RecipeManager rm = new();
            Console.WriteLine(rm.ParseIngredients(lines));
            rm.FindBestCookieBruteForce();

            return rm.FindBestCookieBruteForce(part); 
        }

        static int day16(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            return (part ==1) ? Day16.AuntSueFinder.Part1(lines)
                              : Day16.AuntSueFinder.Part2(lines);
        }

        static int day17(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            int capacity = (input.IndexOf("test") != -1) ? 25 : 150;
            Day17.FridgeFiller ff = new(capacity, lines);

            return (part == 1) ? ff.SolvePart1()
                               : ff.SolvePart2();
        }

        static int day18(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            int iterations = (input.IndexOf("test") != -1) ? ((part ==1) ? 4 :5) : 100;
            Day18.LightPanelManager lpm = new();

            return lpm.Solve(lines, iterations, part);
        }

        static int day19(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day19.MoleculeBuilder mb = new(lines);
            
            return mb.Solve(part);
        }

        static int day20(int part)
        {
            Day20.SantaDeliver sd = new();
            return sd.Solve(34000000, part);
        }

        static int day21(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day21.RPGSimulator rpg = new();
            rpg.ParseInput(lines);

            return rpg.Solve(part);
        }

        static int day22(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day22.RPGWizard rpg = new();
            rpg.ParseInput(lines);

            return rpg.Solve(part);
        }

        static int day23(string input, int part)
        {
            var lines = File.ReadAllLines(input).ToList();
            Day23.JaneMarieComputer computer = new();
            computer.ParseInput(lines);
            return computer.Solve(part);
        }
    }
}