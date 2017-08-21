using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using Sheng.SailingEase.Infrastructure;
using Microsoft.Practices.ServiceLocation;

namespace Sheng.SailingEase.Modules.StartPageModule.View
{
    class StartPageCodePage
    {
        //TODO:CodePage 使用资源文件

        IEnvironmentService _environmentService;

        public StartPageCodePage()
        {
            _environmentService = ServiceUnity.Container.Resolve<IEnvironmentService>();
        }

        public virtual void RenderHeaderSection(StringBuilder builder)
        {
            builder.Append("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">");
            builder.Append("<html>");
            builder.Append("<head>");
            builder.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            builder.Append("<title>欢迎</title>");
            builder.Append("<link href=\"" +
                _environmentService.DataPath +
                Path.DirectorySeparatorChar + "resources" +
                Path.DirectorySeparatorChar + "startpage" +
                Path.DirectorySeparatorChar + "style.css\" rel=\"stylesheet\" type=\"text/css\">");
            builder.Append("</head>");

            builder.Append("<body>");
            builder.Append("<table width=\"100%\"  border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
            builder.Append("<tr>");
            builder.Append("<td><img src=\"" + _environmentService.DataPath +
                Path.DirectorySeparatorChar + "resources" +
                Path.DirectorySeparatorChar + "startpage" +
                Path.DirectorySeparatorChar + "Top.png\" width=\"672\" height=\"74\"></td>");
            builder.Append("</tr>");
            builder.Append("</table>");

            builder.Append("<table width=\"100%\"  border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
            builder.Append("<tr>");
            builder.Append("<td width=\"30\">&nbsp;</td>");
            builder.Append("<td height=\"30\">");
            builder.Append("<a href=\"startpage://start/\">启始页</a>&nbsp;&nbsp;&nbsp;");
            builder.Append("<a href=\"startpage://help/\">帮助与技术支持</a>&nbsp;&nbsp;&nbsp;");
            builder.Append("<a href=\"http://www.SailingEase.com\" target=\"_blank\">网站</a>&nbsp;&nbsp;&nbsp;");
            builder.Append("</td>");
            builder.Append("</tr>");
            builder.Append("</table>");
        }

        public virtual void RenderSectionStartBody(StringBuilder builder)
        {
            builder.Append("<table width=\"100%\"  border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
            builder.Append("<tr>");
            builder.Append("<td>&nbsp;</td>");
            builder.Append("</tr>");
            builder.Append("<tr>");
            builder.Append("<td width=\"30\"><!--这里是最近项目列表--></td>");
            builder.Append("<td>");

            //这里是最近项目列表

            builder.Append("最近项目:");
            builder.Append("<table  width=\"600\"  border=\"1\" cellpadding=\"0\" cellspacing=\"0\" bordercolor=\"#CCCCCC\" style=\"border-collapse:collapse;\">");
            builder.Append("<tr bgcolor=\"#FAFAFA\">");
            builder.Append("<td width=\"180\" height=\"24\" bgcolor=\"#FAFAFA\">&nbsp;&nbsp;<strong>名称</strong></td>");
            builder.Append("<td>&nbsp;&nbsp;<strong>位置</strong></td>");
            builder.Append("</tr>");

            string url = StartPageScheme.OPENPROJECT_URI+ "{0}";

            //循环部分
            foreach (string file in History.List)
            {
                if (File.Exists(file) == false)
                {
                    History.Remove(file);
                    continue;
                }

                builder.Append("<tr>");
                builder.Append("<td height=\"24\">&nbsp;");
                builder.Append("<a href=" + String.Format(url, file) + ">");
                builder.Append(Path.GetFileNameWithoutExtension(file));
                builder.Append("</a>");
                builder.Append("</td>");
                builder.Append("<td>&nbsp;");
                builder.Append(file);
                builder.Append("</td>");
            }
            //循环部分结束

            builder.Append("</table>");

            //最近项目列表输出结束

            builder.Append("</td>");
            builder.Append("</tr>");
            builder.Append("<tr>");
            builder.Append("<td>&nbsp;</td>");
            builder.Append("</tr>");
            builder.Append("</table>");

            builder.Append("<table width=\"100%\"  border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
            builder.Append("<tr>");
            builder.Append("<td width=\"30\">&nbsp;</td>");
            builder.Append("<td>");
            builder.Append("<input type=\"button\" id=\"btnOpenProject\" value=\"打开项目\">");
            builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
            builder.Append("<input type=\"button\" id=\"btnNewProject\" value=\"新建项目\">");
            builder.Append("</td>");
            builder.Append("</tr>");
            builder.Append("</table>");
        }

        public virtual void RenderSectionHelp(StringBuilder builder)
        {
            builder.Append(@"
            <table width='100%'  border='0' cellspacing='0' cellpadding='0'>
              <tr>
                <td colspan='2'>&nbsp;</td>
              </tr>
              <tr>
                <td width='30'><!--这里是最近项目列表--></td>
              <td><p>如何获取帮助?<br>
                <br>
                  请查看 &quot;帮助&quot; 菜单下的 &quot;示例&quot; , &quot;帮助&quot; 及 &quot;如何实现&quot;.<br>
                或浏览网站留意最新信息<br>
                我们将不断推出新的软件版本,及增加更多的示例<br>
                </p>
                </td>
              </tr>
              <tr>
                <td colspan='2'>&nbsp;</td>
              </tr>
            </table>
            ");
        }

        public virtual void RenderFinalPageBodySection(StringBuilder builder)
        {
            builder.Append("</body>");
        }

        public virtual void RenderPageEndSection(StringBuilder builder)
        {
            builder.Append("</html>");
        }

        public string Render(string section)
        {
            StringBuilder builder = new StringBuilder(2048);
            RenderHeaderSection(builder);

            switch (section.ToLowerInvariant())
            {
                case StartPageScheme.START_HOST:
                    RenderSectionStartBody(builder);
                    break;
                case StartPageScheme.HELP_HOST:
                    RenderSectionHelp(builder);
                    break;
            }

            RenderFinalPageBodySection(builder);
            RenderPageEndSection(builder);

            return builder.ToString();
        }
    }
}
