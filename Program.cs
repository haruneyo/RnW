string source = "pure text.txt";
string workingFile = "working file.txt";

for (int i = 1; i <= 60; i++)
{
    WriteHeader(ReadFirstLine());
    RemoveLines(ReadWriteAnswers());
}

void RemoveLines(int c)
{
    var lines = File.ReadAllLines(source);
    File.WriteAllLines(source, lines.Skip(c).ToArray());
}

string ReadFirstLine()
{
    StreamReader sr = new StreamReader(source);
    string extract = sr.ReadLine();
    sr.Close();
    return extract;
}

int ReadWriteAnswers()
{
    StreamReader sr = new StreamReader(source);
    StreamWriter sw = new StreamWriter(workingFile, true);
    string line;
    int count = 0;
    bool check = false;
    sw.WriteLine("Ответы:");
    while (check == false)
    {
        line = sr.ReadLine();
        sw.WriteLine(line);
        if (line == "ANSWER A" || line == "ANSWER B" || 
            line == "ANSWER C" || line == "ANSWER D" ||
            line == "ANSWER E") check = true;
        count++;
        if (count == 10) break;
    }
    sw.WriteLine("-------");
    sr.Close();
    sw.Close();
    return count;
}

void WriteHeader(string extract)
{
    StreamWriter sw = new StreamWriter(workingFile, true);
    sw.WriteLine("Номер:##282.1.х.1.х.1.0.(1)");
    sw.Write("Задание: ");
    sw.WriteLine(extract);
    RemoveLines(1);
    sw.Close();
}

// void Write(string extract)
// {
//     StreamWriter sw = new StreamWriter(workingFile, true);
//     sw.WriteLine(extract);
//     sw.Close();
// }

// string Read()
// {
//     StreamReader sr = new StreamReader(source);
//     string extract = sr.ReadLine();
//     sr.Close();
//     return extract;
// }