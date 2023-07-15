using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusSlavePlus.Models
{
    public class Param : INotifyPropertyChanged
    {
        public Action<Param> Notify = (Param p) => { };
        public ushort Address { get; set; }
        
        private string _value;
        public string Value 
        { 
            get { return _value; }
            set
            {
                _value = value;
                Notify(this);
                UpdateValue("Value");
            }
        }
        public TypeCode Type { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void UpdateValue(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public Param()
        {

        }
    }
}
