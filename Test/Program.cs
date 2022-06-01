using static FuckSharp;
Menu login = new Menu()
{
    startPoint = (2, 1),
    HAlign = Menu.HorAlign.Left,
    VAlign = Menu.VerAlign.Top,
};
Menu menu = new Menu()
{
    startPoint = (2, 1),
    HAlign = Menu.HorAlign.Left,
    VAlign = Menu.VerAlign.Top,
};
login.Items.Add(new List<BaseMenuItem>()
{
    new LabelItem("Discord File Hosting Panel.", 0, 0),
    new LabelItem("Login User.", 0, 1),
    new InputItem("Login:", "", 0, 3)
    {
        ValidAction = new Func<string, string?>((string input) => 
        {
            User.CheckStringState state = User.VerifyUserName(input);
            return state == User.CheckStringState.Successful ? null : state.ToString();
        }),
    },
    new InputItem("Password:", "", 0, 4)
    {
        ValidAction = new Func<string, string?>((string input) =>
        {
            User.CheckStringState state = User.VerifyPassword(input);
            return state == User.CheckStringState.Successful ? null : state.ToString();
        }),
    },
    new ButtonItem("Continue", new(() => { login.Hide(); menu.Show(); }), 0, 6),
}.ToArray());

menu.Items.Add(new List<BaseMenuItem>()
{
    new LabelItem("Discord File Hosting Panel.", 0, 0),
    new ButtonItem("Login", new(() => { login.Show(); }), 0, 3),
    new ButtonItem("Register", new(() => { }), 0, 4),

    new ButtonItem("User Exist", new(() => { }), 0, 5),
    new ButtonItem("User Is Valid", new(() => { }), 0, 6),

    new ButtonListItem("Find Files", 0, 7,
        ("Private", () => { }),
        ("Public", () => { }),
        ("Anon", () => { })),

    new ButtonListItem("Upload Document", 0, 8,
        ("Private", () => { }),
        ("Public", () => { }),
        ("Anon", () => { })),

    new ButtonListItem("Upload File", 0, 9,
        ("Private", () => { }),
        ("Public", () => { }),
        ("Anon", () => { })),

    new ButtonItem("Download File", new(() => { }), 0, 10),
    new ButtonItem("Get New Anon Files", new(() => { }), 0, 11),
    
}.ToArray());
menu.Show();