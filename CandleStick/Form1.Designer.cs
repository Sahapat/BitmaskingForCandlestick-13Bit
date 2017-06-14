namespace CandleStick
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.candleChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.getCandleData = new System.Windows.Forms.OpenFileDialog();
            this.GetData = new System.Windows.Forms.Button();
            this.normalChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.Back = new System.Windows.Forms.Button();
            this.Next = new System.Windows.Forms.Button();
            this.VolumeAvg = new System.Windows.Forms.ComboBox();
            this.Save = new System.Windows.Forms.Button();
            this.saveCsv = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.candleChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.normalChart)).BeginInit();
            this.SuspendLayout();
            // 
            // candleChart
            // 
            chartArea1.Name = "ChartArea1";
            this.candleChart.ChartAreas.Add(chartArea1);
            this.candleChart.Location = new System.Drawing.Point(42, 97);
            this.candleChart.Name = "candleChart";
            this.candleChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            series1.BackSecondaryColor = System.Drawing.SystemColors.Control;
            series1.BorderColor = System.Drawing.SystemColors.Control;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
            series1.Color = System.Drawing.Color.White;
            series1.CustomProperties = "PriceDownColor=Red, PriceUpColor=Lime";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.BrightPastel;
            series1.YValuesPerPoint = 4;
            this.candleChart.Series.Add(series1);
            this.candleChart.Size = new System.Drawing.Size(450, 422);
            this.candleChart.TabIndex = 0;
            this.candleChart.Text = "candleChart";
            // 
            // GetData
            // 
            this.GetData.Location = new System.Drawing.Point(12, 21);
            this.GetData.Name = "GetData";
            this.GetData.Size = new System.Drawing.Size(62, 41);
            this.GetData.TabIndex = 3;
            this.GetData.Text = "GetData";
            this.GetData.UseVisualStyleBackColor = true;
            this.GetData.Click += new System.EventHandler(this.GetData_Click);
            // 
            // normalChart
            // 
            chartArea2.Name = "ChartArea1";
            this.normalChart.ChartAreas.Add(chartArea2);
            this.normalChart.Location = new System.Drawing.Point(535, 97);
            this.normalChart.Name = "normalChart";
            this.normalChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            series2.BackSecondaryColor = System.Drawing.SystemColors.Control;
            series2.BorderColor = System.Drawing.SystemColors.Control;
            series2.ChartArea = "ChartArea1";
            series2.Color = System.Drawing.Color.White;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            series2.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.BrightPastel;
            this.normalChart.Series.Add(series2);
            this.normalChart.Size = new System.Drawing.Size(450, 422);
            this.normalChart.TabIndex = 4;
            this.normalChart.Text = "chart1";
            // 
            // Back
            // 
            this.Back.Location = new System.Drawing.Point(434, 525);
            this.Back.Name = "Back";
            this.Back.Size = new System.Drawing.Size(76, 34);
            this.Back.TabIndex = 5;
            this.Back.Text = "Back";
            this.Back.UseVisualStyleBackColor = true;
            this.Back.Click += new System.EventHandler(this.Back_Click);
            // 
            // Next
            // 
            this.Next.Location = new System.Drawing.Point(516, 525);
            this.Next.Name = "Next";
            this.Next.Size = new System.Drawing.Size(76, 34);
            this.Next.TabIndex = 6;
            this.Next.Text = "Next";
            this.Next.UseVisualStyleBackColor = true;
            this.Next.Click += new System.EventHandler(this.Next_Click);
            // 
            // VolumeAvg
            // 
            this.VolumeAvg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VolumeAvg.FormattingEnabled = true;
            this.VolumeAvg.Location = new System.Drawing.Point(728, 33);
            this.VolumeAvg.Name = "VolumeAvg";
            this.VolumeAvg.Size = new System.Drawing.Size(148, 28);
            this.VolumeAvg.TabIndex = 7;
            this.VolumeAvg.SelectedIndexChanged += new System.EventHandler(this.VolumeAvg_SelectedIndexChanged);
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(882, 30);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(76, 34);
            this.Save.TabIndex = 8;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1032, 571);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.VolumeAvg);
            this.Controls.Add(this.Next);
            this.Controls.Add(this.Back);
            this.Controls.Add(this.normalChart);
            this.Controls.Add(this.GetData);
            this.Controls.Add(this.candleChart);
            this.Name = "Form1";
            this.Text = "CandleStick";
            ((System.ComponentModel.ISupportInitialize)(this.candleChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.normalChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart candleChart;
        private System.Windows.Forms.OpenFileDialog getCandleData;
        private System.Windows.Forms.Button GetData;
        private System.Windows.Forms.DataVisualization.Charting.Chart normalChart;
        private System.Windows.Forms.Button Back;
        private System.Windows.Forms.Button Next;
        private System.Windows.Forms.ComboBox VolumeAvg;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.SaveFileDialog saveCsv;
    }
}

