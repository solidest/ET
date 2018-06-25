using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.TestMap
{
    [Serializable]
    public class TestMapData0
    {
        private string _name = "";

        public TestMapData0(string name)
        {
            _name = name;
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
