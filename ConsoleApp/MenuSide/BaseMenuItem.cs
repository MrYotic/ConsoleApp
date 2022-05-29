using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleApp;
using static MenuItem;

public abstract class BaseMenuItem
{
    protected BaseMenuItem(string line, MenuOptions options = MenuOptions.None)
    {
        Line = line;
        Options = options;
    }
    public int X = 0, Y = 0;
    public int Index { get; set; }
    public string Line { get; set; }
    public MenuOptions Options { get; set; }
    public abstract int Length { get; }
    public abstract void Render(Color color);
}