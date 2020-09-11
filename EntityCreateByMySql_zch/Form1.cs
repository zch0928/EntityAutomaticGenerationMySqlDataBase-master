using EntityCreateByMySql_zch.entity;
using EntityCreateByMySql_zch.service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace EntityCreateByMySql_zch
{
    public partial class Form1 : Form
    {
        string connectionStr = "";
        public Form1()
        {
            InitializeComponent();
        }

        CreateService ser = new CreateService();
        private void button1_Click(object sender, EventArgs e)
        {
            string server = this.textBox1.Text.Trim();
            string name = this.textBox2.Text.Trim();
            string pwd = this.textBox3.Text.Trim();
            connectionStr = string.Format("server={0};port=3306;user={1};password={2};",server,name,pwd);
            List<string> dataBaseList = ser.queryAllDataBase(connectionStr);
            if (dataBaseList != null)
            {
                this.comboBox1.Items.Clear();
                foreach (string s in dataBaseList)
                {
                    this.comboBox1.Items.Add(s);
                }
                this.comboBox1.Enabled = true;
                this.comboBox1.SelectedIndex = 0;
                this.button1.Enabled = false;
                this.button2.Enabled = true;
                this.textBox6.Text = "连接成功！";
            }
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> dataTableList = ser.queryAllDataTable(connectionStr,this.comboBox1.SelectedItem.ToString().Trim());
            this.checkedListBox1.Items.Clear();
            foreach (string s in dataTableList)
            {
                this.checkedListBox1.Items.Add(s);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int count = this.checkedListBox1.Items.Count;
            if (count == 0)
            {
                MessageBox.Show("表为空","提示",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                return;
            }
            for (int i = 0; i < count; i++)
            {
                this.checkedListBox1.SetItemChecked(i,true);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int count = this.checkedListBox1.Items.Count;
            if (count == 0)
            {
                MessageBox.Show("表为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            for (int i = 0; i < count; i++)
            {
                this.checkedListBox1.SetItemChecked(i,false);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int count = this.checkedListBox1.Items.Count;
            if (count == 0)
            {
                MessageBox.Show("表为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            for (int i = 0; i < count; i++)
            {
                this.checkedListBox1.SetItemChecked(i,!this.checkedListBox1.GetItemChecked(i));
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                this.label6.Text = "命名空间：";
            }
            else
            {
                this.label6.Text = "包：";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int count = this.checkedListBox1.Items.Count;
            if (count == 0)
            {
                MessageBox.Show("表为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (this.checkedListBox1.CheckedItems.Count == 0)
            {
                MessageBox.Show("未选中任何表！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string path = this.textBox4.Text.Trim();
            if (path == "")
            {
                MessageBox.Show("路径不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (this.radioButton1.Checked)
            {
                for (int i = 0; i < count; i++)
                {
                    if (this.checkedListBox1.GetItemChecked(i))
                    {
                        string tableName = this.checkedListBox1.Items[i].ToString().Trim();
                        connectionStr += "database=" + this.comboBox1.SelectedItem.ToString().Trim() + ";";
                        List<TableStructure> structureList = ser.queryTableStructure(connectionStr, tableName);
                        if (this.textBox5.Text.Trim() == "")
                        {
                            this.textBox5.Text = "命名空间";
                        }
                        string fileContent = ser.createFileContent(structureList, this.textBox5.Text.Trim(), tableName);
                        System.IO.File.WriteAllText(path + "\\" + tableName + ".cs", fileContent);
                        this.textBox6.Text = this.textBox6.Text + "\n" + tableName + "生成成功！";
                    }
                }
            }else if (this.radioButton2.Checked)
            {
                for (int i = 0; i < count; i++)
                {
                    if (this.checkedListBox1.GetItemChecked(i))
                    {
                        string tableName = this.checkedListBox1.Items[i].ToString().Trim();
                        connectionStr += "database=" + this.comboBox1.SelectedItem.ToString().Trim() + ";";
                        List<TableStructure> structureList = ser.queryTableStructure(connectionStr, tableName);
                        foreach (TableStructure item in structureList)
                        {
                            if (item.Type1.Equals("string"))
                            {
                                item.Type1 = "String";
                            }
                        }
                        if (this.textBox5.Text.Trim() == "")
                        {
                            this.textBox5.Text = "包名";
                        }
                        string fileContent = ser.createJavaFileContent(structureList, this.textBox5.Text.Trim(), tableName);
                        System.IO.File.WriteAllText(path + "\\" + tableName + ".java", fileContent);
                        this.textBox6.Text = this.textBox6.Text + "\n" + tableName + "生成成功！";
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.textBox4.Text = System.IO.Directory.GetCurrentDirectory();
        }
    }
}
