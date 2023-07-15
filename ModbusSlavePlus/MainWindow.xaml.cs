using Modbus.Data;
using Modbus.Device;
using ModbusSlavePlus.Models;
using ModbusSlavePlus.Windows;
using System;
using System.Collections.Generic;
using System.IO.Ports;
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

namespace ModbusSlavePlus
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainModel context;
        public MainWindow()
        {
            InitializeComponent();
            context = (MainModel)DataContext;
        }
        /// <summary>
        /// 打开连接窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ConnectWindow connect = new ConnectWindow();
            connect.Owner = this;
            var result = connect.ShowDialog();
            if (result == true) 
            {
                ConnectionModel data = (ConnectionModel)connect.DataContext;
                context.Sp.PortName = (string)data.SelectedPort.Value;
                context.Sp.BaudRate = (int)data.SelectedBaudRate.Value;
                context.Sp.DataBits = (int)data.SelectedDataBit.Value;
                context.Sp.Parity = (Parity)data.SelectedParity.Value;
                context.Sp.StopBits = (StopBits)data.SelectedStopBits.Value;
                if (!context.Sp.IsOpen)
                {
                    context.Sp.Open();
                    context.Slave = ModbusSerialSlave.CreateRtu(1, context.Sp);
                    context.Slave.DataStore = DataStoreFactory.CreateDefaultDataStore();
                    context.Slave.DataStore.DataStoreWrittenTo += context.DataStore_DataStoreWrittenTo;
                    foreach (Param param in context.Params)
                    {
                        param.Value = context.Slave.DataStore.HoldingRegisters[param.Address + 1].ToString();
                    }
                    Task.Run(() =>
                    {
                        context.Slave.Listen();
                    });
                }
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (context.Sp.IsOpen)
            {
                context.Sp.Close();
            }
        }
    }
}
