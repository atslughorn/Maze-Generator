using System.Text;

namespace Maze_Generator
{
    internal class Maze
    {
        private readonly bool[,] grid;
        private readonly int realWidth;
        private readonly int realHeight;

        /// <summary>
        /// RealWidth and RealHeight are the dimensions of the array. This includes walls, unlike "width" and "height".
        /// </summary>
        public int RealWidth => realWidth;
        public int RealHeight => realHeight;
        /// <summary>
        /// BorderedWidth and BorderedHeight are the dimensions of the array returned by GetGridWithBorder
        /// </summary>
        public int BorderedWidth => realWidth + 2;
        public int BorderedHeight => realHeight + 2;

        public Maze(int width, int height)
        {
            realWidth = width * 2 - 1;
            realHeight = height * 2 - 1;
            // Array for walls and cells, there is some redundancy as some elements are fixed, however this format is very easy to convert to a bitmap
            grid = new bool[RealWidth, RealHeight]; // Default value is false
        }

        /// <summary>
        /// Allowing Coord to index Maze makes code more readable.
        /// </summary>
        /// <param name="c">The Coord object representing the location in the maze. If "c" is not guaranteed to be in range, it should be checked with InBounds.</param>
        /// <returns></returns>
        public bool this[Coord c]
        {
            get { return grid[c.X, c.Y]; }
            set { grid[c.X, c.Y] = value; }
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

        public override string ToString() // For debugging
        {
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
