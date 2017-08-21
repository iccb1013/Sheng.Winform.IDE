/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace Data.SqlCe.Tests.VSTS
{
	public class TestConnectionString
	{
		private string connectionString;
		private string filename;
		public TestConnectionString() : this("test.sdf")
		{
		}
		public TestConnectionString(string filename)
		{
			this.filename = Path.Combine(Environment.CurrentDirectory, filename);
			this.connectionString = "Data Source='{0}'";
			this.connectionString = String.Format(connectionString, filename);
		}
		public string ConnectionString
		{
			get { return this.connectionString; }
		}
		public void CopyFile()
		{
			if (File.Exists(filename))
				File.Delete(filename);
			string sourceFile = Path.Combine(Environment.CurrentDirectory, "TestDb.sdf");
			File.Copy(sourceFile, filename);
		}
		public void DeleteFile()
		{
			File.Delete(filename);
		}
		public string Filename
		{
			get { return this.filename; }
		}
	}
}
