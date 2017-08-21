// Known issues with certain DocumentStyles:
//   DockingMdi:
//    - this is the default value
//    - after switching between layouts, text editor tooltips sometimes do not show up anymore
//   DockingSdi:
//    - in this mode, the tab bar is not shown when there is only one open window
//   DockingWindow:
//    - SharpDevelop 2.x used this mode
//    - it was also the only mode supported by the early DockPanelSuite versions used by SharpDevelop 1.x

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Sheng.SailingEase.Controls;
using Sheng.SailingEase.Controls.Docking;
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Shell.Localisation;
using Sheng.SailingEase.Win32;

namespace Sheng.SailingEase.Shell.View
{
    /// <summary>
    /// 主窗体
    /// </summary>
    partial class ShellView : Form, IShellView
    {
        #region 私有成员

        IEventAggregator _eventAggregator;

        private MenuStripAreoView _menuStrip = new MenuStripAreoView(new MenuStripCodon("Main"))
        {
            Dock = DockStyle.Top
        };

        private MainToolStripPanel _toolStripPanel = new MainToolStripPanel()
        {
            Dock = DockStyle.Top
        };

        private MainToolStripPanelPresenter _toolStripPanelPresenter;

        ///// <summary>
        ///// 主工具栏项目集合
        ///// </summary>
        //private ToolStripItemCodonCollection _mainToolStripCollection;

        private Timer _statusUpdateTimer;

        private ShellViewPresenter _presenter;

        #endregion

        #region 构造和窗体事件

        public ShellView(IEventAggregator eventAggregator)
        {
            InitializeComponent();

            EnvironmentHelper.MainForm = this;

            _eventAggregator = eventAggregator;

            _presenter = new ShellViewPresenter(this);

            //处理 Areo 半透明效果
            this.Paint += new PaintEventHandler(ShellView_Paint);

            this.statusStrip.Renderer = ToolStripRenders.Default;

            //应用资源
            //base.ApplyDefLanguageResource();

            //初始化窗体
            InitialiseForm();

            this.dockPanel.ActiveDocumentChanged += new EventHandler(dockPanel_ActiveDocumentChanged);

            //ProjectLogic.OnProjectOpened += new ProjectServiceHandler(ProjectService_OnProjectOpened);
            //ProjectLogic.OnProjectClosed += new ProjectServiceHandler(ProjectService_OnProjectClosed);

            _statusUpdateTimer = new Timer();
            _statusUpdateTimer.Tick += (sender, e) => { UpdateWorkSpaceStatus(); };

            _statusUpdateTimer.Interval = 500;
            _statusUpdateTimer.Start();
        }

        private void ShellView_Paint(object sender, PaintEventArgs e)
        {
            #region 处理 Areo 效果

            //处理 Dwm 半透明效果
            if (EnvironmentHelper.SupportAreo && EnvironmentHelper.DwmIsCompositionEnabled)
            {
                switch (_RenderMode)
                {
                    case RenderMode.EntireWindow:
                        e.Graphics.FillRectangle(Brushes.Black, this.ClientRectangle);
                        break;

                    case RenderMode.TopWindow:
                        e.Graphics.FillRectangle(Brushes.Black, Rectangle.FromLTRB(0, 0, this.ClientRectangle.Width, _glassMargins.cyTopHeight));
                        break;

                    case RenderMode.Region:
                        if (_blurRegion != null) e.Graphics.FillRegion(Brushes.Black, _blurRegion);
                        break;
                }
            }

            #endregion
        }

        private void ShellView_Load(object sender, EventArgs e)
        {
            UpdateWorkSpaceStatus();

            SubscribeEvent();

            _presenter.ApplicationRun();
        }

        private void ShellView_FormClosing(object sender, FormClosingEventArgs e)
        {
            //防止主窗体还在显示 载入还没有完成就点了关闭
            //FormSplashWindow.CloseSplash(true);
        }

