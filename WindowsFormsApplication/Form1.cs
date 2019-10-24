using System;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;


namespace WindowsFormsApplication5._1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            
            FormBorderStyle = FormBorderStyle.FixedSingle;

            textBox2.PasswordChar = '*';
            textBox3.PasswordChar = '*';

            label3.Visible = false;//error message
            label3.Text = "error message";

            button2.Text = "register";
            textBox3.Visible = false;//repeat password
            label5.Visible = false;
            label4.Text = "Don't have an account?";
            label6.Visible = false;
            textBox4.Visible = false;//name textbox

        }

        public class Global//shared values between Form1 and Form2
        {
            public static string username;
            public static int id;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            int repeats = 0;//to find the place of the user in the table
            int showerr = 0;
            errtimer.Stop();
            Global.username = "";
            const string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=userdatabase.mdb; Persist Security Info=False;";

            if (button1.Text == "login")//the login menu
            {
                if (textBox2.Text == "" || textBox1.Text == "")
                {
                    errtimercounter = 0;
                    errtimer.Start();
                }
                else
                {
                    try//will try to find the user
                    {
                        OleDbConnection conn = new OleDbConnection(connectionString);
                        conn.Open();
                        string query = "SELECT * FROM table1";
                        OleDbCommand cmd = new OleDbCommand(query, conn);
                        OleDbDataReader rdr = cmd.ExecuteReader();
                        rdr.Read();
                        StringBuilder builder = new StringBuilder();
                        while (rdr.Read())
                        {
                            repeats++;                
                            if (rdr.GetString(1) == textBox1.Text && rdr.GetString(2) == textBox2.Text)
                            {
                                Global.username = rdr.GetString(3);
                                Global.id = repeats;
                                MessageBox.Show("ID " + repeats + ". Welcome user " + Global.username);
                                errtimer.Stop();
                            }
                            else if (rdr.GetString(1) != textBox1.Text)
                                showerr++;
                            else if (rdr.GetString(1) == textBox1.Text && rdr.GetString(2) != textBox2.Text)
                            {
                                MessageBox.Show("Wrong password");
                                errtimer.Stop();
                            }
                        }
                        if (repeats == showerr)
                            MessageBox.Show("Not such user in our database");

                        errtimer.Stop();
                        
                        conn.Close();                      
                    }
                    catch (Exception)
                    {
                        errtimercounter = 0;
                        if(Global.username == "")
                            errtimer.Start();
                    }

                    if (Global.username != "")
                    {
                        Form2 f2 = new Form2();
                        f2.Show();
                        this.Hide();
                    }
                }
            }
            else if (button1.Text == "register")//the register menu
            {
                samename = false;//to see if there's already such user
                try
                {
                    OleDbConnection conn1 = new OleDbConnection(connectionString);
                    conn1.Open();
                    string query1 = "SELECT * FROM table1";
                    OleDbCommand cmd1 = new OleDbCommand(query1, conn1);
                    OleDbDataReader rdr = cmd1.ExecuteReader();
                    rdr.Read();
                    StringBuilder builder = new StringBuilder();
                    while (rdr.Read())
                    {
                        if (rdr.GetString(1) == textBox1.Text)
                        {
                            samename = true;
                        }
                    }
                    conn1.Close();
                }
                catch(Exception)
                {
                    errtimercounter = 0;
                    errtimer.Start();
                }

                if (samename == true)
                {
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();                   
                    errtimercounter = 0;
                    errtimer.Start();

                }
                else if (textBox2.Text == "" || textBox3.Text == "" || (textBox2.Text == "" && textBox3.Text == "") || textBox2.Text != textBox3.Text || textBox1.Text == "")
                {
                    errtimercounter = 0;
                    errtimer.Start();
                }
                
                else
                {
                    try//try to create user
                    {
                        OleDbConnection conn = new OleDbConnection(connectionString);
                        conn.Open();

                        string uname, upass;
                        uname = textBox1.Text;
                        upass = textBox2.Text;

                        string query = ("Insert into table1 (uname, upass, name) values ('"+textBox1.Text+"', '"+textBox2.Text+ "', '"+textBox4.Text+"')");
                        OleDbCommand cmd = new OleDbCommand(query, conn);
                        int i = cmd.ExecuteNonQuery();

                        MessageBox.Show("User created");
                        conn.Close();
                        errtimer.Stop();
                        button2.PerformClick();
                        textBox1.Text = uname;
                    }
                    catch (Exception)
                    {
                        errtimercounter = 0;
                        errtimer.Start();
                    }
                }

            }
        }
        bool samename = false;
        public int errtimercounter;

        private void button2_Click(object sender, EventArgs e)//change menus
        {
            if (button2.Text == "register")
            {
                errtimer.Stop();
                label3.Visible = false;
                label4.Text = "Already have an account?";
                button2.Text = "login";
                button1.Text = "register";
                textBox3.Visible = true;
                label5.Visible = true;
                label6.Visible = true;
                textBox4.Visible = true;
                textBox1.Clear();
                textBox2.Clear();
            }
            else
            {
                errtimer.Stop();
                label3.Visible = false;
                label4.Text = "Don't have an account?";
                button2.Text = "register";
                button1.Text = "login";
                textBox3.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                textBox4.Visible = false;
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
            }
        }

        private void errtimer_Tick(object sender, EventArgs e)//the timer for the error messages
        {
            if (button1.Text == "login")
            {
                if (textBox1.Text == "")
                {
                    label3.Visible = true;
                    label3.Text = "Enter a user name";
                    errtimercounter++;
                }
                else if (textBox2.Text == "")
                {
                    label3.Visible = true;
                    label3.Text = "Enter a password";
                    errtimercounter++;
                }
                else if(Global.username == "")
                {
                    label3.Visible = true;
                    label3.Text = "No connection with the database";
                    errtimercounter++;
                }

                if (errtimercounter == 2)
                {
                    label3.Visible = false;
                    label3.Text = "error message";
                    errtimer.Stop();
                }
            }
            else if (button1.Text == "register")
            {
                if (samename == true)
                {
                    label3.Visible = true;
                    label3.Text = "Name already exists";
                    errtimercounter++;
                }
                else
                {
                    if (textBox1.Text == "")
                    {
                        label3.Visible = true;
                        label3.Text = "Enter a user name";
                        errtimercounter++;
                    }
                    else if (textBox2.Text == "" || textBox3.Text == "" || (textBox2.Text == "" && textBox3.Text == ""))
                    {
                        label3.Visible = true;
                        label3.Text = "Enter a password";
                        errtimercounter++;
                    }
                    else if (textBox2.Text != textBox3.Text)
                    {
                        label3.Visible = true;
                        label3.Text = "Invalid passwords";
                        errtimercounter++;
                    }
                    else
                    {
                        label3.Visible = true;
                        label3.Text = "No connection with the database";
                        errtimercounter++;
                    }

                    if (errtimercounter == 2)
                    {
                        label3.Visible = false;
                        label3.Text = "error message";
                        errtimer.Stop();
                    }
                }

                if (errtimercounter == 2)
                {
                    label3.Visible = false;
                    label3.Text = "error message";
                    errtimer.Stop();
                }


            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
    
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
