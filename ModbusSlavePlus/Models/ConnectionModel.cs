using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusSlavePlus.Models
{
    public class ConnectionModel
    {
        public SelectItem SelectedPort { get; set; }
        public SelectItem SelectedBaudRate { get; set; }
        public SelectItem SelectedStopBits { get; set; }
        public SelectItem SelectedParity { get; set; }
        public SelectItem SelectedDataBit { get; set; }
        public List<SelectItem> PortItems { get; set; }
        public List<SelectItem> BaudRateItems { get; set; }
        public List<SelectItem> DataBitItems { get; set; }
        public List<SelectItem> ParityItems { get; set; }
        public List<SelectItem> StopBitItems { get; set; }
        public ConnectionModel() 
        {
            var portItems = new List<SelectItem>();
            foreach(string p in SerialPort.GetPortNames())
            {
                portItems.Add(new SelectItem(p,p));
            }
            PortItems = portItems;
            BaudRateItems = new List<SelectItem>()
            {
                new SelectItem("300 Baud",300),
                new SelectItem("600 Baud",600),
                new SelectItem("1200 Baud",1200),
                new SelectItem("2400 Baud",2400),
                new SelectItem("4800 Baud",4800),
                new SelectItem("9600 Baud",9600),
                new SelectItem("14400 Baud",14400),
                new SelectItem("19200 Baud",19200),
                new SelectItem("38400 Baud",38400),
                new SelectItem("56000 Baud",56000),
                new SelectItem("57600 Baud",57600),
                new SelectItem("115200 Baud",115200),
                new SelectItem("128000 Baud",128000),
                new SelectItem("230400 Baud",230400),
                new SelectItem("256000 Baud",256000),
            };
            DataBitItems = new List<SelectItem>()
            {
                new SelectItem("8 Data bits",8),
                new SelectItem("7 Data bits",7),
            };
            ParityItems = new List<SelectItem>()
            {
                 new SelectItem("None Parity",Parity.None),
                 new SelectItem("Odd Parity",Parity.Odd),
                 new SelectItem("Even Parity",Parity.Even),
            };
            StopBitItems = new List<SelectItem>()
            {
                new SelectItem("1 Stop Bit",StopBits.One),
                new SelectItem("2 Stop Bit",StopBits.Two),
            };
        }
    }

    public class SelectItem
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public SelectItem() { }
        public SelectItem(string name,object value) 
        { 
            Name = name;
            Value = value;
        }
       
    }
}
