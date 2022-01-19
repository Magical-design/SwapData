using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SwapDataUCtr
{
    public class Parm : INotifyPropertyChanged
    {
        

        #region 变量
        private bool mEnable= false;
        public bool Enable

        {
            get { return mEnable; }
            set
            {
                mEnable = value;
                OnPropertyChanged("Enable");
            }
        }
        private ParmCMU mCurParmCMU1=new ParmCMU();
        public ParmCMU CurParmCMU1
        {
            get { return mCurParmCMU1; }   
            set
            {
                mCurParmCMU1 = value;
                OnPropertyChanged("CurParmCMU1");
                CurParmCMU1Changed?.Invoke(value, null);
            }
        }
        
        private List<ParmCMU> mParmCMU1=new List<ParmCMU>();
        public List<ParmCMU> ParmCMU1
        {
            get
            {
                return mParmCMU1;

            }
            set
            {
                mParmCMU1 = value;
                OnPropertyChanged("ParmCMU1");
            }
        }

        private ParmCMU mCurParmCMU2= new ParmCMU();
        public ParmCMU CurParmCMU2
        {
            get { return mCurParmCMU2; }
            set
            {
                mCurParmCMU2 = value;
                OnPropertyChanged("CurParmCMU2");
            }
        }


        private List<ParmCMU> mParmCMU2 = new List<ParmCMU>();
        public List<ParmCMU> ParmCMU2
        {
            get
            {
                return mParmCMU2;

            }
            set
            {
                mParmCMU2 = value;
            }
        }
        #endregion
        public Parm()
        {

           
        }
        #region 事件
        public event EventHandler CurParmCMU1Changed;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class ParmCMU : INotifyPropertyChanged
    {

        #region 变量
        private bool mStatusConnect = false;
        public bool StatusConnect

        {
            get { return mStatusConnect; }
            set
            {
                mStatusConnect = value;
                OnPropertyChanged("StatusConnect");
            }
        }

        private string mDeviceName = "";
        public string DeviceName
        {
            get
            {
                return mDeviceName;
            }
            set
            {
                mDeviceName = value;
                OnPropertyChanged("DeviceName");
            }
        }
        private string mIP = "127.0.0.1";
        public string IP
        {
            get
            {
                return mIP;
            }
            set
            {
                mIP = value;
                OnPropertyChanged("IP");
            }
        }
        private int mPort = 0;
        public int Port
        {
            get { return mPort; }
            set
            {
                mPort = value;
                OnPropertyChanged("Port");
            }
        }

        private string mCurSerial = "COM1";
        public string CurSerial
        {
            get
            {
                return mCurSerial;
            }
            set
            {
                mCurSerial = value;
                OnPropertyChanged("CurSerial");
            }
        }
        private List<string> mSerial = new List<string>();
        public List<string> Serial
        {
            get { return mSerial; }
            set
            {
                mSerial = value;
                OnPropertyChanged("Serial");
            }
        }
        private string mCurBaudRate = "";
        public string CurBaudRate
        {
            get
            {
                return mCurBaudRate;
            }
            set
            {
                mCurBaudRate = value;
                OnPropertyChanged("CurBaudRate");
            }
        }
        private List<string> mBaudRate = new List<string>();
        public List<string> BaudRate
        {
            get { return mBaudRate; }
            set
            {
                mBaudRate = value;
                OnPropertyChanged("BaudRate");
            }
        }
        private int mCurDataBits = 8;
        public int CurDataBits
        {
            get
            {
                return mCurDataBits;
            }
            set
            {
                mCurDataBits = value;
                OnPropertyChanged("CurDataBits");
            }
        }

        private List<int> mDataBits = new List<int>();
        public List<int> DataBits
        {
            get { return mDataBits; }
            set
            {
                mDataBits = value;
                OnPropertyChanged("DataBits");
            }
        }
        private int mCurStopBits = 1;
        public int CurStopBits
        {
            get
            {
                return mCurStopBits;
            }
            set
            {
                mCurStopBits = value;
                OnPropertyChanged("CurStopBits");
            }
        }
        private List<int> mStopBits = new List<int>();
        public List<int> StopBits
        {
            get { return mStopBits; }
            set
            {
                mStopBits = value;
                OnPropertyChanged("StopBits");
            }
        }
        private string mCurParity = "";
        public string CurParity
        {
            get
            {
                return mCurParity;
            }
            set
            {
                mCurParity = value;
                OnPropertyChanged("CurParity");
            }
        }
        private List<string> mParity = new List<string>();
        public List<string> Parity
        {
            get { return mParity; }
            set
            {
                
                mParity = value;
                OnPropertyChanged("Parity");
            }
        }
        private string mCurType = "";
        public string CurType
        {
            get
            {
                return mCurType;
            }
            set
            {
                mCurType = value;
                OnPropertyChanged("CurType");
            }
        }
        private List<string> mType = new List<string>();
        public List<string> Type
        {
            get { return mType; }
            set
            {
                mType = value;
                OnPropertyChanged("Type");
            }
        }

        private int mFirstAdd = 0;
        public int FirstAdd
        {
            get
            {
                return mFirstAdd;
            }
            set
            {
                mFirstAdd = value;
                OnPropertyChanged("FirstAdd");
            }
        }
        private int mLen = 0;
        public int Len
        {
            get
            {
                return mLen;
            }
            set
            {
                mLen = value;
                OnPropertyChanged("Len");
            }
        }

        #endregion
        public  event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
    }


}
