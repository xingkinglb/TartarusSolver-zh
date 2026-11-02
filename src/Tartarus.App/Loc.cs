using System.Globalization;
using System.Windows;

namespace Tartarus.App;

/// <summary>
/// The interface's language. English is the source: a string is its own key, so a line the table below doesn't have
/// stays English and the English interface is exactly what it was. Chinese words for the game's own things (Codes,
/// Torture Instruments, Prison Incidents, chests) are the game's, from models/text/tartarus_text.json (zhCN), not
/// invented here: the player reads on the overlay what the game's own screens say.
/// </summary>
public static class Loc
{
    /// <summary>The language the interface is drawn in: "en" or "zhCN". Set once at start-up (<see cref="Init"/>).</summary>
    public static string Language { get; private set; } = "en";

    public static bool Chinese => Language.StartsWith("zh", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// <paramref name="setting"/>: the player's choice ("auto", "en", "zhCN"); "auto" or empty takes this Windows'
    /// own language (Chinese for any zh-*), else English.
    /// </summary>
    public static void Init(string? setting)
    {
        setting = string.IsNullOrWhiteSpace(setting) ? "auto" : setting.Trim();
        Language = setting.Equals("auto", StringComparison.OrdinalIgnoreCase) ? SystemLanguage() : Normalize(setting);
    }

    private static string Normalize(string tag) => tag.StartsWith("zh", StringComparison.OrdinalIgnoreCase) ? "zhCN" : "en";

    private static string SystemLanguage()
    {
        try
        {
            // InstalledUICulture: the language this Windows was installed in, not the one a program asked for.
            return Normalize(CultureInfo.InstalledUICulture.Name);
        }
        catch (Exception) { return "en"; }
    }

    /// <summary>The line in the interface's language: the translation, or the English itself where there is none.</summary>
    public static string T(string english) =>
        Chinese && Zh.TryGetValue(english, out var zh) ? zh : english;

    /// <summary>A line with {0}, {1}… filled in, numbers as the interface has them ("12,800", "6.4").</summary>
    public static string F(string english, params object?[] args) =>
        string.Format(CultureInfo.InvariantCulture, T(english), args);

    /// <summary>
    /// A game name in two lines for a caption: "Filthy Claws" over "Alpha Wolf" (the game's names split at ": "), Chinese
    /// at its "：".
    /// </summary>
    public static string Caption(string name) =>
        Chinese ? T(name).Replace("：", "：\n", StringComparison.Ordinal) : name.Replace(": ", "\n", StringComparison.Ordinal);

    /// <summary>WPF text: a Chinese face first with Segoe UI after it, so a mixed line ("绮彩 ×2.0") draws whole.</summary>
    public static System.Windows.Media.FontFamily Font =>
        Chinese ? new System.Windows.Media.FontFamily("Microsoft YaHei UI, Segoe UI") : new System.Windows.Media.FontFamily("Segoe UI");

    private static readonly Dictionary<string, string> Zh = new(StringComparer.Ordinal)
    {
        // ---- The game's own Tartarus words (models/text/tartarus_text.json, zhCN) ----
        ["Tartarus Prison"] = "塔罗斯监狱",
        ["Tartarus"] = "塔罗斯",
        ["Make Your Decision"] = "请进行决策",
        ["Select a Destination"] = "请选择目的地",
        ["Select an Inmate to Consume"] = "请选择要吃的囚犯",
        ["Consume"] = "吃",
        ["Chain Consume"] = "连吃",
        ["Consumable"] = "可吃",
        ["Consumable target"] = "可吃目标",
        ["Hunt"] = "寻猎",
        ["Replace"] = "换",
        ["Instigate"] = "煽动",
        ["Bestow"] = "赏赐",
        ["End"] = "结束",
        ["Cancel"] = "取消",
        ["Re-roll"] = "重新随机",
        ["Interrogation Room"] = "审讯室",
        ["Sewer Ratway"] = "暗渠鼠道",
        ["Torture Instrument"] = "刑具",
        ["Torture Instruments"] = "刑具",
        ["Prison Incident"] = "狱中事变",
        ["Prison Incidents"] = "狱中事变",
        ["Objective"] = "目标",
        ["Warden"] = "典狱长",
        ["Warden's Cache"] = "典狱秘藏",
        ["Warden's Treasure"] = "典狱长的珍藏",
        ["Chromatic"] = "绮彩",
        ["Ultimate"] = "至臻",
        ["Legendary"] = "传奇",
        ["Rare"] = "稀有",
        ["Magic"] = "魔法",
        ["Normal"] = "普通",
        ["Riot"] = "暴乱",
        ["Inmate"] = "囚犯",
        ["Inmates"] = "囚犯",
        ["Chest"] = "宝箱",
        ["Riot chest"] = "暴乱宝箱",
        ["Remaining Days"] = "剩余天数",
        ["Destinations"] = "目的地",
        ["Destination"] = "目的地",
        ["Scratch Marks"] = "抓痕",
        ["Rat Nest"] = "鼠窝",
        ["Tusk"] = "獠牙",
        ["Prey"] = "猎物",
        ["Empty"] = "空牢房",
        ["Unopened"] = "未开启",
        ["Locked Cell"] = "未开启牢房",
        ["Opened"] = "已开启",
        ["Opened, empty"] = "已开启，空",
        ["Cell"] = "牢房",
        ["Day"] = "日",
        ["Run theme"] = "局内主题",
        ["Tyranny"] = "滥权",
        ["Opulence"] = "奢靡",
        ["Iron Law"] = "铁律",
        ["Cage Fight"] = "囚徒困斗",
        ["Iron Law theme"] = "铁律主题",

        // ---- The 31 Torture Instruments ----
        ["Raging Beast's Agonizing Muzzle"] = "狂兽的剧痛口枷",
        ["Ferocious Beast's Ravenous Chains"] = "凶兽的饥饿绞链",
        ["Maddened Beast's Barbed Collar"] = "疯兽的倒刺项圈",
        ["Avenger's Casket"] = "复仇者之匣",
        ["Crowded Beastcage"] = "拥挤兽笼",
        ["Barbed Whip"] = "棘刺铁鞭",
        ["Rusty Saw"] = "锈锯",
        ["Executioner's Blacklist"] = "执刑官的黑名册",
        ["Lost Ring of Keys"] = "遗落的钥匙串",
        ["Kin's Coin of Fate"] = "手足的命运硬币",
        ["Searing Branding Iron"] = "灼痛的烙铁",
        ["Felon's Dog Tag"] = "重刑铭牌",
        ["Forged Key"] = "伪造钥匙",
        ["Sorting Hook"] = "挑拣钩",
        ["Scapegoat's Guillotine"] = "替罪人的断头台",
        ["Gaoler's Execution Axe"] = "狱卒的行刑巨斧",
        ["Severed Iron Maiden"] = "切开的人形棺",
        ["Warden's Scales"] = "典狱官的天平",
        ["Hangman's Noose"] = "绞首绳",
        ["Burning Stake"] = "火刑柱",
        ["Iron Pestle"] = "铁杵",
        ["Short-Chained Shackles"] = "短链脚镣",
        ["Long-Chained Shackles"] = "长链脚镣",
        ["Iron Tongs of Lust: Mammoth"] = "恶欲铁钳：巨象",
        ["Elephant-Foot Maul: Mammoth"] = "象脚重锤：巨象",
        ["Blood-Stained Trap: Berserk Lion"] = "染红的捕兽夹：狂狮",
        ["Iron Crown of the Prison King: Berserk Lion"] = "牢中王的铁冠：狂狮",
        ["Filthy Claws: Alpha Wolf"] = "脏污铁爪：头狼",
        ["Fanged Guillotine: Alpha Wolf"] = "狼牙铡刀：头狼",
        ["Squirming Iron Box: Colossal Rat"] = "蠕动铁盒：硕鼠",
        ["Skewered Rat Heads: Colossal Rat"] = "鼠头串：硕鼠",
        ["Unknown Torture Instrument"] = "未识别的刑具",

        // ---- The Warden's Treasures ----
        ["Three-Headed Thumb Ring"] = "三首扳指",
        ["Brass Horn of Strife"] = "斗争铜号",
        ["Crimson Vintage"] = "赤色佳酿",
        ["Brand of Original Sin"] = "原罪烙印",

        // ---- The 17 Prison Incidents ----
        ["High-Risk Convict"] = "重刑犯",
        ["Taste of Freedom"] = "自由的滋味",
        ["Behind Closed Doors"] = "暗箱操作",
        ["Ravenous Appetite"] = "食欲大增",
        ["Extra Portion"] = "加餐",
        ["Shared Fortune"] = "有福同享",
        ["Archenemy"] = "宿敌",
        ["Execution Efficiency"] = "处刑效率",
        ["Crow Whispers"] = "鸦语",
        ["\"Treasure\" Hunt"] = "寻“宝”",
        ["Chain Reaction"] = "连锁反应",
        ["Transfer of Convicts"] = "同罪移押",
        ["Usurpation I"] = "僭越·其一",
        ["Usurpation II"] = "僭越·其二",
        ["Usurpation III"] = "僭越·其三",
        ["Usurpation IV"] = "僭越·其四",

        // ---- The Objectives ----
        ["King of the Caged"] = "困兽称王",
        ["Survival of the Fittest"] = "弱肉强食",
        ["Behind Cell Doors"] = "牢门之后",
        ["Undercurrents"] = "暗流涌动",
        ["Prison Walk"] = "巡狱之路",

        // ---- The Codes ----
        ["Mammoth"] = "巨象",
        ["Lion"] = "狂狮",
        ["Wolf"] = "头狼",
        ["Rat"] = "硕鼠",

        // ---- The window's own words ----
        ["Game window"] = "游戏窗口",
        ["Refresh the window list"] = "刷新窗口列表",
        ["Capture ▶"] = "捕获 ▶",
        ["Stop ■"] = "停止 ■",
        ["Attach to the game automatically"] = "自动连接游戏",
        ["Not capturing"] = "未捕获",
        ["Not capturing a window"] = "未捕获窗口",
        ["Not capturing: the game window is minimized, restore it"] = "未捕获：游戏窗口已最小化，请还原它",
        ["Not capturing: waiting for the game window (Torchlight: Infinite)…"] = "未捕获：正在等待游戏窗口（Torchlight: Infinite）…",
        ["Capturing: {0}"] = "正在捕获：{0}",
        ["Recognition"] = "识别",
        ["Scan"] = "扫描",
        ["Auto"] = "自动",
        ["Automatic"] = "跟随系统",
        ["Interface language"] = "界面语言",
        ["The language this window and the overlay are written in; it changes at the next start"] = "本窗口与叠加层所用的语言；下次启动程序时生效",
        ["The interface's language is kept and comes at the next start of the app."] = "界面语言已保存，下次启动程序时生效。",
        ["Game language"] = "游戏语言",
        ["Detect automatically"] = "自动检测",
        ["The language the game shows its text in, so the app can read it. Not this window's language: that one is below."] = "游戏内文字所用的语言，程序据此识别。这不是本窗口的语言，本窗口的语言在下面选。",
        ["Take the language from the game's settings file; choosing one in the list turns this off"] = "从游戏设置文件里读取语言；在列表里选定语言会关掉此项",
        ["Overlay"] = "叠加层",
        ["Interactable regions"] = "可交互区域",
        ["Frame everything clickable: cells, cards, buttons, Torture Instrument and Chromatic slots, pick cards"] = "给所有能点的地方加框：牢房、卡牌、按钮、刑具与绮彩槽位、选择卡",
        ["Text"] = "文字",
        ["OCR regions and the lines read, on the overlay"] = "在叠加层上显示 OCR 区域和读到的文字",
        ["Seconds from one frame to the next; 0: the next frame as soon as the last one's reading and hint are done"] = "两帧之间的间隔（秒）；0 表示上一帧识别和提示一完成就抓下一帧",
        ["Hints"] = "出招提示",
        ["Move hints"] = "走位提示",
        ["Hints only"] = "只显示提示",
        ["The overlay shows only the move frames and the hint line: no cells, Torture Instruments or Chromatic"] = "叠加层只显示走位框和提示行，不显示牢房、刑具和绮彩",
        ["Faster"] = "更快",
        ["Stronger"] = "更强",
        ["Playouts per move: longer to compute, stronger hint"] = "每步模拟次数：越多提示越准，也算得越久",
        ["Last hint: {0} ms, {1} playouts"] = "上次提示：{0} ms，{1} 次模拟",
        [" · value net"] = " · 价值网络",
        [" · no value net"] = " · 无价值网络",
        [" · policy net"] = " · 策略网络",
        [" · picks net"] = " · 选择网络",
        ["Game log"] = "游戏日志",
        ["Choose…"] = "选择…",
        ["Pick UE_game.log by hand: game folder\\UE_game\\TorchLight\\Saved\\Logs"] = "手动选择 UE_game.log：游戏目录\\UE_game\\TorchLight\\Saved\\Logs",
        ["Find the log automatically: beside the running game, in Steam libraries, in the launcher's settings"] = "自动查找日志：运行中游戏的同级目录、Steam 库、启动器设置",
        ["UE_game.log: game folder\\UE_game\\TorchLight\\Saved\\Logs"] = "UE_game.log：游戏目录\\UE_game\\TorchLight\\Saved\\Logs",
        ["not found: screen recognition only"] = "未找到：只靠画面识别",
        [" (chosen by hand)"] = "（手动指定）",
        [" (found automatically)"] = "（自动找到）",
        ["Statistics"] = "统计",
        ["Reset"] = "重置",
        ["Clear the statistics and count from now on"] = "清空统计，从现在起重新计数",
        ["Clear the statistics ({0} runs, {1} Chromatic) and count from now on?"] = "清空统计（{0} 局、{1} 个绮彩囚犯），从现在起重新计数？",
        ["Tartarus Solver: statistics"] = "Tartarus Solver：统计",
        ["No runs yet: a run counts once the game's log has seen it from its Start to the Riot"] = "还没有记录：一局要由游戏日志从开始看到暴乱，才算一局",
        ["Runs: {0} · Chromatic: {1} ({2} per run)"] = "局数：{0} · 绮彩：{1}（每局 {2}）",
        ["Since {0}; runs the game's log saw from their Start to the Riot"] = "自 {0} 起；统计的是游戏日志从头看到暴乱的局",
        ["Labels and dataset"] = "标注与数据集",
        ["Edit mode"] = "编辑模式",
        ["Save frame"] = "保存帧",
        ["Auto-collect"] = "自动采集",
        ["Save a frame to the dataset whenever the Sandbox read changes (with auto scan)"] = "沙盒读取结果一变就存一帧到数据集（需开自动扫描）",
        ["The Warden's cell, 1-16"] = "典狱长所在的牢房，1-16",
        ["Cells on the Destination cards, left to right: 9,11,12"] = "目的地卡上的牢房号，从左到右，如 9,11,12",
        ["ids, comma-separated: ironbox,muzzle"] = "id，用逗号分隔，如 ironbox,muzzle",
        ["Apply fields"] = "应用字段",
        ["Reset edits"] = "撤销编辑",
        ["Dataset folder"] = "数据集文件夹",
        ["In edit mode click a cell on the overlay: a menu of Code, rarity and flags. Auto scan pauses while editing."] = "编辑模式下点叠加层上的牢房：可选代号、稀有度和各种标记。编辑期间自动扫描暂停。",
        ["Sandbox"] = "沙盒",
        ["Log"] = "日志",
        ["Check for updates"] = "检查更新",
        ["Ask GitHub for a new version at start and every 6 hours"] = "启动时及每 6 小时向 GitHub 查询新版本",
        ["Report a bug"] = "反馈问题",
        ["Send the developer a screenshot, the app's log and the current run's log with your comment"] = "把你的说明连同截图、程序日志和本局游戏日志一起发给开发者",
        ["Version"] = "版本",
        [" (debug)"] = "（调试）",
        [", every"] = "，每隔",

        // ---- Status lines and messages ----
        ["Sandbox · {0} ms"] = "沙盒 · {0} ms",
        ["No Sandbox: {0} · {1} ms"] = "未找到沙盒：{0} · {1} ms",
        ["Tartarus: no Sandbox found ({0} ms)"] = "塔罗斯：未找到沙盒（{0} ms）",
        ["Tartarus: the window's size changed, a new scan is needed"] = "塔罗斯：窗口尺寸变了，需要重新扫描",
        ["The game's picture doesn't fill its window: set the window size in the game's settings, or press Alt+Enter twice"] = "游戏画面没有填满窗口：在游戏设置里调整窗口尺寸，或连按两次 Alt+Enter",
        ["No frames yet (is the window minimized?)"] = "还没有画面（窗口是不是最小化了？）",
        [" (OCR {0}, cells {1})"] = "（OCR {0}，格子 {1}）",
        [" (ocr {0}, cells {1})"] = "（OCR {0}，格子 {1}）",
        [" · presence {0}"] = " · 存在度 {0}",
        [" · {0} ms"] = " · {0} ms",
        [" · {0}×{1} · {2}"] = " · {0}×{1} · {2}",
        ["Not capturing: this Windows is too old, the app needs Windows 10 version 1903 or later"] = "未捕获：这个 Windows 太旧，程序需要 Windows 10 1903 或更高版本",
        ["Not capturing: Windows can't capture windows on this PC (Windows.Graphics.Capture unsupported)"] = "未捕获：这台电脑上的 Windows 无法捕获窗口（不支持 Windows.Graphics.Capture）",
        ["This version of Windows can't capture the game's window, so the app can't read the game.\n\nThe app needs Windows 10 version 1903 or later."] = "这个版本的 Windows 无法捕获游戏窗口，程序也就读不到游戏。\n\n程序需要 Windows 10 1903 或更高版本。",
        ["The program's files are missing from its folder (models/ocr), so the game's text may not be read.\n\nUnpack the whole archive into a folder of its own and run the program from there, not from inside the archive."] = "程序目录里缺少需要的文件（models/ocr），游戏文字可能读不出来。\n\n请把整个压缩包解压到一个独立的文件夹，再从那里运行程序，不要直接在压缩包里运行。",
        ["Tartarus Solver: Game language"] = "Tartarus Solver：游戏语言",
        ["the Sandbox is read, back to the pick (the eye button)"] = "沙盒已读取，可以回到选择界面（眼睛按钮）",
        ["Tartarus overlay"] = "塔罗斯叠加层",

        // ---- The overlay ----
        ["Run theme: {0} (kept for this run)"] = "局内主题：{0}（本局已记下）",
        ["Remaining Days {0}"] = "剩余天数 {0}",
        ["Objective: {0}"] = "目标：{0}",
        ["Chromatic {0}/{1}"] = "绮彩 {0}/{1}",
        ["~{0} ms per move · ≈{1} Chromatic per run ({2} playouts per move)"] = "每步约 {0} ms · 每局约 {1} 个绮彩（每步 {2} 次模拟）",
        [" (cap)"] = "（已达上限）",
        ["(covered)"] = "（被遮挡）",
        ["Unknown Torture Instrument"] = "未识别的刑具",
        [" (remembered)"] = "（沿用记忆）",
        ["Re-roll: {0}"] = "重新随机：{0}",
        ["wanted for data: the network doesn't know its marks yet"] = "缺数据：网络还不认识它的标记",
        ["Hint: {0}"] = "提示：{0}",
        [" ≈ {0}"] = " ≈ {0}",
        [" (tied)"] = "（并列）",
        [" — at the Riot ≈{0} Chromatic · {1} Ultimate · {2} Legendary"] = " — 暴乱结算 ≈{0} 绮彩 · {1} 至臻 · {2} 传奇",
        [" · then {0}"] = " · 之后 {0}",
        ["  ({0} playouts{1})"] = "  （{0} 次模拟{1}）",
        [", computing…"] = "，计算中…",
        ["Cell {0}"] = "{0} 号牢房",
        ["Cell {0}: {1}"] = "{0} 号牢房：{1}",
        ["Consume: Cell {0}"] = "吃：{0} 号牢房",
        ["Hunt: Cell {0}"] = "寻猎：{0} 号牢房",
        ["Sewer Ratway: Cell {0}"] = "暗渠鼠道：{0} 号牢房",
        ["Cancel → {0}"] = "取消 → {0}",
        ["Visible rarity (Unopened)"] = "可见稀有度（未开启）",
        ["Add to Destinations (D{0})"] = "设为目的地 D{0}",
        ["Remove from Destinations (D{0})"] = "取消目的地 D{0}",

        // ---- Updates ----
        ["Update to {0}"] = "更新到 {0}",
        ["Download {0} ({1} MB), install it and restart"] = "下载 {0}（{1} MB），安装并重启",
        ["Open the release's page"] = "打开发布页",
        ["Downloading… {0}%"] = "下载中… {0}%",
        ["Installing…"] = "安装中…",
        ["Download version {0} ({1} MB), put it in place of this one and restart the app?"] = "下载 {0} 版（{1} MB），替换当前版本并重启程序？",
        ["The update failed: {0}"] = "更新失败：{0}",
        ["Tartarus Solver: update"] = "Tartarus Solver：更新",

        // ---- The dialogs ----
        ["Text recognition"] = "文字识别",
        ["OK"] = "确定",
        ["✕ Not now"] = "✕ 暂不",
        ["The language found in the game's settings is marked."] = "游戏设置里查到的语言已标出。",
        ["The game's text can't be read as {0}: move hints may be missing or wrong."] = "游戏文字按 {0} 读不出来：出招提示可能缺失或不准。",
        ["The language of the game's text; the app itself stays in English. You can change it later under Recognition: Game language."] = "这里选的是游戏内文字的语言，不是本窗口的界面语言。之后可以在「识别 → 语言」里改。",
        ["Download"] = "下载",
        ["Not now"] = "暂不",
        ["Retry"] = "重试",
        ["Connecting to {0}…"] = "正在连接 {0}…",
        ["{0} of {1} MB from {2}"] = "{0} / {1} MB，来自 {2}",
        ["Download failed: {0}"] = "下载失败：{0}",
        ["The game is in {0}. Reading its text needs the {0} text recognition model ({1} MB, PaddleOCR). Download it now?\n\nWithout it the game's text won't be recognized; cards and picks are still told apart by their pictures."] = "游戏语言是 {0}。识别游戏文字需要 {0} 的文字识别模型（{1} MB，PaddleOCR）。现在下载吗？\n\n不下的话游戏文字识别不了；卡牌和选择界面仍可凭图案区分。",
        ["What went wrong?"] = "出什么问题了？",
        ["The app caught an error. What were you doing just before it?"] = "程序捕获到一个错误。出错前你在做什么？",
        ["Sending…"] = "发送中…",
        ["Send"] = "发送",
        ["By sending this report you share a screenshot of the game window, the app's log and the current Tartarus run's lines from the game log with the developer. No personal data is sent."] = "发送报告会把游戏窗口截图、程序日志和本局塔罗斯在游戏日志中的相关行交给开发者。不会发送任何个人数据。",
    };
}

/// <summary>
/// The interface's own strings, taken as the XAML file wrote them: <see cref="Loc.Apply"/> walks a window's logical tree
/// once it is built and swaps every line the table has a word for. Nothing in the XAML changes, so the English text stays
/// where the author wrote it and a line added upstream shows in English until it is translated, never blank.
/// </summary>
public static class LocTree
{
    /// <summary>Translates a built window's texts, contents and tooltips in place (a no-op in English).</summary>
    public static void Apply(DependencyObject root)
    {
        if (!Loc.Chinese) return;
        foreach (var child in System.Windows.LogicalTreeHelper.GetChildren(root))
            if (child is DependencyObject node) Apply(node);
        switch (root)
        {
            case System.Windows.Controls.TextBlock { Text.Length: > 0 } text:
                text.Text = Loc.T(text.Text);
                break;
            case System.Windows.Controls.ContentControl { Content: string content } control when content.Length > 0:
                control.Content = Loc.T(content);
                break;
        }
        if (root is System.Windows.FrameworkElement { ToolTip: string tip } element && tip.Length > 0)
            element.ToolTip = Loc.T(tip);
        // A window's title isn't part of the tree below it.
        if (root is System.Windows.Window { Title.Length: > 0 } window)
            window.Title = Loc.T(window.Title);
    }
}
