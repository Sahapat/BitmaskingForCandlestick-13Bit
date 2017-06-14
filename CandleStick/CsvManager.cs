using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace CandleStick
{
    class CsvManager
    {
        private string[][] csvData;
        public string[][] CsvData
        {
            get
            {
                return csvData;
            }
        }

        private int _RowLenght;
        public int RowLenght
        {
            get
            {
                return _RowLenght;
            }
            private set
            {
                _RowLenght = value;
            }
        }

        private int _ColumnLenght;
        public int ColumnLenght
        {
            get
            {
                return _ColumnLenght;
            }
            private set
            {
                _ColumnLenght = value;
            }
        }
        public void ReadData(string path)
        {
            try
            {
                StreamReader read = new StreamReader(path);
                var line = new List<string[]>();

                while (!read.EndOfStream)
                {
                    string[] lines = read.ReadLine().Split(',');
                    line.Add(lines);
                    if (ColumnLenght < lines.Length) ColumnLenght = lines.Length;
                }
                csvData = line.ToArray();
                RowLenght = csvData.Length;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(),"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
        public void WriteData(string path,string data)
        {
            try
            {
                StreamWriter write = new StreamWriter(path);
                write.Write(data);
                write.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Application.Exit();
            }
        }
        public double GetColumnData(int selectColumn,int selectRow)
        {
            double temp = 0;
            try
            {
                temp = double.Parse(csvData[selectRow][selectColumn]);
                return temp;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }
        public string GetColumnDataAsString(int selectColumn,int selectRow)
        {
            return csvData[selectRow][selectColumn];
        }
        public string[] GetColumnsDataAsString(int selectColumn,int columnStart)
        {
            var temp = new List<string>();
            for(int i =columnStart;i<RowLenght;i++)
            {
                temp.Add(csvData[i][selectColumn]);
            }
            string[] outData = temp.ToArray();
            return outData;
        }
        public double[] GetColumnsData(int selectColumn)
        {
            var temp = new List<double>();
            for (int i = 0; i < RowLenght; i++)
            {
                try
                {
                    temp.Add(double.Parse(csvData[i][selectColumn]));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
            double[] outData = temp.ToArray();
            return outData;
        }
        public double[] GetColumnsData(int selectColumn,int columnStart)
        {
            var temp = new List<double>();
            for(int i =columnStart;i<RowLenght;i++)
            {
                try
                {
                    temp.Add(double.Parse(csvData[i][selectColumn]));
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString(),"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    break;
                }
            }
            double[] outData = temp.ToArray();
            return outData;
        }
        public double[] GetColumnsData(int selectColumn, int columnStart,int columnEnd)
        {
            var temp = new List<double>();
            for (int i = columnStart; i <columnEnd; i++)
            {
                try
                {
                    temp.Add(double.Parse(csvData[i][selectColumn]));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
            double[] outData = temp.ToArray();
            return outData;
        }
        public double[] GetRowsData(int selectRow)
        {
            var temp = new List<double>();
            for (int i = 0; i < ColumnLenght; i++)
            {
                try
                {
                    temp.Add(double.Parse(csvData[selectRow][i]));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
            double[] outData = temp.ToArray();
            return outData;
        }
        public double[] GetRowsData(int selectRow,int rowStart)
        {
            var temp = new List<double>();
            for (int i = rowStart; i < ColumnLenght; i++)
            {
                try
                {
                    temp.Add(double.Parse(csvData[selectRow][i]));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
            double[] outData = temp.ToArray();
            return outData;
        }
        public double[] GetRowsData(int selectRow, int rowStart,int rowEnd)
        {
            var temp = new List<double>();
            for (int i = rowStart; i < rowEnd; i++)
            {
                try
                {
                    temp.Add(double.Parse(csvData[selectRow][i]));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
            double[] outData = temp.ToArray();
            return outData;
        }
    }
}
