/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
/*
 * Munger - An Interface pattern on getting and setting values from object through Reflection
 * Author: Phillip Piper
 * Date: 28/11/2008 17:15 
 * Change log:
 * 2009-02-15  JPP  - Made Munger a public class
 * 2009-01-20  JPP  - Made the Munger capable of handling indexed access.
 *                    Incidentally, this removed the ugliness that the last change introduced.
 * 2009-01-18  JPP  - Handle target objects from a DataListView (normally DataRowViews)
 * v2.0
 * 2008-11-28  JPP  Initial version
 * TO DO:
 * Copyright (C) 2006-2008 Phillip Piper
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http:
 * If you wish to use this code in a closed source application, please contact phillip_piper@bigfoot.com.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
namespace Sheng.SailingEase.Controls.ObjectListView
{
    public class Munger
    {
        public Munger()
        {
        }
        public Munger(String aspectName)
        {
            this.AspectName = aspectName;
        }
        public string AspectName
        {
            get { return aspectName; }
            set { 
                aspectName = value;
                if (String.IsNullOrEmpty(aspectName))
                    this.aspectNameParts = new List<string>();
                else
                    this.aspectNameParts = new List<string>(aspectName.Split('.'));
            }
        }
        private string aspectName;
        private List<String> aspectNameParts = new List<string>();
        public Object GetValue(Object target)
        {
            if (this.aspectNameParts.Count == 0)
                return null;
            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.GetField;
            const BindingFlags flags2 = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty;
            foreach (String property in this.aspectNameParts) {
                if (target == null)
                    break;
                try {
                    target = target.GetType().InvokeMember(property, flags, null, target, null);
                }
                catch (MissingMethodException) {
                    try {
                        target = target.GetType().InvokeMember("Item", flags2, null, target, new Object[] { property });
                    }
                    catch {
                        return String.Format("'{0}' is not a parameter-less method, property or field of type '{1}'", property, target.GetType());
                    }
                }
            }
            return target;
        }
        public void PutValue(Object target, Object value)
        {
            if (this.aspectNameParts.Count == 0)
                return;
            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.GetField;
            for (int i = 0; i < this.aspectNameParts.Count-1; i++) {
                if (target == null)
                    break;
                try {
                    target = target.GetType().InvokeMember(this.aspectNameParts[i], flags, null, target, null);
                }
                catch (System.MissingMethodException) {
                    try {
                        const BindingFlags flags2 = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty;
                        target = target.GetType().InvokeMember("Item", flags2, null, target, new Object[] { this.aspectNameParts[i] });
                    }
                    catch {
                        System.Diagnostics.Debug.WriteLine(String.Format("Cannot invoke '{0}' on a {1}", this.aspectNameParts[i], target.GetType()));
                        return;
                    }
                }
            }
            if (target == null)
                return;
            String lastPart = this.aspectNameParts[this.aspectNameParts.Count - 1];
            try {
                const BindingFlags flags3 = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | 
                    BindingFlags.SetProperty | BindingFlags.SetField;
                target.GetType().InvokeMember(lastPart, flags3, null, target, new Object[] { value });
            }
            catch (System.MissingMethodException ex) {
                try {
                    const BindingFlags flags4 = BindingFlags.Public | BindingFlags.NonPublic | 
                        BindingFlags.Instance | BindingFlags.InvokeMethod;
                    target.GetType().InvokeMember(lastPart, flags4, null, target, new Object[] { value });
                }
                catch (System.MissingMethodException ex2) {
                    try {
                        const BindingFlags flags5 = BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty;
                        target = target.GetType().InvokeMember("Item", flags5, null, target,
                            new Object[] { lastPart, value });
                    }
                    catch {
                        System.Diagnostics.Debug.WriteLine("Invoke PutAspectByName failed:");
                        System.Diagnostics.Debug.WriteLine(ex);
                        System.Diagnostics.Debug.WriteLine(ex2);
                    }
                }
            }
        }
    }
}
