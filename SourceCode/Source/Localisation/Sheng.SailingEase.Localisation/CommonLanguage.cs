/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Localisation {
    public class CommonLanguage {
        private static ILanguage _Language;
        public static ILanguage Current {
            get {
                if ((_Language == null)) {
                    _Language = new Chinese__Simplified_();
                }
                return _Language;
            }
            set {
                _Language = value;
            }
        }
        public static System.Collections.Generic.List<ILanguage> GetLanguages() {
            System.Collections.Generic.List<ILanguage> items = new System.Collections.Generic.List<ILanguage>();
            System.Type[] exportedTypes = System.Reflection.Assembly.GetExecutingAssembly().GetExportedTypes();
            for (int i = 0; (i < exportedTypes.Length); i = (i + 1)) {
                if (exportedTypes[i].IsClass) {
                    if ((exportedTypes[i].GetInterface("ILanguage") != null)) {
                        try {
                            object obj = System.Activator.CreateInstance(exportedTypes[i]);
                            ILanguage interfaceReference = ((ILanguage)(obj));
                            if ((interfaceReference != null)) {
                                items.Add(interfaceReference);
                            }
                        }
                        catch (System.Exception ) {
                        }
                    }
                }
            }
            return items;
        }
    }
}
