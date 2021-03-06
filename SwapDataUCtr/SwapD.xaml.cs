using SwapDataUCtr.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace SwapDataUCtr
{
    /// <summary>
    /// SwapD.xaml 的交互逻辑
    /// </summary>
    public partial class SwapD : UserControl
    {
        public SwapDVM swapDVM { get; set; } = new SwapDVM();
        public SwapD()
        {
            InitializeComponent();           
            DataContext = this;
        }
        public SwapD(int id)
        {
            InitializeComponent();
            DataContext = this;
            swapDVM.ID = id;
            swapDVM.InitAndRun();
        }
    }
}
