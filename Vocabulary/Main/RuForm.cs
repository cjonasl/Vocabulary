using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using RuFramework.RuConfigManager;
using System.Globalization;
using System.Reflection;
using System.Collections;
using StaticMethods;

namespace Main
{
    public partial class Main : Form
    {
        private string[] _wordsAndExplanationArray;
        private ArrayList _wordsArray;
        private string _dir;
        private string _file;
        private int _choosenFile;
        private int _numberOfWords;
        private bool _isInNewWordState;

        public Main()
        {
            int currentIndex;

            try
            {
                InitializeComponent();
                _dir = Directory.GetCurrentDirectory();
                Utility.ReadConfig(_dir, out _choosenFile, out currentIndex);
                _file = string.Format("{0}\\Vocabulary{1}.txt", _dir, _choosenFile);
                _wordsAndExplanationArray = Utility.ReturnWordsAndExplanationArray(_file);
                _wordsArray = Utility.ReturnWordsArray(_wordsAndExplanationArray);
                _numberOfWords = _wordsArray.Count;
                this.Text = string.Format("File: Vocabulary{0}.txt (filled with {1} words)", _choosenFile.ToString(), _numberOfWords.ToString());
                hScrollBar1.Value = 1 + currentIndex;
                this.buttonCancel.Enabled = false;
                this.buttonSave.Enabled = false;
                this.textBox1.Text = _wordsAndExplanationArray[hScrollBar1.Value - 1];
                this.label1.Text = hScrollBar1.Value.ToString();
                this.textBox1.Select(0, 0);
                _isInNewWordState = false;
                this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            }
            catch(Exception e)
            {
                this.textBox1.ForeColor = Color.Red;
                this.textBox1.Text = string.Format("An error occured! e.Message = {0}", e.Message);
            }
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            this.textBox1.TextChanged -= new System.EventHandler(this.textBox1_TextChanged);
            this.label1.Text = hScrollBar1.Value.ToString();
            this.textBox1.Text = _wordsAndExplanationArray[hScrollBar1.Value - 1];
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.textBox1.TextChanged -= new System.EventHandler(this.textBox1_TextChanged);
            this.buttonCancel.Enabled = true;
            this.buttonSave.Enabled = true;
            this.hScrollBar1.Enabled = false;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.buttonCancel.Enabled = false;
            this.buttonSave.Enabled = false;
            _wordsAndExplanationArray[hScrollBar1.Value - 1] = this.textBox1.Text;
            Utility.Print(_file, _wordsAndExplanationArray);
            this.hScrollBar1.Enabled = true;

            if (_isInNewWordState)
            {
                _isInNewWordState = false;
                _numberOfWords++;
                this.Text = string.Format("File: Vocabulary{0}.txt (filled with {1} words)", _choosenFile.ToString(), _numberOfWords.ToString());
            }

            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.buttonCancel.Enabled = false;
            this.buttonSave.Enabled = false;
            this.textBox1.Text = _wordsAndExplanationArray[hScrollBar1.Value - 1];
            this.hScrollBar1.Enabled = true;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            int currentIndex = hScrollBar1.Value - 1;
            Utility.CreateNewFile(_dir + "\\Config.txt", string.Format("{0}\r\n{1}", _choosenFile.ToString(), currentIndex.ToString()));
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            string word = this.textBoxFindWord.Text.Trim();

            if (string.IsNullOrEmpty(word))
            {
                MessageBox.Show("A word is not given!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int index = _wordsArray.IndexOf(word);

            if (index >= 0)
            {
                this.hScrollBar1.Value = 1 + index;
                hScrollBar1_Scroll(null, null);
            }
            else if (_numberOfWords < 1000)
            {
                if (MessageBox.Show("The word does not exist. Add the word?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _wordsAndExplanationArray[_numberOfWords] = _wordsAndExplanationArray[_numberOfWords].Replace("?????", word);
                    this.hScrollBar1.Value = 1 + _numberOfWords;
                    _isInNewWordState = true;
                    hScrollBar1_Scroll(null, null);
                }
            }
            else
            {
                MessageBox.Show("The word does not exist and can't be added in this file because it has already 1000 words.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
