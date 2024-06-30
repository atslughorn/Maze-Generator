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
            grid = new bool[RealWidth, RealHeight];
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
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetGridWithBorderAsBytes()
        {
            byte[] whitePixel = [byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue];
            byte[] blackPixel = [byte.MinValue, byte.MinValue, byte.MinValue, byte.MaxValue];

            byte[] bytes = new byte[(BorderedWidth) * (BorderedHeight) * 4];
            using (MemoryStream ms = new(bytes))
            {
                // Top border
                ms.Write(blackPixel);
                ms.Write(whitePixel);
                for (int i = 0; i < RealWidth; i++)
                {
                    ms.Write(blackPixel);
                }

                // Body and side borders
                for (int l = 0; l < RealHeight; l++)
                {
                    ms.Write(blackPixel); // Left border
                    for (int k = 0; k < RealWidth; k++)
                    {
                        ms.Write(grid[k, l] ? whitePixel : blackPixel);
                    }
                    ms.Write(blackPixel); // Right border
                }

                //Bottom border
                for (int i = 0; i < RealWidth; i++)
                {
                    ms.Write(blackPixel);
                }
                ms.Write(whitePixel);
                ms.Write(blackPixel);
            }

            return bytes;
        }

        public override string ToString() {
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
