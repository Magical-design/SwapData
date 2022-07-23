using MLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Collections;
using System.Diagnostics;
using HslCommunication.Profinet.Melsec;
using HslCommunication.Profinet.XINJE;
using HslCommunication.Profinet.Inovance;
using SwapDataUCtr.Model;
using HslCommunication;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using System.IO.Ports;
using MLib.NET;

namespace SwapDataUCtr
{
    public class SwapDVM : INotifyPropertyChanged
    {
        public Device Device1 = new Device();
        public Device Device2 = new Device();
        private int mVisible1 = 0;
        public int Visible1
        {
            get { return mVisible1; }
            set
            {
                mVisible1 = value;
                OnPropertyChanged("Visible1");
            }
        }
        private int mStationVisible1 = 0;
        public int StationVisible1
        {
            get { return mStationVisible1; }
            set
            {
                mStationVisible1 = value;
                OnPropertyChanged("StationVisible1");
            }
        }
        private int mVisible2 = 0;
        public int Visible2
        {
            get { return mVisible2; }
            set
            {
                mVisible2 = value;
                OnPropertyChanged("Visible2");
            }
        }
        private int mStationVisible2 = 0;
        public int StationVisible2
        {
            get { return mStationVisible2; }
            set
            {
                mStationVisible2 = value;
                OnPropertyChanged("StationVisible2");
            }
        }
        private bool mRunning1 = false;
        public bool Running1
        {
            get
            {
                return mRunning1;
            }
            set
            {
                mRunning1 = value;
                OnPropertyChanged("Running1");
            }
        }
        private bool mRunning2 = false;
        public bool Running2
        {
            get
            {
                return mRunning2;
            }
            set
            {
                mRunning2 = value;
                OnPropertyChanged("Running2");
            }
        }
        IXml xml = new IXml();
        private string xmlPath = MFile.path4 + @"Parm\";
        private string xmlName;

        private int mID;
        public int ID
        {
            get { return mID; }
            set
            {
                mID = value;
                OnPropertyChanged("ID");
            }
        }


        public Parm _Parm { get; set; }
        private ArrayList SedData1 = new ArrayList();
        private ArrayList RecData1 = new ArrayList();


