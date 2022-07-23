using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using MLib;
using SwapDataUCtr;

namespace SwapData.ViewModel
{
    public class MainViewVM : ViewBase
    {
        
        public static MainView mMainView;
        public string iniPath = MFile.path4 + "Parameter.ini";
        private ObservableCollection<SwapD> mSwapDataList = new ObservableCollection<SwapD>();
        public ObservableCollection<SwapD> SwapDataList
        {
            get
            {
                return mSwapDataList;
            }
            set
            {
                mSwapDataList = value;
                OnPropertyChanged("SwapDataList");
            }
        }
        private int mSum = 1;
        public int Sum
        {
            get { return mSum; }
            set
            {
                mSum = value;
                Ini.WriteIniData("System", "Sum", Sum.ToString(), iniPath);
            }
        }
        private string mLoginContent = "登录";
        public string LoginContent
        {
            get { return mLoginContent; }
            set {
                mLoginContent = value;
                OnPropertyChanged("LoginContent");
            }
        }
        private bool mainPageShow;
        public bool MainPageShow
        {
            get { return mainPageShow; }
            set
            {
                mainPageShow = value;
                if (mainPageShow)
                    LoginContent = "注销";
                else
                    LoginContent = "登录";
                OnPropertyChanged("MainPageShow");
            }
        }

        public MainViewVM()
        {
            Init();
            Run();
        }
        public DelegateCommand btPageClick
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (!MainPageShow)
                    {
                        Storyboard storyBoard = new Storyboard();
                        ColorAnimation colorAnimation = new ColorAnimation();
                        colorAnimation.From = Brushes.LightGray.Color;
                        colorAnimation.To = Brushes.Red.Color;
                        colorAnimation.AutoReverse = true;
                        colorAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(200));
                        colorAnimation.RepeatBehavior = new RepeatBehavior(TimeSpan.FromMilliseconds(2000));
                        colorAnimation.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("BorderBrush.Color"));

