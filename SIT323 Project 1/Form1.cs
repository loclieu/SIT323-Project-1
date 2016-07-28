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

namespace SIT323_Project_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
     
        }

        String table;
        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int size = -1;
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            Restriction wordList = new Restriction();
            List<WordFormat> newCrozzle = new List<WordFormat>();
            txtShowAll.ScrollBars = ScrollBars.Vertical;

            string[] listWord;
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                try
                {
                    table = "";     // clear all content in table when opening new file
                    txtShowAll.Text = "Error List:";
                    // Reading File
                    var text = File.ReadAllLines(file);
                    txtLine1.Text = text[0];

                    // Seperating the headers
                    string[] fileHeader = text[0].Split(',');
                    // Seperating the list words
                    listWord = text[1].Split(',');
                   foreach(string x in listWord)
                    {
                       // txtShowAll.Text += x.ToString() + ", ";
                    }

                    // Adding headers into restriciton for checking errors
                    try
                    {
             
                        wordList.level = fileHeader[0].ToString();
                        wordList.numWords = Convert.ToInt32(fileHeader[1]);
                        wordList.numRows = Convert.ToInt32(fileHeader[2]);
                        wordList.numCols = Convert.ToInt32(fileHeader[3]);
                        wordList.horizonWord = Convert.ToInt32(fileHeader[4]);
                        wordList.vertiWord = Convert.ToInt32(fileHeader[5]);

                        // Check Level EASY, MEDIUM, HARD
                        if (fileHeader[0].ToString() != "EASY" && fileHeader[0].ToString() != "MEDIUM" && fileHeader[0].ToString() != "HARD")
                        {
                            txtShowAll.Text += Environment.NewLine + "Difficulty Level Error. Please change level to EASY, MEDIUM or HARD!.";
                        }
                        // Check Word List Range 10 ~ 1000
                        if(wordList.numWords  < 10 || wordList.numWords > 1000)
                        {
                            txtShowAll.Text += Environment.NewLine + "The wordlist size: " + wordList.numWords  + " in the header is not within the range 10 to 1000, inclusive.";
                        }
                        // Check nubmer of rows in Crozzle: 4 ~ 400
                        if(wordList.numRows < 4 || wordList.numRows > 400)
                        {
                            txtShowAll.Text += Environment.NewLine + "The rows size: " + wordList.numRows + " in the header is not within the range 4 to 400, inclusive.";
                        }
                        // Check nubmer of Column in Crozzle: 8 ~ 800
                        if (wordList.numCols < 8 || wordList.numCols > 800)
                        {
                            txtShowAll.Text += Environment.NewLine + "The column size: " + wordList.numCols + " in the header is not within the range 8 to 800, inclusive.";
                        }

                        // Adding the Crozzle words into a list
                        for (int i = 2; i < text.Length; i++)
                        {
                            newCrozzle.Add(new WordFormat { position = text[i].Split(',')[0], rowLocation = Convert.ToInt32(text[i].Split(',')[1]), colLocation = Convert.ToInt32(text[i].Split(',')[2]), word = text[i].Split(',')[3] });
                        }
                    }
                    catch (Exception ex)
                    {
                        txtShowAll.Text = String.Format("Incorrect text file format. Please Insert correct Text file. " + ex);
                    }

                    

                   

                    // Check for duplicates
                    var duplicates = newCrozzle.GroupBy(x => x.word).Where(g => g.Count() > 1).Select(g => g.Key);
                    foreach (var o in duplicates)
                    {
                        txtShowAll.Text += Environment.NewLine + "\nDuplicates: " + o.ToString();
                    }

                    // Check if word exist in the word list
                    foreach (var item in newCrozzle)
                    {
                        if (!listWord.Any(item.word.Contains))
                        {
                            txtShowAll.Text += Environment.NewLine + item.word + " is not in the wordlist";        
                        }
                    }
                }
                catch (IOException)
                {
                }

                #region Storing letter by row and col in two dimension array
                char[,] a = new char[400, 800];
                foreach (WordFormat words in newCrozzle)
                {
                    int wordLenght = words.word.Length;
                    int row = words.rowLocation - 1;
                    int col = words.colLocation - 1;

                    if (words.position == "HORIZONTAL")
                    {
                       
                        for (int i = 0; i < wordLenght; i++)
                        {
                           
                            a[row, col] = words.word[i];
                          //  txtShowAll.Text += Environment.NewLine + row + " " + col + " " + wordLenght + " " + wordList.numRows + " " + wordList.numCols;
                            col++;
                            //row++;
                            //col++;
                            //txtShowAll.Text += a[row, col];
                        }
                       // txtShowAll.Text += Environment.NewLine + words.word;
                       // txtShowAll.Text += Environment.NewLine + a[row, col];
                    }
                    if (words.position == "VERTICAL")
                    {
                        for (int i = 0; i < wordLenght; i++)
                        {

                            if (a[row, col] != ' ')
                            {
                                a[row, col] = words.word[i];
                                //  txtShowAll.Text += Environment.NewLine + row + " " + col + " " + wordLenght + " " + wordList.numRows + " " + wordList.numCols;
                                row++;
                            }
                        }
                    }

                }
                #endregion

                #region Displaying the Crozzle
                // Displaying the Crozzle
                for (int i = 0; i < wordList.numRows; i++)
                {
                    table += "<tr>";
                    for (int j = 0; j < wordList.numCols; j++)
                    {
                        if (a[i, j] != ' ')
                        {
                            table += "<td>" + a[i, j].ToString() + "</td>";
                        }
                        else
                            table += "<td> </td>";
                    }
                    table += "</tr>";
                }
                #endregion


                GenerateHtml();
            }
         
            
            Console.WriteLine(size); // <-- Shows file size in debugging mode.
            Console.WriteLine(result); // <-- For debugging use.
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void GenerateHtml()
        {

            String tableStyle = @"<style>
                            table{border: 1px solid black;}
                            td {padding: 60px:}
                            </style>
                            </head>";

            String htmlText = @"<!DOCTYPE html>
                            <html>
                            <head>
                            <style>

                            table{
                            border-collapse:collapse;
                            }

                            table td{
                            border:1px solid black;
                            height: 25px;
                            width: 25px
                            }
                            body{
                            text-align: center;}
                            </style>
                            </head>
                            <body>
                            <table>" + table + @" 
                              
                            </table>

                            </body>
                            </html>";

            webBrowser1.DocumentText = htmlText;;
        }


        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}
