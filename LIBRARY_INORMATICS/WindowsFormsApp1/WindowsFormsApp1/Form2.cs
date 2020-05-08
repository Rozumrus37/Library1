using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
		public DataTable UsersMain = new DataTable();
		public Form2()
        {
            InitializeComponent();
        }
		private void UsersOfBooksDataBaseLoad()
		{
			UsersMain.Columns.Add("Name", typeof(string));
			UsersMain.Columns.Add("Surname", typeof(string));
			UsersMain.Columns.Add("Debts", typeof(int));

			XmlDocument users = new XmlDocument();
			users.Load("..\\..\\UsersOfBooksInfo.xml");

			XmlNodeList nodelist = users.DocumentElement.SelectNodes("*[local-name()='User']");

			foreach (XmlNode node in nodelist)
			{
				UsersMain.Rows.Add(
							node.SelectSingleNode("*[local-name()='name']").InnerText,
							node.SelectSingleNode("*[local-name()='surname']").InnerText,
							node.SelectSingleNode("*[local-name()='debts']").InnerText);
			}


			dataGridView1.DataSource = UsersMain;

		
		}

		private void AddNewUser()
		{
			List<User> Userzhi = new List<User>();

			XmlSerializer Serializer = new XmlSerializer(typeof(List<User>));

			foreach (DataRow row in UsersMain.Rows)
			{
				User UserTemp = new User();

				UserTemp.name = row["name"].ToString();
				UserTemp.surname = row["surname"].ToString();
				UserTemp.debts = Int32.Parse(row["debts"].ToString());


				Userzhi.Add(UserTemp);
			}
			using (FileStream UsersOfBooks = new FileStream("..\\..\\UsersOfBooksInfo.xml", FileMode.OpenOrCreate))
			{
				Serializer.Serialize(UsersOfBooks, Userzhi);
			}
		}

		private void button1_Click(object sender, EventArgs e)
        {
			AddNewUser();
			MessageBox.Show("Books' users have been successfully added");
        }

        private void Form2_Load(object sender, EventArgs e)
        {
			UsersOfBooksDataBaseLoad();
		}
    }
	[Serializable]
	public class User
	{

		public string name;
		public string surname;
		public int debts;
		public User() { }

		public User(string name, string surname, int debts)
		{
			this.name = name;
			this.surname = surname;
			this.debts = debts;
		}
	}
}