        public SwapDVM()
        {

            //int[] vs = new int[] {1000,55,26};
            //var a = vs.SelectMany(s => Enumerable.Range(0, 16).Select(i => (s & (1 << i)) != 0)).ToArray();
            //byte[] b = Enumerable.Range(0, a.Length / 16).Select(i => (byte)a.Select(b => b ? 1 : 0).Skip(i * 16).Take(16).ToArray().Reverse().Aggregate((k, j) => 2 * k + j)).ToArray();
        }
        public void InitAndRun()
        {
            Init();
            Run1();
            Run2();
        }
        private void Init()
        {

            if (HslCommunication.Authorization.SetAuthorizationCode())
                Trace.WriteLine("注册成功");
            else
                Trace.WriteLine("注册失败");
            for (int i = 1; i < 20; i++)
            {
                serialport.Add("COM" + i);
            }
            XmlLoad();
            _Parm.PropertyChanged += new PropertyChangedEventHandler(XmlSave);
            _Parm.CurParmCMU1Changed += GridVisible1;
            _Parm.CurParmCMU2Changed += GridVisible2;
            _Parm.IsCheckedChanged += IsCheckedChanged;
            foreach (ParmCMU parmCMU in _Parm.ParmCMU1)
            {
                parmCMU.PropertyChanged += XmlSave;
            }
            Ctrl1 = new int[devicename.Length];
            Ctrl2 = new int[devicename.Length];
        }
        private void IsCheckedChanged(object sender, EventArgs e)
        {
            Run1();
            Run2();
        }
        private readonly object Lock1 = new object();
        private async void Run1()
        {

            await Task.Run(() =>
            {
                lock (Lock1)
                {
                    //Trace.WriteLine(_Parm.Enable);
                    if (_Parm.Enable)
                    {
                        if (_Parm.CurParmCMU1.DeviceName == devicename[0])
                            if (Ctrl1[0] == 1)
                                return;
                            else
                            {
                                RunClose1();
                                Ctrl1[0] = 1;
                                Fx5U1Run();
                            }

                        else if (_Parm.CurParmCMU1.DeviceName == devicename[1])
                            if (Ctrl1[1] == 1)
                                return;
                            else
                            {
                                RunClose1();
                                Ctrl1[1] = 1;
                                H3U1Run();
                            }

                        else if (_Parm.CurParmCMU1.DeviceName == devicename[2])
                            if (Ctrl1[2] == 1)
                                return;
                            else
                            {
                                RunClose1();
                                Ctrl1[2] = 1;
                                H5U1Run();
                            }

                        else if (_Parm.CurParmCMU1.DeviceName == devicename[3])
                            if (Ctrl1[3] == 1)
                                return;
                            else
                            {
                                RunClose1();
                                Ctrl1[3] = 1;
                                XC1Run();
                            }

                        else if (_Parm.CurParmCMU1.DeviceName == devicename[4])
                            if (Ctrl1[4] == 1)
                                return;
                            else
                            {
                                RunClose1();
                                Ctrl1[4] = 1;
                                XD1Run();
                            }

                        else if (_Parm.CurParmCMU1.DeviceName == devicename[5])
                            if (Ctrl1[5] == 1)
                                return;
                            else
                            {
                                RunClose1();
                                Ctrl1[5] = 1;
                                YAMAHA1Run();
                            }
                        
                        else if (_Parm.CurParmCMU1.DeviceName == devicename[6])
                            if (Ctrl1[6] == 1)
                                return;
                            else
                            {
                                RunClose1();
                                Ctrl1[6] = 1;
                                Epson1Run();
                            }
                        
                    }
                    else
                    {
                        //Trace.WriteLine("执行关闭1");
                        RunClose1();
                        //try
                        //{
                        //    Device1.H3U.ConnectClose();
                        //    Device1.H5U.ConnectClose();
                        //    Device1.Fx5U.ConnectClose();
                        //    if (Device1.XCExtIn == 10)
                        //        Device1.XC.Close();
                        //    Device1.XD.Close();
                        //    Device1.YAMAHA.Close();
                        //    Device1.Epson.Close();
                        //}
                        //catch (Exception)
                        //{
                        //}
                        Running1 = false;
                        SedData1.Clear();
                        RecData1.Clear();
                        _Parm.CurParmCMU1.StatusConnect = false;
                    }
                }
            });

        }
        private void RunClose1()
        {
            for (int i = 0; i < Ctrl1.Length; i++)
            {
                if (Ctrl1[i] != 0)
                {
                    Ctrl1[i] = -1;
                    while (Ctrl1[i] != 0)
                    {
                        Thread.Sleep(20);
                    }
                }
            }
        }
        private readonly object Lock2 = new object();
        private async void Run2()
        {
            await Task.Run(() =>
            {
                lock (Lock2)
                {
                    if (_Parm.Enable)
                    {
                        if (_Parm.CurParmCMU2.DeviceName == devicename[0])
                            if (Ctrl2[0] == 1)
                                return;
                            else
                            {
                                RunClose2();
                                Ctrl2[0] = 1;
                                Fx5U2Run();
                            }
                        else if (_Parm.CurParmCMU2.DeviceName == devicename[1])
                            if (Ctrl2[1] == 1)
                                return;
                            else
                            {
                                RunClose2();
                                Ctrl2[1] = 1;
                                H3U2Run();
                            }
                        else if (_Parm.CurParmCMU2.DeviceName == devicename[2])
                            if (Ctrl2[2] == 1)
                                return;
                            else
                            {
                                RunClose2();
                                Ctrl2[2] = 1;
                                H5U2Run();
                            }
                        else if (_Parm.CurParmCMU2.DeviceName == devicename[3])
                            if (Ctrl2[3] == 1)
                                return;
                            else
                            {
                                RunClose2();
                                Ctrl2[3] = 1;
                                XC2Run();
                            }
                        else if (_Parm.CurParmCMU2.DeviceName == devicename[4])
                            if (Ctrl2[4] == 1)
                                return;
                            else
                            {
                                RunClose2();
                                Ctrl2[4] = 1;
                                XD2Run();
                            }
                        else if (_Parm.CurParmCMU2.DeviceName == devicename[5])
                            if (Ctrl2[5] == 1)
                                return;
                            else
                            {
                                RunClose2();
                                Ctrl2[5] = 1;
                                YAMAHA2Run();
                            }
                        else if (_Parm.CurParmCMU2.DeviceName == devicename[6])
                            if (Ctrl2[6] == 1)
                                return;
                            else
                            {
                                RunClose2();
                                Ctrl2[6] = 1;
                                Epson2Run();
                            }
                    }
                    else
                    {
                        //Trace.WriteLine("执行关闭2");
                        RunClose2();
                        //try
                        //{
                        //    Device2.H3U.ConnectClose();
                        //    Device2.H5U.ConnectClose();
                        //    Device2.Fx5U.ConnectClose();
                        //    if (Device2.XCExtIn == 10)
                        //        Device2.XC.Close();
                        //    Device2.XD.Close();
                        //    Device2.YAMAHA.Close();
                        //    Device2.Epson.Close();
                        //}
                        //catch (Exception)
                        //{
                        //}
                        Running2 = false;
                        SedData1.Clear();
                        RecData1.Clear();
                        _Parm.CurParmCMU2.StatusConnect = false;
                    }
                }
            });
        }
        private void RunClose2()
        {
            for (int i = 0; i < Ctrl2.Length; i++)
            {
                if (Ctrl2[i] != 0)
                {
                    Ctrl2[i] = -1;
                    while (Ctrl2[i] != 0)
                    {
                        Thread.Sleep(20);
                    }
                }
            }
        }
        private int[] Ctrl1;
        private int[] Ctrl2;
        public OperateResult Fx5U1Connect = new OperateResult();
        private async void Fx5U1Run()
        {
            if (ID == 0)
                Device1.Fx5UExtIn = 0;
            else
                Device1.Fx5UExtIn = -1;
            await Task.Run(() => { while (Device1.Fx5UExtIn == -1) Thread.Sleep(20); });
            await Task.Run(() =>
            {
                bool opened = false;
                while (Ctrl1[0] == 1)
                {
                    if (Device1.Fx5UExtIn == 0)
                    {
                        try
                        {
                            Device1.Fx5U = new MelsecMcNet();
                            Fx5U1Connect = new OperateResult();
                            Device1.Fx5U.IpAddress = _Parm.CurParmCMU1.IP;
                            Device1.Fx5U.Port = _Parm.CurParmCMU1.Port;
                            Device1.Fx5U.ConnectTimeOut = 3000;
                            Device1.Fx5U.ConnectClose();
                            Device1.Fx5UExtIn = 10;
                            opened = true;
                            Fx5U1Connect = Device1.Fx5U.ConnectServer();
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex.ToString());
                            return;
                        }
                    }
                    else if (Device1.Fx5UExtIn == -2)
                    {
                        Fx5U1Connect.IsSuccess = false;
                        Device1.Fx5U.ConnectClose();
                        Device1.Fx5UExtIn = -1;
                        continue;
                    }
                    try
                    {
                        try
                        {
                            if (Device1.Fx5U.IpAddressPing() != 0)
                                Fx5U1Connect.IsSuccess = false;
                        }
                        catch (Exception ex)
                        {
                            Fx5U1Connect.IsSuccess = false;
                        }
                        if (Fx5U1Connect.IsSuccess)
                        {
                            Thread.Sleep(20);
                            try
                            {
                                if (_Parm.CurParmCMU1.CurType == "D")
                                    SedData1 = new ArrayList(Device1.Fx5U.ReadInt16(_Parm.CurParmCMU1.CurType + _Parm.CurParmCMU1.SedFirstAdd, (ushort)_Parm.CurParmCMU1.SedLen).Content);
                                else
                                {
                                    if (_Parm.CurParmCMU2.CurType == "D")
                                        SedData1 = new ArrayList(Device1.Fx5U.ReadBool(_Parm.CurParmCMU1.CurType + _Parm.CurParmCMU1.SedFirstAdd, (ushort)(_Parm.CurParmCMU1.SedLen * 16)).Content);
                                    else
                                        SedData1 = new ArrayList(Device1.Fx5U.ReadBool(_Parm.CurParmCMU1.CurType + _Parm.CurParmCMU1.SedFirstAdd, (ushort)_Parm.CurParmCMU1.SedLen).Content);
                                }

                                if (RecData1 != null)
                                {
                                    if (RecData1.Count > 0)
                                    {
                                        if (_Parm.CurParmCMU1.CurType == "D")
                                            Device1.Fx5U.Write(_Parm.CurParmCMU1.CurType + _Parm.CurParmCMU1.RecFirstAdd, (short[])RecData1.ToArray(typeof(short)));
                                        else
                                            Device1.Fx5U.Write(_Parm.CurParmCMU1.CurType + _Parm.CurParmCMU1.RecFirstAdd, (bool[])RecData1.ToArray(typeof(bool)));
                                    }
                                }
                                if (_Parm.CurParmCMU1.CurType != "D" && _Parm.CurParmCMU2.CurType == "D")
                                    Running1 = SedData1.Count == _Parm.CurParmCMU1.SedLen * 16;
                                else
                                    Running1 = SedData1.Count == _Parm.CurParmCMU1.SedLen;
                            }
                            catch (Exception ex)
                            {
                                Trace.WriteLine(ex.ToString());
                                Running1 = false;
                            }
                        }
                        else
                        {
                            Running1 = false;
                            Thread.Sleep(1000);
                            if (Device1.Fx5UExtIn == 10)
                                Fx5U1Connect = Device1.Fx5U.ConnectServer();
                        }
                        if (_Parm.Enable && Fx5U1Connect.IsSuccess != _Parm.CurParmCMU1.StatusConnect)
                            _Parm.CurParmCMU1.StatusConnect = Fx5U1Connect.IsSuccess;
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.ToString());
                        Thread.Sleep(1000);
                    }
                }
                if (opened)
                    Device1.Fx5U.ConnectClose();
                else
                {
                    Device1.Fx5U = new MelsecMcNet();
                    Fx5U1Connect = new OperateResult();
                }
                Running1 = false;
                Ctrl1[0] = 0;
            });

        }
        public OperateResult Fx5U2Connect = new OperateResult();
        private async void Fx5U2Run()
        {
            if (ID == 0)
                Device2.Fx5UExtIn = 0;
            else
                Device2.Fx5UExtIn = -1;
            await Task.Run(() => { while (Device2.Fx5UExtIn == -1) Thread.Sleep(20); });
            int check = Check();
            await Task.Run(() =>
            {
                bool opened = false;
                while (Ctrl2[0] == 1)
                {

                    if (Device2.Fx5UExtIn == 0)
                    {
                        if (check == 0)
                        {
                            try
                            {
                                Device2.Fx5U = new MelsecMcNet();
                                Fx5U2Connect = new OperateResult();
                                Device2.Fx5U.IpAddress = _Parm.CurParmCMU1.IP;
                                Device2.Fx5U.Port = _Parm.CurParmCMU1.Port;
                                Device2.Fx5U.ConnectTimeOut = 3000;
                                Device2.Fx5U.ConnectClose();
                                Device2.Fx5UExtIn = 10;
                                opened = true;
                                Fx5U2Connect = Device1.Fx5U.ConnectServer();

                            }
                            catch (Exception ex)
                            {
                                Trace.WriteLine(ex.ToString());
                                return;
                            }
                        }
                        else
                        {
                            Device2.Fx5U = Device1.Fx5U;
                            Device2.Fx5UExtIn = 2;
                            opened = true;
                        }

                    }
                    else if (Device2.Fx5UExtIn == -2)
                    {
                        Fx5U2Connect.IsSuccess = false;
                        Device2.Fx5U.ConnectClose();
                        Device2.Fx5UExtIn = -1;
                        continue;
                    }
                    if (check != 0 && Device2.Fx5UExtIn == 2)
                        Fx5U2Connect = Fx5U1Connect;
                    try
                    {
                        try
                        {
                            if (Device2.Fx5U.IpAddressPing() != 0)
                                Fx5U2Connect.IsSuccess = false;
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex.ToString());
                            Fx5U2Connect.IsSuccess = false;
                        }
                        if (Fx5U2Connect.IsSuccess)
                        {
                            Thread.Sleep(20);
                            try
                            {
                                if (_Parm.CurParmCMU2.CurType == "D")
                                {
                                    if (_Parm.CurParmCMU1.CurType == "D")
                                        RecData1 = new ArrayList(Device2.Fx5U.ReadInt16(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.SedFirstAdd, (ushort)_Parm.CurParmCMU1.RecLen).Content);
                                    else
                                        RecData1 = new ArrayList(IConvert.ShortToBoolArray(Device2.Fx5U.ReadInt16(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.SedFirstAdd, (ushort)_Parm.CurParmCMU1.RecLen).Content));
                                }
                                else
                                {
                                    if (_Parm.CurParmCMU1.CurType == "D")
                                        RecData1 = new ArrayList(IConvert.BoolToShortArray(Device2.Fx5U.ReadBool(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.SedFirstAdd, (ushort)(_Parm.CurParmCMU1.RecLen * 16)).Content));
                                    else
                                        RecData1 = new ArrayList(Device2.Fx5U.ReadBool(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.SedFirstAdd, (ushort)_Parm.CurParmCMU1.RecLen).Content);
                                }
                                if (SedData1 != null)
                                {
                                    if (SedData1.Count > 0)
                                    {
                                        if (_Parm.CurParmCMU2.CurType == "D")
                                        {
                                            if (_Parm.CurParmCMU1.CurType == "D")
                                                Device2.Fx5U.Write(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.RecFirstAdd, (short[])SedData1.ToArray(typeof(short)));
                                            else
                                                Device2.Fx5U.Write(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.RecFirstAdd, IConvert.BoolToShortArray((bool[])SedData1.ToArray(typeof(bool))));
                                        }
                                        else
                                        {
                                            if (_Parm.CurParmCMU1.CurType == "D")
                                                Device2.Fx5U.Write(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.RecFirstAdd, IConvert.ShortToBoolArray((short[])SedData1.ToArray(typeof(short))));
                                            else
                                                Device2.Fx5U.Write(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.RecFirstAdd, (bool[])SedData1.ToArray(typeof(bool)));
                                        }
                                    }
                                }
                                if (_Parm.CurParmCMU1.CurType != "D" && _Parm.CurParmCMU2.CurType == "D")
                                    Running2 = RecData1.Count == _Parm.CurParmCMU1.RecLen * 16;
                                else
                                    Running2 = RecData1.Count == _Parm.CurParmCMU1.RecLen;
                            }
                            catch (Exception ex)
                            {
                                Trace.WriteLine(ex.ToString());
                                Running2 = false;
                            }
                        }
                        else
                        {
                            Running2 = false;
                            Thread.Sleep(1000);
                            if (Device2.Fx5UExtIn == 10)
                                Fx5U2Connect = Device2.Fx5U.ConnectServer();
                        }
                        if (_Parm.Enable && Fx5U2Connect.IsSuccess != _Parm.CurParmCMU2.StatusConnect)
                            _Parm.CurParmCMU2.StatusConnect = Fx5U2Connect.IsSuccess;
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.ToString());
                        Thread.Sleep(1000);
                    }
                }
                if (opened)
                    Device2.Fx5U.ConnectClose();
                else
                {
                    Device2.Fx5U = new MelsecMcNet();
                    Fx5U2Connect = new OperateResult();
                }
                Running2 = false;
                Ctrl2[0] = 0;
            });

        }
        public OperateResult H3U1Connect = new OperateResult();
        private async void H3U1Run()
        {

            if (ID == 0)
                Device1.H3UExtIn = 0;
            else
                Device1.H3UExtIn = -1;
            await Task.Run(() => { while (Device1.H3UExtIn == -1) Thread.Sleep(20); });
            await Task.Run(() =>
            {
                bool opened = false;
                while (Ctrl1[1] == 1)
                {
                    if (Device1.H5UExtIn == 0)
                    {
                        try
                        {
                            Device1.H3U = new InovanceTcpNet();
                            H3U1Connect = new OperateResult();
                            Device1.H3U.IpAddress = _Parm.CurParmCMU1.IP;
                            Device1.H3U.Port = _Parm.CurParmCMU1.Port;
                            Device1.H3U.ConnectTimeOut = 3000;
                            Device1.H3U.Station = (byte)_Parm.CurParmCMU1.StationID;
                            Device1.H3U.Series = InovanceSeries.H3U;
                            Device1.H3U.DataFormat = HslCommunication.Core.DataFormat.CDAB;
                            Device1.H3U.ConnectClose();
                            opened = true;
                            H3U1Connect = Device1.H3U.ConnectServer();
                            Device1.H3UExtIn = 10;
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex.ToString());
                            return;
                        }
                    }
                    else if (Device1.H3UExtIn == -2)
                    {
                        H3U1Connect.IsSuccess = false;
                        Device1.H3U.ConnectClose();
                        Device1.H3UExtIn = -1;
                        continue;
                    }
                    try
                    {
                        try
                        {
                            if (Device1.H3UExtIn == 10)
                                if (Device1.H3U.IpAddressPing() != 0)
                                    H3U1Connect.IsSuccess = false;
                        }
                        catch (Exception ex)
                        {
                            H3U1Connect.IsSuccess = false;
                        }
                        if (H3U1Connect.IsSuccess)
                        {
                            Thread.Sleep(20);
                            try
                            {
                                if (_Parm.CurParmCMU1.CurType == "D")
                                    SedData1 = new ArrayList(Device1.H3U.ReadInt16(_Parm.CurParmCMU1.CurType + _Parm.CurParmCMU1.SedFirstAdd, (ushort)_Parm.CurParmCMU1.SedLen).Content);
                                else
                                {
                                    if (_Parm.CurParmCMU2.CurType == "D")
                                        SedData1 = new ArrayList(Device1.H3U.ReadCoil(_Parm.CurParmCMU1.CurType + _Parm.CurParmCMU1.SedFirstAdd, (ushort)(_Parm.CurParmCMU1.SedLen * 16)).Content);
                                    else
                                        SedData1 = new ArrayList(Device1.H3U.ReadCoil(_Parm.CurParmCMU1.CurType + _Parm.CurParmCMU1.SedFirstAdd, (ushort)_Parm.CurParmCMU1.SedLen).Content);
                                }

                                if (RecData1 != null)
                                {
                                    if (RecData1.Count > 0)
                                    {
                                        if (_Parm.CurParmCMU1.CurType == "D")
                                            Device1.H3U.Write(_Parm.CurParmCMU1.CurType + _Parm.CurParmCMU1.RecFirstAdd, (short[])RecData1.ToArray(typeof(short)));
                                        else
                                            Device1.H3U.Write(_Parm.CurParmCMU1.CurType + _Parm.CurParmCMU1.RecFirstAdd, (bool[])RecData1.ToArray(typeof(bool)));
                                    }
                                }
                                if (_Parm.CurParmCMU1.CurType != "D" && _Parm.CurParmCMU2.CurType == "D")
                                    Running1 = SedData1.Count == _Parm.CurParmCMU1.SedLen * 16;
                                else
                                    Running1 = SedData1.Count == _Parm.CurParmCMU1.SedLen;
                            }
                            catch (Exception ex)
                            {
                                Trace.WriteLine(ex.ToString());
                                Running1 = false;
                            }
                        }
                        else
                        {
                            Running1 = false;
                            Thread.Sleep(1000);
                            if (Device2.H5UExtIn == 10)
                                H3U1Connect = Device1.H3U.ConnectServer();
                        }
                        if (_Parm.Enable && H3U1Connect.IsSuccess != _Parm.CurParmCMU1.StatusConnect)
                            _Parm.CurParmCMU1.StatusConnect = H3U1Connect.IsSuccess;
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.ToString());
                        Thread.Sleep(1000);
                    }
                }
                if (opened)
                    Device1.H3U.ConnectClose();
                else
                {
                    Device1.H3U = new InovanceTcpNet();
                    H3U1Connect = new OperateResult();
                }
                Running1 = false;
                Ctrl1[1] = 0;
            });

        }
        public OperateResult H3U2Connect = new OperateResult();
        private async void H3U2Run()
        {
            if (ID == 0)
                Device2.H3UExtIn = 0;
            else
                Device2.H3UExtIn = -1;
            await Task.Run(() => { while (Device2.H3UExtIn == -1) Thread.Sleep(20); });
            int check = Check();
            await Task.Run(() =>
            {
                bool opened = false;
                while (Ctrl2[1] == 1)
                {

                    if (Device2.H3UExtIn == 0)
                    {
                        if (check == 0)
                        {
                            try
                            {
                                Device2.H3U = new InovanceTcpNet();
                                H3U2Connect = new OperateResult();
                                Device2.H3U.IpAddress = _Parm.CurParmCMU2.IP;
                                Device2.H3U.Port = _Parm.CurParmCMU2.Port;
                                Device2.H3U.ConnectTimeOut = 3000;
                                Device2.H3U.Station = (byte)_Parm.CurParmCMU2.StationID;
                                Device2.H3U.Series = InovanceSeries.H3U;
                                Device2.H3U.DataFormat = HslCommunication.Core.DataFormat.CDAB;
                                Device2.H3U.ConnectClose();
                                Device2.H3UExtIn = 10;
                                opened = true;
                                H3U2Connect = Device2.H3U.ConnectServer();
                            }
                            catch (Exception ex)
                            {
                                Trace.WriteLine(ex.ToString());
                                return;
                            }
                        }
                        else
                        {
                            Device2.H3U = Device1.H3U;
                            Device2.H3UExtIn = 2;
                        }

                    }
                    else if (Device2.H3UExtIn == -2)
                    {
                        H3U2Connect.IsSuccess = false;
                        Device2.H3U.ConnectClose();
                        Device2.H3UExtIn = -1;
                        continue;
                    }
                    if (check != 0 && Device2.H3UExtIn == 2)
                        H3U2Connect = H3U1Connect;
                    try
                    {
                        try
                        {
                            if (Device2.H3UExtIn == 10)
                                if (Device2.H3U.IpAddressPing() != 0)
                                    H3U2Connect.IsSuccess = false;
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex.ToString());
                            H3U2Connect.IsSuccess = false;
                        }
                        if (H3U2Connect.IsSuccess)
                        {
                            Thread.Sleep(20);
                            try
                            {
                                if (_Parm.CurParmCMU2.CurType == "D")
                                {
                                    if (_Parm.CurParmCMU1.CurType == "D")
                                        RecData1 = new ArrayList(Device2.H3U.ReadInt16(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.SedFirstAdd, (ushort)_Parm.CurParmCMU1.RecLen).Content);
                                    else
                                        RecData1 = new ArrayList(IConvert.ShortToBoolArray(Device2.H3U.ReadInt16(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.SedFirstAdd, (ushort)_Parm.CurParmCMU1.RecLen).Content));
                                }
                                else
                                {
                                    if (_Parm.CurParmCMU1.CurType == "D")
                                        RecData1 = new ArrayList(IConvert.BoolToShortArray(Device2.H3U.ReadCoil(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.SedFirstAdd, (ushort)(_Parm.CurParmCMU1.RecLen * 16)).Content));
                                    else
                                        RecData1 = new ArrayList(Device2.H3U.ReadCoil(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.SedFirstAdd, (ushort)_Parm.CurParmCMU1.RecLen).Content);
                                }
                                if (SedData1 != null)
                                {
                                    if (SedData1.Count > 0)
                                    {
                                        if (_Parm.CurParmCMU2.CurType == "D")
                                        {
                                            if (_Parm.CurParmCMU1.CurType == "D")
                                                Device2.H3U.Write(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.RecFirstAdd, (short[])SedData1.ToArray(typeof(short)));
                                            else
                                                Device2.H3U.Write(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.RecFirstAdd, IConvert.BoolToShortArray((bool[])SedData1.ToArray(typeof(bool))));
                                        }
                                        else
                                        {
                                            if (_Parm.CurParmCMU1.CurType == "D")
                                                Device2.H3U.Write(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.RecFirstAdd, IConvert.ShortToBoolArray((short[])SedData1.ToArray(typeof(short))));
                                            else
                                                Device2.H3U.Write(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.RecFirstAdd, (bool[])SedData1.ToArray(typeof(bool)));
                                        }
                                    }
                                }
                                if (_Parm.CurParmCMU1.CurType != "D" && _Parm.CurParmCMU2.CurType == "D")
                                    Running2 = RecData1.Count == _Parm.CurParmCMU1.RecLen * 16;
                                else
                                    Running2 = RecData1.Count == _Parm.CurParmCMU1.RecLen;
                            }
                            catch (Exception ex)
                            {
                                Trace.WriteLine(ex.ToString());
                                Running2 = false;
                                opened = true;
                            }
                        }
                        else
                        {
                            Running2 = false;
                            Thread.Sleep(1000);
                            if (Device2.H3UExtIn == 10)
                                H3U2Connect = Device2.H3U.ConnectServer();
                        }
                        if (_Parm.Enable && H3U2Connect.IsSuccess != _Parm.CurParmCMU2.StatusConnect)
                            _Parm.CurParmCMU2.StatusConnect = H3U2Connect.IsSuccess;
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.ToString());
                        Thread.Sleep(1000);
                    }
                }
                if (opened)
                    Device2.H3U.ConnectClose();
                else
                {
                    Device2.H3U = new InovanceTcpNet();
                    H3U2Connect = new OperateResult();
                }
                Running2 = false;
                Ctrl2[1] = 0;
            });

        }
        public OperateResult H5U1Connect = new OperateResult();
        private async void H5U1Run()
        {

            if (ID == 0)
                Device1.H5UExtIn = 0;
            else
                Device1.H5UExtIn = -1;
            await Task.Run(() => { while (Device1.H5UExtIn == -1) Thread.Sleep(20); });
            await Task.Run(() =>
            {
                bool opened = false;
                while (Ctrl1[2] == 1)
                {
                    if (Device1.H5UExtIn == 0)
                    {
                        try
                        {
                            Device1.H5U = new InovanceTcpNet();
                            H5U1Connect = new OperateResult();
                            Device1.H5U.IpAddress = _Parm.CurParmCMU1.IP;
                            Device1.H5U.Port = _Parm.CurParmCMU1.Port;
                            Device1.H5U.ConnectTimeOut = 3000;
                            Device1.H5U.Station = (byte)_Parm.CurParmCMU1.StationID;
                            Device1.H5U.Series = InovanceSeries.H5U;
                            Device1.H5U.DataFormat = HslCommunication.Core.DataFormat.CDAB;
                            Device1.H5U.ConnectClose();
                            opened = true;
                            H5U1Connect = Device1.H5U.ConnectServer();
                            Device1.H5UExtIn = 10;

                        }
                        catch (Exception ex)
                        {

                            Trace.WriteLine(ex.ToString());
                            return;
                        }
                    }
                    else if (Device1.H5UExtIn == -2)
                    {
                        H5U1Connect.IsSuccess = false;
                        Device1.H5U.ConnectClose();
                        Device1.H5UExtIn = -1;
                        continue;
                    }
                    try
                    {
                        try
                        {
                            if (Device1.H5UExtIn == 10)
                                if (Device1.H5U.IpAddressPing() != 0)
                                    H5U1Connect.IsSuccess = false;
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex.ToString());
                            H5U1Connect.IsSuccess = false;
                        }
                        if (H5U1Connect.IsSuccess)
                        {
                            Thread.Sleep(20);
                            try
                            {
                                if (_Parm.CurParmCMU1.CurType == "D")
                                    SedData1 = new ArrayList(Device1.H5U.ReadInt16(_Parm.CurParmCMU1.CurType + _Parm.CurParmCMU1.SedFirstAdd, (ushort)_Parm.CurParmCMU1.SedLen).Content);
                                else
                                {
                                    if (_Parm.CurParmCMU2.CurType == "D")
                                        SedData1 = new ArrayList(Device1.H5U.ReadCoil(_Parm.CurParmCMU1.CurType + _Parm.CurParmCMU1.SedFirstAdd, (ushort)(_Parm.CurParmCMU1.SedLen * 16)).Content);
                                    else
                                        SedData1 = new ArrayList(Device1.H5U.ReadCoil(_Parm.CurParmCMU1.CurType + _Parm.CurParmCMU1.SedFirstAdd, (ushort)_Parm.CurParmCMU1.SedLen).Content);
                                }

                                if (RecData1 != null)
                                {
                                    if (RecData1.Count > 0)
                                    {
                                        if (_Parm.CurParmCMU1.CurType == "D")
                                            Device1.H5U.Write(_Parm.CurParmCMU1.CurType + _Parm.CurParmCMU1.RecFirstAdd, (short[])RecData1.ToArray(typeof(short)));
                                        else
                                            Device1.H5U.Write(_Parm.CurParmCMU1.CurType + _Parm.CurParmCMU1.RecFirstAdd, (bool[])RecData1.ToArray(typeof(bool)));
                                    }
                                }
                                if (_Parm.CurParmCMU1.CurType != "D" && _Parm.CurParmCMU2.CurType == "D")
                                    Running1 = SedData1.Count == _Parm.CurParmCMU1.SedLen * 16;
                                else
                                    Running1 = SedData1.Count == _Parm.CurParmCMU1.SedLen;
                            }
                            catch (Exception ex)
                            {
                                Trace.WriteLine(ex.ToString());
                                Running1 = false;
                            }
                        }
                        else
                        {
                            Running1 = false;
                            Thread.Sleep(1000);
                            if (Device1.H5UExtIn == 10)
                                H5U1Connect = Device1.H5U.ConnectServer();
                        }
                        if (_Parm.Enable && H5U1Connect.IsSuccess != _Parm.CurParmCMU1.StatusConnect)
                            _Parm.CurParmCMU1.StatusConnect = H5U1Connect.IsSuccess;
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.ToString());
                        Thread.Sleep(1000);
                    }
                }
                if (opened)
                    Device1.H5U.ConnectClose();
                else
                {
                    Device1.H5U = new InovanceTcpNet();
                    H5U1Connect = new OperateResult();
                }
                Running1 = false;
                Ctrl1[2] = 0;
            });

        }
        public OperateResult H5U2Connect = new OperateResult();
        private async void H5U2Run()
        {
            if (ID == 0)
                Device2.H5UExtIn = 0;
            else
                Device2.H5UExtIn = -1;
            await Task.Run(() => { while (Device2.H5UExtIn == -1) Thread.Sleep(20); });
            int check = Check();
            await Task.Run(() =>
            {
                bool opened = false;
                while (Ctrl2[2] == 1)
                {

                    if (Device2.H5UExtIn == 0)
                    {
                        if (check == 0)
                        {
                            try
                            {
                                Device2.H5U = new InovanceTcpNet();
                                H5U2Connect = new OperateResult();
                                Device2.H5U.IpAddress = _Parm.CurParmCMU2.IP;
                                Device2.H5U.Port = _Parm.CurParmCMU2.Port;
                                Device2.H5U.ConnectTimeOut = 3000;
                                Device2.H5U.Station = (byte)_Parm.CurParmCMU2.StationID;
                                Device2.H5U.Series = InovanceSeries.H5U;
                                Device2.H5U.DataFormat = HslCommunication.Core.DataFormat.CDAB;
                                Device2.H5U.ConnectClose();
                                Device2.H5UExtIn = 10;
                                opened = true;
                                H5U2Connect = Device2.H5U.ConnectServer();
                            }
                            catch (Exception ex)
                            {
                                Trace.WriteLine(ex.ToString());
                                return;
                            }
                        }
                        else
                        {
                            Device2.H5U = Device1.H5U;
                            Device2.H5UExtIn = 2;
                            opened = true;
                        }

                    }
                    else if (Device2.H5UExtIn == -2)
                    {
                        H5U2Connect.IsSuccess = false;
                        Device2.H5U.ConnectClose();
                        Device2.H5UExtIn = -1;
                        continue;
                    }
                    if (check != 0 && Device2.H5UExtIn == 2)
                        H5U2Connect = H5U1Connect;
                    try
                    {
                        try
                        {
                            if (Device2.H5UExtIn == 10)
                                if (Device2.H5U.IpAddressPing() != 0)
                                    H5U2Connect.IsSuccess = false;
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex.ToString());
                            H5U2Connect.IsSuccess = false;
                        }
                        if (H5U2Connect.IsSuccess)
                        {
                            Thread.Sleep(20);
                            try
                            {

                                if (_Parm.CurParmCMU2.CurType == "D")
                                {
                                    if (_Parm.CurParmCMU1.CurType == "D")
                                        RecData1 = new ArrayList(Device2.H5U.ReadInt16(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.SedFirstAdd, (ushort)_Parm.CurParmCMU1.RecLen).Content);
                                    else
                                        RecData1 = new ArrayList(IConvert.ShortToBoolArray(Device2.H5U.ReadInt16(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.SedFirstAdd, (ushort)_Parm.CurParmCMU1.RecLen).Content));
                                }
                                else
                                {
                                    if (_Parm.CurParmCMU1.CurType == "D")
                                       RecData1 = new ArrayList(IConvert.BoolToShortArray(Device2.H5U.ReadCoil(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.SedFirstAdd, (ushort)(_Parm.CurParmCMU1.RecLen * 16)).Content));
                                    else
                                       RecData1 = new ArrayList(Device2.H5U.ReadCoil(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.SedFirstAdd, (ushort)_Parm.CurParmCMU1.RecLen).Content);
                                }
  
                                if (SedData1 != null)
                                {
                                    if (SedData1.Count > 0)
                                    {
                                        if (_Parm.CurParmCMU2.CurType == "D")
                                        {
                                            if (_Parm.CurParmCMU1.CurType == "D")
                                                Device2.H5U.Write(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.RecFirstAdd, (short[])SedData1.ToArray(typeof(short)));
                                            else
                                                Device2.H5U.Write(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.RecFirstAdd, IConvert.BoolToShortArray((bool[])SedData1.ToArray(typeof(bool))));
                                        }
                                        else
                                        {
                                            if (_Parm.CurParmCMU1.CurType == "D")
                                                Device2.H5U.Write(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.RecFirstAdd, IConvert.ShortToBoolArray((short[])SedData1.ToArray(typeof(short))));
                                            else
                                                Device2.H5U.Write(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.RecFirstAdd, (bool[])SedData1.ToArray(typeof(bool)));
                                        }                                            
                                    }
                                }
                                if (_Parm.CurParmCMU1.CurType != "D" && _Parm.CurParmCMU2.CurType == "D")
                                    Running2 = RecData1.Count == _Parm.CurParmCMU1.RecLen * 16;
                                else
                                    Running2 = RecData1.Count == _Parm.CurParmCMU1.RecLen;
                            }
                            catch (Exception ex)
                            {
                                Trace.WriteLine(ex.ToString());
                                Running2 = false;
                            }
                        }
                        else
                        {
                            Running2 = false;
                            Thread.Sleep(1000);
                            if (Device2.H5UExtIn == 10)
                                H5U2Connect = Device2.H5U.ConnectServer();
                        }
                        if (_Parm.Enable && H5U2Connect.IsSuccess != _Parm.CurParmCMU2.StatusConnect)
                            _Parm.CurParmCMU2.StatusConnect = H5U2Connect.IsSuccess;
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.ToString());
                        Thread.Sleep(1000);
                    }
                }
                if (opened)
                    Device2.H5U.ConnectClose();
                else
                {
                    Device2.H5U = new InovanceTcpNet();
                    H5U2Connect = new OperateResult();
                }
                Running2 = false;
                Ctrl2[2] = 0;
            });

        }


        public OperateResult XC1Connect = new OperateResult();
        private async void XC1Run()
        {
            if (ID == 0)
                Device1.XCExtIn = 0;
            else
                Device1.XCExtIn = -1;
            await Task.Run(() => { while (Device1.XCExtIn == -1) Thread.Sleep(20); });
            //bool connectread=false;
            await Task.Run(() =>
            {
                bool opened = false;
                while (Ctrl1[3] == 1)
                {
                    if (Device1.XCExtIn == 0)
                    {
                        try
                        {
                            //Device1.XC.Close();
                            //Device1.XC.Dispose();
                            if (TestSerial(_Parm.CurParmCMU1.CurSerial))
                            {
                                Thread.Sleep(200);
                                continue;
                            }
                            Device1.XC = new XinJESerial();
                            XC1Connect = new OperateResult();
                            Device1.XC.Station = (byte)_Parm.CurParmCMU1.StationID;
                            Device1.XC.Series = XinJESeries.XC;
                            Device1.XC.DataFormat = HslCommunication.Core.DataFormat.ABCD;
                            Device1.XC.ReceiveTimeout = 2000;
                            //Device1.XC.SleepTime = 250;
                            Device1.XC.SerialPortInni(sp =>
                            {
                                sp.PortName = _Parm.CurParmCMU1.CurSerial;
                                sp.BaudRate = Int16.Parse(_Parm.CurParmCMU1.CurBaudRate);
                                sp.DataBits = _Parm.CurParmCMU1.CurDataBits;
                                sp.StopBits = _Parm.CurParmCMU1.CurStopBits == 2 ? System.IO.Ports.StopBits.Two : System.IO.Ports.StopBits.One;
                                sp.Parity = _Parm.CurParmCMU1.CurParity == "None" ? System.IO.Ports.Parity.None : _Parm.CurParmCMU1.CurParity == "Even" ? System.IO.Ports.Parity.Even : System.IO.Ports.Parity.Odd;
                            });
                            Device1.XC.Close();
                            opened = true;
                            Trace.WriteLine("打开XC1");
                            XC1Connect = Device1.XC.Open();
                            Device1.XCExtIn = 10;
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex.ToString());
                            return;
                        }
                    }
                    else if(Device1.XCExtIn == -2)
                    {
                        XC1Connect.IsSuccess = false;
                        Device1.XC.Close();
                        Device1.XCExtIn = -1;
                        continue;
                    }
                    try
                    {
                        if (XC1Connect.IsSuccess)
                        {
                            if (Device1.XCExtIn == 10)
                                try
                                {
                                    //var a=Device1.XC.ReadInt16("D0", 1);
                                    XC1Connect.IsSuccess = Device1.XC.ReadInt16("D0", 1).IsSuccess;
                                    if (!XC1Connect.IsSuccess)
                                        Trace.WriteLine("XC1读取失败");
                                }
                                catch (Exception)
                                {
                                    //connectread = false;
                                    XC1Connect.IsSuccess = false;
                                }
                        }
                        else
                            ;//connectread = false;
                            if (XC1Connect.IsSuccess)
                            {
                            try
                            {
                                if (_Parm.CurParmCMU1.CurType == "D")
                                    SedData1 = new ArrayList(Device1.XC.ReadInt16(_Parm.CurParmCMU1.CurType + _Parm.CurParmCMU1.SedFirstAdd, (ushort)_Parm.CurParmCMU1.SedLen).Content);
                                else
                                {
                                    if (_Parm.CurParmCMU2.CurType == "D")
                                        SedData1 = new ArrayList(Device1.XC.ReadCoil(_Parm.CurParmCMU1.CurType + _Parm.CurParmCMU1.SedFirstAdd, (ushort)(_Parm.CurParmCMU1.SedLen * 16)).Content);
                                    else
                                        SedData1 = new ArrayList(Device1.XC.ReadCoil(_Parm.CurParmCMU1.CurType + _Parm.CurParmCMU1.SedFirstAdd, (ushort)_Parm.CurParmCMU1.SedLen).Content);
                                }

                                if (RecData1 != null)
                                {
                                    if (RecData1.Count > 0)
                                    {
                                        if (_Parm.CurParmCMU1.CurType == "D")
                                            Device1.XC.Write(_Parm.CurParmCMU1.CurType + _Parm.CurParmCMU1.RecFirstAdd, (short[])RecData1.ToArray(typeof(short)));
                                        else
                                            Device1.XC.Write(_Parm.CurParmCMU1.CurType + _Parm.CurParmCMU1.RecFirstAdd, (bool[])RecData1.ToArray(typeof(bool)));
                                    }
                                }
                                if (_Parm.CurParmCMU1.CurType != "D" && _Parm.CurParmCMU2.CurType == "D")
                                    Running1 = SedData1.Count == _Parm.CurParmCMU1.SedLen * 16;
                                else
                                    Running1 = SedData1.Count == _Parm.CurParmCMU1.SedLen;
                            }
                            catch (Exception ex)
                            {
                                Trace.WriteLine(ex.ToString());
                                Running1 = false;
                            }
                        }

                        if (_Parm.Enable && XC1Connect.IsSuccess != _Parm.CurParmCMU1.StatusConnect)
                            _Parm.CurParmCMU1.StatusConnect = XC1Connect.IsSuccess;
                        if (!XC1Connect.IsSuccess)
                        {
                            Running1 = false;
                            Thread.Sleep(1000);
                            if (Device1.XCExtIn == 10)
                            {
                                Device1.XC.Close();
                                Thread.Sleep(200);
                                XC1Connect = Device1.XC.Open();
                            }

                        }
                        else
                            Thread.Sleep(20);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.ToString());
                        Thread.Sleep(1000);
                    }
                }
                if (opened)
                {
                    do
                    {
                        Device1.XC.Close();
                        Device1.XC.Dispose();
                        Thread.Sleep(20);
                    } while (Device1.XC.IsOpen());
                    Trace.WriteLine("关闭XC1");
                }
                else
                {
                    Device1.XC = new XinJESerial();
                    XC1Connect = new OperateResult();
                }
                Running1 = false;
                Ctrl1[3] = 0;


            });

        }

        public OperateResult XC2Connect = new OperateResult();
        private async void XC2Run()
        {
            if (ID == 0)
                Device2.XCExtIn = 0;
            else
                Device2.XCExtIn = -1;
            await Task.Run(() => { while (Device2.XCExtIn == -1) Thread.Sleep(20); });
            int check = Check();
            await Task.Run(() =>
            {
                bool opened = false;
                while (Ctrl2[3] == 1)
                {
                    //if (Device2.XCExtIn == -1 && XC2Connect.IsSuccess)
                    //{
                    //    Device2.XC.Close();
                    //    XC2Connect.IsSuccess = false;
                    //}
                    if (Device2.XCExtIn == 0)
                    {
                        if (check == 0)
                        {
                            try
                            {
                                //Device2.XC.Close();
                                //Device2.XC.Dispose();
                                if (TestSerial(_Parm.CurParmCMU2.CurSerial))
                                {
                                    Thread.Sleep(200);
                                    continue;
                                }
                                Device2.XC = new XinJESerial();
                                XC2Connect = new OperateResult();
                                Device2.XC.Station = (byte)_Parm.CurParmCMU2.StationID;
                                Device2.XC.Series = XinJESeries.XC;
                                Device2.XC.DataFormat = HslCommunication.Core.DataFormat.ABCD;
                                Device2.XC.ReceiveTimeout = 2000;
                                //Device2.XC.SleepTime = 250;
                                Device2.XC.SerialPortInni(sp =>
                                {
                                    sp.PortName = _Parm.CurParmCMU2.CurSerial;
                                    sp.BaudRate = Int16.Parse(_Parm.CurParmCMU2.CurBaudRate);
                                    sp.DataBits = _Parm.CurParmCMU2.CurDataBits;
                                    sp.StopBits = _Parm.CurParmCMU2.CurStopBits == 2 ? System.IO.Ports.StopBits.Two : System.IO.Ports.StopBits.One;
                                    sp.Parity = _Parm.CurParmCMU2.CurParity == "None" ? System.IO.Ports.Parity.None : _Parm.CurParmCMU2.CurParity == "Even" ? System.IO.Ports.Parity.Even : System.IO.Ports.Parity.Odd;
                                });
                                Device2.XC.Close();
                                opened = true;
                                Trace.WriteLine("打开XC2");
                                XC2Connect = Device2.XC.Open();
                                Device2.XCExtIn = 10;
                            }
                            catch (Exception ex)
                            {
                                Trace.WriteLine(ex.ToString());
                                return;
                            }
                        }
                        else
                        {
                            Device2.XC = Device1.XC;
                            Device2.XCExtIn = 2;
                            opened = true;
                        }

                    }
                    else if (Device2.XCExtIn == -2)
                    {
                        XC2Connect.IsSuccess = false;
                        Device2.XC.Close();
                        Device2.XCExtIn = -1;
                        continue;
                    }
                    if (check != 0 && Device2.XCExtIn == 2)
                        XC2Connect = XC1Connect;
                    try
                    {

                        if (XC2Connect.IsSuccess)
                        {
                            if (Device2.XCExtIn == 10)
                                try
                                {
                                    XC2Connect.IsSuccess = Device2.XC.ReadInt16("D0", 1).IsSuccess;
                                    if (!XC2Connect.IsSuccess)
                                        Trace.WriteLine("XC2读取失败");
                                }
                                catch (Exception)
                                {
                                    //connectread = false;
                                    XC2Connect.IsSuccess = false;
                                }
                        }
                        if (XC2Connect.IsSuccess)
                        {
                            try
                            {
                                if (_Parm.CurParmCMU2.CurType == "D")
                                {
                                    if (_Parm.CurParmCMU1.CurType == "D")
                                        RecData1 = new ArrayList(Device2.XC.ReadInt16(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.SedFirstAdd, (ushort)_Parm.CurParmCMU1.RecLen).Content);
                                    else
                                        RecData1 = new ArrayList(IConvert.ShortToBoolArray(Device2.XC.ReadInt16(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.SedFirstAdd, (ushort)_Parm.CurParmCMU1.RecLen).Content));
                                }
                                else
                                {
                                    if (_Parm.CurParmCMU1.CurType == "D")
                                        RecData1 = new ArrayList(IConvert.BoolToShortArray(Device2.XC.ReadCoil(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.SedFirstAdd, (ushort)(_Parm.CurParmCMU1.RecLen * 16)).Content));
                                    else
                                        RecData1 = new ArrayList(Device2.XC.ReadCoil(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.SedFirstAdd, (ushort)_Parm.CurParmCMU1.RecLen).Content);
                                }
                                if (SedData1 != null)
                                {
                                    if (SedData1.Count > 0)
                                    {
                                        if (_Parm.CurParmCMU2.CurType == "D")
                                        {
                                            if (_Parm.CurParmCMU1.CurType == "D")
                                                Device2.XC.Write(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.RecFirstAdd, (short[])SedData1.ToArray(typeof(short)));
                                            else
                                                Device2.XC.Write(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.RecFirstAdd, IConvert.BoolToShortArray((bool[])SedData1.ToArray(typeof(bool))));
                                        }
                                        else
                                        {
                                            if (_Parm.CurParmCMU1.CurType == "D")
                                                Device2.XC.Write(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.RecFirstAdd, IConvert.ShortToBoolArray((short[])SedData1.ToArray(typeof(short))));
                                            else
                                                Device2.XC.Write(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.RecFirstAdd, (bool[])SedData1.ToArray(typeof(bool)));
                                        }
                                    }
                                }
                                if (_Parm.CurParmCMU1.CurType != "D" && _Parm.CurParmCMU2.CurType == "D")
                                    Running2 = RecData1.Count == _Parm.CurParmCMU1.RecLen * 16;
                                else
                                    Running2 = RecData1.Count == _Parm.CurParmCMU1.RecLen;
                            }
                            catch (Exception ex)
                            {
                                Trace.WriteLine(ex.ToString());
                                Running2 = false;
                            }
                        }
                        if (_Parm.Enable && XC2Connect.IsSuccess != _Parm.CurParmCMU2.StatusConnect)
                            _Parm.CurParmCMU2.StatusConnect = XC2Connect.IsSuccess;
                        if (!XC2Connect.IsSuccess)
                        {
                            Running2 = false;
                            Thread.Sleep(1000);
                            if (Device2.XCExtIn == 10)
                            {
                                Device2.XC.Close();
                                Thread.Sleep(200);
                                XC2Connect = Device2.XC.Open();
                            }
                        }
                        else
                            Thread.Sleep(20);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.ToString());
                        Thread.Sleep(1000);
                    }
                }
                if (opened)
                {
                    do
                    {
                        Device2.XC.Close();
                        Device2.XC.Dispose();
                        Thread.Sleep(20);
                    } while (Device2.XC.IsOpen());

                    Trace.WriteLine("关闭XC2");
                }
                else
                {
                    Device2.XC = new XinJESerial();
                    XC2Connect = new OperateResult();
                }
                Running2 = false;
                Ctrl2[3] = 0;
            });

        }
        SerialPort serialPort = new SerialPort();
        public bool TestSerial(String serial_name)
        {
            try
            {
                serialPort = new SerialPort(serial_name);
                serialPort.Open();
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                    serialPort.Dispose();
                    return false;
                }
                else
                {
                    serialPort.Close();
                    serialPort.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                // 串口被占用
                serialPort.Close();
                serialPort.Dispose();
                return true;
            }
        }

        public OperateResult XD1Connect = new OperateResult();
        private async void XD1Run()
        {
            if (ID == 0)
                Device1.XDExtIn = 0;
            else
                Device1.XDExtIn = -1;
            await Task.Run(() => { while (Device1.XDExtIn == -1) Thread.Sleep(20); });
            //bool connectread=false;
            await Task.Run(() =>
            {
                bool opened = false;
                while (Ctrl1[4] == 1)
                {

                    if (Device1.XDExtIn == 0)
                    {
                        try
                        {
                            if (TestSerial(_Parm.CurParmCMU1.CurSerial))
                            {
                                Thread.Sleep(200);
                                continue;
                            }
                            Device1.XD = new XinJESerial();
                            XD1Connect = new OperateResult();
                            Device1.XD.Station = (byte)_Parm.CurParmCMU1.StationID;
                            Device1.XD.Series = XinJESeries.XD;
                            Device1.XD.DataFormat = HslCommunication.Core.DataFormat.ABCD;
                            Device1.XD.ReceiveTimeout = 2000;
                            //Device1.XD.SleepTime = 250;
                            Device1.XD.SerialPortInni(sp =>
                            {
                                sp.PortName = _Parm.CurParmCMU1.CurSerial;
                                sp.BaudRate = Int16.Parse(_Parm.CurParmCMU1.CurBaudRate);
                                sp.DataBits = _Parm.CurParmCMU1.CurDataBits;
                                sp.StopBits = _Parm.CurParmCMU1.CurStopBits == 2 ? System.IO.Ports.StopBits.Two : System.IO.Ports.StopBits.One;
                                sp.Parity = _Parm.CurParmCMU1.CurParity == "None" ? System.IO.Ports.Parity.None : _Parm.CurParmCMU1.CurParity == "Even" ? System.IO.Ports.Parity.Even : System.IO.Ports.Parity.Odd;
                            });
                            Device1.XD.Close();
                            opened = true;
                            XD1Connect = Device1.XD.Open();
                            Device1.XDExtIn = 10;
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex.ToString());
                            return;
                        }
                    }
                    else if (Device1.XDExtIn == -2)
                    {
                        XD1Connect.IsSuccess = false;
                        Device1.XD.Close();
                        Device1.XDExtIn = -1;
                        continue;
                    }
                    try
                    {
                        if (XD1Connect.IsSuccess)
                        {
                            if (Device1.XDExtIn == 10)
                                try
                                {
                                    //var a=Device1.XD.ReadInt16("D0", 1);
                                    XD1Connect.IsSuccess = Device1.XD.ReadInt16("D0", 1).IsSuccess;
                                    if (!XD1Connect.IsSuccess)
                                        Trace.WriteLine("XD1读取失败");
                                }
                                catch (Exception)
                                {
                                    //connectread = false;
                                    XD1Connect.IsSuccess = false;
                                }
                        }
                        else
                            ;//connectread = false;
                        if (XD1Connect.IsSuccess)
                        {
                            try
                            {
                                if (_Parm.CurParmCMU1.CurType == "D")
                                    SedData1 = new ArrayList(Device1.XD.ReadInt16(_Parm.CurParmCMU1.CurType + _Parm.CurParmCMU1.SedFirstAdd, (ushort)_Parm.CurParmCMU1.SedLen).Content);
                                else
                                {
                                    if (_Parm.CurParmCMU2.CurType == "D")
                                        SedData1 = new ArrayList(Device1.XD.ReadCoil(_Parm.CurParmCMU1.CurType + _Parm.CurParmCMU1.SedFirstAdd, (ushort)(_Parm.CurParmCMU1.SedLen * 16)).Content);
                                    else
                                        SedData1 = new ArrayList(Device1.XD.ReadCoil(_Parm.CurParmCMU1.CurType + _Parm.CurParmCMU1.SedFirstAdd, (ushort)_Parm.CurParmCMU1.SedLen).Content);
                                }

                                if (RecData1 != null)
                                {
                                    if (RecData1.Count > 0)
                                    {
                                        if (_Parm.CurParmCMU1.CurType == "D")
                                            Device1.XD.Write(_Parm.CurParmCMU1.CurType + _Parm.CurParmCMU1.RecFirstAdd, (short[])RecData1.ToArray(typeof(short)));
                                        else
                                            Device1.XD.Write(_Parm.CurParmCMU1.CurType + _Parm.CurParmCMU1.RecFirstAdd, (bool[])RecData1.ToArray(typeof(bool)));
                                    }
                                }
                                if (_Parm.CurParmCMU1.CurType != "D" && _Parm.CurParmCMU2.CurType == "D")
                                    Running1 = SedData1.Count == _Parm.CurParmCMU1.SedLen * 16;
                                else
                                    Running1 = SedData1.Count == _Parm.CurParmCMU1.SedLen;
                            }
                            catch (Exception ex)
                            {
                                Trace.WriteLine(ex.ToString());
                                Running1 = false;
                            }
                        }

                        if (_Parm.Enable && XD1Connect.IsSuccess != _Parm.CurParmCMU1.StatusConnect)
                            _Parm.CurParmCMU1.StatusConnect = XD1Connect.IsSuccess;
                        if (!XD1Connect.IsSuccess)
                        {
                            Running1 = false;
                            Thread.Sleep(1000);
                            if (Device1.XCExtIn == 10)
                            {
                                Device1.XD.Close();
                                Thread.Sleep(200);
                                XD1Connect = Device1.XD.Open();
                            }
                        }
                        else
                            Thread.Sleep(20);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.ToString());
                        Thread.Sleep(1000);
                    }
                }
                if (opened)
                {
                    do
                    {
                        Device1.XD.Close();
                        Device1.XD.Dispose();
                        Thread.Sleep(20);
                    } while (Device1.XD.IsOpen());
                }
                else
                {
                    Device1.XD = new XinJESerial();
                    XD1Connect = new OperateResult();
                }
                Running1 = false;
                Ctrl1[4] = 0;
            });

        }

        public OperateResult XD2Connect = new OperateResult();
        private async void XD2Run()
        {
            if (ID == 0)
                Device2.XDExtIn = 0;
            else
                Device2.XDExtIn = -1;
            await Task.Run(() => { while (Device2.XDExtIn == -1) Thread.Sleep(20); });
            int check = Check();
            await Task.Run(() =>
            {
                bool opened = false;
                while (Ctrl2[4] == 1)
                {

                    if (Device2.XDExtIn == 0)
                    {
                        if (check == 0)
                        {
                            try
                            {
                                if (TestSerial(_Parm.CurParmCMU2.CurSerial))
                                {
                                    Thread.Sleep(200);
                                    continue;
                                }
                                Device2.XD = new XinJESerial();
                                XD2Connect = new OperateResult();
                                Device2.XD.Station = (byte)_Parm.CurParmCMU2.StationID;
                                Device2.XD.Series = XinJESeries.XD;
                                Device2.XD.DataFormat = HslCommunication.Core.DataFormat.ABCD;
                                Device2.XD.ReceiveTimeout = 2000;
                                //Device2.XD.SleepTime = 250;
                                Device2.XD.SerialPortInni(sp =>
                                {
                                    sp.PortName = _Parm.CurParmCMU2.CurSerial;
                                    sp.BaudRate = Int16.Parse(_Parm.CurParmCMU2.CurBaudRate);
                                    sp.DataBits = _Parm.CurParmCMU2.CurDataBits;
                                    sp.StopBits = _Parm.CurParmCMU2.CurStopBits == 2 ? System.IO.Ports.StopBits.Two : System.IO.Ports.StopBits.One;
                                    sp.Parity = _Parm.CurParmCMU2.CurParity == "None" ? System.IO.Ports.Parity.None : _Parm.CurParmCMU2.CurParity == "Even" ? System.IO.Ports.Parity.Even : System.IO.Ports.Parity.Odd;
                                });
                                Device2.XD.Close();
                                opened = true;
                                XD2Connect = Device2.XD.Open();
                                Device2.XDExtIn = 10;
                            }

                            catch (Exception ex)
                            {
                                Trace.WriteLine(ex.ToString());
                                return;
                            }
                        }
                        else
                        {
                            Device2.XD = Device1.XD;
                            Device2.XDExtIn = 2;
                            opened = true;
                        }

                    }
                    else if (Device2.XDExtIn == -2)
                    {
                        XD2Connect.IsSuccess = false;
                        Device2.XD.Close();
                        Device2.XDExtIn = -1;
                        continue;
                    }
                    if (check != 0 && Device2.XDExtIn == 2)
                        XD2Connect = XD1Connect;
                    try
                    {
                        if (XD2Connect.IsSuccess)
                        {
                            if (Device2.XDExtIn == 10)
                                try
                                {
                                    XD2Connect.IsSuccess = Device2.XD.ReadInt16("D0", 1).IsSuccess;
                                    if (!XD2Connect.IsSuccess)
                                        Trace.WriteLine("XD2读取失败");
                                }
                                catch (Exception)
                                {
                                    //connectread = false;
                                    XD2Connect.IsSuccess = false;
                                }
                        }
                        if (XD2Connect.IsSuccess)
                        {
                            try
                            {
                                if (_Parm.CurParmCMU2.CurType == "D")
                                {
                                    if (_Parm.CurParmCMU1.CurType == "D")
                                        RecData1 = new ArrayList(Device2.XD.ReadInt16(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.SedFirstAdd, (ushort)_Parm.CurParmCMU1.RecLen).Content);
                                    else
                                        RecData1 = new ArrayList(IConvert.ShortToBoolArray(Device2.XD.ReadInt16(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.SedFirstAdd, (ushort)_Parm.CurParmCMU1.RecLen).Content));
                                }
                                else
                                {
                                    if (_Parm.CurParmCMU1.CurType == "D")
                                        RecData1 = new ArrayList(IConvert.BoolToShortArray(Device2.XD.ReadCoil(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.SedFirstAdd, (ushort)(_Parm.CurParmCMU1.RecLen * 16)).Content));
                                    else
                                        RecData1 = new ArrayList(Device2.XD.ReadCoil(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.SedFirstAdd, (ushort)_Parm.CurParmCMU1.RecLen).Content);
                                }

                                if (SedData1 != null)
                                {
                                    if (SedData1.Count > 0)
                                    {
                                        if (_Parm.CurParmCMU2.CurType == "D")
                                        {
                                            if (_Parm.CurParmCMU1.CurType == "D")
                                                Device2.XD.Write(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.RecFirstAdd, (short[])SedData1.ToArray(typeof(short)));
                                            else
                                                Device2.XD.Write(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.RecFirstAdd, IConvert.BoolToShortArray((bool[])SedData1.ToArray(typeof(bool))));
                                        }
                                        else
                                        {
                                            if (_Parm.CurParmCMU1.CurType == "D")
                                                Device2.XD.Write(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.RecFirstAdd, IConvert.ShortToBoolArray((short[])SedData1.ToArray(typeof(short))));
                                            else
                                                Device2.XD.Write(_Parm.CurParmCMU2.CurType + _Parm.CurParmCMU2.RecFirstAdd, (bool[])SedData1.ToArray(typeof(bool)));
                                        }
                                    }
                                }
                                if (_Parm.CurParmCMU1.CurType != "D" && _Parm.CurParmCMU2.CurType == "D")
                                    Running2 = RecData1.Count == _Parm.CurParmCMU1.RecLen * 16;
                                else
                                    Running2 = RecData1.Count == _Parm.CurParmCMU1.RecLen;
                            }
                            catch (Exception ex)
                            {
                                Trace.WriteLine(ex.ToString());
                                Running2 = false;
                            }
                        }
                        if (_Parm.Enable && XD2Connect.IsSuccess != _Parm.CurParmCMU2.StatusConnect)
                            _Parm.CurParmCMU2.StatusConnect = XD2Connect.IsSuccess;
                        if (!XD2Connect.IsSuccess)
                        {
                            Running2 = false;
                            Thread.Sleep(1000);
                            if (Device2.XDExtIn == 10)
                            {
                                Device2.XD.Close();
                                Thread.Sleep(200);
                                XD2Connect = Device2.XD.Open();
                            }
                        }
                        else
                            Thread.Sleep(20);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.ToString());
                        Thread.Sleep(1000);
                    }
                }
                if (opened)
                {
                    do
                    {
                        Device2.XD.Close();
                        Device2.XD.Dispose();
                        Thread.Sleep(20);
                    } while (Device2.XD.IsOpen());
                }
                else
                {
                    Device2.XD = new XinJESerial();
                    XD2Connect = new OperateResult();
                }
                Running2 = false;
                Ctrl2[4] = 0;
            });

        }
        private async void YAMAHA1Run()
        {
            if (ID == 0)
                Device1.YAMAHAExtIn = 0;
            else
                Device1.YAMAHAExtIn = -1;
            await Task.Run(() => { while (Device1.YAMAHAExtIn == -1) Thread.Sleep(20); });
            await Task.Run(() =>
            {
                while (Ctrl1[5] == 1)
                {
                    if(Device1.YAMAHAExtIn==0)
                    {
                        try

                        {
                            Device1.YAMAHA = new ITcpClient();
                            Device1.YAMAHA.ip = _Parm.CurParmCMU1.IP;
                            Device1.YAMAHA.port = _Parm.CurParmCMU1.Port;
                            Device1.YAMAHA.AutoConnect = true;
                            Device1.YAMAHA.Close();
                            Device1.YAMAHA._Connect();
                            Device1.YAMAHAExtIn = 10;
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex);
                            return;
                        }
                    }
                    else if(Device1.YAMAHAExtIn == 10)
                    {
                        try
                        {
                            if (Device1.YAMAHA.Connect)
                            {
                                
                                try
                                {
                                    bool RecOK = false;
                                    string s = Device1.YAMAHA.Rec();
                                    if(s!="")
                                    {
                                        string[] sArray = s.Split(";");
                                        if(sArray[0]=="IOCMD" && ((sArray[1].Length== _Parm.CurParmCMU1.SedLen * 16 && _Parm.CurParmCMU2.CurType == "D") || (sArray[1].Length == _Parm.CurParmCMU1.SedLen  && _Parm.CurParmCMU2.CurType != "D")))
                                        {
                                            for (int i = 0; i < sArray[1].Length; i++)
                                            {
                                                bool[] boolArray =new bool[sArray[1].Length];
                                                boolArray[i] = sArray[1].Substring(i,1)=="1";
                                                SedData1 = new ArrayList(boolArray);
                                            }
                                            RecOK = true;
                                        }
                                    }


                                    if (RecData1 != null)
                                    {
                                        if (RecData1.Count > 0 && RecOK)
                                        {
                                            string sedstr = "IOCMD;"+ _Parm.CurParmCMU1.SedFirstAdd+";" + (_Parm.CurParmCMU2.CurType == "D"? _Parm.CurParmCMU1.SedLen*16:_Parm.CurParmCMU1.SedLen).ToString() +":" + _Parm.CurParmCMU1.RecFirstAdd + ";" + (_Parm.CurParmCMU2.CurType == "D" ? _Parm.CurParmCMU1.RecLen * 16 : _Parm.CurParmCMU1.RecLen).ToString() + ";";
                                            
                                            for (int i = 0; i < ((bool[])RecData1.ToArray(typeof(bool))).Length; i++)
                                            {
                                                sedstr += ((bool[])RecData1.ToArray(typeof(bool)))[i] ? "1" : "0" ;
                                            }
                                            Device1.YAMAHA.Send(sedstr + "\r\n");
     
                                        }
                                    }

                                    if (_Parm.CurParmCMU1.CurType != "D" && _Parm.CurParmCMU2.CurType == "D")
                                        Running1 = SedData1.Count == _Parm.CurParmCMU1.SedLen * 16;
                                    else
                                        Running1 = SedData1.Count == _Parm.CurParmCMU1.SedLen;
                                }
                                catch (Exception ex)
                                {
                                    Trace.WriteLine(ex.ToString());
                                    Running1 = false;
                                }
                                Thread.Sleep(20);
                            }
                            else
                            {
                                Running1 = false;
                                Thread.Sleep(1000);
                            }
                               
                            if (_Parm.Enable && Device1.YAMAHA.Connect != _Parm.CurParmCMU1.StatusConnect)
                                _Parm.CurParmCMU1.StatusConnect = Device1.YAMAHA.Connect;
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex);
                            Thread.Sleep(1000);
                        }
                    }
                }
            });
            Device1.YAMAHA.Close();
            Running1 = false;
            Ctrl1[5] = 0;
        }
        private async void YAMAHA2Run()
        {
            if (ID == 0)
                Device2.YAMAHAExtIn = 0;
            else
                Device2.YAMAHAExtIn = -1;
            await Task.Run(() => { while (Device2.YAMAHAExtIn == -1) Thread.Sleep(20); });
            await Task.Run(() =>
            {
                while (Ctrl2[5] == 1)
                {
                    if (Device2.YAMAHAExtIn == 0)
                    {
                        try
                        {
                            Device2.YAMAHA = new ITcpClient();
                            Device2.YAMAHA.ip = _Parm.CurParmCMU2.IP;
                            Device2.YAMAHA.port = _Parm.CurParmCMU2.Port;
                            Device2.YAMAHA.AutoConnect = true;
                            Device2.YAMAHA.Close();
                            Device2.YAMAHA._Connect();
                            Device2.YAMAHAExtIn = 10;
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex);
                            return;
                        }
                    }
                    else if (Device2.YAMAHAExtIn == 10)
                    {
                        try
                        {

                            if (Device2.YAMAHA.Connect)
                            {
                                try
                                {
                                    bool RecOK = false;
                                    string s = Device2.YAMAHA.Rec();
                                    if (s != "")
                                    {
                                        string[] sArray = s.Split(";");

                                        if (sArray[0] == "IOCMD")
                                        {
                                            if (sArray.Length == 2)
                                            {
                                                sArray[1] = sArray[1].Replace("\r\n", "");
                                                if ((sArray[1].Length == _Parm.CurParmCMU1.RecLen * 16 && _Parm.CurParmCMU1.CurType == "D") || (sArray[1].Length == _Parm.CurParmCMU1.RecLen && _Parm.CurParmCMU1.CurType != "D"))
                                                {
                                                    bool[] boolArray = new bool[sArray[1].Length];
                                                    for (int i = 0; i < sArray[1].Length; i++)
                                                    {

                                                        boolArray[i] = (sArray[1].Substring(i, 1).ToString() == "1");
                                                        RecData1 = new ArrayList(boolArray);
                                                    }
                                                }
                                            }

                                            RecOK = true;
                                        }

                                    }


                                    if (SedData1 != null)
                                    {
                                        if (SedData1.Count > 0 && RecOK)
                                        {
                                            string sedstr = "IOCMD;" + _Parm.CurParmCMU2.SedFirstAdd + ";" + (_Parm.CurParmCMU1.CurType == "D" ? _Parm.CurParmCMU1.RecLen * 16 : _Parm.CurParmCMU1.RecLen).ToString() + ";" + _Parm.CurParmCMU2.RecFirstAdd + ";" + (_Parm.CurParmCMU1.CurType == "D" ? _Parm.CurParmCMU1.SedLen * 16 : _Parm.CurParmCMU1.SedLen).ToString() + ";";

                                            for (int i = 0; i < ((bool[])SedData1.ToArray(typeof(bool))).Length; i++)
                                            {
                                                sedstr += ((bool[])SedData1.ToArray(typeof(bool)))[i] ? "1" : "0";
                                            }
                                            Device2.YAMAHA.Send(sedstr + "\r\n");
                                        }
                                    }

                                    if (_Parm.CurParmCMU2.CurType != "D" && _Parm.CurParmCMU1.CurType == "D")
                                        Running2 = RecData1.Count == _Parm.CurParmCMU1.SedLen * 16;
                                    else
                                        Running2 = RecData1.Count == _Parm.CurParmCMU1.SedLen;
                                }
                                catch (Exception ex)
                                {
                                    Trace.WriteLine(ex);
                                    Running2 = false;
                                }
                                Thread.Sleep(20);
                            }
                            else
                            {
                                Running2 = false;
                                Thread.Sleep(1000);
                            }

                            if (_Parm.Enable && Device2.YAMAHA.Connect != _Parm.CurParmCMU2.StatusConnect)
                            {
                                _Parm.CurParmCMU2.StatusConnect = Device2.YAMAHA.Connect;

                            }

                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex);
                            Thread.Sleep(1000);
                        }
                    }
                }
            });
            Device2.YAMAHA.Close();
            Running2 = false;
            Ctrl2[5] = 0;
        }
        private async void Epson1Run()
        {
            if (ID == 0)
                Device1.EpsonExtIn = 0;
            else
                Device1.EpsonExtIn = -1;
            await Task.Run(() => { while (Device1.EpsonExtIn == -1) Thread.Sleep(20); });
            await Task.Run(() =>
            {
                while (Ctrl1[6] == 1)
                {
                    if (Device1.EpsonExtIn == 0)
                    {
                        try
                        {
                            Device1.Epson = new ITcpClient();
                            Device1.Epson.ip = _Parm.CurParmCMU1.IP;
                            Device1.Epson.port = _Parm.CurParmCMU1.Port;
                            Device1.Epson.AutoConnect = true;
                            Device1.Epson.Close();
                            Device1.Epson._Connect();
                            Device1.EpsonExtIn = 10;
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex);
                            return;
                        }
                    }
                    else if(Device1.EpsonExtIn == 10)
                    {
                        try
                        {
                            
                            if (Device1.Epson.Connect)
                            {
                                try
                                {
                                    bool RecOK = false;
                                    string s = Device1.Epson.Rec();
                                    if (s != "")
                                    {
                                        string[] sArray = s.Split(";");

                                        if (sArray[0] == "IOCMD" )
                                        {
                                            if (sArray.Length == 2)
                                            {
                                                sArray[1] = sArray[1].Replace("\r\n", "");
                                                if ((sArray[1].Length == _Parm.CurParmCMU1.SedLen * 16 && _Parm.CurParmCMU2.CurType == "D") || (sArray[1].Length == _Parm.CurParmCMU1.SedLen && _Parm.CurParmCMU2.CurType != "D"))
                                                {
                                                    bool[] boolArray = new bool[sArray[1].Length];
                                                    for (int i = 0; i < sArray[1].Length; i++)
                                                    {

                                                        boolArray[i] = (sArray[1].Substring(i, 1).ToString() == "1");
                                                        SedData1 = new ArrayList(boolArray);
                                                    }
                                                }
                                            }

                                            RecOK = true;
                                        }
                                        
                                    }


                                    if (RecData1 != null)
                                    {
                                        if (RecData1.Count > 0 && RecOK)
                                        {
                                            string sedstr = "IOCMD;" + _Parm.CurParmCMU1.SedFirstAdd + ";" + (_Parm.CurParmCMU2.CurType == "D" ? _Parm.CurParmCMU1.SedLen * 16 : _Parm.CurParmCMU1.SedLen).ToString() + ";" + _Parm.CurParmCMU1.RecFirstAdd + ";" + (_Parm.CurParmCMU2.CurType == "D" ? _Parm.CurParmCMU1.RecLen * 16 : _Parm.CurParmCMU1.RecLen).ToString() + ";";

                                            for (int i = 0; i < ((bool[])RecData1.ToArray(typeof(bool))).Length; i++)
                                            {
                                                sedstr += ((bool[])RecData1.ToArray(typeof(bool)))[i] ? "1" : "0";
                                            }
                                            Device1.Epson.Send(sedstr + "\r\n");

                                        }
                                    }

                                    if (_Parm.CurParmCMU1.CurType != "D" && _Parm.CurParmCMU2.CurType == "D")
                                        Running1 = SedData1.Count == _Parm.CurParmCMU1.SedLen * 16;
                                    else
                                        Running1 = SedData1.Count == _Parm.CurParmCMU1.SedLen;
                                }
                                catch (Exception ex)
                                {
                                    Trace.WriteLine(ex);
                                    Running1 = false;
                                }
                                Thread.Sleep(20);
                            }
                            else
                            {
                                Running1 = false;
                                Thread.Sleep(1000);
                            }
                                
                            if (_Parm.Enable && Device1.Epson.Connect != _Parm.CurParmCMU1.StatusConnect)
                            {
                                _Parm.CurParmCMU1.StatusConnect = Device1.Epson.Connect;

                            }

                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex);
                            Thread.Sleep(1000);
                        }
                    }
                }
            });
            Device1.Epson.Close();
            Running1 = false;
            Ctrl1[6] = 0;
        }

        private async void Epson2Run()
        {
            if (ID == 0)
                Device2.EpsonExtIn = 0;
            else
                Device2.EpsonExtIn = -1;
            await Task.Run(() => { while (Device2.EpsonExtIn == -1) Thread.Sleep(20); });
            await Task.Run(() =>
            {
                while (Ctrl2[6] == 1)
                {
                    if (Device2.EpsonExtIn == 0)
                    {
                        try
                        {
                            Device2.Epson = new ITcpClient();
                            Device2.Epson.ip = _Parm.CurParmCMU2.IP;
                            Device2.Epson.port = _Parm.CurParmCMU2.Port;
                            Device2.Epson.AutoConnect = true;
                            Device2.Epson.Close();
                            Device2.Epson._Connect();
                            Device2.EpsonExtIn = 10;
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex);
                            return;
                        }
                    }
                    else if (Device2.EpsonExtIn == 10)
                    {
                        try
                        {

                            if (Device2.Epson.Connect)
                            {
                                try
                                {
                                    bool RecOK = false;
                                    string s = Device2.Epson.Rec();
                                    if (s != "")
                                    {
                                        string[] sArray = s.Split(";");

                                        if (sArray[0] == "IOCMD")
                                        {
                                            if (sArray.Length == 2)
                                            {
                                                sArray[1] = sArray[1].Replace("\r\n", "");
                                                if ((sArray[1].Length == _Parm.CurParmCMU1.RecLen * 16 && _Parm.CurParmCMU1.CurType == "D") || (sArray[1].Length == _Parm.CurParmCMU1.RecLen && _Parm.CurParmCMU1.CurType != "D"))
                                                {
                                                    bool[] boolArray = new bool[sArray[1].Length];
                                                    for (int i = 0; i < sArray[1].Length; i++)
                                                    {

                                                        boolArray[i] = (sArray[1].Substring(i, 1).ToString() == "1");
                                                        RecData1 = new ArrayList(boolArray);
                                                    }
                                                }
                                            }

                                            RecOK = true;
                                        }

                                    }


                                    if (SedData1 != null)
                                    {
                                        if (SedData1.Count > 0 && RecOK)
                                        {
                                            string sedstr = "IOCMD;" + _Parm.CurParmCMU2.SedFirstAdd + ";" + (_Parm.CurParmCMU1.CurType == "D" ? _Parm.CurParmCMU1.RecLen * 16 : _Parm.CurParmCMU1.RecLen).ToString() + ";" + _Parm.CurParmCMU2.RecFirstAdd + ";" + (_Parm.CurParmCMU1.CurType == "D" ? _Parm.CurParmCMU1.SedLen * 16 : _Parm.CurParmCMU1.SedLen).ToString() + ";";

                                            for (int i = 0; i < ((bool[])SedData1.ToArray(typeof(bool))).Length; i++)
                                            {
                                                sedstr += ((bool[])SedData1.ToArray(typeof(bool)))[i] ? "1" : "0" ;
                                            }
                                            Device2.Epson.Send(sedstr + "\r\n");

                                        }
                                    }

                                    if (_Parm.CurParmCMU2.CurType != "D" && _Parm.CurParmCMU1.CurType == "D")
                                        Running2 = RecData1.Count == _Parm.CurParmCMU1.SedLen * 16;
                                    else
                                        Running2 = RecData1.Count == _Parm.CurParmCMU1.SedLen;
                                }
                                catch (Exception ex)
                                {
                                    Trace.WriteLine(ex);
                                    Running2 = false;
                                }
                                Thread.Sleep(20);
                            }
                            else
                            {
                                Running2 = false;
                                Thread.Sleep(1000);
                            }

                            if (_Parm.Enable && Device2.Epson.Connect != _Parm.CurParmCMU2.StatusConnect)
                            {
                                _Parm.CurParmCMU2.StatusConnect = Device2.Epson.Connect;

                            }

                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex);
                            Thread.Sleep(1000);
                        }
                    }
                }
            });
            Device2.Epson.Close();
            Running2 = false;
            Ctrl2[6] = 0;
        }

        private int Check()
        {

            if ((_Parm.CurParmCMU1.DeviceName == devicename[1] || _Parm.CurParmCMU1.DeviceName == devicename[2]) && (_Parm.CurParmCMU1.DeviceName == _Parm.CurParmCMU2.DeviceName && _Parm.CurParmCMU1.IP == _Parm.CurParmCMU2.IP && _Parm.CurParmCMU1.Port == _Parm.CurParmCMU2.Port && _Parm.CurParmCMU1.StationID == _Parm.CurParmCMU2.StationID))
                return 1;
            else if ((_Parm.CurParmCMU1.DeviceName == devicename[3] || _Parm.CurParmCMU1.DeviceName == devicename[4]) && (_Parm.CurParmCMU1.DeviceName == _Parm.CurParmCMU2.DeviceName && _Parm.CurParmCMU1.CurSerial == _Parm.CurParmCMU2.CurSerial && _Parm.CurParmCMU1.CurBaudRate == _Parm.CurParmCMU2.CurBaudRate && _Parm.CurParmCMU1.CurDataBits == _Parm.CurParmCMU2.CurDataBits && _Parm.CurParmCMU1.CurParity == _Parm.CurParmCMU2.CurParity && _Parm.CurParmCMU1.CurStopBits == _Parm.CurParmCMU2.CurStopBits && _Parm.CurParmCMU1.StationID == _Parm.CurParmCMU2.StationID))
                return 1;
            else if ((_Parm.CurParmCMU1.DeviceName == _Parm.CurParmCMU2.DeviceName && _Parm.CurParmCMU1.IP == _Parm.CurParmCMU2.IP && _Parm.CurParmCMU1.Port == _Parm.CurParmCMU2.Port))
            {
                if (_Parm.CurParmCMU1.DeviceName == devicename[0])
                    return 1;
                else
                    return 2;
            }
            return 0;

        }


        //禁止变更devicename元素顺序
        public string[] devicename = { "三菱Fx5U PLC", "汇川H3U PLC", "汇川H5U PLC", "信捷XC PLC", "信捷XD PLC", "YAMAHA机械手", "Epson机械手" };
        public string[] baudRate = { "4800", "9600", "14400", "19200", "38400", "56000", "57600", "115200" };
        public string[] parity = { "None", "Odd", "Even" };
        public int[] databits = { 8, 7 };
        public int[] stopbits = { 1, 2 };
        public string[] type = { "M", "D" };
        private List<string> serialport = new List<string>();

        public void XmlDelete()
        {
            MFile.DeleteFile(xmlPath, xmlName);
        }
        private void XmlLoad()
        {
            xmlPath = MFile.path4 + @"Parm\";
            xmlName = "Parm" + ID + ".xml";
            try
            {

                _Parm = (Parm)xml.deserialize_from_xml(xmlPath, xmlName, typeof(Parm));
                //xml.serialize_to_xml(xmlPath, xmlName, _Parm);

            }
            catch (Exception)
            {
                _Parm = new Parm();
                xml.serialize_to_xml(xmlPath, xmlName, _Parm);
            }
            ParmCMUInit(_Parm.ParmCMU1);
            ParmCMUInit(_Parm.ParmCMU2);


        }
        private void ParmCMUInit(List<ParmCMU> listparmCMU)
        {
            for (int i = 0; i < devicename.Length; i++)
            {


                if (!listparmCMU.Exists((ParmCMU s) => s.DeviceName == devicename[i] ? true : false))
                {
                    listparmCMU.Add(new ParmCMU() { DeviceName = devicename[i] });
                }
                if (listparmCMU[i].CurBaudRate == "")
                    listparmCMU[i].CurBaudRate = baudRate[0];
                if (listparmCMU[i].CurParity == "")
                    listparmCMU[i].CurParity = parity[0];
                if (listparmCMU[i].CurDataBits == 0)
                    listparmCMU[i].CurDataBits = databits[0];
                if (listparmCMU[i].CurStopBits == 0)
                    listparmCMU[i].CurStopBits = stopbits[0];
                for (int j = 0; j < baudRate.Length; j++)
                {
                    if (!listparmCMU[i].BaudRate.Exists((string s) => s == baudRate[j] ? true : false))
                        listparmCMU[i].BaudRate.Add(baudRate[j]);
                }
                if (listparmCMU[i].Serial.Count == 0)
                {
                    listparmCMU[i].Serial = serialport;
                    listparmCMU[i].CurSerial = serialport[0];
                }

                for (int j = 0; j < parity.Length; j++)
                {
                    if (!listparmCMU[i].Parity.Exists((string s) => s == parity[j] ? true : false))
                        listparmCMU[i].Parity.Add(parity[j]);
                }
                for (int j = 0; j < databits.Length; j++)
                {
                    if (!listparmCMU[i].DataBits.Exists((int s) => s == databits[j] ? true : false))
                        listparmCMU[i].DataBits.Add(databits[j]);
                }
                for (int j = 0; j < stopbits.Length; j++)
                {
                    if (!listparmCMU[i].StopBits.Exists((int s) => s == stopbits[j] ? true : false))
                        listparmCMU[i].StopBits.Add(stopbits[j]);
                }

                if (i < 5)
                {
                    for (int j = 0; j < type.Length; j++)
                    {
                        if (!listparmCMU[i].Type.Exists((string s) => s == type[j] ? true : false))
                            listparmCMU[i].Type.Add(type[j]);
                    }
                    if (listparmCMU[i].CurType == "")
                        listparmCMU[i].CurType = type[0];
                }
                else if (i == 5)
                {
                    if (!listparmCMU[i].Type.Exists((string s) => s == "MO" ? true : false))
                        listparmCMU[i].Type.Add("MO");
                    if (listparmCMU[i].CurType == "")
                        listparmCMU[i].CurType = "MO";
                }
                else if (i == 6)
                {
                    if (!listparmCMU[i].Type.Exists((string s) => s == "Memory" ? true : false))
                        listparmCMU[i].Type.Add("Memory");
                    if (listparmCMU[i].CurType == "")
                        listparmCMU[i].CurType = "Memory";
                }
            }
        }
        private void GridVisible1(object sender, EventArgs e)
        {
            ParmCMU parmCMU = (ParmCMU)sender;
            if (parmCMU.DeviceName == devicename[0] || parmCMU.DeviceName == devicename[1] || parmCMU.DeviceName == devicename[2] || parmCMU.DeviceName == devicename[5] || parmCMU.DeviceName == devicename[6])
                Visible1 = 1;
            else if (parmCMU.DeviceName == devicename[3] || parmCMU.DeviceName == devicename[4])
                Visible1 = 2;
            else
                Visible1 = 0;
            StationVisible1 = parmCMU.DeviceName == devicename[5] || parmCMU.DeviceName == devicename[6] || parmCMU.DeviceName == devicename[0] ? 0 : 1;
        }
        private void GridVisible2(object sender, EventArgs e)
        {
            ParmCMU parmCMU = (ParmCMU)sender;
            if (parmCMU.DeviceName == devicename[0] || parmCMU.DeviceName == devicename[1] || parmCMU.DeviceName == devicename[2] || parmCMU.DeviceName == devicename[5] || parmCMU.DeviceName == devicename[6])
                Visible2 = 1;
            else if (parmCMU.DeviceName == devicename[3] || parmCMU.DeviceName == devicename[4])
                Visible2 = 2;
            else
                Visible2 = 0;
            StationVisible2 = parmCMU.DeviceName == devicename[5] || parmCMU.DeviceName == devicename[6] || parmCMU.DeviceName == devicename[0] ? 0 : 1;
        }
        private void XmlSave(object sender, PropertyChangedEventArgs e)
        {
            xml.serialize_to_xml(xmlPath, xmlName, _Parm);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
             System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
