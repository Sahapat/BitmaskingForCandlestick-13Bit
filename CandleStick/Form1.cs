using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;
using System.Numerics;

namespace CandleStick
{
    public partial class Form1 : Form
    {
        private CsvManager csvCandleData = new CsvManager();
        private CandleStickData[] RawData = new CandleStickData[0];
        private CandleNormalData[] NormalData = new CandleNormalData[0];
        private BigInteger[] BinaryCandleProperty = new BigInteger[0];
        private BigInteger[] RawPack = new BigInteger[0];
        private string[] CandleSeries = new string[0];
        private int currentDisplay = 0;
        private const int nextDisplay = 5;
        private const int amountDisplay = 60;
        private string[] items = {"3 VolumesAvg","4 VolumesAvg", "5 VolumesAvg" , "6 VolumesAvg" , "7 VolumesAvg" 
                            , "8 VolumesAvg" ,"9 VolumesAvg" , "10 VolumesAvg" , "11 VolumesAvg" };
        private int DayOfAvgVolume = 0;

        public Form1()
        {
            InitializeComponent();
            InitChart();
            VolumeAvg.Visible = false;
            Save.Visible = false;
            this.MaximizeBox = false;
        }
        private void GetData_Click(object sender, EventArgs e)
        {
            string filter = "CSV file (*.csv)|*.csv";
            getCandleData.Filter = filter;
            DialogResult result = getCandleData.ShowDialog();
            if(result == DialogResult.OK)
            {
                csvCandleData.ReadData(getCandleData.FileName);

                int size = csvCandleData.RowLenght - 1;
                Array.Resize<CandleStickData>(ref RawData, size);
                Array.Resize<BigInteger>(ref BinaryCandleProperty, size);
                Array.Resize<string>(ref CandleSeries, size);
                Array.Resize<BigInteger>(ref RawPack, size);
                Array.Resize<CandleNormalData>(ref NormalData, size);

                SetRawData();
                InitComboBox();
                setNormalData();
                SetChart();
                SetNormalChart();
            }
        }
        private void Back_Click(object sender, EventArgs e)
        {
            if (csvCandleData.CsvData != null)
            {
                currentDisplay = (currentDisplay <= 0) ? currentDisplay = 0 : currentDisplay -= nextDisplay;
                candleChart.Series[0].Points.Clear();
                normalChart.Series[0].Points.Clear();
                SetChart();
                SetNormalChart();
            }
        }
        private void Next_Click(object sender, EventArgs e)
        {
            if (csvCandleData.CsvData != null)
            {
                currentDisplay = ((currentDisplay + nextDisplay) + amountDisplay > RawData.Length) ? currentDisplay = RawData.Length - amountDisplay : currentDisplay += nextDisplay;
                candleChart.Series[0].Points.Clear();
                normalChart.Series[0].Points.Clear();
                SetChart();
                SetNormalChart();
            }
        }
        private void Save_Click(object sender, EventArgs e)
        {
            string select = VolumeAvg.Text;
            if (select == string.Empty) return;
            string filter = "CSV file (*.csv)|*.csv";
            saveCsv.Filter = filter;
            DialogResult result = saveCsv.ShowDialog();
            if(result == DialogResult.OK)
            {
                string path = saveCsv.FileName;
                string outData = GetOutputData();
                csvCandleData.WriteData(path, outData);
            }
        }
        private void VolumeAvg_SelectedIndexChanged(object sender, EventArgs e)
        {
            PackingData();
        }
        private string GetOutputData()
        {
            MessageBox.Show("DayAVG:" + DayOfAvgVolume);
            StringBuilder output = new StringBuilder();
            StringBuilder CandleSeries = new StringBuilder();
            
            output.AppendFormat("{0},{1},{2},{3},{4},{5}", "DT", "BitCandle", "RawCandle","CandleSeries","BitPack","RawPack");
            output.AppendLine();
            for(int i =0;i<RawData.Length;i++)
            {
                CandleSeries.Clear();
                string outBinaryCandle = string.Empty;
                string BitPack = string.Empty;
                outBinaryCandle = ToBinaryString(BinaryCandleProperty[i]);
                BitPack = ToBinaryString(RawPack[i]);
                for (int j = DayOfAvgVolume;j> 0;j--)
                {
                    if(i-j+1 >= 0)
                    {
                        CandleSeries.AppendFormat("{0}", BinaryCandleProperty[i - j+1]);
                    }
                    else
                    {
                        CandleSeries.Append("0");                    }
                    if(j-1 > 0)
                    {
                        CandleSeries.Append(":");
                    }
                }
                while (outBinaryCandle.Length != 13 || BitPack.Length != DayOfAvgVolume*13)
                {
                    if(outBinaryCandle.Length > 13)
                    {
                        outBinaryCandle.Remove(0);
                    }
                    else if(outBinaryCandle.Length < 13)
                    {
                        string temp = "0";
                        temp += outBinaryCandle;
                        outBinaryCandle = temp;
                    }

                    if(BitPack.Length > DayOfAvgVolume*13)
                    {
                        outBinaryCandle.Remove(0);
                    }
                    else if(BitPack.Length < DayOfAvgVolume*13)
                    {
                        string temp = "0";
                        temp += BitPack;
                        BitPack = temp;
                    }
                }
                
                output.AppendFormat("{0},{1},{2},{3},{4},{5}",RawData[i].DateTime,outBinaryCandle,BinaryCandleProperty[i],CandleSeries,BitPack,RawPack[i]);
                output.AppendLine();
            }
            return output.ToString();
        }
        private void InitComboBox()
        {
            Save.Visible = true;
            VolumeAvg.Visible = true;
            VolumeAvg.Items.AddRange(items);
            VolumeAvg.SelectedIndex = 0;
            VolumeAvg.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void InitChart()
        {
            candleChart.Series.Clear();
            candleChart.Series.Add("Series").ChartType = SeriesChartType.Candlestick;
            candleChart.Series[0]["PointWidth"] = "0.65";
            candleChart.Series[0]["PriceUpColor"] = "Lime";
            candleChart.Series[0]["PriceDownColor"] = "Red";
            candleChart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;
            candleChart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gray;
            candleChart.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Gold;
            candleChart.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Gold;
            candleChart.ChartAreas[0].AxisY.IsStartedFromZero = false;
            candleChart.ChartAreas[0].BackColor = Color.Black;
            candleChart.BackColor = Color.Gray;

            normalChart.Series.Clear();
            normalChart.Series.Add("Series").ChartType = SeriesChartType.Candlestick;
            normalChart.Series[0]["PointWidth"] = "0.65";
            normalChart.Series[0]["PriceUpColor"] = "Lime";
            normalChart.Series[0]["PriceDownColor"] = "Red";
            normalChart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;
            normalChart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gray;
            normalChart.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Gold;
            normalChart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            normalChart.ChartAreas[0].AxisY.IsStartedFromZero = false;
            normalChart.ChartAreas[0].BackColor = Color.Black;
            normalChart.BackColor = Color.Gray;
        }
        private void SetChart()
        {
            for (int i = currentDisplay;i< currentDisplay+amountDisplay; i++)
            {
                candleChart.Series[0].Points.AddXY(RawData[i].DateTime, RawData[i].High, RawData[i].Low, RawData[i].Open, RawData[i].Close);
            }
        }
        private void SetNormalChart()
        {
            for (int i = currentDisplay; i < currentDisplay + amountDisplay; i++)
            {
                normalChart.Series[0].Points.AddXY(RawData[i].DateTime, NormalData[i].High, NormalData[i].Low, NormalData[i].Open, NormalData[i].Close);
            }
        }
        private void SetRawData()
        {
            for (int i = 1; i <= RawData.Length; i++)
            {
                RawData[i - 1].DateTime = csvCandleData.GetColumnDataAsString(1, i);
                RawData[i - 1].Open = (float)csvCandleData.GetColumnData(2, i);
                RawData[i - 1].High = (float)csvCandleData.GetColumnData(3, i);
                RawData[i - 1].Low = (float)csvCandleData.GetColumnData(4, i);
                RawData[i - 1].Close = (float)csvCandleData.GetColumnData(5, i);
                RawData[i - 1].Volume = csvCandleData.GetColumnData(6, i);
            }
        }
        private void setNormalData()
        {
            SetNormalData setnormal = new SetNormalData();
            NormalData = setnormal.getNormalData(BinaryCandleProperty, 1000);
        }
        private void PackingData()
        {
            switch(VolumeAvg.Text)
            {
                case "3 VolumesAvg":
                    DayOfAvgVolume = 3;
                    break;
                case "4 VolumesAvg":
                    DayOfAvgVolume = 4;
                    break;
                case "5 VolumesAvg":
                    DayOfAvgVolume = 5;
                    break;
                case "6 VolumesAvg":
                    DayOfAvgVolume = 6;
                    break;
                case "7 VolumesAvg":
                    DayOfAvgVolume = 7;
                    break;
                case "8 VolumesAvg":
                    DayOfAvgVolume = 8;
                    break;
                case "9 VolumesAvg":
                    DayOfAvgVolume = 9;
                    break;
                case "10 VolumesAvg":
                    DayOfAvgVolume = 10;
                    break;
                case "11 VolumesAvg":
                    DayOfAvgVolume = 11;
                    break;
                default:
                    DayOfAvgVolume = 3;
                    break;
            }

            Package Pack = new Package();
            BinaryCandleProperty = Pack.getMaskData(RawData, DayOfAvgVolume);

            for(int i =0;i<BinaryCandleProperty.Length;i++)
            {
                var temp = new BigInteger[DayOfAvgVolume];
                for(int j = 0;j<DayOfAvgVolume;j++)
                {
                    if(i-j >= 0)
                    {
                        temp[j] = BinaryCandleProperty[i - j];
                    }
                    else
                    {
                        temp[j] = 0;
                    }
                }
                RawPack[i] = Pack.Packing(temp, DayOfAvgVolume);
            }

;        }
        private string ToBinaryString(BigInteger data)
        {
            var bytes = data.ToByteArray();
            var idx = bytes.Length - 1;
            var base2 = new StringBuilder(bytes.Length * 8);
            var binary = Convert.ToString(bytes[idx], 2);
            base2.Append(binary);
            for (idx--; idx >= 0; idx--)
            {
                base2.Append(Convert.ToString(bytes[idx], 2).PadLeft(8, '0'));
            }
            return base2.ToString();
        }
        private BigInteger ToDecimal(string RawBinary)
        {
            BigInteger output = 0;
            char[] binary = RawBinary.ToArray();
            int size = binary.Length;

            for (int i = 1; i <= size; i++)
            {
                Console.WriteLine("Loop Count : " + i);
                if (binary[size - i] == '0')
                {
                    continue;
                }
                else
                {
                    output += (BigInteger)Math.Pow(2, i - 1);
                }
            }

            return output;
        }
    }
}
