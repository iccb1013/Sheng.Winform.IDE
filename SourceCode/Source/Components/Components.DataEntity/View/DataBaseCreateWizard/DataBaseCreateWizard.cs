/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sheng.SailingEase.Controls;
namespace Sheng.SailingEase.Components.DataEntityComponent.View
{
    class DataBaseCreateWizard
    {
        public static void Show()
        {
            WizardView view = new WizardView();
            DataBaseCreateOption option = new DataBaseCreateOption();
            view.SetOptionInstance<DataBaseCreateOption>(option);
            view.AddStepPanel(new DataBaseCreateWizard_DataBaseName());
            view.AddStepPanel(new DataBaseCreateWizard_Option());
            view.AddStepPanel(new DataBaseCreateWizard_Account());
            view.AddStepPanel(new DataBaseCreateWizard_Confirm());
            view.AddStepPanel(new DataBaseCreateWizard_Create());
            view.AddStepPanel(new DataBaseCreateWizard_Done());
            view.ShowDialog();
            view.Dispose();
        }
    }
}
