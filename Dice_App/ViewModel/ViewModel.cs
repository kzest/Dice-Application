using Dice_App.Model;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace Dice_App.ViewModel
{
    public class ViewModel
    {
        public MainModel MainDataModel { get; set; }
        public RelayCmd RollDiceCmd { get; set; }
        public RelayCmd ResetCmd { get; set; }

        public ViewModel()
        {
            this.MainDataModel = new MainModel();
            this.MainDataModel.DiceModel1 = new DiceModel(this.MainDataModel, "DiceModel1", MainModel.WHITEMODELPATH);
            this.MainDataModel.DiceModel2 = new DiceModel(this.MainDataModel, "DiceModel2", MainModel.WHITEMODELPATH);
            this.MainDataModel.DiceModel3 = new DiceModel(this.MainDataModel, "DiceModel3", MainModel.WHITEMODELPATH);
            this.MainDataModel.DiceModel4 = new DiceModel(this.MainDataModel, "DiceModel4", MainModel.WHITEMODELPATH);
            this.MainDataModel.DiceModel5 = new DiceModel(this.MainDataModel, "DiceModel5", MainModel.WHITEMODELPATH);
            this.MainDataModel.DiceModel6 = new DiceModel(this.MainDataModel, "DiceModel6", MainModel.WHITEMODELPATH);

            this.RollDiceCmd = new RelayCmd(new Func<object, bool>(x => true), new Action<object>(x => rollDice(x)));

            this.ResetCmd = new RelayCmd(new Func<object, bool>(x => true), new Action<object>(x => 
            {
                if (isAnyDieRotating() == false)
                {
                    resetAllDice();
                    resetTotalSteps();
                    stopRotatingDice();
                }
            }));
        }

        private void rollDice(object parameter)
        {
            if (isAnyDieRotating() == false)
            {
                resetAllDice();

                setNewRotationTargets();

                MainDataModel.RotateTimer = new System.Threading.Timer(new System.Threading.TimerCallback(new Action<object>(x =>
                {
                    checkIfAllDiceRotating();

                    rotateAllDice();

                    setTotalStepsCount();

                })), null, 30, 30);
            }

        }

        private void resetAllDice()
        {
            this.MainDataModel.DiceModel1.XRotAngle = 0;
            this.MainDataModel.DiceModel1.YRotAngle = 0;
            this.MainDataModel.DiceModel1.ZRotAngle = 0;

            this.MainDataModel.DiceModel2.XRotAngle = 0;
            this.MainDataModel.DiceModel2.YRotAngle = 0;
            this.MainDataModel.DiceModel2.ZRotAngle = 0;

            this.MainDataModel.DiceModel3.XRotAngle = 0;
            this.MainDataModel.DiceModel3.YRotAngle = 0;
            this.MainDataModel.DiceModel3.ZRotAngle = 0;

            this.MainDataModel.DiceModel4.XRotAngle = 0;
            this.MainDataModel.DiceModel4.YRotAngle = 0;
            this.MainDataModel.DiceModel4.ZRotAngle = 0;

            this.MainDataModel.DiceModel5.XRotAngle = 0;
            this.MainDataModel.DiceModel5.YRotAngle = 0;
            this.MainDataModel.DiceModel5.ZRotAngle = 0;

            this.MainDataModel.DiceModel6.XRotAngle = 0;
            this.MainDataModel.DiceModel6.YRotAngle = 0;
            this.MainDataModel.DiceModel6.ZRotAngle = 0;

            resetStepUps();
        }

        private void setNewRotationTargets()
        {
            int[] arrI1 = getRandomIntegers(3);
            this.MainDataModel.DiceModel1.XRotTarget = arrI1[0] * 90;
            this.MainDataModel.DiceModel1.YRotTarget = arrI1[1] * 90;
            this.MainDataModel.DiceModel1.ZRotTarget = arrI1[2] * 90;

            int[] arrI2 = getRandomIntegers(3);
            this.MainDataModel.DiceModel2.XRotTarget = arrI2[0] * 90;
            this.MainDataModel.DiceModel2.YRotTarget = arrI2[1] * 90;
            this.MainDataModel.DiceModel2.ZRotTarget = arrI2[2] * 90;

            int[] arrI3 = getRandomIntegers(3);
            this.MainDataModel.DiceModel3.XRotTarget = arrI3[0] * 90;
            this.MainDataModel.DiceModel3.YRotTarget = arrI3[1] * 90;
            this.MainDataModel.DiceModel3.ZRotTarget = arrI3[2] * 90;

            int[] arrI4 = getRandomIntegers(3);
            this.MainDataModel.DiceModel4.XRotTarget = arrI4[0] * 90;
            this.MainDataModel.DiceModel4.YRotTarget = arrI4[1] * 90;
            this.MainDataModel.DiceModel4.ZRotTarget = arrI4[2] * 90;

            int[] arrI5 = getRandomIntegers(3);
            this.MainDataModel.DiceModel5.XRotTarget = arrI5[0] * 90;
            this.MainDataModel.DiceModel5.YRotTarget = arrI5[1] * 90;
            this.MainDataModel.DiceModel5.ZRotTarget = arrI5[2] * 90;

            int[] arrI6 = getRandomIntegers(3);
            this.MainDataModel.DiceModel6.XRotTarget = arrI6[0] * 90;
            this.MainDataModel.DiceModel6.YRotTarget = arrI6[1] * 90;
            this.MainDataModel.DiceModel6.ZRotTarget = arrI6[2] * 90;

            if(isAnyDieHavingStepUp() == false)
            {
                setARandomDieToStepUp();
            }
        }

        private void checkIfAllDiceRotating()
        {   
            if (this.MainDataModel.DiceModel1.XRotAngle >= this.MainDataModel.DiceModel1.XRotTarget
                        &&
                        this.MainDataModel.DiceModel1.YRotAngle >= this.MainDataModel.DiceModel1.YRotTarget
                        &&
                        this.MainDataModel.DiceModel1.ZRotAngle >= this.MainDataModel.DiceModel1.ZRotTarget
                        &&
                        this.MainDataModel.DiceModel2.XRotAngle >= this.MainDataModel.DiceModel2.XRotTarget
                        &&                                                                     
                        this.MainDataModel.DiceModel2.YRotAngle >= this.MainDataModel.DiceModel2.YRotTarget
                        &&                                                                     
                        this.MainDataModel.DiceModel2.ZRotAngle >= this.MainDataModel.DiceModel2.ZRotTarget
                        &&
                        this.MainDataModel.DiceModel3.XRotAngle >= this.MainDataModel.DiceModel3.XRotTarget
                        &&
                        this.MainDataModel.DiceModel3.YRotAngle >= this.MainDataModel.DiceModel3.YRotTarget
                        &&
                        this.MainDataModel.DiceModel3.ZRotAngle >= this.MainDataModel.DiceModel3.ZRotTarget
                        &&
                        this.MainDataModel.DiceModel4.XRotAngle >= this.MainDataModel.DiceModel4.XRotTarget
                        &&
                        this.MainDataModel.DiceModel4.YRotAngle >= this.MainDataModel.DiceModel4.YRotTarget
                        &&
                        this.MainDataModel.DiceModel4.ZRotAngle >= this.MainDataModel.DiceModel4.ZRotTarget
                        &&
                        this.MainDataModel.DiceModel5.XRotAngle >= this.MainDataModel.DiceModel5.XRotTarget
                        &&
                        this.MainDataModel.DiceModel5.YRotAngle >= this.MainDataModel.DiceModel5.YRotTarget
                        &&
                        this.MainDataModel.DiceModel5.ZRotAngle >= this.MainDataModel.DiceModel5.ZRotTarget
                        &&
                        this.MainDataModel.DiceModel6.XRotAngle >= this.MainDataModel.DiceModel6.XRotTarget
                        &&
                        this.MainDataModel.DiceModel6.YRotAngle >= this.MainDataModel.DiceModel6.YRotTarget
                        &&
                        this.MainDataModel.DiceModel6.ZRotAngle >= this.MainDataModel.DiceModel6.ZRotTarget)
            {
                MainDataModel.RotateTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                MainDataModel.RotateTimer.Dispose();
                this.MainDataModel.DiceModel1.IsDiceRotating = false;
            }
            else
            {
                this.MainDataModel.DiceModel1.IsDiceRotating = true;
            }
        }

        private void rotateAllDice()
        {
            //1
            if (this.MainDataModel.DiceModel1.XRotAngle < this.MainDataModel.DiceModel1.XRotTarget)
                this.MainDataModel.DiceModel1.XRotAngle += 10;

            if (this.MainDataModel.DiceModel1.YRotAngle < this.MainDataModel.DiceModel1.YRotTarget)
                this.MainDataModel.DiceModel1.YRotAngle += 10;

            if (this.MainDataModel.DiceModel1.ZRotAngle < this.MainDataModel.DiceModel1.ZRotTarget)
                this.MainDataModel.DiceModel1.ZRotAngle += 10;


            //2
            if (this.MainDataModel.DiceModel2.XRotAngle < this.MainDataModel.DiceModel2.XRotTarget)
                this.MainDataModel.DiceModel2.XRotAngle += 10;

            if (this.MainDataModel.DiceModel2.YRotAngle < this.MainDataModel.DiceModel2.YRotTarget)
                this.MainDataModel.DiceModel2.YRotAngle += 10;

            if (this.MainDataModel.DiceModel2.ZRotAngle < this.MainDataModel.DiceModel2.ZRotTarget)
                this.MainDataModel.DiceModel2.ZRotAngle += 10;


            //3
            if (this.MainDataModel.DiceModel3.XRotAngle < this.MainDataModel.DiceModel3.XRotTarget)
                this.MainDataModel.DiceModel3.XRotAngle += 10;

            if (this.MainDataModel.DiceModel3.YRotAngle < this.MainDataModel.DiceModel3.YRotTarget)
                this.MainDataModel.DiceModel3.YRotAngle += 10;

            if (this.MainDataModel.DiceModel3.ZRotAngle < this.MainDataModel.DiceModel3.ZRotTarget)
                this.MainDataModel.DiceModel3.ZRotAngle += 10;

            //4
            if (this.MainDataModel.DiceModel4.XRotAngle < this.MainDataModel.DiceModel4.XRotTarget)
                this.MainDataModel.DiceModel4.XRotAngle += 10;

            if (this.MainDataModel.DiceModel4.YRotAngle < this.MainDataModel.DiceModel4.YRotTarget)
                this.MainDataModel.DiceModel4.YRotAngle += 10;

            if (this.MainDataModel.DiceModel4.ZRotAngle < this.MainDataModel.DiceModel4.ZRotTarget)
                this.MainDataModel.DiceModel4.ZRotAngle += 10;


            //5
            if (this.MainDataModel.DiceModel5.XRotAngle < this.MainDataModel.DiceModel5.XRotTarget)
                this.MainDataModel.DiceModel5.XRotAngle += 10;

            if (this.MainDataModel.DiceModel5.YRotAngle < this.MainDataModel.DiceModel5.YRotTarget)
                this.MainDataModel.DiceModel5.YRotAngle += 10;

            if (this.MainDataModel.DiceModel5.ZRotAngle < this.MainDataModel.DiceModel5.ZRotTarget)
                this.MainDataModel.DiceModel5.ZRotAngle += 10;


            //6
            if (this.MainDataModel.DiceModel6.XRotAngle < this.MainDataModel.DiceModel6.XRotTarget)
                this.MainDataModel.DiceModel6.XRotAngle += 10;

            if (this.MainDataModel.DiceModel6.YRotAngle < this.MainDataModel.DiceModel6.YRotTarget)
                this.MainDataModel.DiceModel6.YRotAngle += 10;

            if (this.MainDataModel.DiceModel6.ZRotAngle < this.MainDataModel.DiceModel6.ZRotTarget)
                this.MainDataModel.DiceModel6.ZRotAngle += 10;
        }

        private void resetStepUps()
        {
            this.MainDataModel.DiceModel1.StepUpDone = false;
            this.MainDataModel.DiceModel2.StepUpDone = false;
            this.MainDataModel.DiceModel3.StepUpDone = false;
            this.MainDataModel.DiceModel4.StepUpDone = false;
            this.MainDataModel.DiceModel5.StepUpDone = false;
            this.MainDataModel.DiceModel6.StepUpDone = false;
        }

        private void resetTotalSteps()
        {
            this.MainDataModel.DiceModel1.TotalSteps = 0;
            this.MainDataModel.DiceModel2.TotalSteps = 0;
            this.MainDataModel.DiceModel3.TotalSteps = 0;
            this.MainDataModel.DiceModel4.TotalSteps = 0;
            this.MainDataModel.DiceModel5.TotalSteps = 0;
            this.MainDataModel.DiceModel6.TotalSteps = 0;
        }

        private void stopRotatingDice()
        {
            //1
            this.MainDataModel.DiceModel1.XRotTarget = 0;
            this.MainDataModel.DiceModel1.YRotTarget = 0;
            this.MainDataModel.DiceModel1.ZRotTarget = 0;
            this.MainDataModel.DiceModel1.IsDiceRotating = false;

            //2
            this.MainDataModel.DiceModel2.XRotTarget = 0;
            this.MainDataModel.DiceModel2.YRotTarget = 0;
            this.MainDataModel.DiceModel2.ZRotTarget = 0;
            this.MainDataModel.DiceModel2.IsDiceRotating = false;

            //3
            this.MainDataModel.DiceModel3.XRotTarget = 0;
            this.MainDataModel.DiceModel3.YRotTarget = 0;
            this.MainDataModel.DiceModel3.ZRotTarget = 0;
            this.MainDataModel.DiceModel3.IsDiceRotating = false;

            //4
            this.MainDataModel.DiceModel4.XRotTarget = 0;
            this.MainDataModel.DiceModel4.YRotTarget = 0;
            this.MainDataModel.DiceModel4.ZRotTarget = 0;
            this.MainDataModel.DiceModel4.IsDiceRotating = false;

            //5
            this.MainDataModel.DiceModel5.XRotTarget = 0;
            this.MainDataModel.DiceModel5.YRotTarget = 0;
            this.MainDataModel.DiceModel5.ZRotTarget = 0;
            this.MainDataModel.DiceModel5.IsDiceRotating = false;

            //6
            this.MainDataModel.DiceModel6.XRotTarget = 0;
            this.MainDataModel.DiceModel6.YRotTarget = 0;
            this.MainDataModel.DiceModel6.ZRotTarget = 0;
            this.MainDataModel.DiceModel6.IsDiceRotating = false;
        }

        private void setTotalStepsCount()
        {
            //1
            if (this.MainDataModel.DiceModel1.IsDiceRotating == false && this.MainDataModel.DiceModel1.StepUpDone == false)
            {
                if (this.MainDataModel.DiceModel1.IsStepUpImminent() == true)
                {
                    this.MainDataModel.DiceModel1.TotalSteps++;
                    this.MainDataModel.DiceModel1.StepUpDone = true;
                }
            }


            //2
            if (this.MainDataModel.DiceModel2.IsDiceRotating == false && this.MainDataModel.DiceModel2.StepUpDone == false)
            {
                if (this.MainDataModel.DiceModel2.IsStepUpImminent() == true)
                {
                    this.MainDataModel.DiceModel2.TotalSteps++;
                    this.MainDataModel.DiceModel2.StepUpDone = true;
                }
            }


            //3
            if (this.MainDataModel.DiceModel3.IsDiceRotating == false && this.MainDataModel.DiceModel3.StepUpDone == false)
            {
                if (this.MainDataModel.DiceModel3.IsStepUpImminent() == true)
                {
                    this.MainDataModel.DiceModel3.TotalSteps++;
                    this.MainDataModel.DiceModel3.StepUpDone = true;
                }
            }


            //4
            if (this.MainDataModel.DiceModel4.IsDiceRotating == false && this.MainDataModel.DiceModel4.StepUpDone == false)
            {
                if (this.MainDataModel.DiceModel4.IsStepUpImminent() == true)
                {
                    this.MainDataModel.DiceModel4.TotalSteps++;
                    this.MainDataModel.DiceModel4.StepUpDone = true;
                }
            }


            //5
            if (this.MainDataModel.DiceModel5.IsDiceRotating == false && this.MainDataModel.DiceModel5.StepUpDone == false)
            {
                if (this.MainDataModel.DiceModel5.IsStepUpImminent() == true)
                {
                    this.MainDataModel.DiceModel5.TotalSteps++;
                    this.MainDataModel.DiceModel5.StepUpDone = true;
                }
            }


            //6
            if (this.MainDataModel.DiceModel6.IsDiceRotating == false && this.MainDataModel.DiceModel6.StepUpDone == false)
            {
                if (this.MainDataModel.DiceModel6.IsStepUpImminent() == true)
                {
                    this.MainDataModel.DiceModel6.TotalSteps++;
                    this.MainDataModel.DiceModel6.StepUpDone = true;
                }
            }
        }

        private int[] getRandomIntegers (int arrayLen)
        {
            int[] arrInt = new int[arrayLen];
            byte[] arrByt = new byte[arrayLen];

            RNGCryptoServiceProvider rNGCrypto = new RNGCryptoServiceProvider();
            rNGCrypto.GetNonZeroBytes(arrByt);

            arrInt = arrByt.Select<byte, int>(b => ((int)b)/((int)byte.MaxValue / 10)).ToArray<int>();

            return arrInt;
        }

        private bool isAnyDieHavingStepUp()
        {
            bool result = false;

            if(this.MainDataModel.DiceModel1.IsStepUpImminent() == true
                ||
                this.MainDataModel.DiceModel2.IsStepUpImminent() == true
                ||                            
                this.MainDataModel.DiceModel3.IsStepUpImminent() == true
                ||                            
                this.MainDataModel.DiceModel4.IsStepUpImminent() == true
                ||                            
                this.MainDataModel.DiceModel5.IsStepUpImminent() == true
                ||                            
                this.MainDataModel.DiceModel6.IsStepUpImminent() == true)
            { result = true; }

            return result;
        }

        private bool isAnyDieRotating()
        {
            if (this.MainDataModel.DiceModel1.IsDiceRotating == true
                ||
                this.MainDataModel.DiceModel2.IsDiceRotating == true
                ||
                this.MainDataModel.DiceModel3.IsDiceRotating == true
                ||
                this.MainDataModel.DiceModel4.IsDiceRotating == true
                ||
                this.MainDataModel.DiceModel5.IsDiceRotating == true
                ||
                this.MainDataModel.DiceModel6.IsDiceRotating == true)
            { return true; }
            else
            { return false; }
        }
        
        private void setARandomDieToStepUp()
        {
            int iRandom = getRandomIntegers(1)[0];
            while (iRandom <= 0 || iRandom > 6)
                iRandom = getRandomIntegers(1)[0];
            
            switch(iRandom)
            {
                case 1:
                    setupToStepUp(this.MainDataModel.DiceModel1);
                    break;

                case 2:
                    setupToStepUp(this.MainDataModel.DiceModel2);
                    break;

                case 3:
                    setupToStepUp(this.MainDataModel.DiceModel3);
                    break;

                case 4:
                    setupToStepUp(this.MainDataModel.DiceModel4);
                    break;

                case 5:
                    setupToStepUp(this.MainDataModel.DiceModel5);
                    break;

                case 6:
                    setupToStepUp(this.MainDataModel.DiceModel6);
                    break;
            }
        }

        private void setupToStepUp(DiceModel diceModel)
        {

            bool isSetupDone = false;

            //0 or 360
            if ((diceModel.YRotTargetReduced == 0 || diceModel.YRotTargetReduced == 360) && isSetupDone == false)
            {
                diceModel.XRotTarget += Math.Abs(360 - diceModel.XRotTargetReduced);
                isSetupDone = true;
            }

            if ((diceModel.XRotTargetReduced == 0 || diceModel.XRotTargetReduced == 360) && isSetupDone == false)
            {
                diceModel.YRotTarget += Math.Abs(360 - diceModel.YRotTargetReduced);
                isSetupDone = true;
            }

            //90
            if (diceModel.YRotTargetReduced == 90 && isSetupDone == false)
            {
                diceModel.YRotTarget += 90;

                if (diceModel.XRotTargetReduced == 270)
                    diceModel.XRotTarget -= 90;
                else if(diceModel.XRotTargetReduced == 360)
                    diceModel.XRotTarget -= 180;
                else if (diceModel.XRotTargetReduced == 90)
                    diceModel.XRotTarget += 90;

                isSetupDone = true;
            }

            if (diceModel.XRotTargetReduced == 90 && isSetupDone == false)
            {
                diceModel.XRotTarget += 90;

                if (diceModel.YRotTargetReduced == 270)
                    diceModel.YRotTarget -= 90;
                else if (diceModel.XRotTargetReduced == 360)
                    diceModel.YRotTarget -= 180;
                else if (diceModel.XRotTargetReduced == 90)
                    diceModel.YRotTarget += 90;

                isSetupDone = true;
            }

            //180
            if (diceModel.YRotTargetReduced == 180 && isSetupDone == false)
            {
                if (diceModel.XRotTargetReduced == 270)
                    diceModel.XRotTarget -= 90;
                else if (diceModel.XRotTargetReduced == 360)
                    diceModel.XRotTarget -= 180;
                else if (diceModel.XRotTargetReduced == 90)
                    diceModel.XRotTarget += 90;

                isSetupDone = true;
            }

            if (diceModel.XRotTargetReduced == 180 && isSetupDone == false)
            {
                if (diceModel.YRotTargetReduced == 270)
                    diceModel.YRotTarget -= 90;
                else if (diceModel.XRotTargetReduced == 360)
                    diceModel.YRotTarget -= 180;
                else if (diceModel.XRotTargetReduced == 90)
                    diceModel.YRotTarget += 90;

                isSetupDone = true;
            }


            //270
            if (diceModel.YRotTargetReduced == 270 && isSetupDone == false)
            {
                diceModel.YRotTarget += 90;

                diceModel.XRotTarget += Math.Abs(360 - diceModel.XRotTargetReduced);

                isSetupDone = true;
            }

            if (diceModel.XRotTargetReduced == 180 && isSetupDone == false)
            {
                diceModel.XRotTarget += 90;

                diceModel.YRotTarget += Math.Abs(360 - diceModel.YRotTargetReduced);

                isSetupDone = true;
            }
        }
    }
}
