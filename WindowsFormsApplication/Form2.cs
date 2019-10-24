using System;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Drawing;
using System.Text;



namespace WindowsFormsApplication5._1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Size = new Size(430, 270);//the minimum size
            textBox1.Enabled = false;
        }

        public string name;
        public int id;

        public void Form2_Load(object sender, EventArgs e)//load form
        {
            name = Form1.Global.username;//the shared variables
            id = Form1.Global.id;
            textBox1.Text ="User " + id +": "+ name;
            const string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=userdatabase.mdb; Persist Security Info=False;";

            try//try each time to initialize the window with the user's inputs and choices
            {//the path for the profile image
                OleDbConnection conn = new OleDbConnection(connectionString);
                conn.Open();
                string query = "SELECT * FROM table1";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                OleDbDataReader rdr = cmd.ExecuteReader();
                rdr.Read();
                StringBuilder builder = new StringBuilder();

                while (rdr.Read())
                {
                    if(rdr.GetString(3) == name)
                        pictureBox1.ImageLocation = @rdr.GetString(7);
                }
            }
            catch (Exception) { }//if there's no input it will not load the choice

            try
            {//the background color
                OleDbConnection conn = new OleDbConnection(connectionString);
                conn.Open();
                string query = "SELECT * FROM table1";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                OleDbDataReader rdr = cmd.ExecuteReader();
                rdr.Read();
                StringBuilder builder = new StringBuilder();

                while (rdr.Read())
                {
                    if (rdr.GetString(3) == name)
                    {
                        this.BackColor = Color.FromName(rdr.GetString(6));
                    }
                        
                }
            }
            catch (Exception) { }

            try
            {//the dimensions of the window
                OleDbConnection conn = new OleDbConnection(connectionString);
                conn.Open();
                string query = "SELECT * FROM table1";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                OleDbDataReader rdr = cmd.ExecuteReader();
                rdr.Read();
                StringBuilder builder = new StringBuilder();

                while (rdr.Read())
                {
                    if (rdr.GetString(3) == name)
                        this.Size = new Size(Int32.Parse(rdr.GetString(4)), Int32.Parse(rdr.GetString(5)));
                }
            }
            catch (Exception) { }

            
        }

        private void button1_Click(object sender, EventArgs e)//logout button
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
            this.BackColor = SystemColors.Control;
            
        }

        private void button2_Click(object sender, EventArgs e)//delete user button
        {

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete your profile?", "Delete", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string connectionstring = "Provider=Microsoft.Jet.OleDb.4.0; Data Source=userdatabase.mdb";
                OleDbConnection conn = new OleDbConnection(connectionstring);
                conn.Open();
                string query = "Delete from table1 where name='" + name + "'";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                int i = cmd.ExecuteNonQuery();
                conn.Close();
                Form1 f1 = new Form1();
                f1.Show();
                this.Hide();
            }
            
        }


        public Color colorn;

        private void button7_Click(object sender, EventArgs e)//color button
        {
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=userdatabase.mdb; Persist Security Info=False;";

            ColorDialog MyDialog = new ColorDialog();//create a color dialog
            MyDialog.SolidColorOnly = true;
            MyDialog.AllowFullOpen = true;
            MyDialog.ShowHelp = true;

            if (MyDialog.ShowDialog() == DialogResult.OK)//choosing a non known color will not generate it as backcolor
            {
                BackColor = MyDialog.Color;
                colorn = MyDialog.Color;
                MessageBox.Show(MyDialog.Color + colorn.ToString());
                OleDbConnection conn = new OleDbConnection(connectionString);
                conn.Open();

                string query = "Update table1 set color='" + colorn.ToKnownColor() + "' where name='" + name + "'";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                int i = cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public string path;

        private void button6_Click(object sender, EventArgs e)//image button
        {
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=userdatabase.mdb; Persist Security Info=False;";

            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
            if (of.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = of.FileName;
                path = pictureBox1.ImageLocation;

                OleDbConnection conn = new OleDbConnection(connectionString);
                conn.Open();

                string query = "Update table1 set path='" +path + "' where name='" +name+ "'";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                int i = cmd.ExecuteNonQuery();
                conn.Close();
            };
        }

        int parse = 1;
        private void button3_Click(object sender, EventArgs e)//dimensions button
        {
            const string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=userdatabase.mdb; Persist Security Info=False;";

            if (textBox2.Text != null && textBox2.Text != null)//input something
            {
                if (Int32.TryParse(textBox2.Text, out parse) == true && Int32.TryParse(textBox3.Text, out parse) == true)//it must be an integer
                {
                    if (Int64.Parse(textBox2.Text) >= 430 && Int64.Parse(textBox3.Text) >= 270)//it can't be smaller than the minimum window size
                    {
                        OleDbConnection conn = new OleDbConnection(connectionString);
                        conn.Open();

                        string query = "Update table1 set X='" + textBox2.Text + "' where name='" + name + "'";
                        OleDbCommand cmd = new OleDbCommand(query, conn);
                        int i = cmd.ExecuteNonQuery();
                        string query1 = "Update table1 set Y='" + textBox3.Text + "' where name='" + name + "'";
                        OleDbCommand cmd1 = new OleDbCommand(query1, conn);
                        int j = cmd1.ExecuteNonQuery();
                        conn.Close();

                        this.Size = new Size((Int32.Parse(textBox2.Text)), (Int32.Parse(textBox3.Text)));
                    }
                    else
                    {
                        MessageBox.Show("Minimum width 430 and minimum heigth 270!");
                        textBox2.Clear();
                        textBox3.Clear();
                    }
                }
                else
                { 
                    MessageBox.Show("Only integers!");
                    textBox2.Clear();
                    textBox3.Clear();
                }
            }
            else
                MessageBox.Show("Input the dimensions!");
        }
    }
}
