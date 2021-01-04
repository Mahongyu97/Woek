using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public enum ENPCType
    {
        None=0,
        Task=1,
        Functional=2
    }
    public enum ENPCFunctionType
    {
        None=0,
        InvokeShop=1,
        InvokeInsrance=2,
    }

    public class NPCDefine
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ENPCType Type { get; set; }
        public ENPCFunctionType Function { get; set; }
        public int Param { get; set; }
    }
}
