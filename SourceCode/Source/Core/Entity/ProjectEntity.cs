/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.ComponentModel;
namespace Sheng.SailingEase.Core
{
    public class ProjectEntity : EntityBase
    {
        private EnumDataBase _dataBase;
        public EnumDataBase DataBase
        {
            get
            {
                return this._dataBase;
            }
            set
            {
                this._dataBase = value;
            }
        }
        private bool _userModel;
        public bool UserModel
        {
            get
            {
                return this._userModel;
            }
            set
            {
                this._userModel = value;
            }
        }
        private bool _userPopedomModel;
        public bool UserPopedomModel
        {
            get
            {
                return this._userPopedomModel;
            }
            set
            {
                this._userPopedomModel = value;
            }
        }
        private bool _userSubsequent;
        public bool UserSubsequent
        {
            get
            {
                return this._userSubsequent;
            }
            set
            {
                this._userSubsequent = value;
            }
        }
        private string _company = String.Empty;
        public string Company
        {
            get
            {
                return this._company;
            }
            set
            {
                this._company = value;
            }
        }
        private string _version = String.Empty;
        public string Version
        {
            get
            {
                return this._version;
            }
            set
            {
                this._version = value;
            }
        }
        private string _summary = String.Empty;
        public string Summary
        {
            get
            {
                return this._summary;
            }
            set
            {
                this._summary = value;
            }
        }
        private string _copyright = String.Empty;
        public string Copyright
        {
            get
            {
                return this._copyright;
            }
            set
            {
                this._copyright = value;
            }
        }
        private string _splashImg = String.Empty;
        public string SplashImg
        {
            get
            {
                return this._splashImg;
            }
            set
            {
                this._splashImg = value;
            }
        }
        private string _backImg = String.Empty;
        public string BackImg
        {
            get
            {
                return this._backImg;
            }
            set
            {
                this._backImg = value;
            }
        }
        private string _aboutImg = String.Empty;
        public string AboutImg
        {
            get
            {
                return this._aboutImg;
            }
            set
            {
                this._aboutImg = value;
            }
        }
        public ProjectEntity()
        {
            this.XmlRootName = "ProjectInfo";
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.DataBase = (EnumDataBase)xmlDoc.GetInnerObject<int>("/General/DataBase", 0);
            this.UserModel = xmlDoc.GetInnerObject<bool>("/General/UserModel", true);
            this.UserPopedomModel = xmlDoc.GetInnerObject<bool>("/General/UserPopedomModel", true);
            this.UserSubsequent = xmlDoc.GetInnerObject<bool>("/General/UserSubsequent", true);
            this.Company = xmlDoc.GetInnerObject("/About/Company");
            this.Version = xmlDoc.GetInnerObject("/About/Version");
            this.Summary = xmlDoc.GetInnerObject("/About/Summary");
            this.Copyright = xmlDoc.GetInnerObject("/About/Copyright");
            this.SplashImg = xmlDoc.GetInnerObject("/About/SplashImg");
            this.BackImg = xmlDoc.GetInnerObject("/About/BackImg");
            this.AboutImg = xmlDoc.GetInnerObject("/About/AboutImg");
        }
        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());
            xmlDoc.AppendChild("General");
            xmlDoc.AppendChild("About");
            xmlDoc.AppendChild("/General", "PrjName", this.Name);
            xmlDoc.AppendChild("/General", "PrjCode", this.Code);
            xmlDoc.AppendChild("/General", "DataBase", (int)this.DataBase);
            xmlDoc.AppendChild("/General", "UserModel", this.UserModel);
            xmlDoc.AppendChild("/General", "UserPopedomModel", this.UserPopedomModel);
            xmlDoc.AppendChild("/General", "UserSubsequent", this.UserSubsequent);
            xmlDoc.AppendChild("/About", "Company", this.Company);
            xmlDoc.AppendChild("/About", "Version", this.Version);
            xmlDoc.AppendChild("/About", "Summary", this.Summary);
            xmlDoc.AppendChild("/About", "Copyright", this.Copyright);
            xmlDoc.AppendChild("/About", "SplashImg", this.SplashImg);
            xmlDoc.AppendChild("/About", "BackImg", this.BackImg);
            xmlDoc.AppendChild("/About", "AboutImg", this.AboutImg);
            return xmlDoc.ToString();
        }
    }
}
