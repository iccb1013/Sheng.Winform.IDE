/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Diagnostics;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
    public class PerformanceCounterInstanceName
    {
        const int MaxPrefixLength = 32;
        const int MaxSuffixLength = 32;
        readonly string prefix;
        readonly string suffix;
        public PerformanceCounterInstanceName(string prefix,
                                              string suffix)
            : this(prefix, suffix, MaxPrefixLength, MaxSuffixLength) {}
        public PerformanceCounterInstanceName(string prefix,
                                              string suffix,
                                              int maxPrefixLength,
                                              int maxSuffixLength)
        {
            this.prefix = NormalizeStringLength(prefix, maxPrefixLength);
            this.suffix = NormalizeStringLength(suffix, maxSuffixLength);
        }
        static string NormalizeStringLength(string namePart,
                                            int namePartMaxLength)
        {
            return (namePart.Length > namePartMaxLength) ? namePart.Substring(0, namePartMaxLength) : namePart;
        }
        public override string ToString()
        {
            string namePrefix = "";
            if (prefix.Length > 0) namePrefix += (prefix + " - ");
            return namePrefix + suffix;
        }
    }
}
