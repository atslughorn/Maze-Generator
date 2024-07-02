using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
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
        private MazeViewModel MazeViewModel { get; set; }

        public MainWindow()
        {
            this.InitializeComponent();
            MazeViewModel = new();
        }

        private void GenerateAndDisplayMaze()
        {
            var maze = MazeGenerator.GenerateMazePrims(MazeViewModel.InputWidth, MazeViewModel.InputHeight);
            DisplayMaze(maze);
        }

        private void generateMazeButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateAndDisplayMaze();
        }

        private void myImage_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateAndDisplayMaze();
        }

        /// <summary>
        /// Use the byte array to generate a WriteableBitmap and display it
        /// </summary>
        /// <param name="maze">Maze to display</param>
        private void DisplayMaze(Maze maze)
        {
            int scale = (int)Math.Ceiling(Math.Min(imageColumn.ActualWidth / maze.BorderedWidth, grid.ActualHeight / maze.BorderedHeight));
            WriteableBitmap wbmp = new(maze.BorderedWidth * scale, maze.BorderedHeight * scale);
            byte[] pixels = ScaleMazeAndConvertToPixels(maze.GetGridWithBorder(), scale);
            pixels.CopyTo(wbmp.PixelBuffer);
            MazeViewModel.ImageSource = wbmp;
        }

        /// <summary>
        /// Scaling is neccessary to avoid images blurring when they are automatically scaled, creating shapes on a canvas is too slow
        /// </summary>
        /// <param name="arr">2D array of bools, representing black/white</param>
        /// <param name="scale"></param>
        /// <returns></returns>
        private static byte[] ScaleMazeAndConvertToPixels(bool[,] arr, int scale)
        {
            byte[] pixels = new byte[arr.GetLength(0) * arr.GetLength(1) * 4 * scale * scale];
            byte[] whitePixel = [byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue];
            byte[] blackPixel = [byte.MinValue, byte.MinValue, byte.MinValue, byte.MaxValue];

            using (var ms = new MemoryStream(pixels))
            {
                for (int y = 0; y < arr.GetLength(1); y++)
                {
                    for (int a = 0; a < scale; a++) // Repeat every row
                    {
                        for (int x = 0; x < arr.GetLength(0); x++)
                        {
                            for (int b = 0; b < scale; b++) // Repeat every pixel
                            {
                                ms.Write(arr[x, y] ? whitePixel : blackPixel);
                            }
                        }
                    }
                }
            }

            return pixels;
        }

        /// <summary>
        /// Input validation for the maze size, only accepts integers
        /// </summary>
        private void IntInput_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            if (args.NewValue % 1 != 0)
            {
                sender.Value = args.OldValue;
            }
        }
    }
}
