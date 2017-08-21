/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
/*
 * Attributes - Attributes that can be attached to properties of models to allow columns to be
 *              built from them directly
 * Author: Phillip Piper
 * Date: 15/08/2009 22:01
 * Change log:
 * 2009-08-15  JPP  - Initial version
 * To do:
 * Copyright (C) 2009 Phillip Piper
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
using System.Windows.Forms;
namespace Sheng.SailingEase.Controls.ObjectListView
{
    public class OLVColumnAttribute : Attribute
    {
        public OLVColumnAttribute() {
        }
        public OLVColumnAttribute(string title) {
            this.Title = title;
        }
        public string AspectToStringFormat {
            get { return aspectToStringFormat; }
            set { aspectToStringFormat = value; }
        }
        private string aspectToStringFormat;
        public bool CheckBoxes {
            get { return checkBoxes; }
            set { checkBoxes = value; }
        }
        private bool checkBoxes;
        public int DisplayIndex {
            get { return displayIndex; }
            set { displayIndex = value; }
        }
        private int displayIndex;
        public bool FillsFreeSpace {
            get { return fillsFreeSpace; }
            set { fillsFreeSpace = value; }
        }
        private bool fillsFreeSpace;
        public int? FreeSpaceProportion {
            get { return freeSpaceProportion; }
            set { freeSpaceProportion = value; }
        }
        private int? freeSpaceProportion;
        public object[] GroupCutoffs {
            get { return groupCutoffs; }
            set { groupCutoffs = value; }
        }
        private object[] groupCutoffs;
        public string[] GroupDescriptions {
            get { return groupDescriptions; }
            set { groupDescriptions = value; }
        }
        private string[] groupDescriptions;
        public string GroupWithItemCountFormat {
            get { return groupWithItemCountFormat; }
            set { groupWithItemCountFormat = value; }
        }
        private string groupWithItemCountFormat;
        public string GroupWithItemCountSingularFormat {
            get { return groupWithItemCountSingularFormat; }
            set { groupWithItemCountSingularFormat = value; }
        }
        private string groupWithItemCountSingularFormat;
        public bool Hyperlink {
            get { return hyperlink; }
            set { hyperlink = value; }
        }
        private bool hyperlink;
        public string ImageAspectName {
            get { return imageAspectName; }
            set { imageAspectName = value; }
        }
        private string imageAspectName;
        public bool IsEditable {
            get { return isEditable; }
            set {
                isEditable = value;
                this.IsEditableSet = true;
            }
        }
        private bool isEditable = true;
        internal bool IsEditableSet = false;
        public bool IsVisible {
            get { return isVisible; }
            set { isVisible = value; }
        }
        private bool isVisible = true;
        public bool IsTileViewColumn {
            get { return isTileViewColumn; }
            set { isTileViewColumn = value; }
        }
        private bool isTileViewColumn;
        public int MaximumWidth {
            get { return maximumWidth; }
            set { maximumWidth = value; }
        }
        private int maximumWidth = -1;
        public int MinimumWidth {
            get { return minimumWidth; }
            set { minimumWidth = value; }
        }
        private int minimumWidth = -1;
        public HorizontalAlignment TextAlign {
            get { return this.textAlign; }
            set { this.textAlign = value; }
        }
        private HorizontalAlignment textAlign = HorizontalAlignment.Left;
        public String Tag {
            get { return tag; }
            set { tag = value; }
        }
        private String tag;
        public String Title {
            get { return title; }
            set { title = value; }
        }
        private String title;
        public String ToolTipText {
            get { return toolTipText; }
            set { toolTipText = value; }
        }
        private String toolTipText;
        public bool TriStateCheckBoxes {
            get { return triStateCheckBoxes; }
            set { triStateCheckBoxes = value; }
        }
        private bool triStateCheckBoxes;
        public bool UseInitialLetterForGroup {
            get { return useInitialLetterForGroup; }
            set { useInitialLetterForGroup = value; }
        }
        private bool useInitialLetterForGroup;
        public int Width {
            get { return width; }
            set { width = value; }
        }
        private int width = 150;
    }
}
