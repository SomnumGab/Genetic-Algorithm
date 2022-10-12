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

namespace AG
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 1;
            dataGridView1.ColumnCount = 4;
            dataGridView1.Columns[0].Name = "Xreal";
            dataGridView1.Columns[1].Name = "feval(x)";
            dataGridView1.Columns[2].Name = "xbin";
            dataGridView1.Columns[3].Name = "%";
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            dataGridView2.ColumnCount = 6;
            dataGridView2.Columns[0].Name = "N";
            dataGridView2.Columns[1].Name = "T";
            dataGridView2.Columns[2].Name = "pk";
            dataGridView2.Columns[3].Name = "pm";
            dataGridView2.Columns[4].Name = "fmax";
            dataGridView2.Columns[5].Name = "favg";
            dataGridView2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView2.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView2.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView2.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView2.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            checkBox1.Checked = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            var decimals = comboBox1.SelectedIndex + 2;
            var accuracy = Math.Pow(10, (-1 * decimals));
            var a = Int32.Parse(textBox1.Text);
            var b = Int32.Parse(textBox2.Text);
            var N = Int32.Parse(textBox3.Text);
            var pk = Double.Parse(textBox4.Text);
            var pm = Double.Parse(textBox5.Text);
            var T = Int32.Parse(textBox6.Text);
            var elite = checkBox1.Checked;

            Generations gens = new Generations(a, b, N, accuracy, decimals, pk, pm, T, elite, false);
            List<Results> results = gens.GenerateResults(gens.GenerationLoop(gens.GenerateFirstGen()));

            for (int i = 0; i < results.Count; i++)
            {
                String[] row = {results[i].xreal.ToString(), results[i].feval.ToString(), results[i].xbin.ToString(), results[i].percent.ToString()};
                dataGridView1.Rows.Add(row);
            }
            chart1.Series.Clear();

            chart1.Series.Add("Fmax");
            chart1.Series["Fmax"].ChartType = SeriesChartType.Line;
            for (int i = 0; i < gens.dataGraph.Count; i++) chart1.Series["Fmax"].Points.AddY(gens.dataGraph[i].fmax);

            chart1.Series.Add("Average");
            chart1.Series["Average"].ChartType = SeriesChartType.Line;
            for (int i = 0; i < gens.dataGraph.Count; i++) chart1.Series["Average"].Points.AddY(gens.dataGraph[i].Avg);

            chart1.Series.Add("Fmin");
            chart1.Series["Fmin"].ChartType = SeriesChartType.Line;
            for (int i = 0; i < gens.dataGraph.Count; i++) chart1.Series["Fmin"].Points.AddY(gens.dataGraph[i].fmin);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            Testing t = new Testing();
            var testResults = t.TestTime();

            var sorted = testResults.OrderByDescending(a => a.avg).ThenByDescending(a => a.max).Take(10).ToList();

            for (int i = 0; i < sorted.Count; i++)
            {
                String[] row = { sorted[i].N.ToString(), sorted[i].T.ToString(), sorted[i].pk.ToString(), sorted[i].pm.ToString(), sorted[i].max.ToString(), sorted[i].avg.ToString() };
                dataGridView2.Rows.Add(row);
            }
        }
    }
}
