using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC15.Day18
{
    record struct Coord
    {
        public int x;
        public int y;
    }

    internal class LightPanel
    {
        public int part;
        public int width;
        public int height;
        public List<char[]> panel;

        public LightPanel(List<string> rows, int part = 1)
        {
            this.part = part;
            width = rows[0].Length;
            height = rows.Count;

            panel = new();
            foreach (var row in rows)
                panel.Add(row.ToCharArray());
        }

        public LightPanel(LightPanel otherPanel)
        {
            part = otherPanel.part;
            width = otherPanel.width;
            height = otherPanel.height;
            
                panel = new();
            foreach (var row in otherPanel.panel)
                panel.Add(row.ToList().ToArray());  // Copy the row, not add it by refernece !
        }

        public List<Coord> getNeighBors(int cx, int cy)
        {
            List<Coord> retVal = new()
            {
                new Coord() { x = cx - 1, y = cy - 1 },
                new Coord() { x = cx, y = cy - 1 },
                new Coord() { x = cx + 1, y = cy - 1 },

                new Coord() { x = cx - 1, y = cy },
                new Coord() { x = cx + 1, y = cy },

                new Coord() { x = cx - 1, y = cy + 1 },
                new Coord() { x = cx, y = cy + 1 },
                new Coord() { x = cx + 1, y = cy + 1 }
            };

            return retVal.Where(c => (c.x >= 0 && c.x < width) && (c.y >= 0 && c.y < height)).ToList();
        }

        public char GetPos(Coord pos)
            => panel[pos.y][pos.x];

        public char SetPos(Coord pos, char newVal)
            => panel[pos.y][pos.x] = newVal;

        public bool isOn(char value)
            => value == '#';

        public int NumLightsOn()
            => panel.Select(x => x.Where(y => y == '#').Count()).Sum();

        public void Log()
            => panel.ForEach(x => Console.WriteLine(x));

        public void Update(LightPanel previousState)
        {
            for (int j = 0; j < height; j++)
                for (int i = 0; i < width; i++)
                {
                    var neighbors = getNeighBors(i, j);
                    var currentPos = new Coord() { x = i, y = j };
                    var currentValue = previousState.GetPos(currentPos);

                    var neigh = neighbors.Select(x => previousState.GetPos(x)).ToList();
                    var numNeighsOn = neigh.Where(v => v == '#').Count();

                    bool isLightOn = isOn(currentValue);

                    if (isLightOn && (numNeighsOn != 2 && numNeighsOn != 3) )
                        SetPos(currentPos, '.');

                    if (!isLightOn && (numNeighsOn == 3) )
                        SetPos(currentPos, '#');
                }
        }
    }

    internal class LightPanelManager
    {
        public int SolvePart1(List<string> input, int iterations, int part = 1)
        {
            var panel = new LightPanel(input, part);
            var previousState = new LightPanel(panel);

            for (int i = 0; i < iterations; i++)
            {
                panel.Update(previousState);
                previousState = new LightPanel(panel);
            }

            return panel.NumLightsOn();
        }
    }
}
