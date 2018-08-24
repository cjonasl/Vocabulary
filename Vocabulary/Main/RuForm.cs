using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using StaticMethods;
using System.Text;

namespace Main
{
    public partial class Main : Form
    {
        private string[] _wordsAndExplanationArray;
        private ArrayList _wordsArray, _exempelUnmasked, _exempelMasked, _exempelTranslated;
        private string _targetVocabularyFileFullPath;
        private string _fileNameInfoFileFullPath;
        private string _infoText;      
        private int _numberOfWords;
        private bool _infoTextIsShown, _applicationRandomSampleOfVocabularieIsRunning;
        private LocationSizeOfMainFormAndTextBox _locationSizeOfMainFormAndTextBox;
        private StudyRandomSampleOfVocabularies _studyRandomSampleOfVocabularies;
        private bool _setLocationSizeOfMainFormAndTextBoxManuallyInCode;

        public Main()
        {
            int currentIndex;
            string errorMessage;

            try
            {
                InitializeComponent();

                Utility.ReadConfig(Directory.GetCurrentDirectory(), out _targetVocabularyFileFullPath, out _fileNameInfoFileFullPath, out currentIndex, out _locationSizeOfMainFormAndTextBox, out _setLocationSizeOfMainFormAndTextBoxManuallyInCode);

                if (_setLocationSizeOfMainFormAndTextBoxManuallyInCode)
                {
                    int mx = -7, my = 0, mw = 1380, mh = 735, tx = 15, ty = 110, tw = 1340, th = 565; //Default values

                    if ((Screen.PrimaryScreen.WorkingArea.Width == 1366) && (Screen.PrimaryScreen.WorkingArea.Height == 728))
                    {
                        mx = -7;
                        my = 0;
                        mw = 1380;
                        mh = 735;
                        tx = 15;
                        ty = 110;
                        tw = 1340;
                        th = 565;
                    }
                    else if ((Screen.PrimaryScreen.WorkingArea.Width == 1440) && (Screen.PrimaryScreen.WorkingArea.Height == 860))
                    {
                        mx = -10;
                        my = 0;
                        mw = 1460;
                        mh = 867;
                        tx = 15;
                        ty = 110;
                        tw = 1400;
                        th = 700;
                    }

                    this.Location = new Point(mx, my);
                    this.Size = new Size(mw, mh);
                    this.textBox1.Location = new Point(tx, ty);
                    this.textBox1.Size = new Size(tw, th);
                }
                else
                {
                    UpdateLocationAndSiseForMainFormAndTextBox();
                }

                _infoText = Utility.ReturnFileContents(_fileNameInfoFileFullPath);
                _wordsAndExplanationArray = Utility.ReturnWordsAndExplanationArray(_targetVocabularyFileFullPath);
                _wordsArray = Utility.ReturnWordsArray(_wordsAndExplanationArray);
                _numberOfWords = _wordsArray.Count;

                if (!Utility.ExempelsAreOk(Utility.ReturnFileContents(_targetVocabularyFileFullPath), out errorMessage, out _exempelUnmasked, out _exempelMasked, out _exempelTranslated))
                {
                    throw new Exception(errorMessage);
                }

                this.Text = string.Format("File: Vocabulary{0}.txt (filled with {1} words)", _targetVocabularyFileFullPath, _numberOfWords.ToString());
                hScrollBar1.Value = 1 + currentIndex;
                this.buttonCancel.Enabled = false;
                this.buttonSave.Enabled = false;
                this.buttonBack.Enabled = false;
                this.textBox1.Text = _wordsAndExplanationArray[hScrollBar1.Value - 1];
                this.label1.Text = hScrollBar1.Value.ToString();
                this.textBox1.Select(0, 0);
                this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);

                _applicationRandomSampleOfVocabularieIsRunning = false;
            }
            catch(Exception e)
            {
                this.buttonSave.Enabled = false;
                this.buttonCancel.Enabled = false;
                this.buttonRun.Enabled = false;
                this.buttonInfo.Enabled = false;
                this.buttonBack.Enabled = false;
                this.hScrollBar1.Enabled = false;
                this.textBoxCommand.Enabled = false;
                this.label1.Enabled = false;
                this.labelFindWord.Enabled = false;
                this.textBox1.ForeColor = Color.Red;
                this.textBox1.ReadOnly = true;
                this.textBox1.Text = string.Format("An error occured! e.Message:\r\n{0}", e.Message);
                this.textBox1.Select(0, 0);
            }
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            if (!_applicationRandomSampleOfVocabularieIsRunning)
            {
                this.textBox1.TextChanged -= new System.EventHandler(this.textBox1_TextChanged);
            }

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

