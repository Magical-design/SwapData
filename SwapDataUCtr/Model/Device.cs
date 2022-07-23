using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.Profinet.Melsec;
using HslCommunication.Profinet.XINJE;
using HslCommunication.Profinet.Inovance;
using System.IO.Ports;
using MLib.NET;

namespace SwapDataUCtr.Model
{
    public class Device
    {
        public  MelsecMcNet Fx5U=new MelsecMcNet() ;
        public  InovanceTcpNet H5U=new InovanceTcpNet();
        public  InovanceTcpNet H3U=new InovanceTcpNet();
        public  XinJESerial XC = new XinJESerial();
        public  XinJESerial XD = new XinJESerial();
        public  ITcpClient YAMAHA =new ITcpClient();
        public  ITcpClient Epson = new ITcpClient();
        public int Fx5UExtIn = 0;
        public int H5UExtIn = 0;
        public int H3UExtIn = 0;
        public int XCExtIn = 0;
        public int XDExtIn = 0;
        public int YAMAHAExtIn = 0;
        public int EpsonExtIn = 0;

        public Device()
        {

        }     
    }
}
