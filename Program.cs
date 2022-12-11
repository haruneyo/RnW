using System.Text.RegularExpressions;

// ==========  Variables ========== //

string source = "pure text.txt"; // Source file location
string workingFile = "working file.txt"; // Intermediate file location
string workingFormattedFile = "formatted file.txt"; // End file location
bool needToRemoveEmptyLines = false; // True if need to remove all empty lines from the source file
bool needToRemoveWhiteSpaces = false; // True if need to remove all white spaces from the source file

string type = "single"; // Use "mult" for multiple lines for a sentence or "single" for a single line for a sentence
int repeat = 25; // How many questions there are in the file 25

string testNumber = "Номер:##43629.1.х.1.х.1.0.(1)"; // The number that is put before every question

int numberOfAnswers = 0; // If there is no end word after the answer part, 
                         // and/or you know how many answers there are, specify the number here.
                         // Leave 0 otherwise

bool sort = true;  // If you don't need the correct answer to always be on top, put False
                   // If you don't know/have the correct answers, also put False

string keyWordRRCHeaderML = @"А\) "; // Specify the line that would stop the method that reads and writes header
string keyWordRRCAnswersML = "END"; // Specify the line that would stop the method that counts the number of lines in the answer section
string answerMarkers = @"[А-Я]\) "; // Specify the markers that are used in the answer section so that the program can accurately determine the number of answers in a question

char[] replaceStartHeader = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ')', '.', '-', '–', '—', '―', '‒', ' ' }; // Contains characters to remove at the start of the header
char[] replaceEndAnswer = { '.', ',', ':', ';', '-', '–', '—', '―', '‒', ' ' }; // Contains characters to remove at the end of an answer
string[] removeNumberingInAnswers = { @"[А-Я]\) ", @"[0-9]\. ", @"[а-я]\) " }; // Specify digits or letters to remove from the beginning of the answers

bool markerIncluded = true; // True if the correct answer marker is in the answer itself 
                            // False if the correct answer marker is a separate line after the answer

string correctAnswerContains = @"\(\+\)"; // If the correct answer marker is inside the answer, put it here
char[] correctAnswerMarkerRemove = { '(', '+', ')', ' ' }; // Put the correct answer marker here character by character to remove it later

string[] separateAnswerMarker = { "ANSW a", "ANSW b", "ANSW c", "ANSW d", "ANSW e", "ANSW f" }; // Those markers are used to put the correct answer to the first position in cases where there is a separate line specifying the correct answer

string[] keyWordRRCAnswers = { "END" }; // Specify words that begin after the questions

// ==========  Execution ========== //

if (needToRemoveEmptyLines == true) RemoveAllEmptyLines(source); // Removes all empty lines
if (needToRemoveWhiteSpaces == true) RemoveAllWhiteSpaces(source); // Removes all white spaces

if (type == "mult") // Code for texts with multiple lines for one sentence
{
    for (int i = 1; i <= repeat; i++)
    {
        RemoveLines(source, ReadWriteHeaderMultLines(source, workingFile));
        RemoveLines(source, ReadWriteAnswersMultLines(source, workingFile));
        RemoveLines(source, ReadReturnCountMultLines(source, "BEGIN") + 1);
    }
}

if (type == "single") // Code for texts with single line for one sentence
{
    for (int i = 1; i <= repeat; i++)
    {
        for (int j = 0; j < 2; j++)
        {
            // if (ReadWriteSpecificLine(source, workingFile, @"zxc") == true)
            // RemoveLines(source, 1);
        }
        ReadWriteHeaderOneLine(source, workingFile);
        RemoveLines(source, 1);
        RemoveLines(source, ReadWriteAnswers(source, workingFile, numberOfAnswers));
    }
}

for (int i = 1; i <= repeat; i++)
{
    for (int j = 0; j < 2; j++)
    {
        // if (ReadWriteSpecificLine(workingFile, workingFormattedFile, @"zxc") == true)
        // RemoveLines(workingFile, 1);
    }
    ReadWrite(workingFile, workingFormattedFile, 3);
    RemoveLines(workingFile, 3);
    WriteArray
        (SortArray
            (FillArray
                (CreateArray(ReadReturnCountAnswers(workingFile, numberOfAnswers)),
            workingFile),
        sort, markerIncluded),
    workingFormattedFile);
    RemoveLines(workingFile, ReadReturnCountAnswers(workingFile, numberOfAnswers));
}

