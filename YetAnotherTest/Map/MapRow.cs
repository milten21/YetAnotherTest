using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace YetAnotherTest
{
    [Serializable]
    public class MapRow
    {
        public List<MapCell> Columns = new List<MapCell>();

        public MapRow()
        {

        }
    }
}
