namespace AoC15
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Program _instance = new Program();
            string input = "";
            int result = -1;

            int day = 1;
            int part = 1;
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
                default:
                    break;
            }
            Console.WriteLine("Key to exit");
            Console.ReadLine();
        }

        int Day1(string input, int part)
        {
            var lines = File.ReadLines(input).ToList();

            return 0;
        }
    }
}