// ==========  Methods ========== //

void RemoveAllEmptyLines(string original)
{
    var lines = File.ReadAllLines(original).Where(arg => !string.IsNullOrWhiteSpace(arg));
    File.WriteAllLines(original, lines);
}

void RemoveAllWhiteSpaces(string original)
{
    var lines = File.ReadAllLines(original);
    for (int i = 0; i < lines.Length; i++)
    {
        lines[i] = lines[i].Trim();
    }
    File.WriteAllLines(original, lines);
}

bool ReadSpecificLine(string original, string[] specificLine)
{
    StreamReader sr = new StreamReader(original);
    string line = sr.ReadLine();
    for (int i = 0; i < specificLine.Length; i++)
    {
        bool m = Regex.IsMatch(line, specificLine[i]);
        if (m == true) return m;
    }
    sr.Close();
    return false;
}

bool ReadWriteSpecificLine(string original, string target, string specificLine)
{
    StreamReader sr = new StreamReader(original);
    StreamWriter sw = new StreamWriter(target, true);
    string line = sr.ReadLine();
    bool m = Regex.IsMatch(line, specificLine);
    if (m == true)
    {
        sw.WriteLine($"{line}");
    }
    sr.Close();
    sw.Close();
    return m;
}

int ReadReturnCountAnswers(string original, int count = 0)
{
    if (count != 0) return count;
    StreamReader sr = new StreamReader(original);
    string line;
    bool m = false;
    while (m == false)
    {
        line = sr.ReadLine();
        for (int i = 0; i < keyWordRRCAnswers.Length; i++)
        {
            m = Regex.IsMatch(line, keyWordRRCAnswers[i]);
            if (m == true) break;
        }
        count++;
    }
    sr.Close();
    return count;
}

string[] CreateArray(int count)
{
    string[] array = new string[count];
    return array;
}

string[] FillArray(string[] a, string original)
{
    StreamReader sr = new StreamReader(original);
    string line;
    for (int i = 0; i < a.Length; i++)
    {
        line = sr.ReadLine();
        a[i] = line;
    }
    sr.Close();
    return a;
}

string[] SortArray(string[] a, bool sort, bool included)
{
    if (sort == false) return a;

    bool m = false;

    if (included == false)
    {
        for (int i = 1; i < separateAnswerMarker.Length; i++)
        {
            m = Regex.IsMatch(a[a.Length - 1], separateAnswerMarker[i]);
            if (m == true)
            {
                string temp = a[0];
                a[0] = a[i];
                a[i] = temp;
                break;
            }
        }
    }

    if (included == true)
    {
        for (int i = 0; i < a.Length; i++)
        {
            m = Regex.IsMatch(a[i], correctAnswerContains);
            if (m == true)
            {
                string temp = a[0];
                a[0] = a[i];
                a[i] = temp;
                a[0] = a[0].TrimEnd(correctAnswerMarkerRemove); // Change TrimEnd and TrimStart depending on the position of the marker
                break;
            }
        }
    }

    for (int i = 0; i < keyWordRRCAnswers.Length; i++)
    {
        if (Regex.IsMatch(a[a.Length - 1], keyWordRRCAnswers[i]) == true)
        {
            a = a.Where((val, idx) => idx != a.Length - 1).ToArray();
        }

    }

    return a;
}

void WriteArray(string[] a, string target)
{
    StreamWriter sw = new StreamWriter(target, true);
    for (int i = 0; i < a.Length; i++)
    {
        sw.Write("    •    ");
        sw.WriteLine(a[i]);
    }
    sw.WriteLine("-------");
    sw.Close();
}

void ReadWrite(string original, string target, int countOfLines)
{
    StreamReader sr = new StreamReader(original);
    StreamWriter sw = new StreamWriter(target, true);
    string line;
    for (int i = 0; i < countOfLines; i++)
    {
        line = sr.ReadLine();
        sw.WriteLine(line);
    }
    sr.Close();
    sw.Close();
}

void RemoveLines(string original, int c)
{
    var lines = File.ReadAllLines(original);
    File.WriteAllLines(original, lines.Skip(c).ToArray());
}

