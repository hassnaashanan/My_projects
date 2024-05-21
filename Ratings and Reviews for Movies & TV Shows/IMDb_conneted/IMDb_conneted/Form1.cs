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
    public partial class Form1 : Form
    {
        string ordb = "Data source=orcl;User Id=hr; Password=hr;";
            OracleConnection conn;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new OracleConnection(ordb);
            conn.Open();
            OracleCommand d = new OracleCommand();
            d.Connection = conn;
            d.CommandText = "select f_id from filems";
            d.CommandType = CommandType.Text;
            OracleDataReader s = d.ExecuteReader();
            while (s.Read())
            {
                comboBox1.Items.Add(s[0]);
            }
            s.Close();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "GetFilm_Titles";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("titles", OracleDbType.RefCursor, ParameterDirection.Output);
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                comboBox2.Items.Add(dr[0]);
            }
            dr.Close();

            

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            OracleCommand c = new OracleCommand();
            c.Connection = conn;
            c.CommandText = "select w_name from filems f,writer w where f.w_id=w.w_id and f_title=:title ";
            c.CommandType = CommandType.Text;
            c.Parameters.Add("title", comboBox2.SelectedItem.ToString());
            OracleDataReader dr = c.ExecuteReader();
            if(dr.Read())
            {
                textBox1.Text = dr[0].ToString();
            }
            dr.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "insert into filems values(:id,:title,null) ";
            cmd.Parameters.Add("id", textBox2.Text);
            cmd.Parameters.Add("title", comboBox2.Text);
           // cmd.Parameters.Add("writer", textBox1.Text);
            
            int r = cmd.ExecuteNonQuery();
            if(r!=-1)
            {
                comboBox2.Items.Add(comboBox2.Text);
                MessageBox.Show("New Film is added");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OracleCommand x = new OracleCommand();
            x.Connection = conn;
            x.CommandText = "GetRateFilm";
            x.CommandType = CommandType.StoredProcedure;
            x.Parameters.Add("id", comboBox1.SelectedItem.ToString());
            x.Parameters.Add("Rate", OracleDbType.Int32,ParameterDirection.Output);
            x.ExecuteNonQuery();
            
                string y = Convert.ToString(x.Parameters["Rate"].Value);
                textBox3.Text = y;
           
        }
    }
}
