using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleApp;
using static MenuItem;

public abstract class BaseMenuItem
{
    protected BaseMenuItem(string line, int x, int y)
    {
        BaseX = x;
        BaseY = y;
        Line = Name = line;
    }
    public int BaseX { get; set; }
    public int BaseY { get; set; }
    public int X = 0, Y = 0;
    public string Line { get; set; }
    public string Name { get; set; }
    public MenuOptions Options { get; set; } = MenuOptions.Hide;
    public abstract int Length { get; }
    public abstract void Render(Color color);
    public InputItem AsInput => (InputItem)this;
    public LabelItem AsLabel => (LabelItem)this;
    public ButtonListItem AsButtonList => (ButtonListItem)this;
    public ButtonItem AsButton => (ButtonItem)this;
}