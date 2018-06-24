using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.TestMap
{
    [Serializable]
    public class TestMapData
    {
        private string _name = "";

        public TestMapData(string name)
        {
            _name = name;
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
