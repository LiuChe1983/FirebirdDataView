using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FirebirdDataView
{
    public partial class Form1 : Form
    {
        private FbDataBase fbDataBase;

        private void OpenFbDataBase()
        {

            if (openDbFileDialog.ShowDialog() != DialogResult.OK) return;

            string fileName = openDbFileDialog.FileName;
            tb_FileName.Text = fileName;

            fbDataBase = new FbDataBase(fileName);
            //MessageBox.Show(fileName);
            if (!fbDataBase.Open())
            {
                fbDataBase.Close();
                MessageBox.Show(fbDataBase.lastError,"打开数据库文件失败");
                return;
            }
            

            DataTable dt_AllTable = fbDataBase.GetAllTable();
            cb_Tables.DataSource = dt_AllTable.DefaultView;
            cb_Tables.DisplayMember = "TABLE_NAME";
            cb_Tables.ValueMember = "TABLE_NAME";
            label1.Text =string.Format("共有{0}个数据表:",dt_AllTable.Rows.Count);

            fbDataBase.Close();

            //dataGridView1.
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void o打开文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFbDataBase();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //fbDataBase.Close();
        }

        private void btn_Open_Click(object sender, EventArgs e)
        {
            OpenFbDataBase();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void cb_Tables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_Tables.SelectedIndex == -1) return;
            DataRowView view = (DataRowView)cb_Tables.SelectedItem;
            string tableName = view["TABLE_NAME"].ToString();
            if (tableName == "") return;

            DataTable dataTable = fbDataBase.GetDataTable(tableName);
            dataGridView1.DataSource = dataTable.DefaultView;

        }
    }
}