        private bool TestExempelsAreOk()
        {
            bool returnValue = true;
            string errorMessage;
            ArrayList exempelUnmasked, exempelMasked, exempelTranslated;

            try
            {
                if (!Utility.ExempelsAreOk(this.textBox1.Text, out errorMessage, out exempelUnmasked, out exempelMasked, out exempelTranslated))
                {
                    returnValue = false;
                }
            }
            catch
            {
                returnValue = false;
            }

            return returnValue;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (!_infoTextIsShown)
            {
                if (!TestExempelsAreOk())
                {
                    MessageBox.Show("Can't save because there is something wrong!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    _wordsAndExplanationArray[hScrollBar1.Value - 1] = this.textBox1.Text;
                    Utility.Print(_targetVocabularyFileFullPath, _wordsAndExplanationArray);
                    this.hScrollBar1.Enabled = true;
                    this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
                }
            }
            else
            {
               _infoText = this.textBox1.Text;
               Utility.CreateNewFile(_fileNameInfoFileFullPath, _infoText);
            }

            this.buttonCancel.Enabled = false;
            this.buttonSave.Enabled = false;
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
            Utility.CreateNewFile(Directory.GetCurrentDirectory() + "\\Config.txt", string.Format("{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}", _targetVocabularyFileFullPath, _fileNameInfoFileFullPath, currentIndex.ToString(), Utility.ReturnString(_locationSizeOfMainFormAndTextBox), _setLocationSizeOfMainFormAndTextBoxManuallyInCode.ToString()));
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

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            string fileName = string.Format("Vocabulary_{0}.txt", DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss"));

            if(!Directory.Exists("C:\\tmp"))
            {
                Directory.CreateDirectory("C:\\tmp");
            }

            Utility.CreateNewFile("C:\\tmp\\" + fileName, this.textBox1.Text);

            MessageBox.Show("The followinf file was created in folder C:\\tmp\r\n" + fileName, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Command_p1(int maxRowLength)
        {
            string str = "";

            if (_numberOfWords > 0)
            {
                StringBuilder sb = new StringBuilder((string)_wordsArray[0]);
                StringBuilder tmp = new StringBuilder((string)_wordsArray[0]);

                for (int i = 1; i < _numberOfWords; i++)
                {
                    str = tmp.ToString() + string.Format(", {0}", (string)_wordsArray[i]);

                     if (str.Length > maxRowLength)
                    {
                        sb.Append(string.Format("\r\n{0}", (string)_wordsArray[i]));
                        tmp.Clear();
                        tmp.Append(string.Format("{0}", (string)_wordsArray[i]));
                    }
                    else
                    {
                        sb.Append(string.Format(", {0}", (string)_wordsArray[i]));
                        tmp.Append(string.Format(", {0}", (string)_wordsArray[i]));
                    }
                }

                str = sb.ToString();
            }

            Print(str);
        }

        private void Command_em()
        {
            string str = Utility.ReturnString(_exempelMasked);
            Print(str);
        }

        private void Command_eum()
        {
            string str = Utility.ReturnString(_exempelUnmasked);
            Print(str);
        }

        private void Command_et()
        {
            string str = Utility.ReturnString(_exempelTranslated);
            Print(str);
        }

        private void Command_ls()
        {
            string message = string.Format("Main form: (lx,ly,w,h)=({0},{1},{2},{3})\r\nTextbox: (lx,ly,w,h)=({4},{5},{6},{7})",
                this.Location.X.ToString(),
                this.Location.Y.ToString(),
                this.Size.Width.ToString(),
                this.Size.Height.ToString(),
                this.textBox1.Location.X.ToString(),
                this.textBox1.Location.Y.ToString(),
                this.textBox1.Size.Width.ToString(),
                this.textBox1.Size.Height.ToString());

            MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Command_ps()
        {
            string message = string.Format("(X,Y,Width,Height)=({0},{1},{2},{3})",
                Screen.PrimaryScreen.WorkingArea.X,
                Screen.PrimaryScreen.WorkingArea.Y,
                Screen.PrimaryScreen.WorkingArea.Width,
                Screen.PrimaryScreen.WorkingArea.Height);

            MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateLocationAndSiseForMainFormAndTextBox()
        {
            this.Location = new Point(_locationSizeOfMainFormAndTextBox.mlx, _locationSizeOfMainFormAndTextBox.mly);
            this.Size = new Size(_locationSizeOfMainFormAndTextBox.msw, _locationSizeOfMainFormAndTextBox.msh);
            this.textBox1.Location = new Point(_locationSizeOfMainFormAndTextBox.tlx, _locationSizeOfMainFormAndTextBox.tly);
            this.textBox1.Size = new Size(_locationSizeOfMainFormAndTextBox.tsw, _locationSizeOfMainFormAndTextBox.tsh);
        }

        private void Command_sls(string[] v)
        {
            try
            {
                _locationSizeOfMainFormAndTextBox.mlx = int.Parse(v[1]);
                _locationSizeOfMainFormAndTextBox.mly = int.Parse(v[2]);
                _locationSizeOfMainFormAndTextBox.msw = int.Parse(v[3]);
                _locationSizeOfMainFormAndTextBox.msh = int.Parse(v[4]);
                _locationSizeOfMainFormAndTextBox.tlx = int.Parse(v[5]);
                _locationSizeOfMainFormAndTextBox.tly = int.Parse(v[6]);
                _locationSizeOfMainFormAndTextBox.tsw = int.Parse(v[7]);
                _locationSizeOfMainFormAndTextBox.tsh = int.Parse(v[8]);
                UpdateLocationAndSiseForMainFormAndTextBox();
            }
            catch(Exception e)
            {
                MessageBox.Show("An error occured when the command was executed. Error message:\r\n" + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int ReturnNumberOfEntriesWithAtLeastOneExempel()
        {
            int numberOfEntriesWithAtLeastOneExempel = 0;

            for (int i = 0; i < _wordsArray.Count; i++)
            {
                if (_wordsAndExplanationArray[i].IndexOf("Exempel: ") >= 0)
                {
                    numberOfEntriesWithAtLeastOneExempel++;
                }
            }

            return numberOfEntriesWithAtLeastOneExempel;
        }

        private void Command_we()
        {
            int numberOfEntriesWithAtLeastOneExempel = ReturnNumberOfEntriesWithAtLeastOneExempel();
            MessageBox.Show("Number of entries with at least one Exempel = " + numberOfEntriesWithAtLeastOneExempel.ToString(), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Command_study()
        {
            int n, total;
            bool isFinished;

            this.textBox1.Text = _studyRandomSampleOfVocabularies.Next(out n, out total, out isFinished);
            this.Text = string.Format("Show {0} of {1}", n.ToString(), total.ToString());

            if (isFinished)
            {
                this.buttonRun.Enabled = false;
            }
        }
        private void Command_wei()
        {
            string str = Utility.ReturnInfoAboutNumberOfWordsThatHaveAndNotHaveExample(_wordsAndExplanationArray);
            Print(str);
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            if (_applicationRandomSampleOfVocabularieIsRunning)
            {
                Command_study();
                return;
            }

            string str = this.textBoxCommand.Text.Trim();
            int n;

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
                        MessageBox.Show("Incorrect given parameter for command f! It should be f, one blank and then the word.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Command_NewWord(v[1].Trim());
                    }
                    break;
                case "p1":
                    if ((v.Length != 2) || (!int.TryParse(v[1].Trim(), out n)))
                    {
                        MessageBox.Show("Incorrect given parameter for command p1. It should be p1, one blank and then an integer giving max row length.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Command_p1(n);
                    }
                    break;
                case "eum": //Exempel unmasked
                    if (v.Length != 1)
                    {
                        MessageBox.Show("Command \"eum\" should not have any parameters!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Command_eum();
                    }
                    break;
                case "em": //Exempel masked
                    if (v.Length != 1)
                    {
                        MessageBox.Show("Command \"em\" should not have any parameters!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Command_em();
                    }
                    break;
                case "et": //Exempel translated
                    if (v.Length != 1)
                    {
                        MessageBox.Show("Command \"et\" should not have any parameters!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Command_et();
                    }
                    break;
                case "ls": //ls (= Show location and size of of main form and textbox)
                    if (v.Length != 1)
                    {
                        MessageBox.Show("Command \"ls\" should not have any parameters!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Command_ls();
                    }
                    break;
                case "sls": //sls (= Set location and size of of main form and textbox)
                    if (v.Length != 9)
                    {
                        MessageBox.Show("Command \"sls\" should have 8 parameters!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Command_sls(v);
                    }
                    break;
                case "we": //we (= With example, number of entries in array wordsAndExplanationArray with at least on "Exempel: "
                    if (v.Length != 1)
                    {
                        MessageBox.Show("Command \"we\" should not have any parameters!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Command_we();
                    }
                    break;
                case "study": //study fromWord (an integer) toWord (an integer) showWordFirst (true or false)
                    if (v.Length != 4)
                    {
                        MessageBox.Show("Command \"study\" should have 3 parameters!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        int from, to, numberOfEntriesWithAtLeastOneExempel;
                        bool showWordFirst;

                        numberOfEntriesWithAtLeastOneExempel = ReturnNumberOfEntriesWithAtLeastOneExempel();

                        if (!int.TryParse(v[1], out from))
                        {
                            MessageBox.Show("First parameter is not a valid integer!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if ((from < 1) || (from > numberOfEntriesWithAtLeastOneExempel))
                        {
                            MessageBox.Show(string.Format("First parameter must be an integer between 1 and {0}!", numberOfEntriesWithAtLeastOneExempel.ToString()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (!int.TryParse(v[2], out to))
                        {
                            MessageBox.Show("Second parameter is not a valid integer!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (to < from)
                        {
                            MessageBox.Show(string.Format("Second parameter must be less than first parameter!", numberOfEntriesWithAtLeastOneExempel.ToString()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (to > numberOfEntriesWithAtLeastOneExempel)
                        {
                            MessageBox.Show(string.Format("Second parameter must be less than or equal to {0}!", numberOfEntriesWithAtLeastOneExempel.ToString()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (!bool.TryParse(v[3], out showWordFirst))
                        {
                            MessageBox.Show(string.Format("Third parameter must be true or false!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error));
                            return;
                        }

                        _studyRandomSampleOfVocabularies = new StudyRandomSampleOfVocabularies(_wordsAndExplanationArray, _wordsArray, from, to, showWordFirst);
                        this.hScrollBar1.Enabled = false;
                        this.label1.Enabled = false;
                        this.textBoxCommand.Enabled = false;
                        this.buttonInfo.Enabled = false;
                        this.buttonBack.Enabled = true;
                        this.buttonPrint.Enabled = false;
                        this.textBox1.TextChanged -= new System.EventHandler(this.textBox1_TextChanged);
                        _applicationRandomSampleOfVocabularieIsRunning = true;

                        Command_study();
                    }
                    break;
                case "wei": //Word Example Information (information about how many words that have and not have example)
                    if (v.Length != 1)
                    {
                        MessageBox.Show("Command \"wei\" should not have any parameters!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Command_wei();
                    }
                    break;
                case "ps": //Show X, Y, Width and Heigh of primary screen
                    if (v.Length != 1)
                    {
                        MessageBox.Show("Command \"ps\" should not have any parameters!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Command_ps();
                    }
                    break;
                default:
                    MessageBox.Show(string.Format("The command \"{0}\" does not exist!", command), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            this.buttonBack.Enabled = true;
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.hScrollBar1.Enabled = true;
            this.label1.Enabled = true;
            this.buttonRun.Enabled = true;
            this.textBoxCommand.Enabled = true;
            this.buttonInfo.Enabled = true;
            this.buttonBack.Enabled = false;
            hScrollBar1_Scroll(null, null);

            if (this.textBox1.ReadOnly)
            {
                this.textBox1.ReadOnly = false;
            }

            if (_infoTextIsShown)
            {
                _infoTextIsShown = false;
            }

            if (_applicationRandomSampleOfVocabularieIsRunning)
            {
                _applicationRandomSampleOfVocabularieIsRunning = false;
                _studyRandomSampleOfVocabularies = null;
                buttonPrint.Enabled = true;

                if (!this.buttonRun.Enabled)
                {
                    this.buttonRun.Enabled = true;
                }
            }

            this.textBox1.Select(0, 0);
        }

        private void Print(string text)
        {
            this.textBox1.TextChanged -= new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.Text = text;
            this.textBox1.Select(0, 0);
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.hScrollBar1.Enabled = false;
            this.textBox1.ReadOnly = true;
            this.label1.Enabled = false;
            this.buttonRun.Enabled = false;
            this.textBoxCommand.Enabled = false;
            this.buttonInfo.Enabled = false;
            this.buttonBack.Enabled = true;
        }
    }

    public struct LocationSizeOfMainFormAndTextBox
    {
        public int mlx, mly, msw, msh, tlx, tly, tsw, tsh;
    }
}
