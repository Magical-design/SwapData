using MLib;
using SwapData.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SwapData
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowVM mainWindowVM = new MainWindowVM();
        public MainWindow()
        {
            
            CheckStu();
            InitializeComponent();
            DataContext = mainWindowVM;
            //this.Register<LoginPage>("LoginPage");

            MainWindowVM.mMainWindow = this;
        }
        private void CheckStu()
        {

            string procName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            if ((System.Diagnostics.Process.GetProcessesByName(procName)).GetUpperBound(0) > 0)
            {
                Thread.Sleep(1500);
                if ((System.Diagnostics.Process.GetProcessesByName(procName)).GetUpperBound(0) > 0) 
                {
                    MessageBox.Show("该程序已运行，无法创建更多实例!");
                    Environment.Exit(0);
                }

            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mainWindowVM.Close(e);
        }

    }

}
