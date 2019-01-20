using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace V1
{
    public partial class LifeForm : Form
    {
        #region CONSTANTS

        const int MAX_CELLS = 50;
        const int CELL_SIZE = 20;
        const int C_M = 2; // CellMargin

        #endregion

        // 2 Matritzen zur Verwaltung der n'ten und der n+1'ten Generation
        bool[,] _CA = new bool[MAX_CELLS, MAX_CELLS];
        bool[,] _CB = new bool[MAX_CELLS, MAX_CELLS];
        bool[,] _CC; // current ( active ) CellArray n'te Generation

        public LifeForm()
        {
            InitializeComponent();
            timer1.Interval = 100;
            // zum Testen ein paar Zellen setzen
            _CC = _CA;
        }

        private void OnPanelPaint(object sender, PaintEventArgs e)
        {
            DrawGrid(e.Graphics);
            DrawCells(e.Graphics);
        }

        private void OnPanelMouseDown(object sender, MouseEventArgs e)
        {
            // der Cell Editor
            TurnCellOnOff(e.X / CELL_SIZE, e.Y / CELL_SIZE);
            m_panel.Invalidate();
        }

        // Cells des aktiven Arrays (_CC) zeichnen
        void DrawCells(Graphics gr)
        {
            Brush _br = Brushes.Blue;

            for (int i = 0; i < MAX_CELLS; i++)
                for (int j = 0; j < MAX_CELLS; j++)
                {
                    if (_CC[i, j])
                        gr.FillRectangle(_br, i * CELL_SIZE + 5, j * CELL_SIZE + 5, (CELL_SIZE / 2), (CELL_SIZE / 2));
                    //gr.FillRectangle(_br, (i+ C_M) * CELL_SIZE  , (j + C_M) * CELL_SIZE , (CELL_SIZE / 2) , (CELL_SIZE / 2) + C_M);
                }
        }

        void DrawGrid(Graphics gr)
        {
            // Raster zeichnen
            Pen _pen = Pens.Red;

            // <= damit das Feld einen Abschlussrahmen sein
            for (int i = 0; i <= MAX_CELLS; i++)
            {
                gr.DrawLine(_pen, 0, i * CELL_SIZE, MAX_CELLS * CELL_SIZE, i * CELL_SIZE); //Wagrecht
            }

            for (int i = 0; i <= MAX_CELLS; i++)
            {
                gr.DrawLine(_pen, i * CELL_SIZE, 0, i * CELL_SIZE, MAX_CELLS * CELL_SIZE); //senkrecht
            }
        }

        // Nächste Generation berechnen
        private void OnStepButton(object sender, EventArgs e)
        {
            if (_CC == _CA)
            {
                ClearCells(_CB);
                CalcNextGeneration(_CA, _CB);
                _CC = _CB;
            }
            else
            {
                ClearCells(_CA);
                CalcNextGeneration(_CB, _CA);
                _CC = _CA;
            }

            m_panel.Invalidate();
        }

        private void OnTimerChk(object sender, EventArgs e)
        {
            if (timer1.Enabled)
                timer1.Enabled = false;
            else
                timer1.Enabled = true;
        }

        private void OnTimer(object sender, EventArgs e)
        {
            OnStepButton(null, null);
        }

        private void OnClearButton(object sender, EventArgs e)
        {
            ClearCells(_CA);
            ClearCells(_CB);
            m_panel.Invalidate();
        }

        private void OnSave(object sender, EventArgs e)
        {
            #region FileDialog and StreamWriter

            string filename = "";
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Text Files|*.txt|All Files|*.*";
            if (dlg.ShowDialog().Equals(DialogResult.OK))
            {
                filename = dlg.FileName;
                this.DialogResult = DialogResult.OK;
            }

            StreamWriter sw;
            try
            {
                sw = new StreamWriter(filename);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Konnte nicht gefunden werden!");
                return;
            }

            #endregion

            for (int i = 0; i < MAX_CELLS; i++)
            {
                for (int j = 0; j < MAX_CELLS; j++)
                {
                    if (_CC[i, j])
                    {
                        sw.WriteLine("{0},{1}", i.ToString(), j.ToString());
                    }
                }
            }

            MessageBox.Show("gespeichert!");
            sw.Close();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            #region FileDialog and StreamReader

            string filename = "";
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Text Files|*.txt|All Files|*.*";
            if (dlg.ShowDialog().Equals(DialogResult.OK))
            {
                filename = dlg.FileName;
                this.DialogResult = DialogResult.OK;
            }


            StreamReader sr;
            try
            {
                sr = new StreamReader(filename);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Konnte nicht gefunden werden!");
                return;
            }

            #endregion

            ClearCells(_CA);
            ClearCells(_CB);
            string cache;

            while (!sr.EndOfStream)
            {
                cache = sr.ReadLine();
                var cords = cache.Split(',');
                var i = Convert.ToInt32(cords[0]);
                var j = Convert.ToInt32(cords[1]);

                _CC[i, j] = true;
            }

            sr.Close();
            m_panel.Invalidate();

            MessageBox.Show("geladen!");
        }
    }
}