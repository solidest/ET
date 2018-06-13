using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Main
{
    public class Helper
    {
        public static byte[] ConvertIntToByteArray(Int32 m)
        {
            byte[] arry = new byte[4];
            arry[0] = (byte)(m & 0xFF);
            arry[1] = (byte)((m & 0xFF00) >> 8);
            arry[2] = (byte)((m & 0xFF0000) >> 16);
            arry[3] = (byte)((m >> 24) & 0xFF);
            return arry;
        }

        public static Int32 CurrentAppDocVer()
        {
            return Convert.ToInt32(RevisionClass.DocVer);
        }
    }
}
