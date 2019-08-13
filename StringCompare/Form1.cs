using System;
using System.IO;
using System.Windows.Forms;

namespace StringCompare
{
    public partial class Form1 : Form
    {

        public string input1 = string.Empty;
        public string input2 = string.Empty;

        public static int distance = 0;
        public static double percentage = 0;

        bool checkFirstUse = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            input1 = textBox2.Text;
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            input2 = textBox1.Text;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            percentage = similarity(input1, input2);
            richTextBox1.Text = "";
            richTextBox1.Text += percentage;
            canWeDeleteThisTopic();
        }

        public static double similarity(string s1, string s2)
        {

            string longer = s1, shorter = s2;
            if (s1.Length < s2.Length)
            { // longer should always have greater length
                longer = s2; shorter = s1;
            }
            int longerLength = longer.Length;
            if (longerLength == 0) { return 1.0; /* both strings are zero length */ }
            distance = editDistance(longer, shorter);
            return (longerLength - distance) / (double)longerLength;

        }

        public static int editDistance(string s1, string s2)
        {
            s1 = s1.ToLower();
            s2 = s2.ToLower();

            int[] costs = new int[s2.Length + 1];
            for (int i = 0; i <= s1.Length; i++)
            {
                int lastValue = i;
                for (int j = 0; j <= s2.Length; j++)
                {
                    if (i == 0)
                        costs[j] = j;
                    else
                    {
                        if (j > 0)
                        {
                            int newValue = costs[j - 1];
                            if (s1[i - 1] != s2[j - 1])
                                newValue = Math.Min(Math.Min(newValue, lastValue),
                                        costs[j]) + 1;
                            costs[j - 1] = lastValue;
                            lastValue = newValue;
                        }
                    }
                }
                if (i > 0)
                    costs[s2.Length] = lastValue;
            }
            Console.WriteLine("DEBUG----------> " + costs[s2.Length]);
            return costs[s2.Length];
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        public void canWeDeleteThisTopic()
        {
            if(input1.StartsWith("https") || input2.StartsWith("https"))
            {
                if(percentage == 1.0)
                {
                    richTextBox1.Text += Environment.NewLine + "Usuwamy";
                }
                else
                {
                    richTextBox1.Text += Environment.NewLine + "Nie usuwamy";
                }                
            }
            else if((input1.Length < 35 || input2.Length < 35) && distance <= 4 && percentage >= 0.87)
            {
                richTextBox1.Text += Environment.NewLine + "Usuwamy";
            }
            else if ((input1.Length > 35 || input2.Length > 35) && distance <= 3 && percentage >= 0.92)
            {
                richTextBox1.Text += Environment.NewLine + "Usuwamy";
            }
            else if ((input1.Length > 35 || input2.Length > 35) && distance <= 4 && percentage >= 0.95)
            {
                richTextBox1.Text += Environment.NewLine + "Usuwamy";
            }
            else
            {
                richTextBox1.Text += Environment.NewLine + "Nie usuwamy";
            }
        }


        private void Button2_Click(object sender, EventArgs e)
        {
            if(!checkFirstUse)
            {
                // Create a string array with the lines of text
                string lines = richTextBox1.Text + " is the similarity between" + System.Environment.NewLine
                        + input1 + Environment.NewLine
                        + input2 + Environment.NewLine;

                // Set a variable to the Documents path.
                string docPath =
                  Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                // Write the string array to a new file named "WriteLines.txt".
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "WriteLines.txt")))
                {
                    outputFile.WriteLine(lines);
                }
                checkFirstUse = true;
            }
            else
            {
                string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                // Append text to an existing file named "WriteLines.txt".
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "WriteLines.txt"), true))
                {
                    outputFile.WriteLine(richTextBox1.Text + " is the similarity between" + System.Environment.NewLine 
                        + input1 + Environment.NewLine 
                        + input2 + Environment.NewLine);
                }
            }
                

        }
    }
}
