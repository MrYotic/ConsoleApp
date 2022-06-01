using static ConsoleApp;
using static MenuItem;

public class ButtonItem : BaseMenuItem
{
    public ButtonItem(string line, Action action, int x = 0, int y = 0) : base(line, x, y)
    {
        Line = line;
        Action = action;
    }
    public Action Action;

    public override int Length => Line.Length;

    public void Invoke(Menu menu = null)
    {
        if (Options == MenuOptions.Hide)
            menu?.Hide();
        Action?.Invoke();
        if (Options == MenuOptions.Close)
            menu?.Close();
    }
    
    public override void Render(Color color)
    {
        CCP = (X, Y);
        CFC = ConsoleColors[color];
        W = Line;
    }
}
