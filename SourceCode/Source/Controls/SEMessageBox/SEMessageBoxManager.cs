/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.IO;
using System.Collections;
using System.Resources;
using System.Reflection;
namespace Sheng.SailingEase.Controls
{
	public class SEMessageBoxManager
	{
		private static Hashtable _messageBoxes = new Hashtable();
		private static Hashtable _savedResponses = new Hashtable();
        private static Hashtable _standardButtonsText = new Hashtable();
        static SEMessageBoxManager()
        {
                _standardButtonsText[SEMessageBoxButtons.Ok.ToString()] = "Ok";
                _standardButtonsText[SEMessageBoxButtons.Cancel.ToString()] = "Cancel";
                _standardButtonsText[SEMessageBoxButtons.Yes.ToString()] = "Yes";
                _standardButtonsText[SEMessageBoxButtons.No.ToString()] = "No";
                _standardButtonsText[SEMessageBoxButtons.Abort.ToString()] = "Abort";
                _standardButtonsText[SEMessageBoxButtons.Retry.ToString()] = "Retry";
                _standardButtonsText[SEMessageBoxButtons.Ignore.ToString()] = "Ignore";
        }
		public static SEMessageBox CreateMessageBox(string name)
		{
			if(name != null && _messageBoxes.ContainsKey(name))
			{
				string err = string.Format("A MessageBox with the name {0} already exists.",name);
				throw new ArgumentException(err,"name");
			}
			SEMessageBox msgBox = new SEMessageBox();
			msgBox.Name = name;
			if(msgBox.Name != null)
			{
				_messageBoxes[name] = msgBox;
			}
			return msgBox;
		}
		public static SEMessageBox GetMessageBox(string name)
		{
			if(_messageBoxes.Contains(name))
			{
				return _messageBoxes[name] as SEMessageBox;
			}
			else
			{
				return null;
			}
		}
		public static void DeleteMessageBox(string name)
		{
			if(name == null)
				return;
			if(_messageBoxes.Contains(name))
			{
				SEMessageBox msgBox = _messageBoxes[name] as SEMessageBox;
				msgBox.Dispose();
				_messageBoxes.Remove(name);
			}
		}
		public static void WriteSavedResponses(Stream stream)
		{
			throw new NotImplementedException("This feature has not yet been implemented");
		}
		public static void ReadSavedResponses(Stream stream)
		{
			throw new NotImplementedException("This feature has not yet been implemented");
		}
		public static void ResetSavedResponse(string messageBoxName)
		{
            if(messageBoxName == null)
                return;
			if(_savedResponses.ContainsKey(messageBoxName))
			{
				_savedResponses.Remove(messageBoxName);
			}
		}
		public static void ResetAllSavedResponses()
		{
			_savedResponses.Clear();
		}
        internal static void SetSavedResponse(SEMessageBox msgBox, SEMessageBoxButton response)
		{
            if(msgBox.Name == null)
                return;
			_savedResponses[msgBox.Name] = response;
		}
        internal static SEMessageBoxButton GetSavedResponse(SEMessageBox msgBox)
		{
			string msgBoxName = msgBox.Name;
            if(msgBoxName == null)
            {
                return null;
            }
			if(_savedResponses.ContainsKey(msgBoxName))
			{
                return _savedResponses[msgBox.Name] as SEMessageBoxButton;
			}
			else
			{
				return null;
			}
		}
        internal static string GetLocalizedString(string key)
        {
            if(_standardButtonsText.ContainsKey(key))
            {
                return (string)_standardButtonsText[key];
            }
            else
            {
                return null;
            }
        }
	}
}
