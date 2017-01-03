using System;
using System.Windows.Forms;

namespace sudoku
{
    class Field
    {
        private const int nRow = 9;
        private const int nColumn = 9;

        private int p1 = 22;
        private int p2 = 22;

        private int x = 0;
        private int y = 0;

        private int nRecusion = 0;

        private TextBox[,] dynTextbox = new TextBox[9, 9];
        private Label[,] dynLabel = new Label[9, 9];

        private MainForm f;

        public Field(MainForm form)
        {
            this.f = form;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    dynLabel[i, j] = new Label();
                    dynLabel[i, j].Font = new System.Drawing.Font("Arial", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    dynLabel[i, j].Location = new System.Drawing.Point(p1 - 1, p2);
                    dynLabel[i, j].TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                    dynLabel[i, j].Size = new System.Drawing.Size(42, 11);
                    dynLabel[i, j].Click += new System.EventHandler(DynLabelClick);
                    f.Controls.Add(dynLabel[i, j]);
                    dynTextbox[i, j] = new TextBox();
                    dynTextbox[i, j].Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    dynTextbox[i, j].Location = new System.Drawing.Point(p1, p2 + 12);
                    dynTextbox[i, j].MaxLength = 1;
                    dynTextbox[i, j].Size = new System.Drawing.Size(40, 40);
                    dynTextbox[i, j].TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
                    f.Controls.Add(dynTextbox[i, j]);
                    p1 += 46;
                }
                p1 = 22;
                p2 += 58;
            }
        }

