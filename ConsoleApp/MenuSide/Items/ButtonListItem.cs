using static ConsoleApp;
using static MenuItem;

public class ButtonListItem : BaseMenuItem
{
    public ButtonListItem(string line, int x = 0, int y = 0) : base(line, x, y)
    {
        Line = line;
    }
    public ButtonListItem(string line, int x = 0, int y = 0, params (string name, Action action)[] actions) : this(line, x, y)
    {
        Units = actions.Select(z => new ListUnit(z.name, z.action)).ToList();
    }
    public List<ListUnit> Units { get; set; } = new List<ListUnit> { };
    public override int Length => Line.Length + 2 + Enumerable.Range(0, Units.Count).Select(z => Units[z].Name.Length + 1).Sum();
    public void Invoke(int index, Menu menu = null)
    {
        Units[index].Action();
        if (Options == MenuOptions.Close)
            menu?.Close();
        else if (Options == MenuOptions.Hide)
            menu?.Hide();
    }
    public void Render(Color color, int index)
    {
        CCP = (X + Line.Length + 2 + Enumerable.Range(0, index).Select(z => Units[z].Name.Length + 1).Sum(), Y);
        CFC = ConsoleColors[color];
        W = Units[index].Name;
    }
    public override void Render(Color color)
    {
        CCP = (X, Y);
        CFC = ConsoleColors[color];
        W = $"{Line} [{string.Join("\\", Units.Select(z => z.Name))}]";
    }
}
public class ListUnit
{
    public ListUnit(string name, Action action)
    {
        Name = name;
        Action = action;
    }
    public string Name { get; set; }
    public Action Action { get; set; }
}