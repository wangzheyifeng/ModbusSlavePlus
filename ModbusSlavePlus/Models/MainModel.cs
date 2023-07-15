using Modbus.Data;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusSlavePlus.Models
{
    public class MainModel : INotifyPropertyChanged
    {
        private Param _zeroValue;
        public Param ZeroValue 
        { 
            get { return _zeroValue; }
            set
            {
                _zeroValue = value;
            }
        }
        public SerialPort sp;
        ModbusSlave slave;
        public ObservableCollection<Param> Params { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void UpdateValue(string name)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public MainModel()
        {
            ZeroValue = new Param { Value = "1"};
            List<Param> list = new List<Param>()
            {
                new Param{ Address = 0, Notify =  UpdateHoldingRegisters },
                new Param{ Address = 1, Notify =  UpdateHoldingRegisters },
                new Param{ Address = 2, Notify =  UpdateHoldingRegisters },
                new Param{ Address = 3, Notify =  UpdateHoldingRegisters },
                new Param{ Address = 4, Notify =  UpdateHoldingRegisters },
            };
            ObservableCollection<Param> ps = new ObservableCollection<Param>();
            foreach (var p in list)
            {
                ps.Add(p);
            }
            
            sp = new SerialPort();
            sp.PortName = "COM2";
            sp.BaudRate = 9600;
            sp.Open();
            slave = ModbusSerialSlave.CreateRtu(1, sp);
            slave.DataStore = DataStoreFactory.CreateDefaultDataStore();
            slave.DataStore.DataStoreWrittenTo += DataStore_DataStoreWrittenTo;

            foreach (Param param in ps)
            {
                param.Value = slave.DataStore.HoldingRegisters[param.Address + 1].ToString();
            }
            Task.Run(() =>
            {
                slave.Listen();
            });
            Params = ps;
            Params.CollectionChanged += Ps_CollectionChanged;
        }

        private void Ps_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            int newIndex = e.NewStartingIndex;
        }

        public void UpdateHoldingRegisters(Param param)
        {
            slave.DataStore.HoldingRegisters[param.Address + 1] = ushort.Parse(param.Value);
        }

        private void DataStore_DataStoreWrittenTo(object sender, DataStoreEventArgs e)
        {
            foreach (var param in Params)
            {
                param.Value = slave.DataStore.HoldingRegisters[param.Address + 1].ToString();
            }
            UpdateValue("Params");
        }
    }
}
