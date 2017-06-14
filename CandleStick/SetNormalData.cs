using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace CandleStick
{
    class SetNormalData
    {
        private CandleStatus[] status = new CandleStatus[0];
        private CandleNormalData[] normalData = new CandleNormalData[0];
        private int StartPoint;

        private const byte High = 3;
        private const byte Mid = 2;
        private const byte Low = 1;
        private const byte None = 0;

        public CandleNormalData[] getNormalData(BigInteger[] package,int startPoint)
        {
            InitComponent(package,startPoint);
            SetNormal();
            Shifting();
            return normalData;
        }
        private void InitComponent(BigInteger[] package,int startPoint)
        {
            Array.Resize<CandleStatus>(ref status, package.Length);
            Array.Resize<CandleNormalData>(ref normalData, package.Length);
            StartPoint = startPoint;
            for (int i = 0; i < package.Length; i++)
            {
                status[i] = Package.UnPack(package[i]);
            }
        }
        private void SetNormal()
        {
            for(int i =0;i< normalData.Length;i++)
            {
                normalData[i].Low = StartPoint;
                if (status[i].Direction == (int)TypeTrend.UP)
                {
                    if (status[i].LowerShadow == (int)TypeCandle.HIGH)
                    {
                        normalData[i].Open = normalData[i].Low + High;
                    }
                    else if (status[i].LowerShadow == (int)TypeCandle.MID)
                    {
                        normalData[i].Open = normalData[i].Low + Mid;
                    }
                    else if (status[i].LowerShadow == (int)TypeCandle.LOW)
                    {
                        normalData[i].Open = normalData[i].Low + Low;
                    }
                    else
                    {
                        normalData[i].Open = normalData[i].Low + None;
                    }

                    if (status[i].Body == (int)TypeCandle.HIGH)
                    {
                        normalData[i].Close = normalData[i].Open + High;
                    }
                    else if (status[i].Body == (int)TypeCandle.MID)
                    {
                        normalData[i].Close = normalData[i].Open + Mid;
                    }
                    else if (status[i].Body == (int)TypeCandle.LOW)
                    {
                        normalData[i].Close = normalData[i].Open + Low;
                    }
                    else
                    {
                        normalData[i].Close = normalData[i].Open + None;
                    }

                    if (status[i].UpperShadow == (int)TypeCandle.HIGH)
                    {
                        normalData[i].High = normalData[i].Close + High;
                    }
                    else if (status[i].UpperShadow == (int)TypeCandle.MID)
                    {
                        normalData[i].High = normalData[i].Close + Mid;
                    }
                    else if (status[i].UpperShadow == (int)TypeCandle.LOW)
                    {
                        normalData[i].High = normalData[i].Close + Low;
                    }
                    else
                    {
                        normalData[i].High = normalData[i].Close + None;
                    }
                }
                else
                {
                    if (status[i].LowerShadow == (int)TypeCandle.HIGH)
                    {
                        normalData[i].Close = normalData[i].Low + High;
                    }
                    else if (status[i].LowerShadow == (int)TypeCandle.MID)
                    {
                        normalData[i].Close = normalData[i].Low + Mid;
                    }
                    else if (status[i].LowerShadow == (int)TypeCandle.LOW)
                    {
                        normalData[i].Close = normalData[i].Low + Low;
                    }
                    else
                    {
                        normalData[i].Close = normalData[i].Low + None;
                    }

                    if (status[i].Body == (int)TypeCandle.HIGH)
                    {
                        normalData[i].Open = normalData[i].Close + High;
                    }
                    else if (status[i].Body == (int)TypeCandle.MID)
                    {
                        normalData[i].Open = normalData[i].Close + Mid;
                    }
                    else if (status[i].Body == (int)TypeCandle.LOW)
                    {
                        normalData[i].Open = normalData[i].Close + Low;
                    }
                    else
                    {
                        normalData[i].Open = normalData[i].Close + None;
                    }

                    if (status[i].UpperShadow == (int)TypeCandle.HIGH)
                    {
                        normalData[i].High = normalData[i].Open + High;
                    }
                    else if (status[i].UpperShadow == (int)TypeCandle.MID)
                    {
                        normalData[i].High = normalData[i].Open + Mid;
                    }
                    else if (status[i].UpperShadow == (int)TypeCandle.LOW)
                    {
                        normalData[i].High = normalData[i].Open + Low;
                    }
                    else
                    {
                        normalData[i].High = normalData[i].Open + None;
                    }
                }
            }
            
        }
        private void Shifting()
        {
            for(int i = 0;i<normalData.Length;i++)
            {
                if (i != 0)
                {
                    while (Math.Min(normalData[i].Open, normalData[i].Close) != Math.Min(normalData[i - 1].Open, normalData[i - 1].Close))
                    {
                        if (Math.Min(normalData[i].Open, normalData[i].Close) > Math.Min(normalData[i - 1].Open, normalData[i - 1].Close))
                        {
                            ShiftDown(i);
                        }
                        else
                        {
                            ShiftUp(i);
                        }
                    }
                }
                if (status[i].GAP == (int)CandleGAP.GAP)
                {
                    if (status[i].HigherHigh == 1)
                    {
                        while (normalData[i].Low <= normalData[i - 1].High)
                        {
                            ShiftUp(i);
                        }
                    }
                    else
                    {
                        while (normalData[i].High >= normalData[i - 1].Low)
                        {
                            ShiftDown(i);
                        }
                    }
                }
                else
                {
                    if (status[i].HigherHigh == 1)
                    {
                        ShiftUp(i);
                    }
                    else if(status[i].LowerLow == 1)
                    {
                        ShiftDown(i);
                    }
                }
            }
        }
        private void ShiftUp(int idx)
        {
            normalData[idx].Open += 1;
            normalData[idx].Low += 1;
            normalData[idx].Close += 1;
            normalData[idx].High += 1;
        }
        private void ShiftDown(int idx)
        {
            normalData[idx].Open -= 1;
            normalData[idx].Low -= 1;
            normalData[idx].Close -= 1;
            normalData[idx].High -= 1;
        }
    }
}
