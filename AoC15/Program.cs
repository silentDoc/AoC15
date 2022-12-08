using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace AoC15
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Program _instance = new Program();
            string input = "";
            int result = -1;

            int day = 8;
            int part = 1;
            bool test = false;

            input = "./Input/day" + day.ToString() + "_1";
            input += (test) ? "_test.txt" : ".txt";

            Console.WriteLine("AoC 2015 - Day {0} , Part {1} - Test Data {2}", day, part, test);

            result = day switch
            {
                1 => _instance.Day1(input, part),
                2 => _instance.Day2(input, part),
                3 => _instance.Day3(input, part),
                4 => _instance.Day4(input, part),
                5 => _instance.Day5((part == 2 && test) ? "./Input/day5_2_test.txt" : input, part),
                6 => _instance.Day6(input, part),
                7 => _instance.Day7(input, part),
                8 => _instance.Day8(input, part),
                _ => throw new ArgumentException("Wrong day number - unimplemented"),
            };
            Console.WriteLine("Result : {0}", result);
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

        int Day7(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();

            CircuitManager cm = new(part);
            cm.BuildCircuit(lines);
            
            if(part ==1) 
                return cm.GetWireValue("a");

            // Functional version for part 2
            CircuitManagerFunctional cmf= new();
            cmf.BuildCircuit(lines);
            var signal = cmf.GetWireValue("a");
            
            
            cm.OverrideWire("b", WireOperations.assign, signal, "", "");
            return cm.GetWireValue("a");
        }

        int Day8(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();
            StringMemory stringMemo = new();
            int value = stringMemo.Process(lines);

            return value;
        }
    }
}