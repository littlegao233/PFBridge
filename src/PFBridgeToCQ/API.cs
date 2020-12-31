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
        public static IMember GetMember(string GroupNumber, string QQid) => GetMember(long.Parse(GroupNumber), long.Parse(QQid));
        public static IMember GetMember(long GroupNumber, long QQid)
        {
            return CurrentPluginContext.Member(QQid, GroupNumber);
        }
    }
    internal class API : IBridgeQQBase
    {
        public string PluginDataPath
        {
            get
            {
                return CurrentPluginContext.Bot.AppDirectory.FullName;
            }
        }
        public void Log(object Message)
        {
            CurrentPluginContext.Logger.LogSuccess(Message.ToString());
        }

        public void LogErr(object Message)
        { 
            CurrentPluginContext.Logger.LogWarning(   Message.ToString());
        }

        public void SendGroupMessage(string TargetGroup, string Message)
        {
            Data.GetGroup(TargetGroup).Send(Message);
        }

        public void SendPrivateMessageFromGroup(string TargetGroup, string QQid, string Message)
        {
            Data.GetMember(TargetGroup, QQid).Send(Message);
        }
    }
}