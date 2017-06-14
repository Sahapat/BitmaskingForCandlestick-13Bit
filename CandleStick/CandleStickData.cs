using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace CandleStick
{
    struct CandleStickData
    {
        public string DateTime;
        public float High;
        public float Low;
        public float Open;
        public float Close;
        public double Volume;
    };
    struct CandleNormalData
    {
        public float High;
        public float Low;
        public float Open;
        public float Close;
    };
    struct CandleStatus
    {
        public short Direction;
        public short HigherHigh;
        public short LowerLow;
        public short UpperShadow;
        public short LowerShadow;
        public short Body;
        public short GAP;
        public short Volume;
    };
}