        public bool SetField(int i)
        {
            nRecusion++;
            // if the last field (bottom right) is not exceeded ...
            if (i < (nColumn * nRow))
            {
                // and the field i has no entry ...
                if (dynTextbox[Row(i), Column(i)].Text == "")
                {
                    // set to all values from 1 to 9 ...
                    for (int n = 1; n <= 9; n++)
                    {
                        dynTextbox[Row(i), Column(i)].Text = Convert.ToString(n);
                        // the first value that passes all rules ...
                        if (IsColumnOK(i) && IsRowOK(i) && IsSquareOK(i))
                        {
                            // success if we are at the last field
                            if (i == (nColumn * nRow) - 1)
                            {
                                return true;
                            }
                            else
                            {
                                // set true if all further fields are true (recursion)
                                if (SetField(i + 1))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    // none of the values are working
                    dynTextbox[Row(i), Column(i)].Text = ""; // backtracking!!!
                    return false;
                }
                else
                {
                    // success if we are at the last field
                    if (i == (nColumn * nRow) - 1)
                    {
                        return true;
                    }
                    else
                    {
                        // set true or false depending if all further fields are true (recursion)
                        return SetField(i + 1);
                    }
                }
            }
            // if the last field is exceeded
            return true;
        }

        public void Search(Button button)
        {
            if (CheckEntry())
            {
                int field = 0;
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        for (int n = 1; n <= 9; n++)
                        {
                            if ((dynLabel[i, j].Text != "") && (dynLabel[i, j].Text.Contains(Convert.ToString(n))))
                            {
                                if (CheckRow(field, n) || CheckColumn(field, n) || CheckSquare(field, n))
                                {
                                    dynTextbox[i, j].Text = Convert.ToString(n);
                                    button.PerformClick();
                                }
                            }
                        }
                        field++;
                    }
                }
            }
        }

        public void Check(Button button)
        {
            int field = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (dynTextbox[i, j].Text == "")
                    {
                        dynLabel[i, j].Text = "";
                        for (int n = 1; n <= 9; n++)
                        {
                            dynTextbox[i, j].Text = Convert.ToString(n);
                            if (IsColumnOK(field) && IsRowOK(field) && IsSquareOK(field))
                            {
                                dynLabel[i, j].Text += Convert.ToString(n);
                            }
                            dynTextbox[i, j].Text = "";
                        }
                        if (dynLabel[i, j].Text.Length == 1)
                        {
                            dynTextbox[i, j].Text = dynLabel[i, j].Text;
                            button.PerformClick();
                        }
                    }
                    field++;
                }
            }
            ClearLabel();
        }

        public bool CheckEntry()
        {
            int i = 0;
            for (int a = 0; a < 9; a++)
            {
                for (int b = 0; b < 9; b++)
                {
                    if (dynTextbox[a, b].Text != "")
                    {
                        if (!(IsColumnOK(i) && IsRowOK(i) && IsSquareOK(i)))
                        {
                            MessageBox.Show("Incorrect entry", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                    i++;
                }
            }
            return true;
        }

        public void ClearLabel()
        {
            for (int a = 0; a < 9; a++)
            {
                for (int b = 0; b < 9; b++)
                {
                    if (dynTextbox[a, b].Text != "")
                    {
                        dynLabel[a, b].Text = "";
                    }
                }
            }
        }

        public void Reset()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    dynTextbox[i, j].Text = "";
                    dynLabel[i, j].Text = "";
                }
            }
            nRecusion = 0;
        }

        private bool IsColumnOK(int i)
        {
            // is the same value already in the column: return false
            for (int j = 0; j < nColumn - 1; j++)
            {
                for (int k = j + 1; k < nColumn; k++)
                {
                    if ((dynTextbox[Row(i), j].Text == dynTextbox[Row(i), k].Text) && (dynTextbox[Row(i), j].Text != ""))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool IsRowOK(int i)
        {

            // is the same value already in the row: return false
            for (int j = 0; j < nRow - 1; j++)
            {
                for (int k = j + 1; k < nRow; k++)
                {
                    if ((dynTextbox[j, Column(i)].Text == dynTextbox[k, Column(i)].Text) && (dynTextbox[k, Column(i)].Text != ""))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool IsSquareOK(int i)
        {
            // is the same value already in the 3*3 square: return false
            int off1 = (i / 9) / 3;
            int off2 = ((i + 9) % 9) / 3;
            for (int reihen = 0 + off1 * 3; reihen < 3 + off1 * 3; reihen++)
            {
                for (int spalten = 0 + off2 * 3; spalten < 3 + off2 * 3; spalten++)
                {
                    if ((dynTextbox[Row(i), Column(i)].Text == dynTextbox[reihen, spalten].Text) && (Row(i) != reihen) && (Column(i) != spalten))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool CheckRow(int field, int n)
        {
            for (int s = 0; s < 9; s++)
            {
                if ((dynLabel[Row(field), s].Text.Contains(Convert.ToString(n))) && s != Column(field))
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckColumn(int field, int n)
        {
            for (int r = 0; r < 9; r++)
            {
                if ((dynLabel[r, Column(field)].Text.Contains(Convert.ToString(n))) && r != Row(field))
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckSquare(int field, int n)
        {
            // false if the number is already in the 3*3 square
            int off1 = (field / 9) / 3;
            int off2 = ((field + 9) % 9) / 3;
            for (int reihen = 0 + off1 * 3; reihen < 3 + off1 * 3; reihen++)
            {
                for (int spalten = 0 + off2 * 3; spalten < 3 + off2 * 3; spalten++)
                {
                    if ((dynLabel[reihen, spalten].Text.Contains(Convert.ToString(n))) && ((Row(field) != reihen) || (Column(field) != spalten)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private int Column(int i)
        {
            return i % 9;
        }

        private int Row(int i)
        {
            return i / 9;
        }

        private void DynLabelClick(object sender, EventArgs e)
        {
            int a = Cursor.Position.X - f.FindForm().Left;
            int b = Cursor.Position.Y - f.FindForm().Top;
            x = (b - 22) / 58;
            y = (a - 21) / 46;
            if (dynTextbox[x, y].Text != "")
            {
                dynLabel[x, y].Text = "";
            }
            if (dynLabel[x, y].Text != "")
            {
                Input form = new Input();
                form.TextOnForm = dynLabel[x, y].Text;
                form.Closed += new EventHandler(FormClosed);
                form.ShowDialog();
            }
        }

        private void FormClosed(object sender, EventArgs e)
        {
            Input form = sender as Input;
            if (form.DialogResult == DialogResult.OK)
            {
                dynLabel[x, y].Text = form.TextOnForm;
            }
            form.Closed -= new EventHandler(FormClosed);
        }
    }
}
