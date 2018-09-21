using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Dice_App.Model
{
    public class DiceModel : INotifyPropertyChanged
    {
        private Model3DGroup _model3DData = null;
        public Model3DGroup Model3DData
        {
            get
            {
                return _model3DData;
            }

            set
            {
                if (_model3DData != value)
                {
                    _model3DData = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private double _xRotAngle = 0;
        public double XRotAngle
        {
            get { return _xRotAngle; }
            set { if (_xRotAngle != value) { _xRotAngle = value; NotifyPropertyChanged(); } }
        }

        private double _yRotAngle = 0;
        public double YRotAngle
        {
            get { return _yRotAngle; }
            set { if (_yRotAngle != value) { _yRotAngle = value; NotifyPropertyChanged(); } }
        }

        private double _zRotAngle = 0;
        public double ZRotAngle
        {
            get { return _zRotAngle; }
            set { if (_zRotAngle != value) { _zRotAngle = value; NotifyPropertyChanged(); } }
        }

        public double XRotTarget { get; set; }
        public double YRotTarget { get; set; }
        public double ZRotTarget { get; set; }

        public double XRotTargetReduced
        {
            get
            {
                while (this.XRotTarget > 360)
                    this.XRotTarget -= 360;

                return this.XRotTarget;
            }
        }
        public double YRotTargetReduced
        {
            get
            {
                while (this.YRotTarget > 360)
                    this.YRotTarget -= 360;

                return this.YRotTarget;
            }
        }
        public double ZRotTargetReduced
        {
            get
            {
                while (this.ZRotTarget > 360)
                    this.ZRotTarget -= 360;

                return this.ZRotTarget;
            }
        }


        private bool _isDiceRotating = false;
        public bool IsDiceRotating
        {
            get
            {
                if (this.XRotAngle >= this.XRotTarget
                        &&
                        this.YRotAngle >= this.YRotTarget
                        &&
                        this.ZRotAngle >= this.ZRotTarget)
                    _isDiceRotating = false;
                else
                    _isDiceRotating = true;

                return _isDiceRotating;
            }

            set
            {
                _isDiceRotating = value;
            }
        }

        private int _totalSteps;
        public int TotalSteps
        {
            get { return _totalSteps; }
            set { if (_totalSteps != value) { _totalSteps = value; NotifyPropertyChanged(); } }
        }

        public bool StepUpDone { get; set; } = false;


        public MainModel OwnerMainModel { get; set; }
        public string OwnerPropertyName { get; set; }

        public DiceModel(MainModel ownerMainModel, string ownerPropertyName, string modelPath)
        {
            this.OwnerMainModel = ownerMainModel;
            this.OwnerPropertyName = ownerPropertyName;
            this.Model3DData = loadModel(AppDomain.CurrentDomain.BaseDirectory + modelPath);
        }

        private Model3DGroup loadModel(string modelPath)
        {
            Model3DGroup model3DGroup = null;

            FileInfo finFo = new FileInfo(modelPath);
            if (finFo.Exists)
            {
                ObjReader objReader = new ObjReader();
                model3DGroup = objReader.Read(modelPath);
            }

            return model3DGroup;
        }

        public bool IsStepUpImminent()
        {
            if (((this.XRotTargetReduced / 180) % 2 == 1 && (YRotTargetReduced / 180) % 2 == 1)
                    ||
                    ((XRotTargetReduced / 180) % 2 == 0 && XRotTargetReduced != 0
                        &&
                    (YRotTargetReduced / 180) % 2 == 0 && YRotTargetReduced != 0)
                    ||
                    (XRotTargetReduced % 360 == 0
                        &&
                    YRotTargetReduced == 0)
                    ||
                    (XRotTargetReduced == 0
                        &&
                    YRotTargetReduced % 360 == 0)
                    ||
                    (XRotTargetReduced == 0
                        &&
                    YRotTargetReduced == 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (this.PropertyChanged != null)
                this.OwnerMainModel.NotifyPropertyChanged(this.OwnerPropertyName);
        }

        #endregion
    }
}