                        storyBoard.Children.Add(colorAnimation);
                        storyBoard.Begin(mMainView.btLogin, true);
                    }
                }
              );
            }
        }


    public void Init()
        {
            if (HslCommunication.Authorization.SetAuthorizationCode())
                Trace.WriteLine("注册成功");
            else
                Trace.WriteLine("注册失败");
            IniLoad();
            for (int i = 1; i <= Sum; i++)
            {
                SwapDataList.Add(new SwapD(i));
            }
        }
        public void IniLoad()
        {
            try
            {
                Sum = int.Parse(Ini.ReadIniData("System", "Sum", "1", iniPath));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            Ini.IniFileCreate(iniPath);
        }
        public async void Run()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        for (int i = 0; i < SwapDataList.Count; i++)
                        {
                            if (SwapDataList[i].swapDVM._Parm.Enable)
                            {
                                bool same = false;
                                bool same1 = false;
                                bool continuefor = false;
                                bool continuefor1 = false;
                                for (int j = 0; j < SwapDataList.Count; j++)
                                {
                                    if (i != j && SwapDataList[j].swapDVM._Parm.Enable)
                                    {
                                        if (SwapDataList[i].swapDVM._Parm.CurParmCMU1.DeviceName == SwapDataList[i].swapDVM.devicename[5] || SwapDataList[i].swapDVM._Parm.CurParmCMU1.DeviceName == SwapDataList[i].swapDVM.devicename[6])
                                        {
                                            if ((SwapDataList[i].swapDVM._Parm.CurParmCMU1.IP == SwapDataList[j].swapDVM._Parm.CurParmCMU1.IP && SwapDataList[i].swapDVM._Parm.CurParmCMU1.Port == SwapDataList[j].swapDVM._Parm.CurParmCMU1.Port) || (SwapDataList[i].swapDVM._Parm.CurParmCMU1.IP == SwapDataList[j].swapDVM._Parm.CurParmCMU2.IP && SwapDataList[i].swapDVM._Parm.CurParmCMU1.Port == SwapDataList[j].swapDVM._Parm.CurParmCMU2.Port))
                                            {
                                                Application.Current.Dispatcher.Invoke(new Action(() => {
                                                    if (Application.Current.MainWindow.IsLoaded)
                                                    {
                                                        MessageBoxHelper.PrepToCenterMessageBoxOnForm(Application.Current.MainWindow);
                                                        MessageBox.Show("《" + SwapDataList[i].swapDVM.ID + "》的IP和端口与《" + SwapDataList[j].swapDVM.ID + "》中参数重复！");
                                                    }
                                                }));
                                                SwapDataList[i].swapDVM._Parm.Enable = false;
                                                continuefor = true;
                                                break;
                                            }
                                        }
                                        if (SwapDataList[i].swapDVM._Parm.CurParmCMU2.DeviceName == SwapDataList[i].swapDVM.devicename[5] || SwapDataList[i].swapDVM._Parm.CurParmCMU2.DeviceName == SwapDataList[i].swapDVM.devicename[6])
                                        {
                                            if ((SwapDataList[i].swapDVM._Parm.CurParmCMU2.IP == SwapDataList[j].swapDVM._Parm.CurParmCMU1.IP && SwapDataList[i].swapDVM._Parm.CurParmCMU2.Port == SwapDataList[j].swapDVM._Parm.CurParmCMU1.Port) || (SwapDataList[i].swapDVM._Parm.CurParmCMU2.IP == SwapDataList[j].swapDVM._Parm.CurParmCMU2.IP && SwapDataList[i].swapDVM._Parm.CurParmCMU2.Port == SwapDataList[j].swapDVM._Parm.CurParmCMU2.Port))
                                            {
                                                
                                                Application.Current.Dispatcher.Invoke( new Action(() => {
                                                    if(Application.Current.MainWindow.IsLoaded)
                                                    {
                                                        MessageBoxHelper.PrepToCenterMessageBoxOnForm(Application.Current.MainWindow);
                                                        MessageBox.Show("《" + SwapDataList[i].swapDVM.ID + "》的IP和端口与《" + SwapDataList[j].swapDVM.ID + "》中参数重复！");
                                                    }
                                                }));
                                                
                                                SwapDataList[i].swapDVM._Parm.Enable = false;
                                                continuefor = true;
                                                break;
                                            }
                                        }
                                        if (SwapDataList[i].swapDVM._Parm.CurParmCMU1.DeviceName == SwapDataList[j].swapDVM._Parm.CurParmCMU1.DeviceName)
                                        {
                                            if (SwapDataList[i].swapDVM._Parm.CurParmCMU1.DeviceName == SwapDataList[i].swapDVM.devicename[0] || SwapDataList[i].swapDVM._Parm.CurParmCMU1.DeviceName == SwapDataList[i].swapDVM.devicename[1] || SwapDataList[i].swapDVM._Parm.CurParmCMU1.DeviceName == SwapDataList[i].swapDVM.devicename[2])
                                            {
                                                if (SwapDataList[i].swapDVM._Parm.CurParmCMU1.IP == SwapDataList[j].swapDVM._Parm.CurParmCMU1.IP && SwapDataList[i].swapDVM._Parm.CurParmCMU1.Port == SwapDataList[j].swapDVM._Parm.CurParmCMU1.Port)
                                                {
                                                    if (SwapDataList[i].swapDVM._Parm.CurParmCMU1.DeviceName == SwapDataList[i].swapDVM.devicename[0])
                                                    {
                                                        
                                                        if (SwapDataList[i].swapDVM.Device1.Fx5UExtIn == -1 && SwapDataList[j].swapDVM.Device1.Fx5UExtIn == 0)
                                                            continuefor=true;
                                                        if (SwapDataList[j].swapDVM.Device1.Fx5UExtIn == 10)
                                                        {
                                                            same = true;
                                                            if (SwapDataList[i].swapDVM.Device1.Fx5UExtIn == -1)
                                                            {
                                                                SwapDataList[i].swapDVM.Device1.Fx5U = SwapDataList[j].swapDVM.Device1.Fx5U;
                                                                SwapDataList[i].swapDVM.Device1.Fx5UExtIn = 1;
                                                            }
                                                            if (SwapDataList[i].swapDVM.Device1.Fx5UExtIn == 1)
                                                                SwapDataList[i].swapDVM.Fx5U1Connect = SwapDataList[j].swapDVM.Fx5U1Connect;
                                                        }
                                                    }
                                                    else if (SwapDataList[i].swapDVM._Parm.CurParmCMU1.StationID == SwapDataList[i].swapDVM._Parm.CurParmCMU1.StationID)
                                                    {
                                                        
                                                        if (SwapDataList[i].swapDVM._Parm.CurParmCMU1.DeviceName == SwapDataList[i].swapDVM.devicename[1])
                                                        {
                                                            if (SwapDataList[i].swapDVM.Device1.H3UExtIn == -1 && SwapDataList[j].swapDVM.Device1.H3UExtIn == 0)
                                                                continuefor = true;
                                                            if (SwapDataList[j].swapDVM.Device1.H3UExtIn == 10)
                                                            {
                                                                same = true;
                                                                if (SwapDataList[i].swapDVM.Device1.H3UExtIn == -1)
                                                                {
                                                                    SwapDataList[i].swapDVM.Device1.H3U = SwapDataList[j].swapDVM.Device1.H3U;
                                                                    SwapDataList[i].swapDVM.Device1.H3UExtIn = 1;
                                                                }
                                                                if (SwapDataList[i].swapDVM.Device1.H3UExtIn == 1)
                                                                    SwapDataList[i].swapDVM.H3U1Connect = SwapDataList[j].swapDVM.H3U1Connect;
                                                            }
                                                        }
                                                        else if (SwapDataList[i].swapDVM._Parm.CurParmCMU1.DeviceName == SwapDataList[i].swapDVM.devicename[2])
                                                        {
                                                            if (SwapDataList[i].swapDVM.Device1.H5UExtIn == -1 && SwapDataList[j].swapDVM.Device1.H5UExtIn == 0)
                                                                continuefor = true;
                                                            if (SwapDataList[j].swapDVM.Device1.H5UExtIn == 10)
                                                            {
                                                                same = true;
                                                                if (SwapDataList[i].swapDVM.Device1.H5UExtIn == -1)
                                                                {
                                                                    SwapDataList[i].swapDVM.Device1.H5U = SwapDataList[j].swapDVM.Device1.H5U;
                                                                    SwapDataList[i].swapDVM.Device1.H5UExtIn = 1;
                                                                }
                                                                if (SwapDataList[i].swapDVM.Device1.H5UExtIn == 1)
                                                                    SwapDataList[i].swapDVM.H5U1Connect = SwapDataList[j].swapDVM.H5U1Connect;
                                                            }
                                                        }
                                                    }

                                                }
                                            }
                                            else if (SwapDataList[i].swapDVM._Parm.CurParmCMU1.DeviceName == SwapDataList[i].swapDVM.devicename[3] || SwapDataList[i].swapDVM._Parm.CurParmCMU1.DeviceName == SwapDataList[i].swapDVM.devicename[4])
                                            {
                                                if (SwapDataList[i].swapDVM._Parm.CurParmCMU1.CurSerial == SwapDataList[j].swapDVM._Parm.CurParmCMU1.CurSerial && SwapDataList[i].swapDVM._Parm.CurParmCMU1.CurBaudRate == SwapDataList[j].swapDVM._Parm.CurParmCMU1.CurBaudRate && SwapDataList[i].swapDVM._Parm.CurParmCMU1.CurDataBits == SwapDataList[j].swapDVM._Parm.CurParmCMU1.CurDataBits && SwapDataList[i].swapDVM._Parm.CurParmCMU1.CurParity == SwapDataList[j].swapDVM._Parm.CurParmCMU1.CurParity && SwapDataList[i].swapDVM._Parm.CurParmCMU1.CurStopBits == SwapDataList[j].swapDVM._Parm.CurParmCMU1.CurStopBits && SwapDataList[i].swapDVM._Parm.CurParmCMU1.StationID == SwapDataList[j].swapDVM._Parm.CurParmCMU1.StationID)
                                                {
                                                    
                                                    if (SwapDataList[i].swapDVM._Parm.CurParmCMU1.DeviceName == SwapDataList[i].swapDVM.devicename[3])
                                                    {
                                                        if (SwapDataList[i].swapDVM.Device1.XCExtIn == -1 && SwapDataList[j].swapDVM.Device1.XCExtIn == 0)
                                                            continuefor = true;
                                                        if (SwapDataList[j].swapDVM.Device1.XCExtIn == 10)
                                                        {
                                                            same = true;
                                                            if (SwapDataList[i].swapDVM.Device1.XCExtIn == -1)
                                                            {
                                                                SwapDataList[i].swapDVM.Device1.XC = SwapDataList[j].swapDVM.Device1.XC;
                                                                SwapDataList[i].swapDVM.Device1.XCExtIn = 1;
                                                            }
                                                            if (SwapDataList[i].swapDVM.Device1.XCExtIn == 1)
                                                                SwapDataList[i].swapDVM.XC1Connect = SwapDataList[j].swapDVM.XC1Connect;
                                                        }
                                                    }
                                                    else if (SwapDataList[i].swapDVM._Parm.CurParmCMU1.DeviceName == SwapDataList[i].swapDVM.devicename[4])
                                                    {
                                                        if (SwapDataList[i].swapDVM.Device1.XDExtIn == -1 && SwapDataList[j].swapDVM.Device1.XDExtIn == 0)
                                                            continuefor = true;
                                                        if (SwapDataList[j].swapDVM.Device1.XDExtIn == 10)
                                                        {
                                                            same = true;
                                                            if (SwapDataList[i].swapDVM.Device1.XDExtIn == -1)
                                                            {
                                                                SwapDataList[i].swapDVM.Device1.XD = SwapDataList[j].swapDVM.Device1.XD;
                                                                SwapDataList[i].swapDVM.Device1.XDExtIn = 1;
                                                            }
                                                            if (SwapDataList[i].swapDVM.Device1.XDExtIn == 1)
                                                                SwapDataList[i].swapDVM.XD1Connect = SwapDataList[j].swapDVM.XD1Connect;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else if (SwapDataList[i].swapDVM._Parm.CurParmCMU1.DeviceName == SwapDataList[j].swapDVM._Parm.CurParmCMU2.DeviceName)
                                        {
                                            if (SwapDataList[i].swapDVM._Parm.CurParmCMU1.DeviceName == SwapDataList[i].swapDVM.devicename[0] || SwapDataList[i].swapDVM._Parm.CurParmCMU1.DeviceName == SwapDataList[i].swapDVM.devicename[1] || SwapDataList[i].swapDVM._Parm.CurParmCMU1.DeviceName == SwapDataList[i].swapDVM.devicename[2])
                                            {
                                                if (SwapDataList[i].swapDVM._Parm.CurParmCMU1.IP == SwapDataList[j].swapDVM._Parm.CurParmCMU2.IP && SwapDataList[i].swapDVM._Parm.CurParmCMU1.Port == SwapDataList[j].swapDVM._Parm.CurParmCMU2.Port)
                                                {
                                                    if (SwapDataList[i].swapDVM._Parm.CurParmCMU1.DeviceName == SwapDataList[i].swapDVM.devicename[0])
                                                    {
                                                        
                                                        if (SwapDataList[i].swapDVM.Device1.Fx5UExtIn == -1 && SwapDataList[j].swapDVM.Device2.Fx5UExtIn == 0)
                                                            continuefor = true;
                                                        if (SwapDataList[j].swapDVM.Device2.Fx5UExtIn == 10)
                                                        {
                                                            same = true;
                                                            if (SwapDataList[i].swapDVM.Device1.Fx5UExtIn == -1)
                                                            {
                                                                SwapDataList[i].swapDVM.Device1.Fx5U = SwapDataList[j].swapDVM.Device2.Fx5U;
                                                                SwapDataList[i].swapDVM.Device1.Fx5UExtIn = 1;
                                                            }
                                                            if (SwapDataList[i].swapDVM.Device1.Fx5UExtIn == 1)
                                                                SwapDataList[i].swapDVM.Fx5U1Connect = SwapDataList[j].swapDVM.Fx5U2Connect;
                                                        }
                                                    }
                                                    else if (SwapDataList[i].swapDVM._Parm.CurParmCMU1.StationID == SwapDataList[i].swapDVM._Parm.CurParmCMU1.StationID)
                                                    {
                                                        
                                                        if (SwapDataList[i].swapDVM._Parm.CurParmCMU1.DeviceName == SwapDataList[i].swapDVM.devicename[1])
                                                        {
                                                            if (SwapDataList[i].swapDVM.Device1.H3UExtIn == -1 && SwapDataList[j].swapDVM.Device2.H3UExtIn == 0)
                                                                continuefor = true;
                                                            if (SwapDataList[j].swapDVM.Device2.H3UExtIn == 10)
                                                            {
                                                                same = true;
                                                                if (SwapDataList[i].swapDVM.Device1.H3UExtIn == -1)
                                                                {
                                                                    SwapDataList[i].swapDVM.Device1.H3U = SwapDataList[j].swapDVM.Device2.H3U;
                                                                    SwapDataList[i].swapDVM.Device1.H3UExtIn = 1;
                                                                }
                                                                if (SwapDataList[i].swapDVM.Device1.H3UExtIn == 1)
                                                                    SwapDataList[i].swapDVM.H3U1Connect = SwapDataList[j].swapDVM.H3U2Connect;
                                                            }
                                                        }
                                                        else if (SwapDataList[i].swapDVM._Parm.CurParmCMU1.DeviceName == SwapDataList[i].swapDVM.devicename[2])
                                                        {
                                                            if (SwapDataList[i].swapDVM.Device1.H5UExtIn == -1 && SwapDataList[j].swapDVM.Device2.H5UExtIn == 0)
                                                                continuefor = true;
                                                            if (SwapDataList[j].swapDVM.Device2.H5UExtIn == 10)
                                                            {
                                                                same = true;
                                                                if (SwapDataList[i].swapDVM.Device1.H5UExtIn == -1)
                                                                {
                                                                    SwapDataList[i].swapDVM.Device1.H5U = SwapDataList[j].swapDVM.Device2.H5U;
                                                                    SwapDataList[i].swapDVM.Device1.H5UExtIn = 1;
                                                                }
                                                                if (SwapDataList[i].swapDVM.Device1.H5UExtIn == 1)
                                                                    SwapDataList[i].swapDVM.H5U1Connect = SwapDataList[j].swapDVM.H5U2Connect;
                                                            }
                                                        }
                                                    }

                                                }
                                            }
                                            else if (SwapDataList[i].swapDVM._Parm.CurParmCMU1.DeviceName == SwapDataList[i].swapDVM.devicename[3] || SwapDataList[i].swapDVM._Parm.CurParmCMU1.DeviceName == SwapDataList[i].swapDVM.devicename[4])
                                            {
                                                if (SwapDataList[i].swapDVM._Parm.CurParmCMU1.CurSerial == SwapDataList[j].swapDVM._Parm.CurParmCMU2.CurSerial && SwapDataList[i].swapDVM._Parm.CurParmCMU1.CurBaudRate == SwapDataList[j].swapDVM._Parm.CurParmCMU2.CurBaudRate && SwapDataList[i].swapDVM._Parm.CurParmCMU1.CurDataBits == SwapDataList[j].swapDVM._Parm.CurParmCMU2.CurDataBits && SwapDataList[i].swapDVM._Parm.CurParmCMU1.CurParity == SwapDataList[j].swapDVM._Parm.CurParmCMU2.CurParity && SwapDataList[i].swapDVM._Parm.CurParmCMU1.CurStopBits == SwapDataList[j].swapDVM._Parm.CurParmCMU2.CurStopBits && SwapDataList[i].swapDVM._Parm.CurParmCMU1.StationID == SwapDataList[j].swapDVM._Parm.CurParmCMU2.StationID)
                                                {
                                                    
                                                    if (SwapDataList[i].swapDVM._Parm.CurParmCMU1.DeviceName == SwapDataList[i].swapDVM.devicename[3])
                                                    {
                                                        if (SwapDataList[i].swapDVM.Device1.XCExtIn == -1 && SwapDataList[j].swapDVM.Device2.XCExtIn == 0)
                                                            continuefor = true;
                                                        if (SwapDataList[j].swapDVM.Device2.XCExtIn == 10)
                                                        {
                                                            same = true;
                                                            if (SwapDataList[i].swapDVM.Device1.XCExtIn == -1)
                                                            {
                                                                SwapDataList[i].swapDVM.Device1.XC = SwapDataList[j].swapDVM.Device2.XC;
                                                                SwapDataList[i].swapDVM.Device1.XCExtIn = 1;
                                                            }
                                                            if (SwapDataList[i].swapDVM.Device1.XCExtIn == 1)
                                                                SwapDataList[i].swapDVM.XC1Connect = SwapDataList[j].swapDVM.XC2Connect;
                                                        }
                                                    }
                                                    else if (SwapDataList[i].swapDVM._Parm.CurParmCMU1.DeviceName == SwapDataList[i].swapDVM.devicename[4])
                                                    {
                                                        if (SwapDataList[i].swapDVM.Device1.XDExtIn == -1 && SwapDataList[j].swapDVM.Device2.XDExtIn == 0)
                                                            continuefor = true;
                                                        if (SwapDataList[j].swapDVM.Device2.XDExtIn == 10)
                                                        {
                                                            same = true;
                                                            if (SwapDataList[i].swapDVM.Device1.XDExtIn == -1)
                                                            {
                                                                SwapDataList[i].swapDVM.Device1.XD = SwapDataList[j].swapDVM.Device2.XD;
                                                                SwapDataList[i].swapDVM.Device1.XDExtIn = 1;
                                                            }
                                                            if (SwapDataList[i].swapDVM.Device1.XDExtIn == 1)
                                                                SwapDataList[i].swapDVM.XD1Connect = SwapDataList[j].swapDVM.XD2Connect;
                                                        }
                                                    }
                                                }
                                            }
                                        }


                                        if (SwapDataList[i].swapDVM._Parm.CurParmCMU2.DeviceName == SwapDataList[j].swapDVM._Parm.CurParmCMU1.DeviceName)
                                        {
                                            if (SwapDataList[i].swapDVM._Parm.CurParmCMU2.DeviceName == SwapDataList[i].swapDVM.devicename[0] || SwapDataList[i].swapDVM._Parm.CurParmCMU2.DeviceName == SwapDataList[i].swapDVM.devicename[1] || SwapDataList[i].swapDVM._Parm.CurParmCMU2.DeviceName == SwapDataList[i].swapDVM.devicename[2])
                                            {
                                                if (SwapDataList[i].swapDVM._Parm.CurParmCMU2.IP == SwapDataList[j].swapDVM._Parm.CurParmCMU1.IP && SwapDataList[i].swapDVM._Parm.CurParmCMU2.Port == SwapDataList[j].swapDVM._Parm.CurParmCMU1.Port)
                                                {
                                                    if (SwapDataList[i].swapDVM._Parm.CurParmCMU2.DeviceName == SwapDataList[i].swapDVM.devicename[0])
                                                    {
                                                        
                                                        //if (SwapDataList[j].swapDVM.Device1.Fx5UExtIn == 10 || SwapDataList[j].swapDVM.Device1.Fx5UExtIn == 0)
                                                        if (SwapDataList[j].swapDVM.Device1.Fx5UExtIn == 0 && SwapDataList[i].swapDVM.Device2.Fx5UExtIn == -1)
                                                            continuefor1 = true;
                                                        if (SwapDataList[j].swapDVM.Device1.Fx5UExtIn == 10)
                                                        {
                                                            same1 = true;
                                                            if (SwapDataList[i].swapDVM.Device2.Fx5UExtIn == -1)
                                                            {
                                                                SwapDataList[i].swapDVM.Device2.Fx5U = SwapDataList[j].swapDVM.Device1.Fx5U;
                                                                SwapDataList[i].swapDVM.Device2.Fx5UExtIn = 1;
                                                            }
                                                            if (SwapDataList[i].swapDVM.Device2.Fx5UExtIn == 1)
                                                                SwapDataList[i].swapDVM.Fx5U2Connect = SwapDataList[j].swapDVM.Fx5U1Connect;
                                                        }
                                                    }
                                                    else if (SwapDataList[i].swapDVM._Parm.CurParmCMU2.StationID == SwapDataList[i].swapDVM._Parm.CurParmCMU2.StationID)
                                                    {
                                                        
                                                        if (SwapDataList[i].swapDVM._Parm.CurParmCMU2.DeviceName == SwapDataList[i].swapDVM.devicename[1])
                                                        {
                                                            if (SwapDataList[i].swapDVM.Device2.H3UExtIn == -1 && SwapDataList[j].swapDVM.Device1.H3UExtIn == 0)
                                                                continuefor1 = true;
                                                            if (SwapDataList[j].swapDVM.Device1.H3UExtIn == 10)
                                                            {
                                                                same1 = true;
                                                                if (SwapDataList[i].swapDVM.Device2.H3UExtIn == -1)
                                                                {
                                                                    SwapDataList[i].swapDVM.Device2.H3U = SwapDataList[j].swapDVM.Device1.H3U;
                                                                    SwapDataList[i].swapDVM.Device2.H3UExtIn = 1;
                                                                }
                                                                if (SwapDataList[i].swapDVM.Device2.H3UExtIn == 1)
                                                                    SwapDataList[i].swapDVM.H3U2Connect = SwapDataList[j].swapDVM.H3U1Connect;
                                                            }
                                                        }
                                                        else if (SwapDataList[i].swapDVM._Parm.CurParmCMU2.DeviceName == SwapDataList[i].swapDVM.devicename[2])
                                                        {
                                                            if (SwapDataList[i].swapDVM.Device2.H5UExtIn == -1 && SwapDataList[j].swapDVM.Device1.H5UExtIn == 0)
                                                                continuefor1 = true;
                                                            if (SwapDataList[j].swapDVM.Device1.H5UExtIn == 10)
                                                            {
                                                                same1 = true;
                                                                if (SwapDataList[i].swapDVM.Device2.H5UExtIn == -1)
                                                                {
                                                                    SwapDataList[i].swapDVM.Device2.H5U = SwapDataList[j].swapDVM.Device1.H5U;
                                                                    SwapDataList[i].swapDVM.Device2.H5UExtIn = 1;
                                                                }
                                                                if (SwapDataList[i].swapDVM.Device2.H5UExtIn == 1)
                                                                    SwapDataList[i].swapDVM.H5U2Connect = SwapDataList[j].swapDVM.H5U1Connect;
                                                            }
                                                        }
                                                    }

                                                }
                                            }
                                            else if (SwapDataList[i].swapDVM._Parm.CurParmCMU2.DeviceName == SwapDataList[i].swapDVM.devicename[3] || SwapDataList[i].swapDVM._Parm.CurParmCMU2.DeviceName == SwapDataList[i].swapDVM.devicename[4])
                                            {
                                                if (SwapDataList[i].swapDVM._Parm.CurParmCMU2.CurSerial == SwapDataList[j].swapDVM._Parm.CurParmCMU1.CurSerial && SwapDataList[i].swapDVM._Parm.CurParmCMU2.CurBaudRate == SwapDataList[j].swapDVM._Parm.CurParmCMU1.CurBaudRate && SwapDataList[i].swapDVM._Parm.CurParmCMU2.CurDataBits == SwapDataList[j].swapDVM._Parm.CurParmCMU1.CurDataBits && SwapDataList[i].swapDVM._Parm.CurParmCMU2.CurParity == SwapDataList[j].swapDVM._Parm.CurParmCMU1.CurParity && SwapDataList[i].swapDVM._Parm.CurParmCMU2.CurStopBits == SwapDataList[j].swapDVM._Parm.CurParmCMU1.CurStopBits && SwapDataList[i].swapDVM._Parm.CurParmCMU2.StationID == SwapDataList[j].swapDVM._Parm.CurParmCMU1.StationID)
                                                {
                                                    
                                                    if (SwapDataList[i].swapDVM._Parm.CurParmCMU2.DeviceName == SwapDataList[i].swapDVM.devicename[3])
                                                    {
                                                        if (SwapDataList[i].swapDVM.Device2.XCExtIn == -1 && SwapDataList[j].swapDVM.Device1.XCExtIn == 0)
                                                            continuefor1 = true;
                                                        if (SwapDataList[j].swapDVM.Device1.XCExtIn == 10)
                                                        {
                                                            same1 = true;
                                                            if (SwapDataList[i].swapDVM.Device2.XCExtIn == -1)
                                                            {
                                                                SwapDataList[i].swapDVM.Device2.XC = SwapDataList[j].swapDVM.Device1.XC;
                                                                SwapDataList[i].swapDVM.Device2.XCExtIn = 1;
                                                            }
                                                            if (SwapDataList[i].swapDVM.Device2.XCExtIn == 1)
                                                                SwapDataList[i].swapDVM.XC2Connect = SwapDataList[j].swapDVM.XC1Connect;
                                                        }
                                                    }
                                                    else if (SwapDataList[i].swapDVM._Parm.CurParmCMU2.DeviceName == SwapDataList[i].swapDVM.devicename[4])
                                                    {
                                                        if (SwapDataList[i].swapDVM.Device2.XDExtIn == -1 && SwapDataList[j].swapDVM.Device1.XDExtIn == 0)
                                                            continuefor1 = true;
                                                        if (SwapDataList[j].swapDVM.Device1.XDExtIn == 10)
                                                        {
                                                            same1 = true;
                                                            if (SwapDataList[i].swapDVM.Device2.XDExtIn == -1)
                                                            {
                                                                SwapDataList[i].swapDVM.Device2.XD = SwapDataList[j].swapDVM.Device1.XD;
                                                                SwapDataList[i].swapDVM.Device2.XDExtIn = 1;
                                                            }
                                                            if (SwapDataList[i].swapDVM.Device2.XDExtIn == 1)
                                                                SwapDataList[i].swapDVM.XD1Connect = SwapDataList[j].swapDVM.XD1Connect;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else if (SwapDataList[i].swapDVM._Parm.CurParmCMU2.DeviceName == SwapDataList[j].swapDVM._Parm.CurParmCMU2.DeviceName)
                                        {
                                            if (SwapDataList[i].swapDVM._Parm.CurParmCMU2.DeviceName == SwapDataList[i].swapDVM.devicename[0] || SwapDataList[i].swapDVM._Parm.CurParmCMU2.DeviceName == SwapDataList[i].swapDVM.devicename[1] || SwapDataList[i].swapDVM._Parm.CurParmCMU2.DeviceName == SwapDataList[i].swapDVM.devicename[2])
                                            {
                                                if (SwapDataList[i].swapDVM._Parm.CurParmCMU2.IP == SwapDataList[j].swapDVM._Parm.CurParmCMU2.IP && SwapDataList[i].swapDVM._Parm.CurParmCMU2.Port == SwapDataList[j].swapDVM._Parm.CurParmCMU2.Port)
                                                {
                                                    if (SwapDataList[i].swapDVM._Parm.CurParmCMU2.DeviceName == SwapDataList[i].swapDVM.devicename[0])
                                                    {
                                                        
                                                        if (SwapDataList[i].swapDVM.Device2.Fx5UExtIn == -1 && SwapDataList[j].swapDVM.Device2.Fx5UExtIn == 0)
                                                            continuefor1 = true;
                                                        if (SwapDataList[j].swapDVM.Device2.Fx5UExtIn == 10)
                                                        {
                                                            same1 = true;
                                                            if (SwapDataList[i].swapDVM.Device2.Fx5UExtIn == -1)
                                                            {
                                                                SwapDataList[i].swapDVM.Device2.Fx5U = SwapDataList[j].swapDVM.Device2.Fx5U;
                                                                SwapDataList[i].swapDVM.Device2.Fx5UExtIn = 1;
                                                            }
                                                            if (SwapDataList[i].swapDVM.Device2.Fx5UExtIn == 1)
                                                                SwapDataList[i].swapDVM.Fx5U2Connect = SwapDataList[j].swapDVM.Fx5U2Connect;
                                                        }
                                                    }
                                                    else if (SwapDataList[i].swapDVM._Parm.CurParmCMU2.StationID == SwapDataList[i].swapDVM._Parm.CurParmCMU1.StationID)
                                                    {
                                                        
                                                        if (SwapDataList[i].swapDVM._Parm.CurParmCMU2.DeviceName == SwapDataList[i].swapDVM.devicename[1])
                                                        {
                                                            if (SwapDataList[i].swapDVM.Device2.H3UExtIn == -1 && SwapDataList[j].swapDVM.Device2.H3UExtIn == 0)
                                                                continuefor1 = true;
                                                            if (SwapDataList[j].swapDVM.Device2.H3UExtIn == 10)
                                                            {
                                                                same1 = true;
                                                                if (SwapDataList[i].swapDVM.Device2.H3UExtIn == -1)
                                                                {
                                                                    SwapDataList[i].swapDVM.Device2.H3U = SwapDataList[j].swapDVM.Device2.H3U;
                                                                    SwapDataList[i].swapDVM.Device2.H3UExtIn = 1;
                                                                }
                                                                if (SwapDataList[i].swapDVM.Device2.H3UExtIn == 1)
                                                                    SwapDataList[i].swapDVM.H3U2Connect = SwapDataList[j].swapDVM.H3U2Connect;
                                                            }
                                                        }
                                                        else if (SwapDataList[i].swapDVM._Parm.CurParmCMU2.DeviceName == SwapDataList[i].swapDVM.devicename[2])
                                                        {
                                                            if (SwapDataList[i].swapDVM.Device2.H5UExtIn == -1 && SwapDataList[j].swapDVM.Device2.H5UExtIn == 0)
                                                                continuefor1 = true;
                                                            if (SwapDataList[j].swapDVM.Device2.H5UExtIn == 10)
                                                            {
                                                                same1 = true;
                                                                if (SwapDataList[i].swapDVM.Device2.H5UExtIn == -1)
                                                                {
                                                                    SwapDataList[i].swapDVM.Device2.H5U = SwapDataList[j].swapDVM.Device2.H5U;
                                                                    SwapDataList[i].swapDVM.Device2.H5UExtIn = 1;
                                                                }
                                                                if (SwapDataList[i].swapDVM.Device2.H5UExtIn == 1)
                                                                    SwapDataList[i].swapDVM.H5U2Connect = SwapDataList[j].swapDVM.H5U2Connect;
                                                            }
                                                        }
                                                    }

                                                }
                                            }
                                            else if (SwapDataList[i].swapDVM._Parm.CurParmCMU2.DeviceName == SwapDataList[i].swapDVM.devicename[3] || SwapDataList[i].swapDVM._Parm.CurParmCMU2.DeviceName == SwapDataList[i].swapDVM.devicename[4])
                                            {
                                                if (SwapDataList[i].swapDVM._Parm.CurParmCMU2.CurSerial == SwapDataList[j].swapDVM._Parm.CurParmCMU2.CurSerial && SwapDataList[i].swapDVM._Parm.CurParmCMU2.CurBaudRate == SwapDataList[j].swapDVM._Parm.CurParmCMU2.CurBaudRate && SwapDataList[i].swapDVM._Parm.CurParmCMU2.CurDataBits == SwapDataList[j].swapDVM._Parm.CurParmCMU2.CurDataBits && SwapDataList[i].swapDVM._Parm.CurParmCMU2.CurParity == SwapDataList[j].swapDVM._Parm.CurParmCMU2.CurParity && SwapDataList[i].swapDVM._Parm.CurParmCMU2.CurStopBits == SwapDataList[j].swapDVM._Parm.CurParmCMU2.CurStopBits && SwapDataList[i].swapDVM._Parm.CurParmCMU2.StationID == SwapDataList[j].swapDVM._Parm.CurParmCMU2.StationID)
                                                {
                                                    
                                                    if (SwapDataList[i].swapDVM._Parm.CurParmCMU2.DeviceName == SwapDataList[i].swapDVM.devicename[3])
                                                    {
                                                        if (SwapDataList[i].swapDVM.Device2.XCExtIn == -1 && SwapDataList[j].swapDVM.Device2.XCExtIn == 0)
                                                            continuefor1 = true;
                                                        if (SwapDataList[j].swapDVM.Device2.XCExtIn == 10)
                                                        {
                                                            same1 = true;
                                                            if (SwapDataList[i].swapDVM.Device2.XCExtIn == -1)
                                                            {
                                                                SwapDataList[i].swapDVM.Device2.XC = SwapDataList[j].swapDVM.Device2.XC;
                                                                SwapDataList[i].swapDVM.Device2.XCExtIn = 1;
                                                            }
                                                            if (SwapDataList[i].swapDVM.Device2.XCExtIn == 1)
                                                                SwapDataList[i].swapDVM.XC2Connect = SwapDataList[j].swapDVM.XC2Connect;
                                                        }
                                                    }
                                                    else if (SwapDataList[i].swapDVM._Parm.CurParmCMU2.DeviceName == SwapDataList[i].swapDVM.devicename[4])
                                                    {
                                                        if (SwapDataList[i].swapDVM.Device2.XDExtIn == -1 && SwapDataList[j].swapDVM.Device2.XDExtIn == 0)
                                                            continuefor1 = true;
                                                        if (SwapDataList[j].swapDVM.Device2.XDExtIn == 10)
                                                        {
                                                            same1 = true;
                                                            if (SwapDataList[i].swapDVM.Device2.XDExtIn == -1)
                                                            {
                                                                SwapDataList[i].swapDVM.Device2.XD = SwapDataList[j].swapDVM.Device2.XD;
                                                                SwapDataList[i].swapDVM.Device2.XDExtIn = 1;
                                                            }
                                                            if (SwapDataList[i].swapDVM.Device2.XDExtIn == 1)
                                                                SwapDataList[i].swapDVM.XD2Connect = SwapDataList[j].swapDVM.XD2Connect;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (continuefor)
                                    continue;
                                if (SwapDataList[i].swapDVM.Device1.Fx5UExtIn == -1)
                                    SwapDataList[i].swapDVM.Device1.Fx5UExtIn = 0;
                                if (SwapDataList[i].swapDVM.Device1.H5UExtIn == -1)
                                    SwapDataList[i].swapDVM.Device1.H5UExtIn = 0;
                                if (SwapDataList[i].swapDVM.Device1.H3UExtIn == -1)
                                    SwapDataList[i].swapDVM.Device1.H3UExtIn = 0;
                                if (SwapDataList[i].swapDVM.Device1.XCExtIn == -1)
                                    SwapDataList[i].swapDVM.Device1.XCExtIn = 0;
                                if (SwapDataList[i].swapDVM.Device1.XDExtIn == -1)
                                    SwapDataList[i].swapDVM.Device1.XDExtIn = 0;
                                if (SwapDataList[i].swapDVM.Device1.YAMAHAExtIn == -1)
                                    SwapDataList[i].swapDVM.Device1.YAMAHAExtIn = 0;
                                if (SwapDataList[i].swapDVM.Device1.EpsonExtIn == -1)
                                    SwapDataList[i].swapDVM.Device1.EpsonExtIn = 0;
                                if (continuefor1)
                                    continue;
                                if (SwapDataList[i].swapDVM.Device2.Fx5UExtIn == -1)
                                    SwapDataList[i].swapDVM.Device2.Fx5UExtIn = 0;
                                if (SwapDataList[i].swapDVM.Device2.H5UExtIn == -1)
                                    SwapDataList[i].swapDVM.Device2.H5UExtIn = 0;
                                if (SwapDataList[i].swapDVM.Device2.H3UExtIn == -1)
                                    SwapDataList[i].swapDVM.Device2.H3UExtIn = 0;
                                if (SwapDataList[i].swapDVM.Device2.XCExtIn == -1)
                                    SwapDataList[i].swapDVM.Device2.XCExtIn = 0;
                                if (SwapDataList[i].swapDVM.Device2.XDExtIn == -1)
                                    SwapDataList[i].swapDVM.Device2.XDExtIn = 0;
                                if (SwapDataList[i].swapDVM.Device2.YAMAHAExtIn == -1)
                                    SwapDataList[i].swapDVM.Device2.YAMAHAExtIn = 0;
                                if (SwapDataList[i].swapDVM.Device2.EpsonExtIn == -1)
                                    SwapDataList[i].swapDVM.Device2.EpsonExtIn = 0;
                                if (!same)
                                {
                                    if (SwapDataList[i].swapDVM.Device1.Fx5UExtIn == 1)
                                    {
                                        SwapDataList[i].swapDVM.Fx5U1Connect.IsSuccess = false;
                                        SwapDataList[i].swapDVM.Device1.Fx5U.ConnectClose();
                                        SwapDataList[i].swapDVM.Device1.Fx5UExtIn = -2;
                                    }
                                    if (SwapDataList[i].swapDVM.Device1.H5UExtIn == 1)
                                    {
                                        SwapDataList[i].swapDVM.H5U1Connect.IsSuccess = false;
                                        SwapDataList[i].swapDVM.Device1.H5U.ConnectClose();
                                        SwapDataList[i].swapDVM.Device1.H5UExtIn = -2;
                                    }
                                    if (SwapDataList[i].swapDVM.Device1.H3UExtIn == 1)
                                    {
                                        SwapDataList[i].swapDVM.H3U1Connect.IsSuccess = false;
                                        SwapDataList[i].swapDVM.Device1.H3U.ConnectClose();
                                        SwapDataList[i].swapDVM.Device1.H3UExtIn = -2;
                                    }  
                                    if (SwapDataList[i].swapDVM.Device1.XCExtIn == 1)
                                    {
                                        SwapDataList[i].swapDVM.XC1Connect.IsSuccess = false;
                                        SwapDataList[i].swapDVM.Device1.XC.Close();
                                        SwapDataList[i].swapDVM.Device1.XCExtIn = -2;
                                    }
                                        
                                    if (SwapDataList[i].swapDVM.Device1.XDExtIn == 1)
                                    {
                                        SwapDataList[i].swapDVM.XD1Connect.IsSuccess = false;
                                        SwapDataList[i].swapDVM.Device1.XD.Close();
                                        SwapDataList[i].swapDVM.Device1.XDExtIn = -2;
                                    }
                                        
                                }

                                if (!same1)
                                {
                                    if (SwapDataList[i].swapDVM.Device2.Fx5UExtIn == 1)
                                    {
                                        SwapDataList[i].swapDVM.Fx5U2Connect.IsSuccess = false;
                                        SwapDataList[i].swapDVM.Device2.Fx5U.ConnectClose();
                                        SwapDataList[i].swapDVM.Device2.Fx5UExtIn = -2;
                                    }
                                        
                                    if (SwapDataList[i].swapDVM.Device2.H5UExtIn == 1)
                                    {
                                        SwapDataList[i].swapDVM.H5U2Connect.IsSuccess = false;
                                        SwapDataList[i].swapDVM.Device2.H5U.ConnectClose();
                                        SwapDataList[i].swapDVM.Device2.H5UExtIn = -2;
                                    }
                                        
                                    if (SwapDataList[i].swapDVM.Device2.H3UExtIn == 1)
                                    {
                                        SwapDataList[i].swapDVM.H3U2Connect.IsSuccess = false;
                                        SwapDataList[i].swapDVM.Device2.H3U.ConnectClose();
                                        SwapDataList[i].swapDVM.Device2.H3UExtIn = -2;
                                    }
                                    if (SwapDataList[i].swapDVM.Device2.XCExtIn == 1)
                                    {
                                        SwapDataList[i].swapDVM.XC2Connect.IsSuccess = false;
                                        SwapDataList[i].swapDVM.Device2.XC.Close();
                                        SwapDataList[i].swapDVM.Device2.XCExtIn = -2;
                                    }   
                                    if (SwapDataList[i].swapDVM.Device2.XDExtIn == 1)
                                    {
                                        SwapDataList[i].swapDVM.XD2Connect.IsSuccess = false;
                                        SwapDataList[i].swapDVM.Device2.XD.Close();
                                        SwapDataList[i].swapDVM.Device2.XDExtIn = -2;
                                    }  
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex);
                    }
                    Thread.Sleep(50);
                }
            });
        }

        public DelegateCommand btAddOneClick
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddOne();
                    Sum++;
                    OnPropertyChanged("SwapDataList");
                }
              );
            }
        }
        public DelegateCommand btRemoveOneClick
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    RemoveOne();
                    OnPropertyChanged("SwapDataList");
                }
              );
            }
        }
        LoginPage loginPage;
        public DelegateCommand btLoginClick
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if(!MainPageShow)
                    {
                        loginPage = new LoginPage();
                        loginPage.myLogin.mLoginVM.PasswordSetVal = "123";
                        loginPage.myLogin.mLoginVM.LoginCompleteEvent += MLoginVM_LoginCompleteEvent; ;
                        loginPage.Owner = MainWindowVM.mMainWindow;//将主窗口设置为AboutWindow的拥有者
                        loginPage.WindowStartupLocation = WindowStartupLocation.CenterOwner;//打开的初始位置设置为在Owner的中央         
                        loginPage.ShowDialog();
                    }
                    else
                        MainPageShow=false;
                }
              );
            }
        }

        private void MLoginVM_LoginCompleteEvent(object? sender, EventArgs e)
        {
            if ((bool)sender)
            {
                loginPage.Close();
                MainPageShow =true;
            }
            else
                MainPageShow = false;
        }

        public void AddOne()
        {
            SwapDataList.Add(new SwapD(Sum + 1));
        }
        public void RemoveOne()
        {
            if (SwapDataList.Count > 1)
            {
                if (MessageBox.Show("确认删除《"+ SwapDataList.Last() .swapDVM.ID+ "》？", "此删除不可恢复", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        //SwapDataList.Last().swapDVM._Parm.Enable = false;
                        SwapDataList.Last().swapDVM.XmlDelete();
                        SwapDataList.Remove(SwapDataList.Last());
                        Sum--;
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex);
                    }
                }
            }
        }
    }
}
