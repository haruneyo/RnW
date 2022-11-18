using System.Text.RegularExpressions;

string source = "pure text.txt";
string workingFile = "working file.txt";
string workingFormattedFile = "formatted file.txt";

for (int i = 1; i <= 1; i++)
{
    RemoveEmptyLines(source);
    if (ReadWriteSpecificLine(source, workingFile, "К разделу") == true)
        RemoveLines(source, 1);
    RemoveEmptyLines(source);
    ReadWriteHeader(source, workingFile);
    RemoveLines(source, 1);
    RemoveEmptyLines(source);
    RemoveLines(source, ReadWriteAnswers(source, workingFile));
    RemoveEmptyLines(source);
}

// for (int i = 1; i <= 120; i++)
// {
//     if (ReadWriteSpecificLine(workingFile, workingFormattedFile, "К разделу") == true)
//         RemoveLines(workingFile, 1);
//     ReadWrite(workingFile, workingFormattedFile, 3);
//     RemoveLines(workingFile, 3);
//     WriteArray
//         (SortArray
//             (FillArray
//                 (CreateArray(ReadReturnCount(workingFile)), workingFile)),
//                                                     workingFormattedFile);
//     RemoveLines(workingFile, ReadReturnCount(workingFile));
// }

void RemoveEmptyLines(string whereToRemove)
{
    while (CheckIfEmptyLine(whereToRemove) != false) RemoveLines(whereToRemove, 1);
}

bool CheckIfEmptyLine(string original)
{
    StreamReader sr = new StreamReader(original);
    string line = sr.ReadLine();
    bool check = false;
    if (line == "") check = true;
    sr.Close();
    return check;
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

int ReadReturnCount(string original)
{
    StreamReader sr = new StreamReader(original);
    string line;
    bool check = false;
    int count = 0;
    while (check == false)
    {
        line = sr.ReadLine();
        if (line == "ANSW a" || line == "ANSW b" ||
            line == "ANSW c" || line == "ANSW d" ||
            line == "ANSW e" || line == "ANSW f") check = true;
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
    int count = ReadReturnCount(original);
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
    if (a[a.Length - 1] == "ANSW b")
    {
        string temp = a[0];
        a[0] = a[1];
        a[1] = temp;
    }
    if (a[a.Length - 1] == "ANSW c")
    {
        string temp = a[0];
        a[0] = a[2];
        a[2] = temp;
    }
    if (a[a.Length - 1] == "ANSW d")
    {
        string temp = a[0];
        a[0] = a[3];
        a[3] = temp;
    }
    if (a[a.Length - 1] == "ANSW e")
    {
        string temp = a[0];
        a[0] = a[4];
        a[4] = temp;
    }
    if (a[a.Length - 1] == "ANSW f")
    {
        string temp = a[0];
        a[0] = a[5];
        a[5] = temp;
    }
    a = a.Where((val, idx) => idx != a.Length - 1).ToArray();
    return a;
}

void WriteArray(string[] a, string target)
{
    StreamWriter sw = new StreamWriter(target, true);
    for (int i = 0; i < a.Length; i++)
    {
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
    int count = ReadReturnCount(original);
    StreamReader sr = new StreamReader(original);
    StreamWriter sw = new StreamWriter(target, true);
    string line;
    sw.WriteLine("Ответы:");
    for (int i = 0; i < count; i++)
    {
        line = sr.ReadLine();
        line = Regex.Replace(line, @"[0-9]\) ", string.Empty); // [0-9] for digits, [a-z] for letters
        sw.WriteLine(line);
    }
    sw.Close();
    sr.Close();
    return count;
}

void ReadWriteHeader(string original, string target)
{
    StreamReader sr = new StreamReader(original);
    StreamWriter sw = new StreamWriter(target, true);
    string line = sr.ReadLine();
    char[] replace = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ')', ' '};
	line = line.TrimStart(replace);
    sw.WriteLine("Номер:##43659.1.х.1.х.1.0.(1)");
    sw.Write("Задание: ");
    sw.WriteLine(line);
    sr.Close();
    sw.Close();
}