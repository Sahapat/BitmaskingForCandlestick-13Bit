using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Numerics;

namespace CandleStick
{
    class SetNormalData
    {
        private CandleStatus[] status = new CandleStatus[0];
        private CandleNormalData[] normalData = new CandleNormalData[0];
        private int StartPoint;
        private short ShiftSize = 1;
        private short SizePerUnit = 2; //It's mean 1 point per unit

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
                if(status[i].Direction == (int)TypeTrend.UP)
                {
                    normalData[i].Open = normalData[i].Low + (status[i].LowerShadow * SizePerUnit);
                    normalData[i].Close = normalData[i].Open + (status[i].Body * SizePerUnit);
                    normalData[i].High = normalData[i].Close + (status[i].UpperShadow * SizePerUnit);
                }
                else
                {
                    normalData[i].Close = normalData[i].Low + (status[i].LowerShadow * SizePerUnit);
                    normalData[i].Open = normalData[i].Close + (status[i].Body * SizePerUnit);
                    normalData[i].High = normalData[i].Open + (status[i].UpperShadow * SizePerUnit);
                }
            }
            
        }
        private void Shifting()
        {
            for(int i = 1;i<=normalData.Length-1;i++)
            {
                SetDefault(i);
                if (status[i].GAP == (int)CandleGAP.GAP)
                {
                    if (status[i].HigherHigh == (int)TypeTrend.UP)
                    {
                        int Distance = 0;
                        int LowCurrent = normalData[i].Low;
                        int HighBefore = normalData[i - 1].High;
                        Distance = Math.Abs(LowCurrent - HighBefore) + ShiftSize;
                        ShiftUp(i, Distance);
                    }
                    else
                    {
                        int Distance = 0;
                        int HighCurrent = normalData[i].High;
                        int LowBefore = normalData[i - 1].Low;
                        Distance = Math.Abs(LowBefore - HighCurrent) + ShiftSize;
                        ShiftDown(i, Distance);
                    }
                }
                else
                {
                    int MinCurrent = Math.Min(normalData[i].Close, normalData[i].Open);
                    int MaxCurrent = Math.Max(normalData[i].Close, normalData[i].Open);
                    int MinBefore = Math.Min(normalData[i-1].Close, normalData[i-1].Open);
                    int MaxBefore = Math.Max(normalData[i-1].Close, normalData[i-1].Open);

                    if (status[i].HigherHigh == 1)
                    {
                        if (status[i].HigherLow == 1)
                        {
                            int Distance = Math.Abs(MinCurrent - MaxBefore);
                            ShiftUp(i, Distance);
                        }
                        else
                        {
                            int Distance = (MaxCurrent - MaxBefore < 0) ? Distance = (MaxBefore - MaxCurrent) : Distance = 0;
                            ShiftUp(i, Distance+ShiftSize);
                            if(status[i].LowerLow == 1)
                            {
                                int MinCur = Math.Min(normalData[i].Open, normalData[i].Close);
                                int MinBef = Math.Min(normalData[i - 1].Open, normalData[i - 1].Close);

                                ShiftDown(i, (MinCur - MinBef) + ShiftSize);
                            }
                        }
                    }
                    else if (status[i].LowerLow == 1)
                    {
                        if (status[i].LowerHigh == 1)
                        {
                            int Distance = Math.Abs(MaxCurrent - MinBefore);
                            ShiftDown(i, Distance);
                        }
                        else
                        {
                            int Distance = (MinBefore - MinCurrent <= 0) ? Distance = MinCurrent - MinBefore : Distance = 0;
                            ShiftDown(i, Distance+ShiftSize);
                            if (Math.Max(normalData[i].Open, normalData[i].Close) > Math.Max(normalData[i - 1].Open, normalData[i - 1].Close))
                            {
                                ShiftDown(i, Math.Max(normalData[i].Open, normalData[i].Close) - Math.Max(normalData[i - 1].Open, normalData[i - 1].Close));
                            }
                        }
                    }
                }
            }
            CheckDefualtNormal();
        }
        private void SetDefault(int idx)
        {
            while (Math.Min(normalData[idx].Open, normalData[idx].Close) != Math.Min(normalData[idx - 1].Open, normalData[idx - 1].Close))
            {
                if (Math.Min(normalData[idx].Open, normalData[idx].Close) > Math.Min(normalData[idx - 1].Open, normalData[idx - 1].Close))
                {
                    ShiftDown(idx,ShiftSize);
                }
                else
                {
                    ShiftUp(idx,ShiftSize);
                }
            }
        }
        private void ShiftUp(int idx,int shift)
        {
            normalData[idx].Open += shift;
            normalData[idx].Low += shift;
            normalData[idx].Close += shift;
            normalData[idx].High += shift;
        }
        private void ShiftDown(int idx,int shift)
        {
            normalData[idx].Open -= shift;
            normalData[idx].Low -= shift;
            normalData[idx].Close -= shift;
            normalData[idx].High -= shift;
        }
        private void CheckDefualtNormal()
        {
            for(int i =0;i<normalData.Length;i++)
            {
                if(normalData[i].Low > normalData[i].High)
                {
                    MessageBox.Show("Invalid : "+i);
                }
                if (normalData[i].Open > normalData[i].High)
                {
                    MessageBox.Show("Invalid : "+i);
                }
                if (normalData[i].Close > normalData[i].High)
                {
                    MessageBox.Show("Invalid : "+i);
                }
            }
        }
    }
}
