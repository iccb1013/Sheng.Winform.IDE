/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Globalization;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Data
{
	public class ConnectionString
	{
		private const char CONNSTRING_DELIM = ';';
		private string connectionString;
		private string connectionStringWithoutCredentials;
		private string userIdTokens;
		private string passwordTokens;
		public ConnectionString(string connectionString, string userIdTokens, string passwordTokens)
		{
			if (string.IsNullOrEmpty(connectionString)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "connectionString");
			if (string.IsNullOrEmpty(userIdTokens)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "userIdTokens");
			if (string.IsNullOrEmpty(passwordTokens)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "passwordTokens");
			this.connectionString = connectionString;
			this.userIdTokens = userIdTokens;
			this.passwordTokens = passwordTokens;
			this.connectionStringWithoutCredentials = null;
		}
		public string UserName
		{
			get
			{
				string lowConnString = connectionString.ToLower(CultureInfo.CurrentCulture);
				int uidPos;
				int uidMPos;
				GetTokenPositions(userIdTokens, out uidPos, out uidMPos);
				if (0 <= uidPos)
				{
					int uidEPos = lowConnString.IndexOf(CONNSTRING_DELIM, uidMPos);
					return connectionString.Substring(uidMPos, uidEPos - uidMPos);
				}
				else
				{
					return String.Empty;
				}
			}
			set
			{
				string lowConnString = connectionString.ToLower(CultureInfo.CurrentCulture);
				int uidPos;
				int uidMPos;
				GetTokenPositions(userIdTokens, out uidPos, out uidMPos);
				if (0 <= uidPos)
				{
					int uidEPos = lowConnString.IndexOf(CONNSTRING_DELIM, uidMPos);
					connectionString = connectionString.Substring(0, uidMPos) +
						value + connectionString.Substring(uidEPos);
				}
				else
				{
					string[] tokens = userIdTokens.Split(',');
					connectionString += tokens[0] + value + CONNSTRING_DELIM;
				}
			}
		}
		public string Password
		{
			get
			{
				string lowConnString = connectionString.ToLower(CultureInfo.CurrentCulture);
				int pwdPos;
				int pwdMPos;
				GetTokenPositions(passwordTokens, out pwdPos, out pwdMPos);
				if (0 <= pwdPos)
				{
					int pwdEPos = lowConnString.IndexOf(CONNSTRING_DELIM, pwdMPos);
					return connectionString.Substring(pwdMPos, pwdEPos - pwdMPos);
				}
				else
				{
					return String.Empty;
				}
			}
			set
			{
				string lowConnString = connectionString.ToLower(CultureInfo.CurrentCulture);
				int pwdPos;
				int pwdMPos;
				GetTokenPositions(passwordTokens, out pwdPos, out pwdMPos);
				if (0 <= pwdPos)
				{
					int pwdEPos = lowConnString.IndexOf(CONNSTRING_DELIM, pwdMPos);
					connectionString = connectionString.Substring(0, pwdMPos) + value + connectionString.Substring(pwdEPos);
				}
				else
				{
					string[] tokens = passwordTokens.Split(',');
					connectionString += tokens[0] + value + CONNSTRING_DELIM;
				}
			}
		}
		public override string ToString()
		{
			return connectionString;
		}
		public string ToStringNoCredentials()
		{
			if (connectionStringWithoutCredentials == null)
				connectionStringWithoutCredentials = RemoveCredentials(connectionString);
			return connectionStringWithoutCredentials;
		}
		public ConnectionString CreateNewConnectionString(string connectionStringToFormat)
		{
			return new ConnectionString(connectionStringToFormat, userIdTokens, passwordTokens);
		}
		private void GetTokenPositions(string tokenString, out int tokenPos, out int tokenMPos)
		{
			string[] tokens = tokenString.Split(',');
			int currentPos = -1;
			int previousPos = -1;
			string lowConnString = connectionString.ToLower(CultureInfo.CurrentCulture);
			tokenPos = -1;
			tokenMPos = -1;
			foreach (string token in tokens)
			{
				currentPos = lowConnString.IndexOf(token);
				if (currentPos > previousPos)
				{
					tokenPos = currentPos;
					tokenMPos = currentPos + token.Length;
					previousPos = currentPos;
				}
			}
		}
		private string RemoveCredentials(string connectionStringToModify)
		{
			StringBuilder connStringNoCredentials = new StringBuilder();
			string[] tokens = connectionStringToModify.ToLower(CultureInfo.CurrentCulture).Split(CONNSTRING_DELIM);
			string thingsToRemove = userIdTokens + "," + passwordTokens;
			string[] avoidTokens = thingsToRemove.ToLower(CultureInfo.CurrentCulture).Split(',');
			foreach (string section in tokens)
			{
				bool found = false;
				string token = section.Trim();
				if (token.Length != 0)
				{
					foreach (string avoidToken in avoidTokens)
					{
						if (token.StartsWith(avoidToken))
						{
							found = true;
							break;
						}
					}
					if (!found)
					{
						connStringNoCredentials.Append(token + CONNSTRING_DELIM);
					}
				}
			}
			return connStringNoCredentials.ToString();
		}
	}
}
