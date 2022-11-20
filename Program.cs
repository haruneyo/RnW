using System.Text.RegularExpressions;

string source = "pure text.txt";
string workingFile = "working file.txt";
string workingFormattedFile = "formatted file.txt";
bool needToRemoveEmptyLines = false;
bool needToRemoveWhiteSpaces = false;

string testNumber = "Номер:##43680.1.х.1.х.1.0.(1)"; // The number that is put before every question
string keyWordRRCHeader = @"А\) "; // Specify the line that would stop the method that reads and writes headers
char[] replaceStartHeader = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ')', '.', '-', '–', '—', '―', '‒', ' ' }; // Contains characters to remove at the start of the header
char[] replaceEndAnswer = { '.', ',', ':', ';', '-', '–', '—', '―', '‒' }; // Contains characters to remove at the end of an answer
//string[] removeLineIfContains = { "Примечан", "Пояснение", "Комментарий" }; // Obsolete for now
string[] removeNumberingInAnswers = { @"\-", @"[А-Я]\) ", @"\- " }; // Specify digits or letters to remove from the beginning of the answers
string[] keyWordRRCAnswers = { "Примечан", "Пояснение", "Комментарий" }; // Specify words that begin after the questions
string rightAnswerContains = @"\(\+\)"; // If the right answer marker is inside the answer, put it here
char[] rightAnswerMarkerRemove = { '(', '+', ')' }; // Put the right answer marker here character by character to remove it later

if (needToRemoveEmptyLines == true) RemoveAllEmptyLines(source);
if (needToRemoveWhiteSpaces == true) RemoveAllWhiteSpaces(source);

ReadWriteHeaderUntil(source, workingFile);
// for (int i = 1; i <= 4; i++) //199
// {
//     // if (ReadWriteSpecificLine(source, workingFile, "К разделу") == true)
//     //     RemoveLines(source, 1);
//     ReadWriteHeaderOneLine(source, workingFile);
//     RemoveLines(source, 1);
//     RemoveLines(source, ReadWriteAnswers(source, workingFile));
// }

// for (int i = 1; i <= 4; i++)
// {
//     // if (ReadWriteSpecificLine(workingFile, workingFormattedFile, "К разделу") == true)
//     //     RemoveLines(workingFile, 1);
//     ReadWrite(workingFile, workingFormattedFile, 3);
//     RemoveLines(workingFile, 3);
//     WriteArray
//         (SortArray
//             (FillArray
//                 (CreateArray(ReadReturnCountAnswers(workingFile)), workingFile)),
//                                                     workingFormattedFile);
//     RemoveLines(workingFile, ReadReturnCountAnswers(workingFile));
// }

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

int ReadReturnCountAnswers(string original)
{
    StreamReader sr = new StreamReader(original);
    string line;
    bool m = false;
    int count = 0;
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
    int count = ReadReturnCountAnswers(original);
    StreamReader sr = new StreamReader(original);
    string line;
    for (int i = 0; i < count; i++)
    {
        line = sr.ReadLine();
        a[i] = line;
    }
    sr.Close();
    return a;
}

string[] SortArray(string[] a)
{
    // if (a[a.Length - 1] == "ANSW b") // REWRITE THIS BIT USING AN ARRAY WITH KEYWORDS AND ISMATCH METHOD
    // {
    //     string temp = a[0];
    //     a[0] = a[1];
    //     a[1] = temp;
    // }
    // if (a[a.Length - 1] == "ANSW c")
    // {
    //     string temp = a[0];
    //     a[0] = a[2];
    //     a[2] = temp;
    // }
    // if (a[a.Length - 1] == "ANSW d")
    // {
    //     string temp = a[0];
    //     a[0] = a[3];
    //     a[3] = temp;
    // }
    // if (a[a.Length - 1] == "ANSW e")
    // {
    //     string temp = a[0];
    //     a[0] = a[4];
    //     a[4] = temp;
    // }
    // if (a[a.Length - 1] == "ANSW f")
    // {
    //     string temp = a[0];
    //     a[0] = a[5];
    //     a[5] = temp;
    // }
    bool m = false;
    for (int i = 0; i < a.Length; i++)
    {
        m = Regex.IsMatch(a[i], rightAnswerContains);
        if (m == true)
        {
            string temp = a[0];
            a[0] = a[i];
            a[i] = temp;
            a[0] = a[0].TrimEnd(rightAnswerMarkerRemove);
            break;
        }
    }
    a = a.Where((val, idx) => idx != a.Length - 1).ToArray();
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

int ReadWriteAnswers(string original, string target)
{
    int count = ReadReturnCountAnswers(original);
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

void ReadWriteHeaderUntil(string original, string target)
{
    StreamReader sr = new StreamReader(original);
    StreamWriter sw = new StreamWriter(target, true);
    string lineWriter;
    string lineReady = string.Empty;
    int count = ReadReturnCountHeader(original);
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
}

int ReadReturnCountHeader(string original)
{
    StreamReader sr = new StreamReader(original);
    int count = -1;
    bool m = false;
    string lineCounter;
    while (m == false)
    {
        lineCounter = sr.ReadLine();
        m = Regex.IsMatch(lineCounter, keyWordRRCHeader);
        count++;
        if (m == true) break;
    }
    sr.Close();
    return count;
}