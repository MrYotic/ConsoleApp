using static ConsoleApp;
using static MenuItem;

public class LabelItem : BaseMenuItem
{
    public LabelItem(string line, int x = 0, int y = 0) : base(line, x, y) => Line = line;
    public override int Length => Line.Length;
    public override void Render(Color color)
    {
        CCP = (X, Y);
        CFC = ConsoleColors[color];
        W = Line;
    }
}
