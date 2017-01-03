using System;
using System.Drawing;
using System.Windows.Forms;

namespace sudoku
{
    public partial class MainForm : Form
    {
        Field f;

        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //Stift definieren
            Pen BlackPen = new Pen(Color.Black, 3);
            Pen GreyPen = new Pen(Color.DarkGray, 2);
            e.Graphics.DrawRectangle(BlackPen, 18, 18, 416, 524);

            for (int i = 0; i < 8; i++)
            {
                int x = 65 + i * 46;
                int y = 75 + i * 58;
                if (((i + 1) % 3) != 0)
                {
                    e.Graphics.DrawLine(GreyPen, x, 20, x, 540);
                    e.Graphics.DrawLine(GreyPen, 20, y, 432, y);
                }
            }
            e.Graphics.DrawLine(BlackPen, 157, 20, 157, 542);
            e.Graphics.DrawLine(BlackPen, 295, 20, 295, 542);
            e.Graphics.DrawLine(BlackPen, 20, 191, 434, 191);
            e.Graphics.DrawLine(BlackPen, 20, 365, 434, 365);
        }

        void MainFormLoad(object sender, EventArgs e)
        {
            f = new Field(this);
            ToolTip tip1 = new ToolTip();
            tip1.SetToolTip(button1, "Shows the possible numbers on each field.\nIf there's only one number left,\nthe number gets filled into the field");
            ToolTip tip2 = new ToolTip();
            tip2.SetToolTip(button2, "Looks for numbers that only can be in that field,\non this row/column/square");
            ToolTip tip3 = new ToolTip();
            tip3.SetToolTip(button3, "Solves the Sudoku completely");
            ToolTip tip4 = new ToolTip();
            tip4.SetToolTip(button4, "Resets all fields");
        }

        private void ButtonCheckClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (f.CheckEntry())
            {
                f.Check(button1);
                button2.Enabled = true;
            }
            Cursor = Cursors.Default;
        }

        private void ButtonSearchClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            f.Search(button1);
            Cursor = Cursors.Default;
        }

        private void ButtonSolveClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (f.CheckEntry())
            {
                if (f.SetField(0))
                {
                    f.ClearLabel();
                }
            }
            Cursor = Cursors.Default;
        }

        private void ButtonResetClick(object sender, EventArgs e)
        {
            f.Reset();
            button2.Enabled = false;
        }

        void MainFormKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F1)
            {
                e.Handled = true;
                MessageBox.Show("Created By Samuel Reutimann\nVersion 1.0", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
    }
}
