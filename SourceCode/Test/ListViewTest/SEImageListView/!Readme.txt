ListView -> LayoutManager -> Renderer

ListView 可以指定不同的 LayoutManager ，LayoutManager 又可以指定不同的 Renderer

是 一对多 对多 的关系

但为了简化外部调用
使这层关系简化为 一对多 对一
即为每一个 Renderer 指定一个唯一的 LayoutManager
这样的外部设置控件的布局方式时，容易设置，不会说先把LayoutManager取出来，判断到底是哪种LayoutManager，
然后做类型转换，转换为实现LayoutManager类型之后，再去设置它可用的Renderer，这样做比较复杂，麻烦，且容易出错
因为无法约束外部调用者的类型转换

把外部调用简化为 一对多对一的关系之后，设置ListView的布局，只要设置不同类型的LayoutManager的即可
不存在不同的LayoutManager下又是不同的Renderer的情况

重载的不同LayoutManager，基本只做一件事，设置Renderer，当然也有可能需要设置ItemSize这样与绘制密切相关的属性