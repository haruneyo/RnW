string source = "pure text.txt";
string workingFile = "working file2.txt";
string workingFormattedFile = "formatted file.txt";

for (int i = 1; i <= 60; i++)
{
    WriteHeader(ReadFirstLine());
    RemoveLines(source, ReadWriteAnswers(source, workingFile));
}
for (int i = 1; i <= 60; i++)
{
    ReadWrite(workingFile, workingFormattedFile, 3);
    RemoveLines(workingFile, 3);
    WriteArray
        (SortArray
            (FillArray
                (CreateArray(ReadReturnCount(workingFile)), workingFile)),
                                                    workingFormattedFile);
    RemoveLines(workingFile, ReadReturnCount(workingFile) + 1);
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
        if (line == "ANSWER A" || line == "ANSWER B" ||
            line == "ANSWER C" || line == "ANSWER D" ||
            line == "ANSWER E") check = true;
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
    if (a[a.Length - 1] == "ANSWER B")
    {
        string temp = a[0];
        a[0] = a[1];
        a[1] = temp;
    }
    if (a[a.Length - 1] == "ANSWER C")
    {
        string temp = a[0];
        a[0] = a[2];
        a[2] = temp;
    }
    if (a[a.Length - 1] == "ANSWER D")
    {
        string temp = a[0];
        a[0] = a[3];
        a[3] = temp;
    }
    if (a[a.Length - 1] == "ANSWER E")
    {
        string temp = a[0];
        a[0] = a[4];
        a[4] = temp;
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

string ReadFirstLine()
{
    StreamReader sr = new StreamReader(source);
    string extract = sr.ReadLine();
    sr.Close();
    return extract;
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
        sw.WriteLine(line);
    }
    sw.WriteLine("-------");
    sw.Close();
    sr.Close();
    return count;
}

void WriteHeader(string extract)
{
    StreamWriter sw = new StreamWriter(workingFile, true);
    sw.WriteLine("Номер:##282.1.х.1.х.1.0.(1)");
    sw.Write("Задание: ");
    sw.WriteLine(extract);
    RemoveLines(source, 1);
    sw.Close();
}