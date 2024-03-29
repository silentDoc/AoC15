﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC15.Day02
{
    internal class PresentBox
    {
        int length;
        int width;
        int height;

        public int WrapArea = -1;
        public int RibbonLength = -1;

        public PresentBox(string dimensions)
        {
            string patternText = @"\d+";
            Regex r = new(patternText);
            var dims = r.Matches(dimensions).Select(x => int.Parse(x.Value)).ToList();

            length = dims[0];
            width = dims[1];
            height = dims[2];

            var smallerSides = dims.OrderBy(x => x).Take(2).ToList();

            WrapArea = 2 * length * width
                       + 2 * width * height
                       + 2 * height * length
                       + smallerSides[0] * smallerSides[1];

            RibbonLength = smallerSides[0] * 2
                           + smallerSides[1] * 2
                           + length * width * height;
        }
    }
}
