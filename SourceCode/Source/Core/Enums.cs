using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Sheng.SailingEase.Core
{
    # region 系统枚举

    //TODO:也应该定义成类，并向外提供所支持的数据类型
    /// <summary>
    /// 可使用的数据库类型
    /// </summary>
    public enum EnumDataBase
    {
        [LocalizedDescription("EnumDataBase_SqlServer")]
        SqlServer = 0
    }

    //TODO:应该定义成类
    //已经没用了
    /// <summary>
    /// 数据项类型（SQL Server）
    /// </summary>
    public enum EnumDataItemType
    {
        /// <summary>
        /// 定长字符串
        /// </summary>
        [LocalizedDescription("EnumDataItemType_Char")]
        Char = 0,
        /// <summary>
        /// 变长字符串
        /// </summary>
        [LocalizedDescription("EnumDataItemType_Varchar")]
        Varchar = 1,
        /// <summary>
        /// 日期时间
        /// </summary>
        [LocalizedDescription("EnumDataItemType_SmallDatetime")]
        SmallDatetime = 2,
        /// <summary>
        /// 双精度型
        /// </summary>
        [LocalizedDescription("EnumDataItemType_Decimal")]
        Decimal = 3,
        /// <summary>
        /// 单精度型
        /// </summary>
        [LocalizedDescription("EnumDataItemType_Float")]
        Float = 4,
        /// <summary>
        /// 整型
        /// </summary>
        [LocalizedDescription("EnumDataItemType_Int")]
        Int = 5,
        /// <summary>
        /// 长文本
        /// </summary>
        [LocalizedDescription("EnumDataItemType_Text")]
        Text = 6,
        /// <summary>
        /// 数据流
        /// </summary>
        [LocalizedDescription("EnumDataItemType_Image")]
        Image = 7,
        /// <summary>
        /// 布尔型
        /// </summary>
        [LocalizedDescription("EnumDataItemType_Bit")]
        Bit = 8,
        /// <summary>
        /// 唯一标识
        /// </summary>
        [LocalizedDescription("EnumDataItemType_Uniqueidentifier")]
        Uniqueidentifier = 9
    }

    ///// <summary>
    ///// 窗口元素的具体控件类型，包括所有可用控件
    ///// 元素比较多，不使用Flags，需要多选时，使用EnumFormElementControlTypeCollection集合来实现
    ///// //TODO:EnumFormElementControlType 考虑改进，不使用枚举
    ///// </summary>
    //public enum EnumFormElementControlType
    //{
    //    Null = 0,
    //    /// <summary>
    //    /// 任意,用于一些列表,下拉列表过滤控件
    //    /// </summary>
    //    ANY = 1,
    //    /// <summary>
    //    /// 文本框
    //    /// </summary>
    //    TextBox = 2,
    //    /// <summary>
    //    /// 复合文本框
    //    /// </summary>
    //    ComboBox = 3,
    //    /// <summary>
    //    /// 数据列表
    //    /// </summary>
    //    DataList = 4,
    //    /// <summary>
    //    /// 静态文本
    //    /// </summary>
    //    Label = 5,
    //    /// <summary>
    //    /// 图片框
    //    /// </summary>
    //    PictureBox = 6,
    //    /// <summary>
    //    /// 普通按钮
    //    /// </summary>
    //    Button = 7,
    //    /// <summary>
    //    /// 数据列，仅用于DataList内
    //    /// </summary>
    //    DataColumn = 8,
    //    /// <summary>
    //    /// 分隔条（用于菜单，或工具栏中）
    //    /// </summary>
    //    Separator = 9,
    //    /// <summary>
    //    /// 工具栏按钮
    //    /// </summary>
    //    ToolStripButton = 10,
    //    /// <summary>
    //    /// 工具栏文本
    //    /// </summary>
    //    ToolStripLabel = 11,
    //    ///// <summary>
    //    ///// 菜单栏,工具栏菜单项
    //    ///// [否决] 忘记为什么要加这个了
    //    ///// </summary>
    //    //ToolStripMenuItem = 12,
    //}

    ///// <summary>
    ///// 动作枚举，与可用动作一一对应
    ///// </summary>
    //public enum EnumEvent
    //{
    //    //TODO:EnumEvent 否决，可以改用Attribute之类的方法解决
    //    ClearFormData = 0,
    //    Exit = 1,
    //    LoadDataToForm = 2,
    //    LockProgram = 3,
    //    NewGuid = 4,
    //    OpenForm = 5,
    //    ReceiveData = 6,
    //    //RefreshList = 7,
    //    ReLogin = 8,
    //    SaveFormData = 9,
    //    StartProcess = 10,
    //    UpdateFormData = 11,
    //    CloseForm = 12,
    //    OpenSystemForm = 13,
    //    ValidateFormData = 14,
    //    CallAddIn = 15,
    //    ReturnDataToCallerForm = 16,
    //    //DataListOperator = 17,
    //    DeleteData = 18,
    //    /// <summary>
    //    /// 数据列表的刷新事件
    //    /// </summary>
    //    DataListRefresh = 19,
    //    /// <summary>
    //    /// 调用对象方法
    //    /// </summary>
    //    CallEntityMethod = 20,
    //    /// <summary>
    //    /// 数据列表的新增行事件
    //    /// </summary>
    //    DataListAddRow = 21,
    //    /// <summary>
    //    /// 数据列表的更新行事件
    //    /// </summary>
    //    DataListUpdateRow = 22,
    //    /// <summary>
    //    /// 数据列表的删除行事件
    //    /// </summary>
    //    DataListDeleteRow = 23
    //}

    #endregion

    /// <summary>
    /// 是否
    /// </summary>
    public enum EnumTrueFalse
    {
        /// <summary>
        /// 否
        /// </summary>
        [LocalizedDescription("EnumTrueFalse_False")]
        [DefaultValue(false)]
        False = 0,
        /// <summary>
        /// 是
        /// </summary>
        [LocalizedDescription("EnumTrueFalse_True")]
        [DefaultValue(true)]
        True = 1
    }

    /// <summary>
    /// 选数据源时可用的系统类型
    /// </summary>
    public enum EnumSystemDataSource
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [LocalizedDescription("EnumSystemDataSource_UserId")]
        UserId = 0,
        /// <summary>
        /// 用户名
        /// </summary>
        [LocalizedDescription("EnumSystemDataSource_UserName")]
        UserName = 1,
        /// <summary>
        /// 用户登录名
        /// </summary>
        [LocalizedDescription("EnumSystemDataSource_UserLoginName")]
        UserLoginName = 2,
        /// <summary>
        /// 用户所在组Id
        /// 若用户不属于任何组，组Id用全0表示
        /// </summary>
        [LocalizedDescription("EnumSystemDataSource_UserGroupId")]
        UserGroupId = 3,
        /// <summary>
        /// 用户所在组名
        /// </summary>
        [LocalizedDescription("EnumSystemDataSource_UserGroupName")]
        UserGroupName = 4,
        /// <summary>
        /// 当前日期
        /// </summary>
        [LocalizedDescription("EnumSystemDataSource_Date")]
        Date = 5,
        /// <summary>
        /// 当前日期时间
        /// </summary>
        [LocalizedDescription("EnumSystemDataSource_DateTime")]
        DateTime = 6
    }

    //TODO:移动到 IDataBaseProvide
    /// <summary>
    /// 匹配方式
    /// 如用于刷新列表事件的条件设置部分
    /// </summary>
    public enum EnumMatchType
    {
        /// <summary>
        /// 等于
        /// </summary>
        [LocalizedDescription("EnumMatchType_Equal")]
        Equal = 0,
        /// <summary>
        /// 匹配
        /// </summary>
        [LocalizedDescription("EnumMatchType_Like")]
        Like = 1,
        /// <summary>
        /// 大于
        /// </summary>
        [LocalizedDescription("EnumMatchType_Large")]
        Large = 2,
        /// <summary>
        /// 大于等于
        /// </summary>
        [LocalizedDescription("EnumMatchType_LargeEqual")]
        LargeEqual = 3,
        /// <summary>
        /// 小于
        /// </summary>
        [LocalizedDescription("EnumMatchType_Less")]
        Less = 4,
        /// <summary>
        /// 小于等于
        /// </summary>
        [LocalizedDescription("EnumMatchType_LessEqual")]
        LessEqual = 5,
        /// <summary>
        /// 不等于
        /// </summary>
        [LocalizedDescription("EnumMatchType_NotEqual")]
        NotEqual = 6
    }

    /// <summary>
    /// 数据源类型
    /// </summary>
    public enum EnumDataSourceType
    {
        //TODO;考虑去除EditControl和DataList
        EditControl = 0,
        DataList = 1,
        System = 2
    }

    /// <summary>
    /// 否决
    /// 事件中所使用的数据源的来源类型
    /// 允许Flags
    /// </summary>
    [Flags]
    public enum EnumEventDataSource
    {
        //TODO:考虑改进，考虑把SYSTEM改成一个类似于窗体元素一样的类
        //可以注册到一个管理器中使用 Typeof这样的方法来使用

        /// <summary>
        /// 没有
        /// </summary>
        Null = 0,
        /// <summary>
        /// 全部
        /// </summary>
        ANY = 1,
        /// <summary>
        /// 窗体元素
        /// </summary>
        FormElement = 2,
        /// <summary>
        /// 系统
        /// </summary>
        System = 4
    }

    /// <summary>
    /// 目标窗体
    /// </summary>
    public enum EnumTargetWindow
    {
        /// <summary>
        /// 当前
        /// </summary>
        [LocalizedDescription("EnumTargetForm_Current")]
        Current = 0,
        /// <summary>
        /// 调用者
        /// </summary>
        [LocalizedDescription("EnumTargetForm_Caller")]
        Caller = 1
    }
}

