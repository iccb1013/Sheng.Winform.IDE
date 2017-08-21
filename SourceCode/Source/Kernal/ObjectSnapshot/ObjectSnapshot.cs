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
using System.Diagnostics;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Kernal
{
    
    public class ObjectSnapshot
    {
        private object _object;
        private object _mirrorObject;
        private ISnapshot _snapShot;
        public ObjectSnapshot(object obj)
        {
            _snapShot = obj as ISnapshot;
            if (_snapShot == null)
            {
                Debug.Assert(false, "对象没有实现 ISnapshot ");
                throw new ArgumentException();
            }
            _object = obj;
        }
        public void Snapshot()
        {
            _mirrorObject = _snapShot.Copy();
        }
        public void Revert()
        {
            SnapshotMemberCollection members = new SnapshotMemberCollection();
            foreach (ObjectCompareResult result in ObjectCompare.Compare(_mirrorObject, _object))
            {
                members.Add(result.MemberName, result.SourceValue, result.CompareValue);
            }
            foreach (SnapshotMember member in members)
            {
                member.SetMember(_object, SnapshotMember.EnumMemberValue.OldValue);
            }
        }
        public void AcceptChange()
        {
            Snapshot();
        }
    }
}
