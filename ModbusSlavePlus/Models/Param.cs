using ModbusSlavePlus.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusSlavePlus.Models
{
    public class Param : INotifyPropertyChanged
    {
        public List<Param> OtherParams;
        public Action<Param> Notify = (Param p) => { };
        public string Name { get; set; }
        public ushort Address { get; set; }
        public string Unit { get; set; }
        private string _convertedValue;
        public string ConvertedValue 
        { 
            get { return _convertedValue; }
            set
            {
                if (_convertedValue != value)
                {
                    _convertedValue = value;
                    // 更新Value
                    UpdateValueByType();
                }
            }
        }
        public List<ParamDictionary> Dic { get; set; }

        public bool IsDisable { get; set; }
        private ushort _value;
        public ushort Value 
        { 
            get { return _value; }
            set
            {
                _value = value;
                Notify(this);
                //UpdateValue("Value");
                UpdateConvertedValueByType();
            }
        }
        private TypeCode _type = TypeCode.UInt16;
        public TypeCode Type 
        { 
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    // Type变更，暂时的逻辑只管自己，不管别人
                    UpdateConvertedValueByType();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void UpdateValue(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public Param()
        {
        }
        /// <summary>
        /// 根据类型修改值
        /// </summary>
        private void UpdateConvertedValueByType()
        {
            switch (Type)
            {
                case TypeCode.UInt16:
                    _convertedValue = Value.ToString();
                    break;
                case TypeCode.Int16:
                    _convertedValue = ((short)Value).ToString();
                    break;
                case TypeCode.Single:
                    _convertedValue = ConvertTool.GetFloatFormUshortArray(Value, OtherParams[Address+1].Value).ToString();
                    break;
                case TypeCode.UInt32:
                    _convertedValue = ConvertTool.GetUIntFormUshort(Value, OtherParams[Address + 1].Value).ToString();
                    break;
                case TypeCode.Int32:
                    _convertedValue = ConvertTool.GetIntFormUshort(Value, OtherParams[Address + 1].Value).ToString();
                    break;
                default:
                    _convertedValue = Value.ToString();
                    break;
            }
            UpdateValue("ConvertedValue");
            // 刷新一下前面的参数
            if (Address > 0 && Address < OtherParams.Count)
            {
                OtherParams[Address - 1].UpdateConvertedValueByType();
            }
        }

        /// <summary>
        /// 设置值
        /// </summary>
        private void UpdateValueByType()
        {
            if (ConvertedValue != null)
            {
                switch (Type)
                {
                    case TypeCode.UInt16:
                        _value = ushort.Parse(ConvertedValue);
                        break;
                    case TypeCode.Int16:
                        _value = (ushort)short.Parse(ConvertedValue);
                        break;
                    case TypeCode.Single:
                        ushort[] sa = ConvertTool.FloatToUshort(float.Parse(ConvertedValue));
                        _value = sa[0];
                        OtherParams[Address + 1]._value = sa[1];
                        break;
                    case TypeCode.UInt32:
                        ushort[] ua = ConvertTool.UInt32ToUshort(UInt32.Parse(ConvertedValue));
                        _value = ua[0];
                        OtherParams[Address + 1]._value = ua[1];
                        break;
                    case TypeCode.Int32:
                        ushort[] ia = ConvertTool.Int32ToUshort(Int32.Parse(ConvertedValue));
                        _value = ia[0];
                        OtherParams[Address + 1]._value = ia[1];
                        break;
                    default:
                        _value = ushort.Parse(ConvertedValue);
                        break;
                }
            }
            Notify(this);
        }

        /// <summary>
        /// 更新关联的参数的类型
        /// </summary>
        private void UpdateRelatedType()
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
