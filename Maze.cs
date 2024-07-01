using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;

namespace Maze_Generator
{
    internal class Maze
    {
        private readonly bool[,] grid;
        private readonly int realWidth;
        private readonly int realHeight;

        /// <summary>
        /// 
        /// </summary>
        public int RealWidth => realWidth;

        public int RealHeight => realHeight;
        public int BorderedWidth => realWidth + 2;
        public int BorderedHeight => realHeight + 2;

        public Maze(int width, int height)
        {
            realWidth = width * 2 - 1;
            realHeight = height * 2 - 1;
            // Array for walls and cells, there is some redundancy as some elements are fixed, however this format is very easy to convert to a bitmap
            grid = new bool[RealWidth, RealHeight]; // Default value is false
        }

        public bool this[Coord c]
        {
            get { return grid[c.X, c.Y]; }
            set {  grid[c.X, c.Y] = value; }
        }

        public bool InBounds(Coord c)
        {
            return c.X >= 0 && c.X < RealWidth && c.Y >= 0 && c.Y < RealHeight;
        }

        /// <summary>
        /// Add a border around the edge of the maze and convert it to pixels
        /// </summary>
        /// <returns>Pixels as an array of bytes</returns>
        public bool[,] GetGridWithBorder()
        {
            bool[,] newGrid = new bool[BorderedWidth, BorderedHeight];

            newGrid[1, 0] = true; // Gap for the entrance

            // Body
            for (int y = 0; y < RealHeight; y++)
            {
                for (int x = 0; x < RealWidth; x++)
                {
                    newGrid[x + 1, y + 1] = grid[x, y];
                }
            }

            newGrid[BorderedWidth - 2, BorderedHeight - 1] = true; // Gap for the exit

            return newGrid;
        }

        public override string ToString() { // For debugging
            StringBuilder sb = new();
            sb.Append("█ ");
            sb.AppendLine(new string('█', grid.GetLength(0)));
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                sb.Append('█');
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    sb.Append(grid[x, y] ? ' ' : '█');
                }
                sb.Append("█\n");
            }
            sb.Append(new string('█', grid.GetLength(0)));
            sb.Append(" █");
            return sb.ToString();
        }
    }
}
