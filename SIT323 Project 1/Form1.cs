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
                    txtShowAll.Text = "";
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
                        // Check Level 
                        if (fileHeader[0].ToString() != "EASY" && fileHeader[0].ToString() != "MEDIUM" && fileHeader[0].ToString() != "HARD")
                        {
                            txtShowAll.Text += "Difficulty Level Error. Please change level to EASY, MEDIUM or HARD!.";
                        }
                        wordList.level = fileHeader[0].ToString();
                        wordList.numWords = Convert.ToInt32(fileHeader[1]);
                        wordList.numRows = Convert.ToInt32(fileHeader[2]);
                        wordList.numCols = Convert.ToInt32(fileHeader[3]);
                        wordList.horizonWord = Convert.ToInt32(fileHeader[4]);
                        wordList.vertiWord = Convert.ToInt32(fileHeader[5]);

                        // Adding the Crozzle words into a list
                        for (int i = 2; i < text.Length; i++)
                        {
                            newCrozzle.Add(new WordFormat { position = text[i].Split(',')[0], colLocaiton = Convert.ToInt32(text[i].Split(',')[1]), rowLocation = Convert.ToInt32(text[i].Split(',')[2]), word = text[i].Split(',')[3] });
                        }
                    }
                    catch (Exception ex)
                    {
                        txtShowAll.Text = String.Format("An Error has occured: '{0}'", ex);
                    }

                    foreach(var o in newCrozzle)
                    {
                       //txtShowAll.Text += "\n " + o.word;
                       
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
              
                
              // Displaying the Crozzle
                for (int i = 0; i < wordList.numRows; i++)
                {
                    table += "<tr>";
                    for (int j = 0; j < wordList.numCols; j++)
                    {
                        table += "<td></td>";
                    }
                    table += "</tr>";
                }
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
