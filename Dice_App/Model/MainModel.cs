using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Dice_App.Model
{
    public class MainModel : INotifyPropertyChanged
    {
        public const string BLUEMODELPATH = "/Assets/Dice_Blue.obj";
        public const string WHITEMODELPATH = "/Assets/Dice_White.obj";

        public Timer RotateTimer { get; set; }

        public DiceModel DiceModel1 { get; set; }
        public DiceModel DiceModel2 { get; set; }
        public DiceModel DiceModel3 { get; set; }
        public DiceModel DiceModel4 { get; set; }
        public DiceModel DiceModel5 { get; set; }
        public DiceModel DiceModel6 { get; set; }

        public MainModel()
        { }

        
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
