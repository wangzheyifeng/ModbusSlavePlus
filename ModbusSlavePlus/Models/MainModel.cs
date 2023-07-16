using Modbus.Data;
using Modbus.Device;
using ModbusSlavePlus.Commands;
using ModbusSlavePlus.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ModbusSlavePlus.Models
{
    public class MainModel : INotifyPropertyChanged
    {
        private MainWindow window;
        public SerialPort Sp;
        public ModbusSlave Slave;
        public byte SlaveId { get; set; } = 1;
        public ushort Address { get; set; } = 0;
        public ushort Quantity { get; set; } = 10;
        private List<Param> _params;
        public List<Param> Params 
        { 
            get { return  _params; }
            set
            {
                _params = value;
                UpdateValue("Params");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void UpdateValue(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private bool isCanExec = true;
        public ICommand ConnectCommand => new BaseCommand(OpenConnectWindow, CanExec);
        public ICommand OpenSetupWindowCommand => new BaseCommand(OpenSetupWindow, CanExec);
        public ICommand CloseCommand => new BaseCommand(CloseConnect, CanExec);
        private bool CanExec(object ps)
        {
            return isCanExec;
        }

        public MainModel(MainWindow window)
        {
            this.window = window;
            Sp = new SerialPort();
            UpdateParamTable();
        }


        public void UpdateHoldingRegisters(Param param)
        {
            Slave.DataStore.HoldingRegisters[param.Address + 1] = ushort.Parse(param.Value);
        }

        public void DataStore_DataStoreWrittenTo(object sender, DataStoreEventArgs e)
        {
            foreach (var param in Params)
            {
                param.Value = Slave.DataStore.HoldingRegisters[param.Address + 1].ToString();
            }
            UpdateValue("Params");
        }

        /// <summary>
        /// 根据地址和数量显示参数列表
        /// </summary>
        private void UpdateParamTable()
        {
            List<Param> ps = new List<Param>();
            for(int i = Address;i<Address + Quantity; i++)
            {
                ps.Add(new Param { Address = (ushort)i, Notify = UpdateHoldingRegisters });
            }
            Params = ps;
        }

        #region command
        public void OpenConnectWindow(object ps)
        {
            ConnectWindow connect = new ConnectWindow();
            connect.Owner = window;
            var result = connect.ShowDialog();
            if (result == true)
            {
                if (!Sp.IsOpen)
                {
                    ConnectionModel data = (ConnectionModel)connect.DataContext;
                    Sp.PortName = (string)data.SelectedPort.Value;
                    Sp.BaudRate = (int)data.SelectedBaudRate.Value;
                    Sp.DataBits = (int)data.SelectedDataBit.Value;
                    Sp.Parity = (Parity)data.SelectedParity.Value;
                    Sp.StopBits = (StopBits)data.SelectedStopBits.Value;
                    Sp.Open();
                    Slave = ModbusSerialSlave.CreateRtu(1, Sp);
                    Slave.DataStore = DataStoreFactory.CreateDefaultDataStore();
                    Slave.DataStore.DataStoreWrittenTo += DataStore_DataStoreWrittenTo;
                    foreach (Param param in Params)
                    {
                        param.Value = Slave.DataStore.HoldingRegisters[param.Address + 1].ToString();
                    }
                    Task.Run(() =>
                    {
                        Slave.Listen();
                    });
                }
            }
        }

        public void CloseConnect(object ps)
        {
            if (Sp.IsOpen)
            {
                Sp.Close();
            }
        }

        public void OpenSetupWindow(object ps)
        {
            SetupWindow w = new SetupWindow(SlaveId, Address, Quantity);
            w.Owner = window;
            var result = w.ShowDialog();
            if (result == true)
            {
                var data = (SetupModel)w.DataContext;
                SlaveId = data.SlaveId;
                Address = data.Address;
                Quantity = data.Quantity;
                if (Slave != null)
                {
                    Slave.UnitId = SlaveId;
                }
                UpdateParamTable();
            }
        }
        #endregion
    }
}