/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Modules.Project
{
    static class ProjectPersist
    {
        public static bool NewProject(string path, string prjWorkSpaceFileName)
        {
            SEXmlDocument xmlDoc;
            XmlNode xmlnode;
            XmlElement xmlele;
            # region ������Ŀ�ļ� *.SIMBEWS
            xmlDoc = new SEXmlDocument();
            xmlDoc.CreateDefaultDeclaration();
            xmlele = xmlDoc.CreateElement("PrjWorkSpace");
            xmlele.SetAttribute("Version", Application.ProductVersion);
            xmlele.AppendChild(xmlDoc.CreateElement("ProjectInfo"));
            xmlele.AppendChild(xmlDoc.CreateElement("MainMenu"));
            xmlele.AppendChild(xmlDoc.CreateElement("MainToolStrip"));
            xmlele.AppendChild(xmlDoc.CreateElement("DataEntity"));
            xmlele.AppendChild(xmlDoc.CreateElement("Form"));
            xmlele.AppendChild(xmlDoc.CreateElement("Enum"));
            xmlDoc.AppendChild(xmlele);
            xmlDoc.SelectSingleNode("PrjWorkSpace/ProjectInfo").InnerText = "01.SEC";
            xmlDoc.SelectSingleNode("PrjWorkSpace/MainMenu").InnerText = "02.SEC";
            xmlDoc.SelectSingleNode("PrjWorkSpace/MainToolStrip").InnerText = "03.SEC";
            xmlDoc.SelectSingleNode("PrjWorkSpace/DataEntity").InnerText = "04.SEC";
            xmlDoc.SelectSingleNode("PrjWorkSpace/Form").InnerText = "05.SEC";
            xmlDoc.SelectSingleNode("PrjWorkSpace/Enum").InnerText = "06.SEC";
            XMLFile.WriteXmlFile(path + "\\" + prjWorkSpaceFileName + ".SIMBEWS", xmlDoc.OuterXml);
            ProjectEntityDev projectEntity = new ProjectEntityDev();
            projectEntity.Name = prjWorkSpaceFileName;
            projectEntity.Code = "NewProject";
            projectEntity.UserModel = true;
            projectEntity.UserPopedomModel = true;
            projectEntity.UserSubsequent = true;
            xmlDoc = new SEXmlDocument();
            xmlDoc.CreateDefaultDeclaration();
            xmlDoc.AppendInnerXml(projectEntity.ToXml());
            XMLFile.WriteXmlFile(path + "\\01.SEC", xmlDoc.OuterXml);
            xmlDoc = new SEXmlDocument();
            xmlDoc.CreateDefaultDeclaration();
            xmlele = xmlDoc.CreateElement("MainMenus");
            xmlDoc.AppendChild(xmlele);
            XMLFile.WriteXmlFile(path + "\\02.SEC", xmlDoc.OuterXml);
            xmlDoc = new SEXmlDocument();
            xmlDoc.CreateDefaultDeclaration();
            xmlDoc.AppendChild("MainToolStrip");
            xmlDoc.AppendChild("MainToolStrip", "ImageSize", ((int)SEToolStripImageSize.Medium).ToString());
            XMLFile.WriteXmlFile(path + "\\03.SEC", xmlDoc.OuterXml);
            xmlDoc = new SEXmlDocument();
            xmlDoc.CreateDefaultDeclaration();
            xmlele = xmlDoc.CreateElement("DataEntity");
            xmlDoc.AppendChild(xmlele);
            XMLFile.WriteXmlFile(path + "\\04.SEC", xmlDoc.OuterXml);
            xmlDoc = new SEXmlDocument();
            xmlDoc.CreateDefaultDeclaration();
            xmlele = xmlDoc.CreateElement("Forms");
            xmlDoc.AppendChild(xmlele);
            xmlnode = xmlDoc.SelectSingleNode("Forms");
            xmlele = xmlDoc.CreateElement("Folders");
            xmlnode.AppendChild(xmlele);
            XMLFile.WriteXmlFile(path + "\\05.SEC", xmlDoc.OuterXml);
            xmlDoc = new SEXmlDocument();
            xmlDoc.CreateDefaultDeclaration();
            xmlele = xmlDoc.CreateElement("Enums");
            xmlDoc.AppendChild(xmlele);
            XMLFile.WriteXmlFile(path + "\\06.SEC", xmlDoc.OuterXml);
            DirectoryInfo di = new DirectoryInfo(path + "\\Resources");
            di.Create();
            return true;
        }
        public static void GetProjectEntity(ref ProjectEntityDev projectEntity, string prjWorkSpaceFile)
        {
            FileInfo fi = new FileInfo(prjWorkSpaceFile);
            projectEntity = new ProjectEntityDev();
            XmlDocument xmlDoc;
            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(XMLFile.ReadXmlFile(prjWorkSpaceFile));
            string strProjectInfoFile = fi.DirectoryName + "\\" + xmlDoc.SelectSingleNode("PrjWorkSpace/ProjectInfo").InnerText;
            xmlDoc.LoadXml(XMLFile.ReadXmlFile(strProjectInfoFile));
            projectEntity.FromXml(xmlDoc.SelectSingleNode("ProjectInfo").OuterXml);
        }
        public static void SaveProjectEntity()
        {
            FileInfo fi = new FileInfo(WorkSpace.WorkSpaceFile);
            XmlDocument xmlDoc;
            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(XMLFile.ReadXmlFile(WorkSpace.WorkSpaceFile));
            string strProjectInfoFile = fi.DirectoryName + "\\" + xmlDoc.SelectSingleNode("PrjWorkSpace/ProjectInfo").InnerText;
            XmlNode xmlnode = xmlDoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            XMLFile.WriteXmlFile(strProjectInfoFile, xmlnode.OuterXml + WorkSpace.ProjectEntity.ToXml());
        }
        public static void BuildRuntimeFile(string objPath)
        {
            XElement rtBinDoc = new XElement("PRJ");
            rtBinDoc.Add(new XAttribute("Version", Application.ProductVersion));
            rtBinDoc.Add(XElement.Parse(XMLFile.ReadXmlFile(WorkSpace.DataEntityFile)));
            rtBinDoc.Add(XElement.Parse(XMLFile.ReadXmlFile(WorkSpace.EnumFile)));
            rtBinDoc.Add(XElement.Parse(XMLFile.ReadXmlFile(WorkSpace.FormFile)));
            rtBinDoc.Add(XElement.Parse(XMLFile.ReadXmlFile(WorkSpace.MainMenuFile)));
            rtBinDoc.Add(XElement.Parse(XMLFile.ReadXmlFile(WorkSpace.MainToolStripFile)));
            rtBinDoc.Add(XElement.Parse(XMLFile.ReadXmlFile(WorkSpace.ProjectInfoFile)));
            XMLFile.WriteXmlFile(objPath + "\\RT.BIN", rtBinDoc.ToString());
        }
    }
}
