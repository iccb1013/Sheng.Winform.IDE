工具栏和主菜单实现

把所有主菜单项和工具栏项缓存起来，可以考虑在程序启动时就把所有可能用到的初始化出来（但可能会对启动时的效率有所影响）
所有子窗体实现一个IWorkBenchWindow接口，WorkBench中定义一个当前活动窗体的属性，当切换当前活动窗体时，设置这个属性，这个属性的set中，调用IWorkBenchWindow所绑定的主菜单顶层项目（可能多个）和工具栏项目（可能多个），判断当前是否要显示，还需要调用原来的活动子窗口的这两个项目，以把原来的窗口所绑定的项目不显示或不可用（如果需要）。
工作区子窗体定义一个基类，这个基类重写活动和非活动两个事件，去设置WorkBench的当前活动窗口属性，不要忘记再调用base.的。
所有菜单项目和工具栏项目自己定义一套类来表示，初始化的时候再绑定成WinformUI控件，项目是否可用，就通过调用这个对象的属性来获取，可能各需要定义一个抽象基类实现默认的成员或方法。



获取右键菜单打开者的方法
ContextMenuStrip menu = ((ToolStripMenuItem)sender).GetCurrentParent() as ContextMenuStrip;
MessageBox.Show(menu.SourceControl.Name);


ToolStripComboBoxCodon 的使用
            //toolStripCodon.Items.Add(new ToolStripLabelCodon("LabelRender", "呈现方式:"));
            //List<Renderer> renders = new List<Renderer>();
            //renders.Add(new Renderer("默认", new ImageListViewRenderers.DefaultRenderer()));
            //renders.Add(new Renderer("平铺", new ImageListViewRenderers.TilesRenderer()));
            //toolStripCodon.Items.Add(new ToolStripComboBoxCodon("Render", renders,
            //    (sender, args) =>
            //    {
            //        Renderer render = (Renderer)args.SelectedItem;
            //        imageListView.SetRenderer(render.Render);
            //        imageListView.Focus();
            //    })
            //    {
            //        DisplayMember = "Name",
            //    });