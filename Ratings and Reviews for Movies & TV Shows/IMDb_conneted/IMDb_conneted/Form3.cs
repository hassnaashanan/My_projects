using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IMDb_conneted
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 n = new Form1();
            n.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 n = new Form2();
            n.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form4 n = new Form4();
            n.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form5 n = new Form5();
            n.Show();
        }
    }
}
