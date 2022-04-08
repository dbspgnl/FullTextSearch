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

    /*
        1. 버튼을 클릭하면 검색창 단어와 리스트 박스의 경로를 찾음 v
        2. 리스트 박스의 경로를 통해서 파일 안에 데이터를 읽음 v
        3. 파일 안에 데이터에 검색어가 포함되어 있으면 해당 리스트에 색깔 표시
     */


    public partial class Form1 : Form
    {
        string[] files = null;

        public Form1()
        {
            InitializeComponent();

            listBox1.AllowDrop = true;
            listBox1.DragDrop += listBoxFiles_DragDrop;
            listBox1.DragEnter += listBoxFiles_DragEnter;
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox1.DrawItem += new DrawItemEventHandler(listBox1_DrawItem);
            textBox1.KeyDown += textBox1_KeyDown;
        }

        private void listBoxFiles_DragEnter(object sender, DragEventArgs e)
        {
            Debug.WriteLine("listBoxFiles_DragEnter");
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
            
        }

        private void listBoxFiles_DragDrop(object sender, DragEventArgs e)
        {
            Debug.WriteLine("listBoxFiles_DragDrop");
            files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                listBox1.Items.Add(file);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.SelectionMode = SelectionMode.MultiExtended; // 다중 선택 모드
            string result = "";
            string word = textBox1.Text;
            List<int> indexList = new List<int>();

            // selected 초기화
            listBox1.SelectedIndex = -1;

            foreach (var input_items in listBox1.Items) 
            {
                // 파일 경로 담기
                result += string.Format("{0} ", input_items);
                string textValue = System.IO.File.ReadAllText(input_items.ToString());
                Debug.WriteLine(textValue);
                // 해당하는 인덱스 담기
                if (textValue.Contains(word, StringComparison.OrdinalIgnoreCase))
                {
                    Debug.WriteLine("글자가 포함되어 있습니다.");
                    indexList.Add(listBox1.Items.IndexOf(input_items));
                }

            }
            Debug.WriteLine(indexList);
            //해당하는 데이터 selected 하기
            foreach (var index in indexList)
            {
                listBox1.SetSelected(index, true);
            }

            Debug.WriteLine("button1_Click(list): "+ result);
            Debug.WriteLine("button1_Click(word): " + word);
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            bool isItemSelected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);
            int itemIndex = e.Index;
            if (itemIndex >= 0 && itemIndex < listBox1.Items.Count)
            {
                Graphics g = e.Graphics;

                // Background Color
                SolidBrush backgroundColorBrush = new SolidBrush((isItemSelected) ? Color.Red : Color.White);
                g.FillRectangle(backgroundColorBrush, e.Bounds);

                // Set text color
                string itemText = listBox1.Items[itemIndex].ToString();

                SolidBrush itemTextColorBrush = (isItemSelected) ? new SolidBrush(Color.White) : new SolidBrush(Color.Black);
                g.DrawString(itemText, e.Font, itemTextColorBrush, listBox1.GetItemRectangle(itemIndex).Location);

                // Clean up
                backgroundColorBrush.Dispose();
                itemTextColorBrush.Dispose();
            }

            e.DrawFocusRectangle();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

    }
}
