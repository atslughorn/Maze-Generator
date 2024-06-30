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
        private ImageSource imageSource;

        public event PropertyChangedEventHandler PropertyChanged;

        public ImageSource ImageSource {
            get
            {
                return imageSource;
            }
            set { 
                imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
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
