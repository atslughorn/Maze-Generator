using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze_Generator
{
    public class MazeViewModel : INotifyPropertyChanged
    {
        public int InputWidth { get; set; }
        public int InputHeight { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private ImageSource imageSource;
        public ImageSource ImageSource {
            get
            {
                return imageSource;
            }
            set { 
                imageSource = value;
                OnPropertyChanged(nameof(ImageSource)); // To update the bound Image object
            }
        }

        public MazeViewModel() {
            InputWidth = 100;
            InputHeight = 100;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