int ReadWriteAnswers(string original, string target, int count)
{
    if (count == 0)
    {
        count = ReadReturnCountAnswers(original);
    }
    StreamReader sr = new StreamReader(original);
    StreamWriter sw = new StreamWriter(target, true);
    string line;
    sw.WriteLine("Ответы:");
    for (int i = 0; i < count; i++)
    {
        line = sr.ReadLine();
        for (int j = 0; j < removeNumberingInAnswers.Length; j++)
        {
            line = Regex.Replace(line, removeNumberingInAnswers[j], string.Empty);
        }
        line = line.TrimEnd(replaceEndAnswer);
        sw.WriteLine(line);
    }
    sw.Close();
    sr.Close();
    return count;
}

void ReadWriteHeaderOneLine(string original, string target)
{
    StreamReader sr = new StreamReader(original);
    StreamWriter sw = new StreamWriter(target, true);
    string line = sr.ReadLine();
    line = line.TrimStart(replaceStartHeader);
    sw.WriteLine(testNumber);
    sw.Write("Задание: ");
    sw.WriteLine(line);
    sr.Close();
    sw.Close();
}

int ReadWriteHeaderMultLines(string original, string target)
{
    StreamReader sr = new StreamReader(original);
    StreamWriter sw = new StreamWriter(target, true);
    string lineWriter;
    string lineReady = string.Empty;
    int count = ReadReturnCountMultLines(original, keyWordRRCHeaderML);
    string[] array = new string[count];
    for (int i = 0; i < array.Length; i++)
    {
        lineWriter = sr.ReadLine();
        array[i] = lineWriter;
        lineReady += array[i] + " ";
    }
    lineReady = lineReady.TrimStart(replaceStartHeader);
    sw.WriteLine(testNumber);
    sw.Write("Задание: ");
    sw.WriteLine(lineReady);
    sr.Close();
    sw.Close();
    return count;
}

int ReadWriteAnswersMultLines(string original, string target)
{
    StreamReader sr = new StreamReader(original);
    StreamWriter sw = new StreamWriter(target, true);
    string line;
    int count = ReadReturnCountMultLines(original, keyWordRRCAnswersML);
    int numAnswers = ReadReturnNumberOfAnswers(original, count);
    string[] arrayAll = new string[count];
    string[] arrayAnswers = new string[numAnswers];
    for (int i = 0; i < arrayAll.Length; i++)
    {
        line = sr.ReadLine();
        arrayAll[i] = line;
    }

    string[] thing = { @"Б\) ", @"В\) ", @"Г\) ", @"Д\) ", @"Е\) ", "END" };
    int n = 0;
    for (int j = 0; j < arrayAnswers.Length; j++)
    {
        if (j == arrayAnswers.Length - 1)
        {
            thing[j] = "END";
        }
        int index = -1;
        if (j == arrayAnswers.Length - 1)
        {
            index++;
        }
        bool m = false;
        for (int i = 0; i < arrayAll.Length; i++)
        {
            m = Regex.IsMatch(arrayAll[i], thing[j]);
            index++;
            if (m == true) break;
        }
        for (; n < index; n++)
        {
            arrayAnswers[j] += arrayAll[n] + " ";
        }
        n = index;
    }

    for (int j = 0; j < removeNumberingInAnswers.Length; j++)
    {
        for (int i = 0; i < arrayAnswers.Length; i++)
        {
            arrayAnswers[i] = Regex.Replace(arrayAnswers[i], removeNumberingInAnswers[j], string.Empty);
            arrayAnswers[i] = arrayAnswers[i].TrimEnd(replaceEndAnswer);
        }
    }

    sw.WriteLine("Ответы:");
    for (int i = 0; i < arrayAnswers.Length; i++)
    {
        sw.WriteLine(arrayAnswers[i]);
    }
    sw.WriteLine("END");
    sw.Close();
    sr.Close();
    return count + 1;
}

int ReadReturnCountMultLines(string original, string endWord)
{
    StreamReader sr = new StreamReader(original);
    int count = -1;
    bool m = false;
    string lineCounter;
    while (m == false)
    {
        lineCounter = sr.ReadLine();
        m = Regex.IsMatch(lineCounter, endWord);
        count++;
        if (m == true) break;
    }
    sr.Close();
    return count;
}

int ReadReturnNumberOfAnswers(string original, int repeats)
{
    StreamReader sr = new StreamReader(original);
    int count = 0;
    bool m = false;
    string line;
    for (int i = 0; i < repeats; i++)
    {
        line = sr.ReadLine();
        m = Regex.IsMatch(line, answerMarkers);
        if (m == true) count++;
    }
    sr.Close();
    return count;
}