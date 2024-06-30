using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Maze_Generator
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void GenerateAndDisplayMaze()
        {
            var maze = GenerateMazePrims((int)widthInput.Value, (int)heightInput.Value);
            DisplayMaze(maze, mazeImage);
            Debug.WriteLine(maze);
        }

        private void generateMazeButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateAndDisplayMaze();
        }

        private void myImage_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateAndDisplayMaze();
        }

        private static Maze GenerateMazePrims(int width, int height)
        {
            Random random = new();
            // Outer walls are not included, array for walls and cells, there is some redundancy as some elements will never be changed, however this format makes it easy to create a bitmap
            var maze = new Maze(width, height); // Default value is false
            var topLeft = new Coord(0, 0);
            maze[topLeft] = true; // Mark first cell as visited
            List<Coord> walls = [new Coord(0, 1), new Coord(1, 0)]; // Add first cell's walls

            while (walls.Count > 0)
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
            return walls.Where(wall => maze.InBounds(wall)).ToArray();
        }

        private static void DisplayMaze(Maze maze, Image image)
        {
            WriteableBitmap wbmp = new(maze.RealWidth + 2, maze.RealHeight + 2);
            //byte[] byteArray = Array.ConvertAll<bool, byte>(maze.grid., b => b ? byte.MinValue : byte.MaxValue);

            maze.GetGridWithBorderAsBytes().CopyTo(wbmp.PixelBuffer);

            //byte[] whitePixel = [byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue];
            //byte[] blackPixel = [byte.MinValue, byte.MinValue, byte.MinValue, byte.MaxValue];

            //using (var bufferStream = wbmp.PixelBuffer.AsStream())
            //{
            //    foreach (var pixel in maze.grid)
            //    {
            //        bufferStream.Write(pixel ? whitePixel : blackPixel, 0, 4);
            //    }
            //}

            image.Source = wbmp;
            image.Width = maze.RealWidth + 2;
            image.Height = maze.RealHeight + 2;
        }
    }
}
