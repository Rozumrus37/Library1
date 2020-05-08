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
    public partial class Form1 : Form
    {
        private DataTable BooksMain = new DataTable();
        public Form1()
        {
            InitializeComponent();
        }

		private void BooksDataBaseLoad()
		{
			BooksMain.Columns.Add("Title", typeof(string));
			BooksMain.Columns.Add("Author", typeof(string));
			BooksMain.Columns.Add("Genre", typeof(string));
			BooksMain.Columns.Add("Pages", typeof(int));

			XmlDocument books = new XmlDocument();
			books.Load("..\\..\\BooksInfo.xml");

			XmlNodeList nodelist = books.DocumentElement.SelectNodes("*[local-name()='Book']");

			foreach (XmlNode node in nodelist)
			{
				BooksMain.Rows.Add(
							node.SelectSingleNode("*[local-name()='title']").InnerText,
							node.SelectSingleNode("*[local-name()='author']").InnerText,
							node.SelectSingleNode("*[local-name()='genre']").InnerText,
							node.SelectSingleNode("*[local-name()='pages']").InnerText);
			}


			dataGridView1.DataSource = BooksMain;

			
		}

		private void AddNewBook()
		{
			List<Book> Knihy = new List<Book>();

			XmlSerializer Serializer = new XmlSerializer(typeof(List<Book>));

			foreach (DataRow row in BooksMain.Rows)
			{
				Book BookTemp = new Book();

				BookTemp.title = row["title"].ToString();
				BookTemp.author = row["author"].ToString();
				BookTemp.genre = row["genre"].ToString();
				BookTemp.pages = Int32.Parse(row["pages"].ToString());


				Knihy.Add(BookTemp);
			}
			using (FileStream FileBooks = new FileStream("..\\..\\BooksInfo.xml", FileMode.OpenOrCreate))
			{
				Serializer.Serialize(FileBooks, Knihy);
			}
		}

		private void button1_Click(object sender, EventArgs e)
        {
			AddNewBook();
			MessageBox.Show("Books have been successfully added");
		}

        private void Form1_Load(object sender, EventArgs e)
        {
			BooksDataBaseLoad();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Hide();
			Form2 form2 = new Form2();
			form2.ShowDialog();
		}
	}
	[Serializable]
	public class Book
	{

		public string title;
		public string author;
		public string genre;
		public int pages;
		public Book() { }

		public Book(string title, string author, string genre, int pages)
		{
			this.title = title;
			this.author = author;
			this.genre = genre;
			this.pages = pages;
		}
	}
}
