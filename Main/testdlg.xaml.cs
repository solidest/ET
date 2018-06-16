using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ET.Main
{
    /// <summary>
    /// testdlg.xaml 的交互逻辑
    /// </summary>
    public partial class testdlg : Window
    {
        public testdlg(ET.Interface.ETPage p)
        {
            InitializeComponent();
            gridMain.Children.Add(p);
        }
    }
}
