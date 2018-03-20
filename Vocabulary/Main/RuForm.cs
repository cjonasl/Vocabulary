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
        private string _targetVocabularyFileFullPath;
        private string _fileNameInfoFileFullPath;
        private string _infoText;      
        string _newWord;
        private int _numberOfWords;
        private bool _isInNewWordState;
        private bool _infoTextIsShown;

        public Main()
        {
            int currentIndex;

            try
            {
                InitializeComponent();
                buttonBack.Visible = false;
                Utility.ReadConfig(Directory.GetCurrentDirectory(), out _targetVocabularyFileFullPath, out _fileNameInfoFileFullPath, out currentIndex);
                _infoText = Utility.ReturnFileContents(_fileNameInfoFileFullPath);
                _wordsAndExplanationArray = Utility.ReturnWordsAndExplanationArray(_targetVocabularyFileFullPath);
                _wordsArray = Utility.ReturnWordsArray(_wordsAndExplanationArray);
                _numberOfWords = _wordsArray.Count;
                this.Text = string.Format("File: Vocabulary{0}.txt (filled with {1} words)", _targetVocabularyFileFullPath, _numberOfWords.ToString());
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
                this.textBox1.Select(0, 0);
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

            if (!_infoTextIsShown)
            {
                _wordsAndExplanationArray[hScrollBar1.Value - 1] = this.textBox1.Text;
                Utility.Print(_targetVocabularyFileFullPath, _wordsAndExplanationArray);
                this.hScrollBar1.Enabled = true;

                if (_isInNewWordState)
                {
                    _isInNewWordState = false;
                    _numberOfWords++;

                    if (_wordsArray.IndexOf(_newWord) >= 0)
                    {
                        throw new Exception("_wordsArray.IndexOf(_newWord) >= 0");
                    }

                    _wordsArray.Add(_newWord);

                    this.Text = string.Format("File: Vocabulary{0}.txt (filled with {1} words)", _targetVocabularyFileFullPath, _numberOfWords.ToString());
                }

                this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            }
            else
            {
               _infoText = this.textBox1.Text;
               Utility.CreateNewFile(_fileNameInfoFileFullPath, _infoText);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.buttonCancel.Enabled = false;
            this.buttonSave.Enabled = false;

            if (!_infoTextIsShown)
            {
                this.textBox1.Text = _wordsAndExplanationArray[hScrollBar1.Value - 1];
                this.hScrollBar1.Enabled = true;
            }
            else
            {
                this.textBox1.Text = _infoText;
            }

            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            int currentIndex = hScrollBar1.Value - 1;
            Utility.CreateNewFile(Directory.GetCurrentDirectory() + "\\Config.txt", string.Format("{0}\r\n{1}\r\n{2}", _targetVocabularyFileFullPath, _fileNameInfoFileFullPath, currentIndex.ToString()));
        }

        private void Command_NewWord()
        {
            int index = _wordsArray.IndexOf(_newWord);

            if (index >= 0)
            {
                this.hScrollBar1.Value = 1 + index;
                hScrollBar1_Scroll(null, null);
            }
            else if (_numberOfWords < 1000)
            {
                if (MessageBox.Show("The word does not exist. Add the word?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _wordsAndExplanationArray[_numberOfWords] = _wordsAndExplanationArray[_numberOfWords].Replace("?????", _newWord);
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

        private void buttonRun_Click(object sender, EventArgs e)
        {
            string str = this.textBoxCommand.Text.Trim();

            if (string.IsNullOrEmpty(str))
            {
                MessageBox.Show("A command is not given!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] v = str.Split(' ');

            string command = v[0].Trim();

            switch(command)
            {
                case "f":
                    if ((v.Length == 1) || string.IsNullOrEmpty(v[1].Trim()))
                    {
                        MessageBox.Show("A word is not given!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        _newWord = v[1].Trim();
                        Command_NewWord();

                    }
                    break;
            }

        }

        private void buttonInfo_Click(object sender, EventArgs e)
        {
            this.textBox1.TextChanged -= new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.Text = _infoText;
            this.textBox1.Select(0, 0);
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            _infoTextIsShown = true;
            this.hScrollBar1.Enabled = false;
            this.label1.Enabled = false;
            this.buttonRun.Enabled = false;
            this.textBoxCommand.Enabled = false;
            this.buttonInfo.Enabled = false;
            this.buttonBack.Visible = true;
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.hScrollBar1.Enabled = true;
            this.label1.Enabled = true;
            this.buttonRun.Enabled = true;
            this.textBoxCommand.Enabled = true;
            this.buttonInfo.Enabled = true;
            this.buttonBack.Visible = false;
            hScrollBar1_Scroll(null, null);
        }
    }
}
