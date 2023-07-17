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
        public string Name { get; set; }
        public ushort Address { get; set; }
        public string Unit { get; set; }
        public string ConvertedValue { get; set; }
        public List<ParamDictionary> Dic { get; set; }

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
        public TypeCode Type { get; set; } = TypeCode.UInt16;

        public event PropertyChangedEventHandler PropertyChanged;
        public void UpdateValue(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public Param()
        {
        }
    }

    public class ParamDictionary
    {
        public string Name { get; set; }
        public ushort Value { get; set; }
        public ParamDictionary() { }
    }

}
