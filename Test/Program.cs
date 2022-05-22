using static FuckSharp;
using static ConsoleApp.Color;

ConsoleApp app = new ConsoleApp();
app.WriteLine(DarkRed, "cc").WriteLine("connection").Next.Write(Green, "value is ", 100).Skip();
for(int i = 0; i < 100; i++)
{
    app.Now.Update(DarkYellow, "suck on: ", i);
    Thread.Sleep(10);
}
app.Next.Write(Green, "value is ", 100);