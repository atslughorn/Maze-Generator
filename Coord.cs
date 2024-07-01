using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze_Generator
{
    public readonly struct Coord(int x, int y)
    {
        public int X { get; } = x;
        public int Y { get; } = y;

        public readonly Coord GetAbove() // These methods might seem unneccessary, but they make the code more readable
        {
            return new Coord(X, Y - 1);
        }
        public readonly Coord GetBelow()
        {
            return new Coord(X, Y + 1);
        }
        public readonly Coord GetLeft()
        {
            return new Coord(X - 1, Y);
        }
        public readonly Coord GetRight()
        {
            return new Coord(X + 1, Y);
        }

        public override readonly string ToString() => $"({X}, {Y})";
    }
}
