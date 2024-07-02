using System;
using System.Collections.Generic;
using System.Linq;

namespace Maze_Generator
{
    internal class MazeGenerator
    {
        /// <summary>
        /// Generates a maze using Iterative randomized Prim's algorithm from Wikipedia.org
        /// </summary>
        /// <param name="width">Number of cells horizontally (min 2)</param>
        /// <param name="height">Number of cells vertically (min 2)</param>
        /// <returns></returns>
        public static Maze GenerateMazePrims(int width, int height)
        {
            if (width < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(width), "width must be at least 2");
            }
            if (height < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(height), "height must be at least 2");
            }

            Random random = new();
            var maze = new Maze(width, height);
            var topLeft = new Coord(0, 0);
            maze[topLeft] = true; // Mark first cell as visited
            List<Coord> walls = [new Coord(0, 1), new Coord(1, 0)]; // Add first cell's walls

            while (walls.Count > 0) // Repeat until all walls have been visited
            {
                int index = random.Next(walls.Count);
                var wall = walls[index];

                Coord cell1;
                Coord cell2;
                if (wall.X % 2 == 0) // Horizontal wall -
                {
                    cell1 = wall.GetAbove();
                    cell2 = wall.GetBelow();
                }
                else // Vertical wall |
                {
                    cell1 = wall.GetLeft();
                    cell2 = wall.GetRight();
                }

                if (!(maze[cell1] && maze[cell2]))
                {
                    // At least one cell is unvisited, so we only need to check one of them
                    Coord unvisitedCell = !maze[cell1] ? cell1 : cell2;
                    var newWalls = VisitCellAndGetAdjacentWalls(maze, unvisitedCell);
                    walls.AddRange(newWalls);
                    maze[wall] = true;
                }
                walls.RemoveAt(index);
            }

            return maze;
        }

        private static Coord[] VisitCellAndGetAdjacentWalls(Maze maze, Coord cell)
        {
            maze[cell] = true;
            Coord[] walls = [cell.GetLeft(), cell.GetRight(), cell.GetAbove(), cell.GetBelow()];
            return walls.Where(maze.InBounds).ToArray();
        }
    }
}
