/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Text;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters
{
	public class GenericTextFormatter<T>
	{
		private IEnumerable<Formatter<T>> formatters;
		public GenericTextFormatter(string template, IDictionary<string, TokenHandler<T>> tokenHandlers)
		{
			this.formatters = SetUpFormatters(template, tokenHandlers);
		}
		private static IEnumerable<Formatter<T>> SetUpFormatters(
			string template,
			IDictionary<string, TokenHandler<T>> tokenHandlers)
		{
			List<Formatter<T>> formatters = new List<Formatter<T>>();
			int currentRunStart = 0;
			int currentIndex = 0;
			while (currentIndex < template.Length)
			{
				if ('{' == template[currentIndex++])
				{
					int currentTokenStart = currentIndex;
					while (currentIndex < template.Length && char.IsLetterOrDigit(template[currentIndex]))
					{
						currentIndex++;
					}
					if (currentIndex == template.Length)
					{
						break;	
					}
					string tokenName = template.Substring(currentTokenStart, currentIndex - currentTokenStart);
					TokenHandler<T> tokenHandler;
					if (tokenHandlers.TryGetValue(tokenName, out tokenHandler))
					{
						Formatter<T> formatter = tokenHandler(template, ref currentIndex);
						if (formatter != null)
						{
							string previousRun = template.Substring(currentRunStart, currentTokenStart - currentRunStart - 1);
							if (previousRun.Length > 0)
							{
								formatters.Add(o => previousRun);
							}
							formatters.Add(formatter);
							currentRunStart = currentIndex;
						}
					}
				}
			}
			{
				string previousRun = template.Substring(currentRunStart, currentIndex - currentRunStart);
				if (previousRun.Length > 0)
				{
					formatters.Add(o => previousRun);
				}
			}
			return formatters;
		}
		public static TokenHandler<T> CreateSimpleTokenHandler(string constant)
		{
			return CreateSimpleTokenHandler(o => constant);
		}
		public static TokenHandler<T> CreateSimpleTokenHandler(Formatter<T> formatter)
		{
			return delegate(string template, ref int currentIndex)
			{
				if ('}' == template[currentIndex])
				{
					currentIndex++;		
					return formatter;
				}
				return null;
			};
		}
		public static TokenHandler<T> CreateParameterizedTokenHandler(ParameterizedFormatterFactory<T> formatterFactory)
		{
			return delegate(string template, ref int currentIndex)
			{
				string parameter = string.Empty;
				if (template[currentIndex] == '(')
				{
					int parameterStart = ++currentIndex;
					while (true)
					{
						while (currentIndex < template.Length && template[currentIndex++] != ')')
							;
						if (currentIndex == template.Length)
							return null;
						if (template[currentIndex] == '}')
						{
							break;		
						}
					}
					parameter = template.Substring(parameterStart, currentIndex - parameterStart - 1);
				}
				currentIndex++;	
				return formatterFactory(parameter);
			};
		}
		public void Format(T instance, StringBuilder output)
		{
			foreach (var formatter in this.formatters)
			{
				output.Append(formatter(instance));
			}
		}
	}
	public delegate Formatter<T> TokenHandler<T>(string template, ref int currentIndex);
	public delegate string Formatter<T>(T instance);
	public delegate Formatter<T> ParameterizedFormatterFactory<T>(string parameter);
}
