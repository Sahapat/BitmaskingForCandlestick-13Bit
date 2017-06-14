using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace CandleStick
{
    enum TypeCandle
    {
        NONE,
        LOW,
        MID,
        HIGH
    };
    enum TypeTrend
    {
        DOWN,
        UP
    };
    enum CandleGAP
    {
        NotGAP,
        GAP
    };
    enum CheckVolume
    {
        NotPeak,
        Peak
    };

    class Package
    {
        public static CandleStatus[] UnPacks(BigInteger package,int PackageCount)
        {
            int packageKey = 2047;
            CandleStatus[] output = new CandleStatus[PackageCount];
            BigInteger[] singleData = new BigInteger[PackageCount];
            
            byte getSingleDatashift = 0;
            for(int i =0;i<PackageCount;i++)
            {
                singleData[i] = (package & (packageKey<<getSingleDatashift))>>getSingleDatashift;
                getSingleDatashift += 11;
            }

            byte Key1 = 1;
            byte Key2 = 3; 
            for(int i =0;i<PackageCount;i++)
            {
                output[i].Volume = (short)(singleData[i] & Key1);
                output[i].LowerShadow = (short)((singleData[i] & Key2 << 1)>>1);
                output[i].Body = (short)((singleData[i] & Key2 << 3)>>3);
                output[i].UpperShadow = (short)((singleData[i] & Key2 << 5) >> 5);
                output[i].GAP = (short)((singleData[i] & Key1 << 7) >> 7);
                output[i].Direction = (short)((singleData[i] & Key1 << 8) >> 8);
                output[i].LowerLow = (short)((singleData[i] & Key1 << 9) >> 9);
                output[i].HigherHigh = (short)((singleData[i] & Key1 << 10) >> 10);
            }
            return output;
        }
        public static CandleStatus UnPack(BigInteger package)
        {
            CandleStatus output = new CandleStatus();
            byte Key1 = 1;
            byte Key2 = 3;

            output.Volume = (short)(package & Key1);
            output.LowerShadow = (short)((package & Key2 << 1) >> 1);
            output.Body = (short)((package & Key2 << 3) >> 3);
            output.UpperShadow = (short)((package & Key2 << 5) >> 5);
            output.GAP = (short)((package & Key1 << 7) >> 7);
            output.Direction = (short)((package & Key1 << 8) >> 8);
            output.LowerLow = (short)((package & Key1 << 9) >> 9);
            output.HigherHigh = (short)((package & Key1 << 10) >> 10);

            return output;
        }
        public BigInteger Packing(BigInteger[] data,int NumPack)
        {
            BigInteger output = 0;
            for(int i =0;i<NumPack;i++)
            {
                output = (output | (data[i] << (i * 11)));
            }
            return output;
        }
        public BigInteger[] getMaskData(CandleStickData[] rawData, int DayOfAvgVolume)
        {
            int Rawsize = rawData.Length;

            CandleStatus[] maskData = new CandleStatus[Rawsize];
            BigInteger[] output = new BigInteger[Rawsize];

            double[] body = new double[Rawsize];
            double[] UpShadow = new double[Rawsize];
            double[] LowShadow = new double[Rawsize];

            double avgBody = 0;
            double avgUpShadow = 0;
            double avgLowShadow = 0;
            double SD_Body = 0;
            double SD_UpShadow = 0;
            double SD_LowShadow = 0;

            double[] Uscentroid = { -0.552689665062178, 0.615773990557240, 2.955830732137820 };
            double[] LScentroid = { -0.479483091630587, 0.571950622887706, 3.379221758271600 };
            double[] Bodycentroid = { -0.568489143858862, 0.492655804140342, 2.732240511936160 };

            //Calculate Average
            for(int i = 0;i<Rawsize;i++)
            {
                body[i] = Math.Abs(rawData[i].Open - rawData[i].Close);
                if (rawData[i].Open >= rawData[i].Close)
                {
                    UpShadow[i] = rawData[i].High - rawData[i].Open;
                    LowShadow[i] = rawData[i].Close - rawData[i].Low;
                }
                else
                {
                    UpShadow[i] = rawData[i].High - rawData[i].Close;
                    LowShadow[i] = rawData[i].Open - rawData[i].Low;
                }

                avgBody += body[i];
                avgUpShadow += UpShadow[i];
                avgLowShadow += LowShadow[i];
            }
            avgBody /= Rawsize;
            avgUpShadow /= Rawsize;
            avgLowShadow /= Rawsize;
            //Set SD
            SD_Body = GetSD(body);
            SD_LowShadow = GetSD(LowShadow);
            SD_UpShadow = GetSD(UpShadow);
            
            //Set property
            for(int i =0;i<maskData.Length;i++)
            {
                maskData[i].Direction = checkDirection(rawData[i].Open, rawData[i].Close);
                maskData[i].Body = statusBody(body[i], SD_Body, avgBody, Bodycentroid);
                maskData[i].LowerShadow = statusLowerShadow(LowShadow[i], SD_LowShadow, avgLowShadow, LScentroid);
                maskData[i].UpperShadow = statusUpperShadow(UpShadow[i], SD_UpShadow, avgUpShadow, Uscentroid);

                if(i != 0)
                {
                    maskData[i].HigherHigh = checkHH(rawData[i], rawData[i - 1]);
                    maskData[i].LowerLow = checkLL(rawData[i], rawData[i - 1]);
                    maskData[i].GAP = checkGAP(rawData[i],rawData[i-1]);
                }
                if (i-DayOfAvgVolume >= 0)
                {
                    double avgLast = 0;
                    int LastDay = 1;
                    for (int j = i; j < (i+DayOfAvgVolume); j++,LastDay++)
                    {
                        avgLast += rawData[j-LastDay].Volume;
                    }
                    avgLast /= DayOfAvgVolume;
                    maskData[i].Volume = checkVolume(rawData[i].Volume, avgLast);
                }
                else
                {
                    maskData[i].Volume = 0;
                }
            }
            
            for (int i = 0;i<maskData.Length;i++)
            {
                output[i] = Mask(maskData[i]);  
            }
            return output;
        }
        private BigInteger Mask(CandleStatus data)
        {
            BigInteger temp = 0;
            temp = temp | data.Volume;
            temp = temp | (data.LowerShadow<<1);
            temp = temp | (data.Body << 3);
            temp = temp | (data.UpperShadow << 5);
            temp = temp | (data.GAP << 7);
            temp = temp | (data.Direction << 8);
            temp = temp | (data.LowerLow << 9);
            temp = temp | (data.HigherHigh << 10);
            return temp;
        }
        private double GetSD(double[] data)
        {
            int size = data.Length;
            double temp1 = 0;
            double temp2 = 0;
            double SD = 0;

            for (int i = 0; i < size; i++)
            {
                temp1 += Math.Pow(data[i], 2);
                temp2 += data[i];
            }

            SD = ((size * temp1) - Math.Pow(temp2, 2)) / (size * (size - 1));
            SD = Math.Sqrt(SD);
            return SD;
        }
        private short checkDirection(double open,double close)
        {
            if (open < close) return (short)TypeTrend.UP;
            else return (short)TypeTrend.DOWN;
        }//check
        private short checkHH(CandleStickData current,CandleStickData before)
        {
            double currentMax = Math.Max(current.Open, current.Close);
            double beforeMax = Math.Max(before.Open, before.Close);

            if (currentMax > beforeMax)
            {
                return (short)TypeTrend.UP;
            }
            else return (short)TypeTrend.DOWN;
        }//check
        private short checkLL(CandleStickData current, CandleStickData before)
        {
            double currentMin = Math.Min(current.Open, current.Close);
            double beforeMin = Math.Min(before.Open, before.Close);

            if (currentMin < beforeMin)
            {
                return (short)TypeTrend.UP;
            }
            else return (short)TypeTrend.DOWN;
        }//check
        private short statusUpperShadow(double US,double US_SD,double avgUpShadow,double[] UScentroid)
        {
            if (US == 0) return (short)TypeCandle.NONE;
            else
            {
                double STD = (US - avgUpShadow) / US_SD;
                if (STD < (UScentroid[0] + UScentroid[1]) / 2)
                {
                    return (short)TypeCandle.LOW;
                }
                else if (STD > (UScentroid[1] + UScentroid[2]) / 2)
                {
                    return (short)TypeCandle.HIGH;
                }
                else return (short)TypeCandle.MID;
            }
        }//check
        private short statusLowerShadow(double LS,double LS_SD,double avgLowShadow,double[] LScentroid)
        {
            if (LS == 0) return (short)TypeCandle.NONE;
            else
            {
                double STD = (LS - avgLowShadow) / LS_SD;

                if (STD < (LScentroid[0] + LScentroid[1]) / 2)
                {
                    return (short)TypeCandle.LOW;
                }
                else if (STD > (LScentroid[1] + LScentroid[2]) / 2)
                {
                    return (short)TypeCandle.HIGH;
                }
                else return (short)TypeCandle.MID;
            }
        }//check
        private short statusBody(double Body,double Body_SD,double avgBody,double[] Bodycentroid)
        {
            if (Body == 0) return (short)TypeCandle.NONE;
            else
            {
                double STD = (Body - avgBody) / Body_SD;
                if (STD < (Bodycentroid[0] + Bodycentroid[1]) / 2)
                {
                    return (short)TypeCandle.LOW;
                }
                else if (STD > (Bodycentroid[1] + Bodycentroid[2]) / 2)
                {
                    return (short)TypeCandle.HIGH;
                }
                else return (short)TypeCandle.MID;
            }
        }//check
        private short checkGAP(CandleStickData currentData,CandleStickData beforeData)
        {
            if (currentData.Low > beforeData.High||currentData.High < beforeData.Low)
            {
                return (short)CandleGAP.GAP;
            }

            else
                return (short)CandleGAP.NotGAP;
        }//check
        private short checkVolume(double current,double avgLast)
        {
            if (current > avgLast)
            {
                return (short)CheckVolume.Peak;
            }
            else return (short)CheckVolume.NotPeak;
        }//check
    }
}
