using MLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SwapData.ViewModel
{
    public class SettingVM : ViewBase
    {
        private string iniPath = MFile.path4 + "Parameter.ini";
        public SettingVM()
        {
            IniLoad();
            
        }

        public static bool boot;
        public bool Boot
        {
            get { return boot; }
            set
            {
                
                boot = value;
                Ini.WriteIniData("Setting", "开机启动", boot.ToString(), iniPath);
                OnPropertyChanged("Boot");
            }
        }
        public static bool exitCheck;
        public bool ExitCheck
        {
            get { return exitCheck; }
            set
            {
                exitCheck = value;
                Ini.WriteIniData("Setting", "退出验证", exitCheck.ToString(), iniPath);
                OnPropertyChanged("ExitCheck");
            }
        }
        public void IniLoad()
        {
            Ini.IniFileCreate(iniPath);
            Boot = Convert.ToBoolean(Ini.ReadIniData("Setting", "开机启动", "False", iniPath));
            ExitCheck = Convert.ToBoolean(Ini.ReadIniData("Setting", "退出验证", "False", iniPath));
        }
        public DelegateCommand btBootClick
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if(AutoStart.SetMeStart(Boot))
                    {
                         MessageBoxHelper.PrepToCenterMessageBoxOnForm(Application.Current.MainWindow);
                         MessageBoxResult result = MessageBox.Show("操作成功");
                    }
                    else
                    {
                        MessageBoxHelper.PrepToCenterMessageBoxOnForm(Application.Current.MainWindow);
                        MessageBoxResult result = MessageBox.Show("需管理员权限运行程序，是否重启为管理员权限！", "退出前确认", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        Boot = false;
                        if (result == MessageBoxResult.Yes)
                            Permission();
                        
                    }
                }
              );
            }
        }
        public  void Permission()
        {
            /**
            * 当前用户是管理员的时候，直接启动应用程序
            * 如果不是管理员，则使用启动对象启动程序，以确保使用管理员身份运行
            */
            //获得当前登录的Windows用户标示
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            //判断当前登录用户是否为管理员
            if (principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                //如果是管理员，则直接运行
                //Run(new LoginWindow());
            }
            else if (!System.Diagnostics.Debugger.IsAttached)
            {
                //创建启动对象
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.UseShellExecute = true;
                startInfo.WorkingDirectory = Environment.CurrentDirectory;
                startInfo.FileName = Process.GetCurrentProcess().MainModule.FileName;
                //设置启动动作,确保以管理员身份运行
                startInfo.Verb = "runas";
                try
                {
                    System.Diagnostics.Process.Start(startInfo);
                    
                    Application.Current.Shutdown();
                    Environment.Exit(0);
                }
                catch
                {
                    //return;
                    MessageBox.Show("操作失败！");
                }
                //退出

            }
        }
    }
}
