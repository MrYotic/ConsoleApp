public class ItemCollection
{
    private List<BaseMenuItem> items = new List<BaseMenuItem>();
    public void Add(params BaseMenuItem[] items) => this.items.AddRange(items);
    public int Count => items.Count;
    public List<BaseMenuItem> ToList() => items;
    public BaseMenuItem this[string name]
    {
        get => items.FindIndex(z => z.Name == name).Ex(z => z == -1 ? null : items[z]);
        set => items.FindIndex(z => z.Name == name).Ex(z => z == -1 ? null : items[z] = value);
    }
    public BaseMenuItem this[BaseMenuItem item]
    {
        get => items.FindIndex(z => z == item).Ex(z => z == -1 ? null : items[z]);
        set => items.FindIndex(z => z == item).Ex(z => z == -1 ? null : items[z] = value);
    }
    public BaseMenuItem this[int index]
    {
        get => index.Ex(z => z >= Count ? null : items[index]);
        set => index.Ex(z => z >= Count ? null : items[index] = value);
    }
}