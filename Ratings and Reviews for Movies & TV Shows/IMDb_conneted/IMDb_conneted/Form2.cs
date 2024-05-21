using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
namespace IMDb_conneted
{
    
    public partial class Form2 : Form
    {
        OracleDataAdapter adapter;
        OracleCommandBuilder builder;
        DataSet ds;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            

        }

        private void save_Click(object sender, EventArgs e)
        {
            builder = new OracleCommandBuilder(adapter);
            adapter.Update(ds.Tables[0]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string constr = "Data Source =orcl; User Id=hr;Password=hr;";
            string cmdstr = " ";

            if (User_Info.Checked)
                cmdstr = "select * from users";
            else if (radioButton2.Checked)
                cmdstr = "select * from filems";
            else if (radioButton1.Checked)
                cmdstr = "select * from rating";
            else if (radioButton3.Checked)
                cmdstr = "select * from reviews";
            else if (radioButton4.Checked)
                cmdstr = "select * from writer";


            adapter = new OracleDataAdapter(cmdstr, constr);

            ds = new DataSet();

            adapter.Fill(ds);

            dataGridView1.DataSource = ds.Tables[0];
        }
    }
}
