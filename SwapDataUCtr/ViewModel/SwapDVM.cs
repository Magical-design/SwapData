using MLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SwapDataUCtr
{
    public class SwapDVM:INotifyPropertyChanged
    {
        private int mVisible = 0;
        public int Visible
        {
            get { return mVisible; }
            set 
            {
                mVisible = value;
                OnPropertyChanged("Visible");
            }
        }
        IXml xml = new IXml();
        public string xmlPath = MFile.path4 + @"Parm\";
        public string xmlName;

        private int mID;
        public int ID
        {
            get { return mID; }
            set 
            {
                mID = value;
                XmlLoad();
            }
        }
        
        private Parm mParm;
        public Parm _Parm
        {
            get { return mParm; }
            set
            { 
                mParm = value;
                
            }
        }

        public SwapDVM()
        {
       
            Init();
            
        }

        public void Init()
        {
            XmlLoad();
            _Parm.PropertyChanged += new PropertyChangedEventHandler(XmlSave);
            _Parm.CurParmCMU1.PropertyChanged += new PropertyChangedEventHandler(XmlSave);
            //_Parm.CurParmCMU2.PropertyChanged += new PropertyChangedEventHandler(XmlSave);
            _Parm.CurParmCMU1Changed += GridVisible;


        }
         
        public string[] devicename = { "三菱Fx5U PLC", "汇川H3U PLC", "汇川H5U PLC", "信捷XC PLC", "信捷XD PLC", "YAMAHA机械手", "Epson机械手" };

        public void XmlLoad()
        {
            xmlPath = MFile.path4 + @"Parm\" ;
            xmlName = "Parm" + ID + ".xml";
            try
            {
                
                _Parm = (Parm)xml.deserialize_from_xml(xmlPath, xmlName, typeof(Parm));
                
                if (!_Parm.ParmCMU1.Exists((ParmCMU s) => s.DeviceName == devicename[0] ? true : false))
                {
                    _Parm.ParmCMU1.Add(new ParmCMU() { DeviceName= devicename[0] });
                }
                if (!_Parm.ParmCMU1.Exists((ParmCMU s) => s.DeviceName == devicename[1] ? true : false))
                {
                    _Parm.ParmCMU1.Add(new ParmCMU() { DeviceName = devicename[1] });
                }
                if (!_Parm.ParmCMU1.Exists((ParmCMU s) => s.DeviceName == devicename[2] ? true : false))
                {
                    _Parm.ParmCMU1.Add(new ParmCMU() { DeviceName = devicename[2] });
                }
                if (!_Parm.ParmCMU1.Exists((ParmCMU s) => s.DeviceName == devicename[3] ? true : false))
                {
                    _Parm.ParmCMU1.Add(new ParmCMU() { DeviceName = devicename[3] });
                }
                if (!_Parm.ParmCMU1.Exists((ParmCMU s) => s.DeviceName == devicename[4] ? true : false))
                {
                    _Parm.ParmCMU1.Add(new ParmCMU() { DeviceName = devicename[4] });
                }

                //xml.serialize_to_xml(xmlPath, xmlName, _Parm);

            }
            catch (Exception)
            {
                _Parm = new Parm();
                xml.serialize_to_xml(xmlPath, xmlName, _Parm);
            }

        }
        public void GridVisible(object sender,EventArgs e)
        {
            ParmCMU parmCMU = (ParmCMU)sender;
            if (parmCMU.DeviceName == devicename[0] || parmCMU.DeviceName == devicename[1] || parmCMU.DeviceName == devicename[2] || parmCMU.DeviceName == devicename[5] || parmCMU.DeviceName == devicename[6])
                Visible = 1;
            else if(parmCMU.DeviceName == devicename[3] || parmCMU.DeviceName == devicename[4])
                Visible = 2;
            else
                Visible = 0;
        }
        public void XmlSave(object sender, PropertyChangedEventArgs e)
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