        private void ShellView_FormClosed(object sender, FormClosedEventArgs e)
        {
            //ProjectLogic.OnProjectOpened -= new ProjectServiceHandler(ProjectService_OnProjectOpened);
            //ProjectLogic.OnProjectClosed -= new ProjectServiceHandler(ProjectService_OnProjectClosed);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化窗体
        /// 这个事件放在构造里，而不是load里
        /// 因为窗体初始状态如果是最大化，windows会首先让它显示出来，再执行load中的代码
        /// 这样就会使窗体在未加载完成时，看上去是一个灰色的空窗体，效果不是很好
        /// </summary>
        private void InitialiseForm()
        {
            DockPanelPresenter dockPanelPresenter = new DockPanelPresenter(this.dockPanel);

            //初始化菜单
            _menuStrip.ItemRegister += (args) =>
            {
                args.Item.MouseEnter += (sender, e) => { SetStatusMessage(args.Codon.Description); };
                args.Item.MouseLeave += (sender, e) => { SetStatusMessage(null); };
            };

            this.Controls.Add(_menuStrip);
            //this.MainMenuStrip = _menuStrip;
            //_menuStrip.BringToFront();
            NavigationInitialise.InitialiseMenu(_menuStrip);

            //初始化工具栏
            ToolStripView mainToolStrip = new ToolStripView(new ToolStripCodon("Main"));
            mainToolStrip.Renderer = ToolStripRenders.TransparentToolStrip;
            _toolStripPanel.Controls.Add(mainToolStrip);
            this.Controls.Add(_toolStripPanel);
            NavigationInitialise.InitialiseToolStrip(mainToolStrip);

            _toolStripPanelPresenter = new MainToolStripPanelPresenter(_toolStripPanel);

            //把主菜单显示到顶上面
            _menuStrip.SendToBack();

            InitAreo();

            //调用一次关闭工作区方法
            //CloseWorkSpace();

            //显示开始页面
            //Sheng.SIMBE.IDE.Gui.StartPageInstance.Show();

            //FormSplashWindow.CloseSplash();

            this.Activate();

            //Workbench.Instance.SetStatusBarMessage(Language.Current.StatusBarMessage_Ready);
        }

        /// <summary>
        /// 更新当前主界面的工作区状态
        /// </summary>
        private void UpdateWorkSpaceStatus()
        {
            this._toolStripPanel.UpdateStatus();
            this._menuStrip.UpdateStatus();
        }

        private void CloseView(IDockContent view)
        {
            if (view.DockHandler.HideOnClose)
            {
                view.DockHandler.Hide();
            }
            else
            {
                view.DockHandler.Close();
            }
        }

        private void SubscribeEvent()
        {
            _eventAggregator.GetEvent<ProjectPreCloseEvent>().Subscribe((e) =>
            {
                foreach (IDockContent item in this.dockPanel.DocumentsToArray())
                {
                    item.DockHandler.Close();
                }
            });
        }

        #endregion

        #region 事件处理

        void dockPanel_ActiveDocumentChanged(object sender, EventArgs e)
        {
            IView view = dockPanel.ActiveDocument as IView;

            //发布事件
            ActiveWorkbenchViewChangedEventArgs args = new ActiveWorkbenchViewChangedEventArgs(view);
            _eventAggregator.GetEvent<ActiveWorkbenchViewChangedEvent>().Publish(args);
        }

        protected override void WndProc(ref Message msg)
        {
            base.WndProc(ref msg); // let the normal winproc process it

            const int WM_DWMCOMPOSITIONCHANGED = 0x031E;
            const int WM_NCHITTEST = 0x84;
            const int HTCLIENT = 0x01;

            ////当系统颜色改变时，发送此消息给所有顶级窗口 
            //const int WM_SYSCOLORCHANGE = 0x0015; 

            switch (msg.Msg)
            {
                #region 处理 Areo 效果

                case WM_NCHITTEST:
                    if (HTCLIENT == msg.Result.ToInt32())
                    {
                        // it's inside the client area

                        // Parse the WM_NCHITTEST message parameters
                        // get the mouse pointer coordinates (in screen coordinates)
                        Point p = new Point();
                        p.X = (msg.LParam.ToInt32() & 0xFFFF);// low order word
                        p.Y = (msg.LParam.ToInt32() >> 16); // hi order word

                        // convert screen coordinates to client area coordinates
                        p = PointToClient(p);

                        // if it's on glass, then convert it from an HTCLIENT
                        // message to an HTCAPTION message and let Windows handle it from then on
                        if (PointIsOnGlass(p))
                            msg.Result = new IntPtr(2);
                    }
                    break;

                case WM_DWMCOMPOSITIONCHANGED:
                    if (DwmApi.DwmIsCompositionEnabled() == false)
                    {
                        _RenderMode = RenderMode.None;
                        _glassMargins = null;
                        if (_blurRegion != null)
                        {
                            _blurRegion.Dispose();
                            _blurRegion = null;
                        }
                    }
                    else
                    {
                        InitAreo();
                    }
                    break;

                #endregion
            }
        }

        #endregion

        #region IShellView 成员

        public Form Form
        {
            get { return this; }
        }

        public Point ToolStripPanelLocation
        {
            get { return this._toolStripPanel.Location; }
        }

        public new Point PointToScreen(Point point)
        {
            return base.PointToScreen(point);
        }

        public void Show<T>() where T : IView
        {
            IView view = ServiceUnity.Container.Resolve<T>();
            this.Show(view);
        }

        public void Show<T>(object singleKey) where T : IView
        {
            if (ActivateWindow(singleKey) == false)
            {
                IView view = ServiceLocator.Current.GetInstance<IUnityContainer>().Resolve<T>();
                this.Show(view);
            }
        }

        public void Show(Func<IView> func, object singleKey)
        {
            if (ActivateWindow(singleKey) == false)
            {
                this.Show(func());
            }
        }

        public void Show(IView view)
        {
            if (ActivateWindow(view.SingleKey) == false)
            {
                IDockContent context = view as IDockContent;
                if (context.DockHandler.TabPageContextMenuStrip == null)
                    context.DockHandler.TabPageContextMenuStrip = GlobalTabPageContextMenuStrip.Instance;

                view.Show(this.dockPanel);
            }
        }

        public void CloseView(IView view)
        {
            IDockContent content = view as IDockContent;
            if (content == null)
                return;

            CloseView(content);
        }

        public T GetView<T>() where T : IView
        {
            foreach (IDockContent content in this.dockPanel.Contents)
            {
                if (content is T)
                {
                    return (T)content;
                }
            }

            return default(T);
        }

        public bool ActivateWindow(object singleKey)
        {
            if (singleKey == null)
                return false;

            bool activated = false;

            IView eachView;
            foreach (IDockContent content in this.dockPanel.Contents)
            {
                eachView = content as IView;
                if (eachView == null)
                    continue;

                if (eachView.Single && eachView.CompareSingleKey(singleKey))
                {
                    ((DockContent)content).Activate();
                    activated = true;
                    break;
                }
            }

            return activated;
        }

        public void SetStatusMessage(string msg)
        {
            if (String.IsNullOrEmpty(msg) == false)
                this.toolStripStatusMessage.Text = msg;
        }

        public void RegisterMenu(string path, IToolStripItemCodon toolStripItem)
        {
            this._menuStrip.RegisterItem(path, toolStripItem);
        }

        public void RegisterToolStrip(string path, IToolStripItemCodon toolStripItem)
        {
            this._toolStripPanel.RegisterItem(path, toolStripItem);
        }

        public void ActivateToolStrip(ToolStripCodon toolStripCodon)
        {
            _toolStripPanelPresenter.ActivateToolStrip(toolStripCodon);
        }

        public void DeactiveToolStrip(ToolStripCodon toolStripCodon)
        {
            _toolStripPanelPresenter.DeactiveToolStrip(toolStripCodon);
        }

        public void DestroyToolStrip(System.Collections.Generic.List<ToolStripCodon> toolStripCodonList)
        {
            _toolStripPanelPresenter.DestroyToolStrip(toolStripCodonList);
        }

        public void CloseAllViewButThis()
        {
            IDockContent view = this.dockPanel.ActiveDocument;
            if (view == null)
                return;

            foreach (IDockContent item in this.dockPanel.DocumentsToArray())
            {
                if (item == view)
                    continue;

                CloseView(item);
            }
        }

        public void CloseAllView()
        {
            foreach (IDockContent item in this.dockPanel.DocumentsToArray())
            {
                CloseView(item);
            }
        }

        public void CloseView()
        {
            IDockContent view = this.dockPanel.ActiveDocument;
            if (view == null)
                return;

            CloseView(view);
        }

        #endregion

        #region Areo效果

        private DwmApi.MARGINS _glassMargins;
        private enum RenderMode { None, EntireWindow, TopWindow, Region };
        private RenderMode _RenderMode;
        private Region _blurRegion;

        private void InitAreo()
        {
            if (EnvironmentHelper.SupportAreo && EnvironmentHelper.DwmIsCompositionEnabled)
            {
                _glassMargins = new DwmApi.MARGINS(0, this._menuStrip.Height, 0, 0);
                //m_glassMargins = new DwmApi.MARGINS(0, 150 ,0, 0);
                _RenderMode = RenderMode.TopWindow;

                if (DwmApi.DwmIsCompositionEnabled())
                    DwmApi.DwmExtendFrameIntoClientArea(this.Handle, _glassMargins);

                // reset window border style in case DwmEnableBlurBehindWindow was set
                //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;

                this.Invalidate();
                this._menuStrip.Invalidate();
            }
        }

        private bool PointIsOnGlass(Point p)
        {
            // test for region or entire client area
            // or if upper window glass and inside it.
            // not perfect, but you get the idea
            return _glassMargins != null &&
                (_glassMargins.cyTopHeight <= 0 ||
                 _glassMargins.cyTopHeight > p.Y);
        }

        #endregion

    }
}