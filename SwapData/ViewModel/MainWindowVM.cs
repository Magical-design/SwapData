using MLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SwapData.ViewModel
{
    
    public class MainWindowVM : ViewBase
    {
        public static MainWindow mMainWindow;
        private bool settingPageShow = false;
        public bool SettingPageShow
        {
            get { return settingPageShow; }
            set 
            {
                settingPageShow = value;
                OnPropertyChanged("SettingPageShow");
            }
        }
        private int tabCtrlSelectIndex = 0;
        public int TabCtrlSelectIndex
        {
            get { return tabCtrlSelectIndex; }
            set 
            {
                tabCtrlSelectIndex = value;
                OnPropertyChanged("TabCtrlSelectIndex");
            }
        }

        public DelegateCommand btOpenSettingPageClick
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    OpenSettingPage();
                }
              );
            }
        }
        public static string Password
        {
            get { return (System.DateTime.Now.Hour * System.DateTime.Now.Day).ToString(); }
        }
        private LoginPage loginPage;
        private void OpenSettingPage()
        {
            SettingPageShow = false;
            loginPage=new LoginPage();
            loginPage.myLogin.mLoginVM.PasswordSetVal = Password;
            loginPage.myLogin.mLoginVM.LoginCompleteEvent += MLoginVM_LoginCompleteEvent;
            loginPage.Owner = mMainWindow;//将主窗口设置为AboutWindow的拥有者
            loginPage.WindowStartupLocation = WindowStartupLocation.CenterOwner;//打开的初始位置设置为在Owner的中央
            //WindowManager.ShowDialog("LoginPage",this);
            loginPage.ShowDialog();

        }

        private void MLoginVM_LoginCompleteEvent(object? sender, EventArgs e)
        {
            if ((bool)sender)
            {
                loginPage.Close();
                SettingPageShow = true;
                TabCtrlSelectIndex = 1;
            }
            else
            {
                SettingPageShow = false;
                TabCtrlSelectIndex = 0;
            }
                
        }
        public void Close(System.ComponentModel.CancelEventArgs e)
        {
            if(SettingVM.exitCheck)
            {
                e.Cancel = true;
                loginPage = new LoginPage();
                loginPage.myLogin.mLoginVM.PasswordSetVal = Password;
                loginPage.myLogin.mLoginVM.LoginCompleteEvent += MLoginVM_LoginCompleteEvent1;
                loginPage.Owner = mMainWindow;//将主窗口设置为AboutWindow的拥有者
                loginPage.WindowStartupLocation = WindowStartupLocation.CenterOwner;//打开的初始位置设置为在Owner的中央         
                loginPage.ShowDialog();
            }
            else
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(Application.Current.MainWindow);
                MessageBoxResult result = MessageBox.Show("确定退出软件吗？", "退出前确认", MessageBoxButton.YesNo, MessageBoxImage.Question);
                //不关闭窗口
                if (result == MessageBoxResult.No)
                    e.Cancel = true;
            }
        }

        private void MLoginVM_LoginCompleteEvent1(object? sender, EventArgs e)
        {
            if ((bool)sender)
            {
                loginPage.Close();
                //Application.Current.MainWindow.Close();
                Environment.Exit(0);
            }
        }

    }
}
