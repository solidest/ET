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
    /// InputDlg.xaml 的交互逻辑
    /// </summary>
    public partial class InputDlg : Window
    {
        public InputDlg()
        {
           
            InitializeComponent();
            input.Focus();
        }

        public ET.Service.ETService.ValidateStringCallBack Validate { get; set; }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (Validate != null)
            {
                var ret = Validate.Invoke(this.input.Text);
                if (ret == String.Empty)
                    this.DialogResult = true;
                else
                    this.tip.Text = ret;
            }
            else if (input.Text != string.Empty)
                this.DialogResult = true;
            else
                this.tip.Text = "输入值不能为空，请重新输入!";
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void input_TextChanged(object sender, TextChangedEventArgs e)
        {
            tip.Text = "";
        }
    }
}
