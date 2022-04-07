using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FullTextSearch
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            listBox1.AllowDrop = true;
            listBox1.DragDrop += listBoxFiles_DragDrop;
            listBox1.DragEnter += listBoxFiles_DragEnter;

        }

        private void listBoxFiles_DragEnter(object sender, DragEventArgs e)
        {
            Debug.WriteLine("listBoxFiles_DragEnter");
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
            
        }

        private void listBoxFiles_DragDrop(object sender, DragEventArgs e)
        {
            Debug.WriteLine("listBoxFiles_DragDrop");
            // filename add
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                listBox1.Items.Add(file);
                // file line read
                string textValue = System.IO.File.ReadAllText(file);
                Debug.WriteLine(textValue);

            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
