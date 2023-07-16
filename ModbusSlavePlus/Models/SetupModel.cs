using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusSlavePlus.Models
{
    public class SetupModel
    {
        public byte SlaveId { get; set; }
        public ushort Address { get; set; }
        public ushort Quantity { get; set; }
        public SetupModel(byte slaveId,ushort address,ushort quantity) 
        { 
            SlaveId = slaveId;
            Address = address;
            Quantity = quantity;
        }
    }
}
