using System.Windows.Forms;

namespace sudoku
{
    public partial class Input : Form
    {
        public Input()
        {
            InitializeComponent();
        }

        public string TextOnForm
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        void InputKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                e.Handled = true;
                DialogResult = DialogResult.OK;
                Close();
            }
            if (e.KeyData == Keys.Escape)
            {
                e.Handled = true;
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }
}
