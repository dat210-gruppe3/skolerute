using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using skolerute.data;

namespace skolerute.GPS
{
    public interface IGPSservice
    {
         Coordinate GetGpsCoordinates();
    }
}
