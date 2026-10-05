namespace PluginCore;

/// <summary>
///     自定义按键枚举
/// </summary>
public enum EKey
{
    [System.ComponentModel.Description("lang.kitopia.keys.not_set")]
    未设置 = -1,
    [System.ComponentModel.Description("lang.kitopia.keys.backspace")]
    退格 = 8,
    [System.ComponentModel.Description("lang.kitopia.keys.tab")]
    制表 = 9,
    [System.ComponentModel.Description("lang.kitopia.keys.clear")]
    清空 = 12,
    [System.ComponentModel.Description("lang.kitopia.keys.enter")]
    回车 = 13,
    Shift = 16,
    Ctrl = 17,
    Alt = 18,
    [System.ComponentModel.Description("lang.kitopia.keys.pause")]
    暂停 = 19,
    [System.ComponentModel.Description("lang.kitopia.keys.caps_lock")]
    大写锁定 = 20,
    Esc = 27,
    [System.ComponentModel.Description("lang.kitopia.keys.space")]
    空格 = 32,
    [System.ComponentModel.Description("lang.kitopia.keys.page_up")]
    向前翻页 = 33,
    [System.ComponentModel.Description("lang.kitopia.keys.page_down")]
    向后翻页 = 34,
    [System.ComponentModel.Description("lang.kitopia.keys.end")]
    结束 = 35,
    [System.ComponentModel.Description("lang.kitopia.keys.home")]
    首页 = 36,
    [System.ComponentModel.Description("lang.kitopia.keys.left")]
    左箭头 = 37,
    [System.ComponentModel.Description("lang.kitopia.keys.up")]
    上箭头 = 38,
    [System.ComponentModel.Description("lang.kitopia.keys.right")]
    右箭头 = 39,
    [System.ComponentModel.Description("lang.kitopia.keys.down")]
    下箭头 = 40,
    [System.ComponentModel.Description("lang.kitopia.keys.print")]
    打印 = 42,
    [System.ComponentModel.Description("lang.kitopia.keys.print_screen")]
    截屏 = 44,
    [System.ComponentModel.Description("lang.kitopia.keys.insert")]
    插入 = 45,
    [System.ComponentModel.Description("lang.kitopia.keys.delete")]
    删除 = 46,
    [System.ComponentModel.Description("lang.kitopia.keys.help")]
    帮助 = 47,
    [System.ComponentModel.Description("lang.kitopia.keys.digit_0")]
    数字0 = 48,
    [System.ComponentModel.Description("lang.kitopia.keys.digit_1")]
    数字1 = 49,
    [System.ComponentModel.Description("lang.kitopia.keys.digit_2")]
    数字2 = 50,
    [System.ComponentModel.Description("lang.kitopia.keys.digit_3")]
    数字3 = 51,
    [System.ComponentModel.Description("lang.kitopia.keys.digit_4")]
    数字4 = 52,
    [System.ComponentModel.Description("lang.kitopia.keys.digit_5")]
    数字5 = 53,
    [System.ComponentModel.Description("lang.kitopia.keys.digit_6")]
    数字6 = 54,
    [System.ComponentModel.Description("lang.kitopia.keys.digit_7")]
    数字7 = 55,
    [System.ComponentModel.Description("lang.kitopia.keys.digit_8")]
    数字8 = 56,
    [System.ComponentModel.Description("lang.kitopia.keys.digit_9")]
    数字9 = 57,
    A = 65,
    B = 66,
    C = 67,
    D = 68,
    E = 69,
    F = 70,
    G = 71,
    H = 72,
    I = 73,
    J = 74,
    K = 75,
    L = 76,
    M = 77,
    N = 78,
    O = 79,
    P = 80,
    Q = 81,
    R = 82,
    S = 83,
    T = 84,
    U = 85,
    V = 86,
    W = 87,
    X = 88,
    Y = 89,
    Z = 90,
    [System.ComponentModel.Description("lang.kitopia.keys.windows")]
    Windows键 = 91,
    [System.ComponentModel.Description("lang.kitopia.keys.right_windows")]
    右Windows键 = 92,
    [System.ComponentModel.Description("lang.kitopia.keys.menu")]
    在应用程序键 = 93,
    [System.ComponentModel.Description("lang.kitopia.keys.sleep")]
    睡眠键 = 94,
    [System.ComponentModel.Description("lang.kitopia.keys.numpad_0")]
    小键盘数字0 = 96,
    [System.ComponentModel.Description("lang.kitopia.keys.numpad_1")]
    小键盘数字1 = 97,
    [System.ComponentModel.Description("lang.kitopia.keys.numpad_2")]
    小键盘数字2 = 98,
    [System.ComponentModel.Description("lang.kitopia.keys.numpad_3")]
    小键盘数字3 = 99,
    [System.ComponentModel.Description("lang.kitopia.keys.numpad_4")]
    小键盘数字4 = 100,
    [System.ComponentModel.Description("lang.kitopia.keys.numpad_5")]
    小键盘数字5 = 101,
    [System.ComponentModel.Description("lang.kitopia.keys.numpad_6")]
    小键盘数字6 = 102,
    [System.ComponentModel.Description("lang.kitopia.keys.numpad_7")]
    小键盘数字7 = 103,
    [System.ComponentModel.Description("lang.kitopia.keys.numpad_8")]
    小键盘数字8 = 104,
    [System.ComponentModel.Description("lang.kitopia.keys.numpad_9")]
    小键盘数字9 = 105,
    [System.ComponentModel.Description("lang.kitopia.keys.numpad_multiply")]
    小键盘乘法 = 106,
    [System.ComponentModel.Description("lang.kitopia.keys.numpad_add")]
    小键盘加法 = 107,
    [System.ComponentModel.Description("lang.kitopia.keys.numpad_subtract")]
    小键盘减法 = 109,
    [System.ComponentModel.Description("lang.kitopia.keys.numpad_decimal")]
    小键盘小数 = 110,
    [System.ComponentModel.Description("lang.kitopia.keys.numpad_divide")]
    小键盘除法 = 111,
    F1 = 112,
    F2 = 113,
    F3 = 114,
    F4 = 115,
    F5 = 116,
    F6 = 117,
    F7 = 118,
    F8 = 119,
    F9 = 120,
    F10 = 121,
    F11 = 122,
    F12 = 123,
    [System.ComponentModel.Description("lang.kitopia.keys.num_lock")]
    数字锁 = 144,
    [System.ComponentModel.Description("lang.kitopia.keys.scroll_lock")]
    滚动锁 = 145,
    [System.ComponentModel.Description("lang.kitopia.keys.volume_down")]
    音量减 = 174,
    [System.ComponentModel.Description("lang.kitopia.keys.volume_up")]
    音量加 = 175,
    [System.ComponentModel.Description("lang.kitopia.keys.mute")]
    取消静音 = 181,
    [System.ComponentModel.Description("lang.kitopia.keys.play_pause")]
    播放暂停 = 179,
    [System.ComponentModel.Description("lang.kitopia.keys.stop")]
    停止 = 183,
    [System.ComponentModel.Description("lang.kitopia.keys.next_track")]
    下一曲 = 176,
    [System.ComponentModel.Description("lang.kitopia.keys.previous_track")]
    上一曲 = 177,
    [System.ComponentModel.Description("lang.kitopia.keys.mail")]
    邮件 = 180,
    [System.ComponentModel.Description("lang.kitopia.keys.media_select")]
    选择媒体 = 181,
    [System.ComponentModel.Description("lang.kitopia.keys.launch_app_1")]
    启动应用程序1 = 182,
    [System.ComponentModel.Description("lang.kitopia.keys.launch_app_2")]
    启动应用程序2 = 183,
    SemiColon = 186,
    Equal = 187,
    Comma = 188,
    Dash = 189,
    Period = 190,
    ForwardSlash = 191,
    GraveAccent = 192,
    OpenBracket = 219,
    BackSlash = 220,
    CloseBraket = 221,
    SingleQuote = 222
}