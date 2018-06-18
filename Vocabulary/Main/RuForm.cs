using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using StaticMethods;

namespace Main
{
    public partial class Main : Form
    {
        private string[] _wordsAndExplanationArray;
        private ArrayList _wordsArray, _exampleNotMasked, _exampleMasked, _translated;
        private string _targetVocabularyFileFullPath;
        private string _fileNameInfoFileFullPath;
        private string _infoText;      
        private int _numberOfWords;
        private bool _infoTextIsShown;

        public Main()
        {
            int currentIndex;
            string errorMessage;

            try
            {
                InitializeComponent();
                buttonBack.Visible = false;
                Utility.ReadConfig(Directory.GetCurrentDirectory(), out _targetVocabularyFileFullPath, out _fileNameInfoFileFullPath, out currentIndex);
                _infoText = Utility.ReturnFileContents(_fileNameInfoFileFullPath);
                _wordsAndExplanationArray = Utility.ReturnWordsAndExplanationArray(_targetVocabularyFileFullPath);
                _wordsArray = Utility.ReturnWordsArray(_wordsAndExplanationArray);
                _numberOfWords = _wordsArray.Count;

                if (!Utility.ExempelsAreOk(Utility.ReturnFileContents(_targetVocabularyFileFullPath), out errorMessage, out _exampleNotMasked, out _exampleMasked, out _translated))
                {
                    throw new Exception(errorMessage);
                }

                this.Text = string.Format("File: Vocabulary{0}.txt (filled with {1} words)", _targetVocabularyFileFullPath, _numberOfWords.ToString());
                hScrollBar1.Value = 1 + currentIndex;
                this.buttonCancel.Enabled = false;
                this.buttonSave.Enabled = false;
                this.textBox1.Text = _wordsAndExplanationArray[hScrollBar1.Value - 1];
                this.label1.Text = hScrollBar1.Value.ToString();
                this.textBox1.Select(0, 0);
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

        private void Command_NewWord(string newWord)
        {
            int index = _wordsArray.IndexOf(newWord);

            if (index >= 0)
            {
                this.hScrollBar1.Value = 1 + index;
                hScrollBar1_Scroll(null, null);
            }
            else if (_numberOfWords < 1000)
            {
                if (MessageBox.Show("The word does not exist. Add the word?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _wordsArray.Add(newWord);
                    _numberOfWords++;
                    _wordsAndExplanationArray[_numberOfWords - 1] = _wordsAndExplanationArray[_numberOfWords - 1].Replace("?????", newWord);
                    this.hScrollBar1.Value = _numberOfWords;                
                    Utility.Print(_targetVocabularyFileFullPath, _wordsAndExplanationArray);
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
                    if (v.Length != 2)
                    {
                        MessageBox.Show("Incorrect given parameter for command f. It should be f, one blank and then the word.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Command_NewWord(v[1].Trim());
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
