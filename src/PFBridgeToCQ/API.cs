using PFBridgeCore;
using HuajiTech.CoolQ;
using System.Collections.Generic;

namespace PFBridgeToCQ
{
    internal static class Data
    {
        private static List<IGroup> GroupList = new List<IGroup>();
        public static IGroup GetGroup(string GroupNumber) => GetGroup(long.Parse(GroupNumber));
        public static IGroup GetGroup(long GroupNumber)
        {
            int i = GroupList.FindIndex(l => l.Number == GroupNumber);
            if (i == -1)
            {
                var group = CurrentPluginContext.Group(GroupNumber);
                GroupList.Add(group);
                return group;
            }
            else
                return GroupList[i];
        }
    }
    internal class API : IBridgeBase
    {
        public void Log(string Message)
        {
            CurrentPluginContext.Logger.Log(Message);
        }
        public void SendGroupMessage(string TargetGroup, string Message)
        {
            Data.GetGroup(TargetGroup).Send(Message);
        }
    }
}