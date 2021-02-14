using PFBridgeCore;
using Sora.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PFBridgeForOneBot
{
    public static class CodeEx
    {
        internal static string CQDeCode(string source)
        {
            if (source == null) return string.Empty;
            return CQDeCode(new StringBuilder(source));
        }
        internal static string CQDeCode(StringBuilder builder)
        {
            builder = builder.Replace("&#91;", "[");
            builder = builder.Replace("&#93;", "]");
            builder = builder.Replace("&#44;", ",");
            builder = builder.Replace("&amp;", "&");
            return builder.ToString();
        }
        /// <summary>
		/// 获取字符串副本的转义形式
		/// </summary>
		/// <param name="source">欲转义的原始字符串</param>
		/// <param name="enCodeComma">是否转义逗号, 默认 <code>false</code></param>
		/// <exception cref="ArgumentNullException">参数: source 为 null</exception>
		/// <returns>返回转义后的字符串副本</returns>
		public static string CQEnCode(string source, bool enCodeComma)
        {
            if (source == null) return string.Empty;
            StringBuilder builder = new StringBuilder(source);
            builder = builder.Replace("&", "&amp;");
            builder = builder.Replace("[", "&#91;");
            builder = builder.Replace("]", "&#93;");
            if (enCodeComma)
                builder = builder.Replace(",", "&#44;");
            return builder.ToString();
        }
        internal static string ParseMessage(string raw, long group)
        { 
            StringBuilder builder = new StringBuilder(raw);
            foreach (var code in CQCode.Parse(raw))
            {
                switch (code.Function)
                {
                    case CQFunction.Face:
                        builder.Replace(code.Original, APIs.API.ParseMessageFormat.Face); break;
                    case CQFunction.Image:
                        builder.Replace(code.Original, APIs.API.ParseMessageFormat.Image); break;
                    case CQFunction.Record:
                        builder.Replace(code.Original, APIs.API.ParseMessageFormat.Record); break;
                    case CQFunction.Video:
                        builder.Replace(code.Original, APIs.API.ParseMessageFormat.Video); break;
                    case CQFunction.Music:
                        {
                            string url; if (!code.Items.TryGetValue("url", out url)) url = "?";
                            string title; if (!code.Items.TryGetValue("title", out title)) title = "?";
                            builder.Replace(code.Original, string.Format(APIs.API.ParseMessageFormat.Music, url, title)); break;
                        }
                    case CQFunction.At:
                        string target = code.Items["qq"];
                        if (target == "all") builder.Replace(code.Original, APIs.API.ParseMessageFormat.AtAll);
                        else builder.Replace(code.Original, string.Format(APIs.API.ParseMessageFormat.At, GetMemberCard(group, long.Parse(target))));
                        break;
                    case CQFunction.Share:
                        {
                            string url; if (!code.Items.TryGetValue("url", out url)) url = "?";
                            string title; if (!code.Items.TryGetValue("title", out title)) title = "?";
                            builder.Replace(code.Original, string.Format(APIs.API.ParseMessageFormat.Share, url, title)); break;
                        }
                    case CQFunction.Reply:
                        builder.Replace(code.Original, APIs.API.ParseMessageFormat.Reply); break;
                    case CQFunction.Forward:
                        builder.Replace(code.Original, APIs.API.ParseMessageFormat.Forward); break;
                    //case Sora.Enumeration.CQFunction.Poke:
                    //    builder.Replace(code.Original,APIs.API.ParseMessageFormat.Image); break;
                    case CQFunction.Xml:
                        builder.Replace(code.Original, APIs.API.ParseMessageFormat.Xml); break;
                    case CQFunction.Json:
                        builder.Replace(code.Original, APIs.API.ParseMessageFormat.Json); break;
                    //case Sora.Enumeration.CQFunction.RedBag: break;
                    //case Sora.Enumeration.CQFunction.Gift: break;
                    case CQFunction.CardImage:
                        builder.Replace(code.Original, APIs.API.ParseMessageFormat.Image); break;
                    //case Sora.Enumeration.CQFunction.TTS: break;
                    default:
                        builder.Replace(code.Original, string.Format(APIs.API.ParseMessageFormat.Unknown, code.UnknownType)); break;
                }
            }
            return CQDeCode(builder);
        }
        private static byte mbc = 0;
        internal static string GetMemberCard(long group, long qq)
        {
            bool cache;
            if (mbc > 50) { mbc = 0; cache = false; } else { mbc++; cache = true; }
            var t = API.BaseApi.GetGroupMemberInfo(group, qq, cache).AsTask();
            t.Wait();
            string result = t.Result.memberInfo.Card;
            if (string.IsNullOrEmpty(result)) { result = t.Result.memberInfo.Nick; }
            return result;
        }

    }

    /// <summary>
    /// 表示 CQ码 的类
    /// </summary>
    public class CQCode
    {
        #region --字段--
        private static readonly Lazy<Regex[]> _regices = new Lazy<Regex[]>(InitializeRegex);

        private string _originalString;
        private CQFunction _type;
        internal string UnknownType;
        private Dictionary<string, string> _items;
        #endregion

        #region --属性--
        public string Original { get => _originalString; }
        /// <summary>
        /// 获取一个值, 指示当前实例的功能
        /// </summary>
        public CQFunction Function { get { return _type; } }

        /// <summary>
        /// 获取当前实例所包含的所有项目
        /// </summary>
        public Dictionary<string, string> Items { get { return _items; } }

        /// <summary>
        /// 获取一个值, 指示当前实例是否属于图片 <see cref="CQCode"/>
        /// </summary>
        public bool IsImageCQCode { get { return EqualIsImageCQCode(this); } }

        /// <summary>
        /// 获取一个值, 指示当前实例是否属于语音 <see cref="CQCode"/>
        /// </summary>
        public bool IsRecordCQCode { get { return EqualIsRecordCQCode(this); } }
        #endregion

        #region --构造函数--
        /// <summary>
        /// 使用 CQ码 字符串初始化 <see cref="CQCode"/> 类的新实例
        /// </summary>
        /// <param name="str">CQ码字符串 或 包含CQ码的字符串</param>
        private CQCode(string str)
        {
            this._originalString = str;

            #region --解析 CqCode--
            Match match = _regices.Value[0].Match(str);
            if (!match.Success)
            {
                throw new FormatException("无法解析所传入的字符串, 字符串非CQ码格式!");
            }
            #endregion

            #region --解析CQ码类型--
            if (!System.Enum.TryParse<CQFunction>(match.Groups[1].Value, true, out _type))
            {
                this._type = CQFunction.Unknown;    // 解析不出来的时候, 直接给一个默认
                UnknownType = match.Groups[1].Value;
            }
            #endregion

            #region --解析键值对--
            MatchCollection collection = _regices.Value[1].Matches(match.Groups[2].Value);
            this._items = new Dictionary<string, string>(collection.Count);
            foreach (Match item in collection)
            {
                this._items.Add(item.Groups[1].Value, CodeEx.CQDeCode(item.Groups[2].Value));
            }
            #endregion
        }

        /// <summary>
        /// 初始化 <see cref="CQCode"/> 类的新实例
        /// </summary>
        /// <param name="type">CQ码类型</param>
        /// <param name="keyValues">包含的键值对</param>
        public CQCode(CQFunction type, params KeyValuePair<string, string>[] keyValues)
        {
            this._type = type;
            this._items = new Dictionary<string, string>(keyValues.Length);
            foreach (KeyValuePair<string, string> item in keyValues)
            {
                this._items.Add(item.Key, item.Value);
            }

            this._originalString = null;
        }
        #endregion

        #region --公开方法--
        /// <summary>
        /// 从字符串中解析出所有的 CQ码, 转换为 <see cref="CQCode"/> 集合
        /// </summary>
        /// <param name="source">原始字符串</param>
        /// <returns>返回等效的 <see cref="List{CqCode}"/></returns>
        public static List<CQCode> Parse(string source)
        {
            MatchCollection collection = _regices.Value[0].Matches(source);
            List<CQCode> codes = new List<CQCode>(collection.Count);
            foreach (Match item in collection)
            {
                codes.Add(new CQCode(item.Groups[0].Value));
            }
            return codes;
        }
        /// <summary>
        /// 判断是否是图片 <see cref="CQCode"/>
        /// </summary>
        /// <param name="code">要判断的 <see cref="CQCode"/> 实例</param>
        /// <returns>如果是图片 <see cref="CQCode"/> 返回 <see langword="true"/> 否则返回 <see langword="false"/></returns>
        public static bool EqualIsImageCQCode(CQCode code)
        {
            return code.Function == CQFunction.Image;
        }
        /// <summary>
        /// 判断是否是语音 <see cref="CQCode"/>
        /// </summary>
        /// <param name="code">要判断的 <see cref="CQCode"/> 实例</param>
        /// <returns>如果是语音 <see cref="CQCode"/> 返回 <see langword="true"/> 否则返回 <see langword="false"/></returns>
        public static bool EqualIsRecordCQCode(CQCode code)
        {
            return code.Function == CQFunction.Record;
        }
        /// <summary>
        /// 确定指定的对象是否等于当前对象
        /// </summary>
        /// <param name="obj">要与当前对象进行比较的对象</param>
        /// <returns>如果指定的对象等于当前对象，则为 <code>true</code>，否则为 <code>false</code></returns>	
        public override bool Equals(object obj)
        {
            CQCode code = obj as CQCode;
            if (code != null)
            {
                return string.Equals(this._originalString, code._originalString);
            }
            return base.Equals(obj);
        }
        /// <summary>
        /// 返回该字符串的哈希代码
        /// </summary>
        /// <returns> 32 位有符号整数哈希代码</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() & this._originalString.GetHashCode();
        }
        /// <summary>
        /// 返回此实例等效的CQ码形式
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this._originalString == null)
            {
                if (this._items.Count == 0)
                {
                    // 特殊CQ码, 抖动窗口
                    this._originalString = string.Format("[CQ:{0}]", _type.ToString());
                }
                else
                {
                    // 普通CQ码, 带参数
                    StringBuilder builder = new StringBuilder();
                    builder.Append("[CQ:");
                    builder.Append(this._type.ToString());   // function
                    foreach (KeyValuePair<string, string> item in this._items)
                    {
                        builder.AppendFormat(",{0}={1}", item.Key, CodeEx.CQEnCode(item.Value, true));
                    }
                    builder.Append("]");
                    this._originalString = builder.ToString();
                }
            }
            return this._originalString;
        }
        /// <summary>
        /// 处理返回用于发送的字符串
        /// </summary>
        /// <returns>用于发送的字符串</returns>
        public string ToSendString()
        {
            return this.ToString();
        }
        #endregion

        #region --私有方法--
        /// <summary>
        /// 延时初始化正则表达式
        /// </summary>
        /// <returns></returns>
        private static Regex[] InitializeRegex()
        {
            // 此处延时加载, 以提升运行速度
            return new Regex[]
            {
                new Regex(@"\[CQ:([A-Za-z]*)(?:(,[^\[\]]+))?\]", RegexOptions.Compiled),    // 匹配CQ码
                new Regex(@",([A-Za-z]+)=([^,\[\]]+)", RegexOptions.Compiled)               // 匹配键值对
            };
        }
        #endregion
    }
}
