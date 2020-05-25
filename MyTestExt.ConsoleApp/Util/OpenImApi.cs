//using System;
//using System.Collections.Generic;
//using System.Linq;
//using MyTestExt.Utils.Json;
//using Top.Api;
//using Top.Api.Domain;
//using Top.Api.Request;
//using Top.Api.Response;
//using Top.Api.Util;

//namespace ConsoleApplication1.Util
//{
//    public class OpenImApi
//    {
//        #region Properties  ！！！！内容有改动


//        public static string AppKey
//        {
//            get;
//            set;
//        }


//        public static string AppSecret
//        {
//            get;
//            set;
//        }

//        private static ITopClient _client;

//        private static ITopClient Client
//        {
//            get
//            {
//                return _client ??
//                       (_client =
//                           new DefaultTopClient("http://gw.api.taobao.com/router/rest", AppKey, AppSecret, "json"));
//            }
//        }

//        #endregion

//        private static string GetResponseJson<T>(ITopRequest<T> request
//            , Dictionary<string, bool> handErr = null, Dictionary<string, bool> replayErr = null)
//            where T : TopResponse
//        {
//            if (Client == null || request == null) return null;

//            var ret = string.Empty;
//            var counter = 1;
//            do
//            {
//                try
//                {
//                    // case0：正常直接返回“响应原始内容”
//                    var response = Client.Execute(request);
//                    ret = response.Body;
//                    if (!response.IsError)
//                        return ret;

//                    // case1：返回自定义处理的错误（优先级最高）
//                    var rspCode = string.Format("{0}-{1}", response.ErrCode, response.SubErrCode);
//                    if (handErr != null && handErr.Keys.Contains(rspCode))
//                    {
//                        handErr[rspCode] = true;
//                        return ret;
//                    }

//                    // case2：请求重试。ISP 错误： 此类错误为服务内部错误。对于这种错误处理，一般采用稍后尝试
//                    if ((!string.IsNullOrWhiteSpace(response.ErrCode) && response.ErrCode.Contains("isp.")) ||
//                        (!string.IsNullOrWhiteSpace(response.SubErrCode) && response.SubErrCode.Contains("isp.")))
//                    {
//                        Log.WriteLog(
//                            string.Format("调用OpenImApi 出错！云旺服务内部错误，稍后重试。第 {0}/10 次。 \n 错误信息：{1} \n 请求信息：{2}，{3}\n"
//                                , counter, JsonNet.Serialize(response), request, JsonNet.Serialize(request)));
//                        System.Threading.Thread.Sleep(1000); // 延迟一秒重试时间
//                        continue;
//                    }

//                    // case2.1：请求重试。自定义重复处理的错误
//                    if (replayErr != null && replayErr.Keys.Contains(rspCode))
//                    {
//                        replayErr[rspCode] = true;
//                        Log.WriteLog(
//                            string.Format("调用OpenImApi 出错！自定义重试错误，稍后重试。第 {0}/10 次，增速+2。 \n 错误信息：{1} \n 请求信息：{2}，{3}\n"
//                                , counter, JsonNet.Serialize(response), request, JsonNet.Serialize(request)));
//                        System.Threading.Thread.Sleep(1000); // 延迟一秒重试时间
//                        counter += 2;
//                        continue;
//                    }

//                    // case3：返回错误。ISV 错误： 这种错误产生于参数非法。需要调用方检查参数
//                    Log.WriteLog(
//                        string.Format("调用OpenImApi 出错！请求参数非法，请检查检查请求参数。 \n 错误信息：{0} \n 请求信息：{1}，{2}\n"
//                            , JsonNet.Serialize(response), request, JsonNet.Serialize(request)));
//                    return ret;
//                }
//                catch (Exception ex)
//                {
//                    // case4：运行错误（比如网络中断）。重试
//                    Log.WriteLog(
//                        string.Format("调用OpenImApi 出错！程序运行发生异常，稍后重试。第 {0}/10 次。 \n 错误信息：{1} \n 请求信息：{2}，{3}\n"
//                            , counter, JsonNet.Serialize(ex), request, JsonNet.Serialize(request)));
//                    Log.WriteLog(ex);
//                    System.Threading.Thread.Sleep(1000); // 延迟一秒重试时间
//                }
//            } while (counter++ < 10);

//            return ret; // case2.5 重试后依然出错，返回错误
//        }


//        #region 用户维护

//        /// <summary>
//        /// taobao.openim.users.add (添加用户)
//        /// </summary>
//        /// <param name="uid">im用户名</param>
//        /// <param name="pwd">im密码</param>
//        /// <param name="nick">昵称，不能大于20个字符</param>
//        /// <param name="icon">头像url</param>
//        /// <returns></returns>
//        public static bool UserAdd(string uid, string pwd, string nick = null, string icon = null)
//        {
//            if (string.IsNullOrWhiteSpace(uid) || string.IsNullOrWhiteSpace(pwd)) return false;

//            var request = new OpenimUsersAddRequest
//            {
//                Userinfos_ = new List<Userinfos>
//                {
//                    new Userinfos
//                    {
//                        Userid = uid,
//                        Password = pwd,
//                        Nick = !string.IsNullOrWhiteSpace(nick) && nick.Length > 20 ? nick.Substring(20) : nick,
//                        IconUrl = icon
//                    }
//                }
//            };
//            var json = GetResponseJson(request);
//            if (string.IsNullOrWhiteSpace(json)) return false;
//            var objResult = TopUtils.ParseResponse<OpenimUsersAddResponse>(json);
//            return objResult != null && objResult.UidSucc != null && objResult.UidSucc.Contains(uid);
//        }

//        /// <summary>
//        /// taobao.openim.users.update (批量更新用户信息) 在这里简化为单个更新
//        /// </summary>
//        /// <param name="uid">im用户名</param>
//        /// <param name="pwd">im密码</param>
//        /// <param name="nick">昵称，不能大于20个字符</param>
//        /// <param name="icon">头像url</param>
//        /// <returns></returns>
//        public static bool UserUpdate(string uid, string pwd, string nick, string icon = null)
//        {
//            if (string.IsNullOrWhiteSpace(uid)) return false;

//            var request = new OpenimUsersUpdateRequest
//            {
//                Userinfos_ = new List<Userinfos>
//                {
//                    new Userinfos
//                    {
//                        Userid = uid,
//                        Password = pwd,
//                        Nick = !string.IsNullOrWhiteSpace(nick) && nick.Length > 20 ? nick.Substring(20) : nick,
//                        IconUrl = icon
//                    }
//                }
//            };
//            var json = GetResponseJson(request);
//            var objResult = string.IsNullOrWhiteSpace(json)
//                ? null
//                : TopUtils.ParseResponse<OpenimUsersUpdateResponse>(json);
//            var flag = objResult != null && objResult.UidSucc != null && objResult.UidSucc.Contains(uid);

//            // 批量修改相关群的群昵称
//            if (flag) UsernickAsync(uid, nick);

//            return flag;
//        }

//        /// <summary>
//        /// taobao.openim.users.update (批量更新用户信息) 在这里简化为单个更新
//        /// </summary>
//        /// <param name="uid">im用户名</param>
//        /// <param name="nick">昵称，不能大于20个字符</param>
//        /// <returns></returns>
//        public static bool UserUpdateNick(string uid, string nick)
//        {
//            if (string.IsNullOrWhiteSpace(uid)) return false;

//            var request = new OpenimUsersUpdateRequest
//            {
//                Userinfos_ = new List<Userinfos>
//                {
//                    new Userinfos
//                    {
//                        Userid = uid,
//                        Nick = !string.IsNullOrWhiteSpace(nick) && nick.Length > 20 ? nick.Substring(20) : nick
//                    }
//                }
//            };
//            var json = GetResponseJson(request);
//            var objResult = string.IsNullOrWhiteSpace(json)
//                ? null
//                : TopUtils.ParseResponse<OpenimUsersUpdateResponse>(json);
//            var flag = objResult != null && objResult.UidSucc != null && objResult.UidSucc.Contains(uid);

//            // 批量修改相关群的群昵称
//            if (flag) UsernickAsync(uid, nick);

//            return flag;
//        }

//        /// <summary>
//        /// taobao.openim.users.update (批量更新用户信息) 在这里简化为单个更新
//        /// </summary>
//        /// <param name="uid">im用户名</param>
//        /// <param name="icon">头像Url 地址</param>
//        /// <returns></returns>
//        public static bool UserUpdateIcon(string uid, string icon)
//        {
//            if (string.IsNullOrWhiteSpace(uid)) return false;

//            var dictUserIcon = new Dictionary<string, string>();
//            dictUserIcon[uid] = icon;
//            var result = UserUpdateIcon(dictUserIcon);
//            var flag = result != null && result.SuccUids != null && result.SuccUids.Contains(uid);
//            return flag;
//        }

//        /// <summary>
//        /// taobao.openim.users.update (批量更新用户信息) 在这里简化为单个更新
//        /// </summary>
//        /// <param name="dictUserIcon">用户、头像 键值对</param>
//        /// <returns></returns>
//        public static UserExec UserUpdateIcon(Dictionary<string, string> dictUserIcon)
//        {
//            if (!dictUserIcon.Any()) return null;

//            var userinfos = dictUserIcon.Select(c => new Userinfos {Userid = c.Key, IconUrl = c.Value}).ToList();
//            var request = new OpenimUsersUpdateRequest
//            {
//                Userinfos_ = userinfos
//            };
//            var json = GetResponseJson(request);
//            var objResult = string.IsNullOrWhiteSpace(json)
//                ? null
//                : TopUtils.ParseResponse<OpenimUsersUpdateResponse>(json);
//            if (objResult == null)
//                return null;

//            var ret = new UserExec {FailUids = objResult.UidFail, SuccUids = objResult.UidSucc};
//            return ret;
//        }

//        /// <summary>
//        /// taobao.openim.users.delete (删除用户)
//        /// </summary>
//        /// <param name="uid">需要删除的用户</param>
//        /// <returns></returns>
//        public static bool UserDelete(string uid)
//        {
//            if (string.IsNullOrWhiteSpace(uid)) return false;

//            var request = new OpenimUsersDeleteRequest
//            {
//                Userids = uid
//            };
//            var json = GetResponseJson(request);
//            if (string.IsNullOrWhiteSpace(json)) return false;
//            var objResult = TopUtils.ParseResponse<OpenimUsersDeleteResponse>(json);
//            return objResult != null && objResult.Result != null &&
//                   objResult.Result.Contains("ok", StringComparer.OrdinalIgnoreCase);
//        }

//        /// <summary>
//        /// taobao.openim.users.get (批量获取用户信息)
//        /// </summary>
//        /// <param name="uid">单个用户id</param>
//        /// <returns></returns>
//        public static Userinfos UserGet(string uid)
//        {
//            var res = UsersGet(uid);
//            return (res != null && res.Any()) ? res.FirstOrDefault() : null;
//        }

//        /// <summary>
//        /// taobao.openim.users.get (批量获取用户信息)
//        /// </summary>
//        /// <param name="uids">批量用户ids，用','分隔</param>
//        /// <returns></returns>
//        public static List<Userinfos> UsersGet(string uids)
//        {
//            if (string.IsNullOrWhiteSpace(uids)) return null;
//            var arrUid = uids.Split(',').Where(c => !string.IsNullOrWhiteSpace(c)).ToList();
//            if (!arrUid.Any()) return null;

//            var ret = new List<Userinfos>();
//            const int size = 100; // 最大列表长度：100
//            var count = Math.Ceiling((double)arrUid.Count() / size);
//            for (var i = 0; i < count; i++)
//            {
//                var arrI = arrUid.Skip(i * size).Take(size);
//                var request = new OpenimUsersGetRequest { Userids = string.Join(",", arrI) };
//                var json = GetResponseJson(request);
//                var res = string.IsNullOrWhiteSpace(json) ? null : TopUtils.ParseResponse<OpenimUsersGetResponse>(json);
//                if (res != null && !res.IsError)
//                    ret.AddRange(res.Userinfos);
//            }
//            return ret;
//        }

//        #endregion


//        #region 消息发送

//        /// <summary>
//        /// taobao.openim.immsg.push (openim标准消息发送)
//        /// </summary>
//        /// <param name="fromId">消息发送者</param>
//        /// <param name="toIds">消息接受者,以逗号“,”分隔</param>
//        /// <param name="msgType">消息类型。0:文本消息。1:图片消息，只支持jpg、gif。2:语音消息，只支持amr。8:地理位置信息。</param>
//        /// <param name="context">发送的消息内容。根据不同消息类型，传不同的值。0(文本消息):填消息内容字符串。1(图片):base64编码的jpg或gif文件。3(语音):base64编码的amr文件。8(地理位置):经纬度，格式如 111,222</param>
//        /// <param name="mediaAttr">json map，媒体信息属性。根据msgtype变化。0(文本):填空即可。 1(图片):需要图片格式，{"type":"jpg"}或{"type":"gif"}。 2(语音): 需要文件格式和语音长度信息{"type":"amr","playtime":5}</param>
//        /// <param name="fromTaobao">如果为1，则表示发送方是一个淘宝账号，该账号必须是本appkey绑定过的客服账号【ex.思普达目标达成:目标服务员2号】，并且只能给本appkey的用户发消息。通过该参数可以从服务端发起一个客服到用户的会话。</param>
//        /// <param name="toAppkey">接收方appkey，默认本app，跨app发送时需要用到</param>
//        /// <returns></returns>
//        public static OpenimImmsgPushResponse ImmsgPush(string fromId, string toIds, long msgType, string context
//            , string mediaAttr, int fromTaobao = 0, string toAppkey = null)
//        {
//            if (string.IsNullOrWhiteSpace(fromId)
//                || string.IsNullOrWhiteSpace(toIds)
//                || string.IsNullOrWhiteSpace(context)
//                || !new[] {0L, 1L, 2L, 8L}.Contains(msgType)
//                || (msgType != 0L && string.IsNullOrWhiteSpace(mediaAttr)))
//                return null;
//            var toUsers = toIds.Split(',').Where(c => !string.IsNullOrWhiteSpace(c)).ToList();
//            if (!toUsers.Any()) return null;

//            var obj = new ImMsgDomainExt
//            {
//                FromUser = fromId,
//                ToUsers = toUsers,
//                MsgType = msgType,
//                Context = context,
//                MediaAttr = mediaAttr,
//                FromTaobao = fromTaobao,
//                ToAppkey = string.IsNullOrWhiteSpace(toAppkey) ? AppKey : toAppkey
//            };
//            var request = new OpenimImmsgPushRequest {Immsg_ = obj};

//            var json = GetResponseJson(request);
//            return string.IsNullOrWhiteSpace(json) ? null : TopUtils.ParseResponse<OpenimImmsgPushResponse>(json);
//        }

//        /// <summary>
//        /// taobao.openim.custmsg.push (推送自定义openim消息)
//        /// </summary>
//        /// <param name="fromId">发送方userid</param>
//        /// <param name="toIds">接受者userid列表，单次发送用户数小于100</param>
//        /// <param name="summary">客户端最近消息里面显示的消息摘要</param>
//        /// <param name="data">发送的自定义数据，sdk默认无法解析消息，该数据需要客户端自己解析</param>
//        /// <param name="aps">apns推送时，里面的aps结构体json字符串，aps.alert为必填字段。本字段为可选，若为空，则表示不进行apns推送。aps.size() + apns_param.size() 小于 200</param>
//        /// <param name="apnsParam">apns推送的附带数据。客户端收到apns消息后，可以从apns结构体的"d"字段中取出该内容。aps.size() + apns_param.size() 小于 200</param>
//        /// <param name="invisible">表示消息是否在最近会话列表里面展示。如果为1，则消息不在列表展示，可以认为服务端透明的给客户端下法了一个数据，ios的提示任然通过aps字段控制</param>
//        /// <param name="fromNick">可以指定发送方的显示昵称，默认为空，自动使用发送方用户id作为nick</param>
//        /// <param name="fromTaobao">如果为1，则表示发送方是一个淘宝账号，该账号必须是本appkey绑定过的客服账号【ex.思普达目标达成:目标服务员2号】，并且只能给本appkey的用户发消息。通过该参数可以从服务端发起一个客服到用户的会话。</param>
//        /// <param name="toAppkey">接收方appkey，不填默认是发送方appkey，如需跨app发送，需要走审批流程，请联系技术答疑</param>
//        /// <returns></returns>
//        public static List<OpenimCustmsgPushResponse> CustmsgPush(string fromId, List<string> toIds, string summary,
//            string data
//            , string aps = null, string apnsParam = null, int invisible = 0, string fromNick = null
//            , int fromTaobao = 0, string toAppkey = null)
//        {
//            if (string.IsNullOrWhiteSpace(fromId) || toIds == null || toIds.Count == 0
//                || string.IsNullOrWhiteSpace(summary) || string.IsNullOrWhiteSpace(data))
//                return null;
//            toAppkey = string.IsNullOrWhiteSpace(toAppkey) ? AppKey : toAppkey;

//            // ImApi限制单次接受者不能超过100人，所以调整循环分批发送
//            toIds.Sort();
//            var rets = new List<OpenimCustmsgPushResponse>();
//            var num = 99;
//            var cntReplay = Math.Ceiling(toIds.Count / (double)num);
//            for (var i = 0; i < cntReplay; i++)
//            {
//                var request = new OpenimCustmsgPushRequest
//                {
//                    Custmsg_ = new CustMsgDomainExt
//                    {
//                        FromUser = fromId,
//                        ToUsers = toIds.Skip(i * num).Take(num).ToList(),
//                        Summary = summary,
//                        Data = data,
//                        Aps = aps,
//                        ApnsParam = apnsParam,
//                        Invisible = invisible,
//                        FromNick = fromNick,
//                        FromTaobao = fromTaobao,
//                        ToAppkey = toAppkey
//                    }
//                };
//                var json = GetResponseJson(request);
//                var ret = string.IsNullOrWhiteSpace(json)
//                    ? null
//                    : TopUtils.ParseResponse<OpenimCustmsgPushResponse>(json);
//                if (ret != null)
//                    rets.Add(ret);
//                else
//                {
//                    // todo.记录一下错误日志，分析错误用，过后删除
//                    var tmp1 = aps ?? "";
//                    var tmp2 = apnsParam ?? "";
//                    Log.WriteLog(
//                        string.Format("调用OpenImApi 出错！推送定义消息！aps+apnsParam charLength:{0}, DefaultByteSize:{1} \n aps:{2} \n apnsParam:{3}",
//                            (tmp1 + tmp2).Length, System.Text.Encoding.Default.GetBytes(tmp1 + tmp2).Length
//                            , tmp1, tmp2));
//                }
//            }

//            return rets;
//        }

//        /// <summary>
//        /// sdk未启用
//        /// </summary>
//        /// <param name="uid"></param>
//        /// <param name="tribeId"></param>
//        /// <param name="atFlag"></param>
//        /// <param name="atMembers"></param>
//        /// <param name="msgType"></param>
//        /// <param name="context"></param>
//        /// <param name="mediaAttr"></param>
//        /// <param name="isPush"></param>
//        /// <returns></returns>
//        public static OpenimCustmsgPushResponse TribeSendmsg(string uid, long tribeId, int atFlag, string atMembers
//            , long msgType, string context, string mediaAttr = null, bool isPush = true)
//        {
//            //    if (string.IsNullOrWhiteSpace(fromId) || string.IsNullOrWhiteSpace(toIds)
//            //        || string.IsNullOrWhiteSpace(summary) || string.IsNullOrWhiteSpace(data))
//            //        return null;
//            //toAppkey = string.IsNullOrWhiteSpace(toAppkey) ? AppKey : toAppkey;

//            //var obj = new OpenimTribeSendmsgRequest
//            //{
//            //     FromUser = fromId,
//            //      ToUsers = toIds.Split(',').ToList(),
//            //       Summary = summary,
//            //        Data = data,
//            //         Aps = aps,
//            //         ApnsParam = apnsParam,
//            //         Invisible = invisible,
//            //         FromNick = fromNick,
//            //         ToAppkey = toAppkey,
//            //};
//            //var request = new OpenimCustmsgPushRequest {Custmsg_ = obj};

//            //var json = GetResponseJson(request);
//            //return TopUtils.ParseResponse<OpenimTribeSendmsgResponse>(json);
//            return null;
//        }

//        #endregion


//        #region 聊天信息导出

//        /// <summary>
//        /// taobao.openim.relations.get (获取openim账号的聊天关系)
//        /// </summary>
//        /// <param name="begDate">查询起始日期。不得早于一个月</param>
//        /// <param name="endDate">查询结束日期。不得早于一个月></param>
//        /// <param name="uid">用户id</param>
//        /// <returns></returns>
//        public static List<OpenImUser> RelationsGet(string uid, DateTime begDate, DateTime endDate)
//        {
//            if (string.IsNullOrWhiteSpace(uid)) return null;
//            if (begDate.CompareTo(DateTime.Now.AddMonths(-1)) < 0
//                || endDate.CompareTo(DateTime.Now.AddMonths(-1)) < 0
//                || begDate.CompareTo(endDate) > 0)
//                return null;

//            var request = new OpenimRelationsGetRequest
//            {
//                BegDate = begDate.ToString("yyyyMMdd"),
//                EndDate = endDate.ToString("yyyyMMdd"),
//                User_ = new OpenImUser
//                {
//                    Uid = uid,
//                    AppKey = AppKey
//                }
//            };

//            // 自定义错误处理
//            var hand1 = "15-50000"; // "query chatRelation failed. code=-9"
//            var handErrs = new Dictionary<string, bool>();
//            handErrs[hand1] = false;
//            var json = "";
//            for (var i = 0; i < 3; i++)
//            {
//                json = GetResponseJson(request, handErrs);
//                if (!handErrs[hand1])
//                    break;
//                System.Threading.Thread.Sleep(1000);
//            }

//            var res = string.IsNullOrWhiteSpace(json) ? null : TopUtils.ParseResponse<OpenimRelationsGetResponse>(json);
//            return (res == null || res.IsError) ? null : (res.Users ?? new List<OpenImUser>());
//            //ps.返回结果可能是 {"openim_relations_get_response":{"users":{},"request_id":"z2bblz29k7hj"}}  res.Users对象为空
//        }

//        /// <summary>
//        /// taobao.openim.chatlogs.get (openim聊天记录查询接口)
//        /// </summary>
//        /// <param name="uid">用户id</param>
//        /// <param name="targetId">聊天对象id</param>
//        /// <param name="begDate">查询开始时间。必须在一个月内</param>
//        /// <param name="endDate">查询结束时间。必须在一个月内</param>
//        /// <param name="targetAppkey">目标账户appkey</param>
//        /// <returns>结果集按聊天时间正序</returns>
//        public static EsMessage[] ChatMsgsGet(string uid, string targetId
//            , DateTime begDate, DateTime endDate, string targetAppkey = null)
//        {
//            var res = ChatlogsGet(uid, targetId, begDate, endDate, targetAppkey);
//            if (res == null) return new EsMessage[0];

//            var rets = new EsMessage[res.Length];
//            for (var i = 0; i < res.Length; i++)
//                rets[i] = res[i].GetMessage(uid, targetId);

//            return rets;
//        }

//        /// <summary>
//        /// taobao.openim.chatlogs.get (openim聊天记录查询接口)
//        /// </summary>
//        /// <param name="uid">用户id</param>
//        /// <param name="targetId">聊天对象id</param>
//        /// <param name="begDate">查询开始时间。必须在一个月内</param>
//        /// <param name="endDate">查询结束时间。必须在一个月内</param>
//        /// <param name="targetAppkey">目标账户appkey</param>
//        /// <returns>结果集按聊天时间正序</returns>
//        public static RoamingMessage[] ChatlogsGet(string uid, string targetId
//            , DateTime begDate, DateTime endDate, string targetAppkey = null)
//        {
//            var rets = new List<RoamingMessage>();
//            var nextKey = string.Empty;
//            do
//            {
//                var res = ChatlogsGetBase(uid, targetId, begDate, endDate, 1000, nextKey, targetAppkey);
//                if (res == null || res.Messages == null) return null; // 如果一次数据不完整则放弃操作
//                rets.AddRange(res.Messages);

//                nextKey = res.NextKey;
//            } while (!string.IsNullOrEmpty(nextKey));

//            rets.Sort((x, y) => x.Time.CompareTo(y.Time));
//            return rets.ToArray(); // 按聊天时间正序输出
//        }

//        /// <summary>
//        /// taobao.openim.chatlogs.get (openim聊天记录查询接口)
//        /// </summary>
//        /// <param name="uid">用户id</param>
//        /// <param name="targetId">聊天对象id</param>
//        /// <param name="begDate">查询开始时间。必须在一个月内</param>
//        /// <param name="endDate">查询结束时间。必须在一个月内</param>
//        /// <param name="count">查询条数，必须为1-1000</param>
//        /// <param name="nextKey">迭代key</param>
//        /// <param name="targetAppkey">目标账户appkey</param>
//        /// <returns></returns>
//        public static RoamingMessageResult ChatlogsGetBase(string uid, string targetId
//            , DateTime begDate, DateTime endDate, int count, string nextKey = null, string targetAppkey = null)
//        {
//            if (string.IsNullOrWhiteSpace(uid) || string.IsNullOrWhiteSpace(targetId)) return null;
//            if (begDate.CompareTo(DateTime.Now.AddMonths(-1)) < 0
//                || endDate.CompareTo(DateTime.Now.AddMonths(-1)) < 0
//                || begDate.CompareTo(endDate) > 0
//                || count < 0 || count > 1000)
//                return null;

//            var request = new OpenimChatlogsGetRequest
//            {
//                Begin = GetUtc(begDate),
//                End = GetUtc(endDate),
//                User1_ = new OpenImUser { Uid = uid },
//                User2_ = new OpenImUser
//                {
//                    Uid = targetId,
//                    AppKey = string.IsNullOrWhiteSpace(targetAppkey) ? AppKey : targetAppkey
//                },
//                Count = count,
//                NextKey = nextKey
//            };
//            var json = GetResponseJson(request);
//            var res = string.IsNullOrWhiteSpace(json) ? null : TopUtils.ParseResponse<OpenimChatlogsGetResponse>(json);
//            return (res == null || res.IsError || res.Result == null) ? null : res.Result;
//            // 这里需要返回包括 NextKey的值，供后面分页
//        }


//        /// <summary>
//        /// taobao.openim.tribelogs.get (openim 群聊天记录导出接口)
//        /// </summary>
//        /// <param name="tribeId">群id</param>
//        /// <param name="begDate">查询开始时间。必须在一个月内</param>
//        /// <param name="endDate">查询结束时间。必须在一个月内</param>
//        /// <returns>结果集按聊天时间正序</returns>
//        public static EsMessage[] TribeMsgsGet(string tribeId, DateTime begDate, DateTime endDate)
//        {
//            var res = TribelogsGet(tribeId, begDate, endDate);
//            if (res == null) return new EsMessage[0];

//            var rets = new EsMessage[res.Length];
//            for (var i = 0; i < res.Length; i++)
//                rets[i] = res[i].GetMessage(tribeId);

//            return rets;
//        }

//        /// <summary>
//        /// taobao.openim.tribelogs.get (openim 群聊天记录导出接口)
//        /// </summary>
//        /// <param name="tribeId">群id</param>
//        /// <param name="begDate">查询开始时间。必须在一个月内</param>
//        /// <param name="endDate">查询结束时间。必须在一个月内</param>
//        /// <returns>结果集按聊天时间正序</returns>
//        public static OpenimTribelogsGetResponse.TribeMessageDomain[] TribelogsGet(string tribeId, DateTime begDate,
//            DateTime endDate)
//        {
//            var rets = new List<OpenimTribelogsGetResponse.TribeMessageDomain>();
//            var nextKey = string.Empty;
//            do
//            {
//                var res = TribelogsGetBase(tribeId, begDate, endDate, 1000, nextKey);
//                if (res == null || res.Messages == null) return null; // 如果一次数据不完整则放弃操作
//                rets.AddRange(res.Messages);

//                nextKey = res.NextKey;
//            } while (!string.IsNullOrEmpty(nextKey));

//            rets.Sort((x, y) => x.Time.CompareTo(y.Time));
//            return rets.ToArray(); // 按聊天时间正序输出
//        }

//        /// <summary>
//        /// taobao.openim.tribelogs.get (openim 群聊天记录导出接口)
//        /// </summary>
//        /// <param name="tribeId">群id</param>
//        /// <param name="begDate">查询开始时间。必须在一个月内</param>
//        /// <param name="endDate">查询结束时间。必须在一个月内</param>
//        /// <param name="count">查询条数，必须为1-1000</param>
//        /// <param name="nextKey">迭代key</param>
//        /// <returns></returns>
//        public static OpenimTribelogsGetResponse.TribeMessageResultDomain TribelogsGetBase(string tribeId
//            , DateTime begDate, DateTime endDate, int count, string nextKey = null)
//        {
//            if (string.IsNullOrWhiteSpace(tribeId)) return null;
//            if (begDate.CompareTo(DateTime.Now.AddMonths(-1)) < 0
//                || endDate.CompareTo(DateTime.Now.AddMonths(-1)) < 0
//                || begDate.CompareTo(endDate) > 0
//                || count < 0 || count > 1000)
//                return null;

//            var request = new OpenimTribelogsGetRequest
//            {
//                TribeId = tribeId,
//                Begin = GetUtc(begDate),
//                End = GetUtc(endDate),
//                Count = count,
//                Next = nextKey
//            };
//            var json = GetResponseJson(request);
//            var res = string.IsNullOrWhiteSpace(json) ? null : TopUtils.ParseResponse<OpenimTribelogsGetResponse>(json);
//            return (res == null || res.IsError || res.Data == null) ? null : res.Data; // 这里需要返回包括 NextKey的值，供后面分页
//        }

//        /// <summary>
//        /// taobao.openim.app.chatlogs.get (openim应用聊天记录查询)
//        /// </summary>
//        /// <param name="begDate">查询开始时间。必须在一个月内</param>
//        /// <param name="endDate">查询结束时间。必须在一个月内</param>
//        /// <returns>结果集按聊天时间正序</returns>
//        public static EsMessage[] AppChatlogsGet(DateTime begDate, DateTime endDate)
//        {
//            var rets = new List<EsMessage>();
//            var nextKey = string.Empty;
//            do
//            {
//                var res = AppChatlogsGetBase(begDate, endDate, 1000, nextKey);
//                if (res == null || res.Messages == null) return null; // 如果一次数据不完整则放弃操作
//                rets.AddRange(res.Messages);

//                nextKey = res.NextKey;
//            } while (!string.IsNullOrEmpty(nextKey));

//            rets.Sort((x, y) => x.Time.CompareTo(y.Time));
//            return rets.ToArray(); // 按聊天时间正序输出
//        }


//        /// <summary>
//        /// taobao.openim.app.chatlogs.get (openim应用聊天记录查询)
//        /// </summary>
//        /// <param name="begDate">查询开始时间。必须在一个月内</param>
//        /// <param name="endDate">查询结束时间。必须在一个月内</param>
//        /// <param name="count">查询条数，必须为1-1000</param>
//        /// <param name="nextKey">迭代key</param>
//        /// <returns></returns>
//        public static EsMessageResult AppChatlogsGetBase(DateTime begDate, DateTime endDate, int count,
//            string nextKey = null)
//        {
//            if (begDate.CompareTo(DateTime.Now.AddMonths(-1)) < 0
//                || endDate.CompareTo(DateTime.Now.AddMonths(-1)) < 0
//                || begDate.CompareTo(endDate) > 0
//                || count < 0 || count > 1000)
//                return null;

//            var request = new OpenimAppChatlogsGetRequest
//            {
//                Beg = GetUtc(begDate),
//                End = GetUtc(endDate),
//                Count = count,
//                Next = nextKey
//            };
//            var json = GetResponseJson(request);
//            var res = (string.IsNullOrWhiteSpace(json))
//                ? null
//                : TopUtils.ParseResponse<OpenimAppChatlogsGetResponse>(json);
//            return (res == null || res.IsError || res.Result == null) ? null : res.Result; // 这里需要返回包括 NextKey的值，供后面分页
//        }


//        #endregion


//        #region 聊天信息导入

//        /// <summary>
//        /// taobao.openim.chatlogs.import (openim单聊消息导入)
//        /// </summary>
//        /// <param name="messages">消息列表，最大列表长度：20</param>
//        /// <returns></returns>
//        public static bool ChatlogsImport(List<OpenimChatlogsImportRequest.TextMessageDomain> messages)
//        {
//            if (messages == null || messages.Count == 0) return false;

//            var request = new OpenimChatlogsImportRequest
//            {
//                Messages_ = messages
//            };
//            var json = GetResponseJson(request);
//            if (string.IsNullOrWhiteSpace(json)) return false;
//            var objRet = TopUtils.ParseResponse<OpenimChatlogsImportResponse>(json);
//            return objRet != null && objRet.Succ;
//        }

//        /// <summary>
//        /// taobao.openim.tribelogs.import (openim群聊天记录导入)
//        /// </summary>
//        /// <param name="tribeId">群id</param>
//        /// <param name="messages">消息列表，最大列表长度：20</param>
//        /// <returns></returns>
//        public static bool TribelogsImport(long tribeId,
//            List<OpenimTribelogsImportRequest.TribeTextMessageDomain> messages)
//        {
//            if (tribeId < 0) return false;
//            if (messages == null || messages.Count == 0) return false;

//            var request = new OpenimTribelogsImportRequest
//            {
//                TribeId = tribeId,
//                Messages_ = messages
//            };
//            var json = GetResponseJson(request);
//            if (string.IsNullOrWhiteSpace(json)) return false;
//            var objRet = TopUtils.ParseResponse<OpenimTribelogsImportResponse>(json);
//            return objRet != null && objRet.Succ;
//        }

//        #endregion


//        #region 群成员维护

//        /// <summary>
//        /// taobao.openim.tribe.create (创建群/讨论组)
//        /// </summary>
//        /// <param name="uid">用户id</param>
//        /// <param name="tribeName">群名称</param>
//        /// <param name="notice">群公告</param>
//        /// <param name="tribeType">0：普通群，有管理员角色，对成员加入有权限控制。1：讨论组，没有管理员，不能解散</param>
//        /// <param name="dictUser">相关用户属性</param>
//        /// <param name="memberUids">群的成员。为普通群时，可不填。为讨论组时，必选。多用户以","分隔,最大用户个数：1000，建议前端限制为500，剩余部分改为邀请方式加入</param>
//        /// <returns></returns>
//        public static TribeInfo TribeCreate(string uid, string tribeName, string notice,
//            int tribeType, string memberUids, Dictionary<string, string> dictUser)
//        {
//            if (string.IsNullOrWhiteSpace(uid)
//                || string.IsNullOrWhiteSpace(tribeName)
//                || !(new[] { 0, 1 }.Contains(tribeType))
//                || string.IsNullOrWhiteSpace(memberUids))
//                return null;
//            var arrUid = memberUids.Split(',').ToList().Where(c => !string.IsNullOrWhiteSpace(c)).ToList();
//            var oprMax = 500; // 一次创建API最大允许1000人，实测 700+是极限，为了稳妥建议限制为 500
//            if (!arrUid.Any() || arrUid.Count() > oprMax) return null;

//            var request = new OpenimTribeCreateRequest
//            {
//                User_ = new OpenImUser { Uid = uid, AppKey = AppKey },
//                TribeName = tribeName,
//                Notice = string.IsNullOrWhiteSpace(notice) ? "." : notice,
//                TribeType = tribeType,
//                Members_ = arrUid.Select(c => new OpenImUser { Uid = c, AppKey = AppKey }).ToList()
//            };
//            var json = GetResponseJson(request);
//            var res = string.IsNullOrWhiteSpace(json)
//                ? null
//                : TopUtils.ParseResponse<OpenimTribeCreateResponse>(json);
//            if (res == null || res.TribeInfo == null || res.IsError) return null;

//            // 新增成功，异步更新用户群昵称(for.ios)
//            TribemembernickAsync(res.TribeInfo.TribeId, uid, dictUser); // 优先更新创建人的
//            TribemembernickAsync(res.TribeInfo.TribeId, string.Join(",", arrUid), dictUser);

//            return res.TribeInfo;
//        }

//        /// <summary>
//        /// taobao.openim.tribe.invite (OPENIM群邀请加入)
//        /// </summary>
//        /// <param name="oprUid">用户id</param>
//        /// <param name="tribeId">群id</param>
//        /// <param name="memberUids">邀请人员列表</param>
//        /// <param name="dictUser">相关用户属性</param>
//        /// <returns>true/false 表示添加是否成功</returns>
//        public static TribeMemberExec TribeInvite(string oprUid, long tribeId, string memberUids
//            , Dictionary<string, string> dictUser)
//        {
//            var succUids = new List<string>();
//            var failUids = memberUids.Split(',').ToList();

//            #region 条件过滤

//            if (tribeId < 0
//                || string.IsNullOrWhiteSpace(oprUid)
//                || string.IsNullOrWhiteSpace(memberUids)) return null;
//            var reqUids = memberUids.Split(',').Where(c => !string.IsNullOrWhiteSpace(c)).ToList();
//            if (!reqUids.Any())
//                return new TribeMemberExec { TribeId = tribeId, SuccUids = succUids, FailUids = failUids };

//            // 服务器已存在人员过滤，（超过2000 或者 操作人不存在则失败）
//            var imUsers = TribeGetMembers(oprUid, tribeId);
//            if (imUsers == null || !imUsers.Any() || imUsers.Count >=2000
//                || !(imUsers.Any(c => c.Uid == oprUid)))
//                return new TribeMemberExec { TribeId = tribeId, SuccUids = succUids, FailUids = failUids };
//            var ret = new TribeMemberExec
//            {
//                TribeId = tribeId,
//                SuccUids = reqUids.Where(reqUid => imUsers.Exists(imUser => imUser.Uid == reqUid)).ToList()
//            };
//            ret.FailUids = reqUids.Where(reqUid => !ret.SuccUids.Contains(reqUid)).ToList();
//            if (!ret.FailUids.Any())
//                return ret; // 如果本次请求不需要处理，则直接返回

//            #endregion

//            #region 按批次进行邀请操作（群成员越多，邀请超时的可能性越大）

//            var reqUsers = ret.FailUids.Select(c => new OpenImUser { Uid = c, AppKey = AppKey }).ToList();
//            var reqLength = imUsers.Count() > 200 ? 10 : 30; // "远程服务调用超时"，这个错误会产生调用返回异常，但是数据已经写入的情况
//            var hand1 = "15-isp.top-remote-connection-timeout";
//            var handErrs = new Dictionary<string, bool>();
//            var reqCnt = Math.Ceiling((double)reqUsers.Count() / reqLength);
//            for (var i = 0; i < reqCnt; i++)
//            {
//                var currUser = reqUsers.Skip(i * reqLength).Take(reqLength).ToList();
//                var request = new OpenimTribeInviteRequest
//                {
//                    User_ = new OpenImUser { Uid = oprUid, AppKey = AppKey },
//                    TribeId = tribeId,
//                    Members_ = currUser
//                };
//                handErrs[hand1] = false; // 每次循环重置
//                var json = GetResponseJson(request, handErrs);
//                var res = string.IsNullOrWhiteSpace(json)
//                    ? null
//                    : TopUtils.ParseResponse<OpenimTribeInviteResponse>(json);
//                if (!handErrs[hand1] && (res != null && !res.IsError))
//                {
//                    // 本次批量提交成功，更新（增加）到成功的，更新（重置）失败的（待提交的）
//                    ret.SuccUids.AddRange(currUser.Select(c => c.Uid));
//                    ret.FailUids = reqUids.Where(reqUid => !ret.SuccUids.Contains(reqUid)).ToList();
//                }
//            }

//            #endregion

//            #region 根据失败用户数（待提交的），一个个进行提交，并根据返回值判断是否有效

//            if (ret.FailUids.Any())
//            {
//                var hand2 = "15-6"; // "OPENIM_TRIBE_DUP_MEMBER"，人员重复错误
//                var handErrs2 = new Dictionary<string, bool>();
//                var request2 = new OpenimTribeInviteRequest
//                {
//                    User_ = new OpenImUser { Uid = oprUid, AppKey = AppKey },
//                    TribeId = tribeId
//                };

//                #region 因为邀请和查询间存在延迟，所以暂先单个提交，发现存在重复错误后，重刷一遍人员，再单个递交

//                do
//                {
//                    var reqUids2 = ret.FailUids.ToArray();
//                    foreach (var reqUid2 in reqUids2)
//                    {
//                        request2.Members_ = new List<OpenImUser> { new OpenImUser { Uid = reqUid2, AppKey = AppKey } };
//                        handErrs2[hand2] = false; // 每次循环重置
//                        var json2 = GetResponseJson(request2, handErrs2);
//                        var res2 = string.IsNullOrWhiteSpace(json2)
//                            ? null
//                            : TopUtils.ParseResponse<OpenimTribeInviteResponse>(json2);
//                        if (handErrs2[hand2] || (res2 != null && !res2.IsError))
//                        {
//                            // 发生自定义错误(人员重复)，或者提交正常，假定数据有效
//                            ret.SuccUids.Add(reqUid2);
//                            ret.FailUids.Remove(reqUid2);
//                        }

//                        #region 如果是人员重复错误，表示批量更新生效，重新比较一遍

//                        if (handErrs2[hand2])
//                        {
//                            var imUsers2 = TribeGetMembers(oprUid, tribeId);
//                            if (imUsers2 != null && imUsers2.Any())
//                            {
//                                var tmpArrFail = ret.FailUids.ToArray();
//                                foreach (
//                                    var failUid in tmpArrFail.Where(failUid => imUsers2.Exists(c => c.Uid == failUid)))
//                                {
//                                    ret.SuccUids.Add(failUid);
//                                    ret.FailUids.Remove(failUid);
//                                }
//                            }
//                            break; // 发生自定义错误(人员重复)，中断本轮循环单个请求，刷新人员列表
//                        }

//                        #endregion
//                    } // end.of foreach
//                } while (handErrs2[hand2] && ret.FailUids.Any());

//                #endregion
//            }

//            #endregion

//            // 异步更新用户群昵称(for.ios)
//            TribemembernickAsync(tribeId, string.Join(",", ret.SuccUids), dictUser);

//            return ret;
//        }

//        /// <summary>
//        /// taobao.openim.tribe.join (OPENIM群主动加入)
//        /// </summary>
//        /// <param name="uid">用户id</param>
//        /// <param name="tribeId">群id</param>
//        /// <param name="dictUser">相关用户属性</param>
//        /// <returns></returns>
//        public static bool TribeJoin(string uid, long tribeId, Dictionary<string, string> dictUser)
//        {
//            if (string.IsNullOrWhiteSpace(uid)) return false;
//            if (tribeId < 0) return false;

//            var request = new OpenimTribeJoinRequest
//            {
//                User_ = new OpenImUser { Uid = uid, AppKey = AppKey },
//                TribeId = tribeId
//            };
//            var json = GetResponseJson(request);
//            if (string.IsNullOrWhiteSpace(json)) return false;
//            var objRet = TopUtils.ParseResponse<OpenimTribeJoinResponse>(json);
//            var res = objRet != null && !objRet.IsError;

//            // 内嵌更新用户群昵称
//            if (res) TribemembernickAsync(tribeId, uid, dictUser);

//            return res;
//        }

//        /// <summary>
//        /// taobao.openim.tribe.expel (OPENIM群踢出成员)
//        /// </summary>
//        /// <param name="uid">用户id</param>
//        /// <param name="tribeId">群id</param>
//        /// <param name="trgUid">被移除用户id</param>
//        /// <returns></returns>
//        public static bool TribeExpel(string uid, long tribeId, string trgUid)
//        {
//            if (string.IsNullOrWhiteSpace(uid) || string.IsNullOrWhiteSpace(trgUid)) return false;
//            if (tribeId < 0) return false;

//            var request = new OpenimTribeExpelRequest
//            {
//                User_ = new OpenImUser { Uid = uid, AppKey = AppKey },
//                TribeId = tribeId,
//                Member_ = new OpenImUser { Uid = trgUid, AppKey = AppKey }
//            };
//            var json = GetResponseJson(request);
//            if (string.IsNullOrWhiteSpace(json)) return false;
//            var objRet = TopUtils.ParseResponse<OpenimTribeExpelResponse>(json);
//            return objRet != null && !objRet.IsError;
//        }

//        /// <summary>
//        /// taobao.openim.tribe.quit (OPENIM群成员退出)
//        /// </summary>
//        /// <param name="uid">用户id</param>
//        /// <param name="tribeId">群id</param>
//        /// <returns></returns>
//        public static bool TribeQuit(string uid, long tribeId)
//        {
//            if (string.IsNullOrWhiteSpace(uid)) return false;
//            if (tribeId < 0) return false;

//            var request = new OpenimTribeQuitRequest
//            {
//                User_ = new OpenImUser { Uid = uid, AppKey = AppKey },
//                TribeId = tribeId
//            };
//            var json = GetResponseJson(request);
//            if (string.IsNullOrWhiteSpace(json)) return false;
//            var objRet = TopUtils.ParseResponse<OpenimTribeQuitResponse>(json);
//            return objRet != null && !objRet.IsError;
//        }

//        /// <summary>
//        /// (OPENIM群管理员退出)
//        /// </summary>
//        /// <param name="uid">用户id</param>
//        /// <param name="tribeId">群id</param>
//        /// <param name="mgrUid">返回的新群管理员 oprUid</param>
//        /// <returns></returns>
//        public static bool TribeQuitmanager(string uid, long tribeId, out string mgrUid)
//        {
//            mgrUid = string.Empty;
//            var flag = TribeQuit(uid, tribeId);
//            var counter = 2;
//            while (flag && counter > 0)
//            {
//                System.Threading.Thread.Sleep(3000); // 延时
//                var svrUsers = TribeGetMembers(uid, tribeId);
//                if (svrUsers == null || !svrUsers.Any())
//                    return true;

//                mgrUid = svrUsers.Where(c => c.Role == "host").Select(c => c.Uid).FirstOrDefault();
//                if (!string.IsNullOrWhiteSpace(mgrUid) && mgrUid != uid)
//                    break; // 确保延时有效
//                counter--;
//            }

//            return flag;
//        }

//        /// <summary>
//        /// taobao.openim.tribe.setmembernick (设置群成员昵称)
//        /// </summary>
//        /// <param name="uid">用户id</param>
//        /// <param name="tribeId">群id</param>
//        /// <param name="trgUid">目标用户id</param>
//        /// <param name="nick">设置的昵称</param>
//        /// <remarks>设置群成员昵称，存在如下两种场景 1 群主或管理员设置群成员昵称，该操作有权限控制。只针对普通群的群主和管理员开发此功能；讨论组群主不支持此设置操作 2 群成员设置自己的昵称，该功能对群所有成员开放</remarks>
//        /// <returns></returns>
//        public static bool TribeSetmembernick(string uid, long tribeId, string trgUid, string nick)
//        {
//            if (string.IsNullOrWhiteSpace(uid) || string.IsNullOrWhiteSpace(trgUid) || string.IsNullOrWhiteSpace(nick))
//                return false;
//            if (tribeId < 0) return false;

//            var request = new OpenimTribeSetmembernickRequest
//            {
//                User_ = new OpenimTribeSetmembernickRequest.UserDomain { Uid = uid, AppKey = AppKey },
//                TribeId = tribeId,
//                Member_ = new OpenimTribeSetmembernickRequest.UserDomain { Uid = trgUid, AppKey = AppKey },
//                Nick = nick
//            };
//            var replayErr = new Dictionary<string, bool>();
//            replayErr["15-10000"] = false; // "TRIBE_INVALID_ARGUMENTS" 参数缺失，但实际上无问题
//            var json = GetResponseJson(request, null, replayErr);
//            if (string.IsNullOrWhiteSpace(json)) return false;
//            var objRet = TopUtils.ParseResponse<OpenimTribeSetmembernickResponse>(json);
//            return objRet != null && !objRet.IsError;
//        }

//        /// <summary>
//        /// taobao.openim.tribe.setmanager (OPENIM群设置管理员)
//        /// </summary>
//        /// <param name="uid">用户id</param>
//        /// <param name="tribeId">群id</param>
//        /// <param name="targetId">管理员用户id</param>
//        /// <returns></returns>
//        public static bool TribeSetManager(string uid, long tribeId, string targetId)
//        {
//            if (string.IsNullOrWhiteSpace(uid) || string.IsNullOrWhiteSpace(targetId)) return false;
//            if (tribeId < 0) return false;

//            var request = new OpenimTribeSetmanagerRequest
//            {
//                User_ = new OpenImUser { Uid = uid, AppKey = AppKey },
//                Tid = tribeId,
//                Member_ = new OpenImUser { Uid = targetId, AppKey = AppKey }
//            };
//            var json = GetResponseJson(request);
//            if (string.IsNullOrWhiteSpace(json)) return false;
//            var objRet = TopUtils.ParseResponse<OpenimTribeSetmanagerResponse>(json);
//            return objRet != null && !objRet.IsError;
//        }

//        /// <summary>
//        /// taobao.openim.tribe.unsetmanager (OPENIM群取消管理员)
//        /// </summary>
//        /// <param name="uid">用户id</param>
//        /// <param name="tribeId">群id</param>
//        /// <param name="targetId">对象用户id</param>
//        /// <returns></returns>
//        public static bool TribeUnsetmanager(string uid, long tribeId, string targetId)
//        {
//            if (string.IsNullOrWhiteSpace(uid) || string.IsNullOrWhiteSpace(targetId)) return false;
//            if (tribeId < 0) return false;

//            var request = new OpenimTribeUnsetmanagerRequest
//            {
//                User_ = new OpenImUser { Uid = uid, AppKey = AppKey },
//                Tid = tribeId,
//                Member_ = new OpenImUser { Uid = targetId, AppKey = AppKey }
//            };
//            var json = GetResponseJson(request);
//            return !string.IsNullOrWhiteSpace(json) &&
//                   (TopUtils.ParseResponse<OpenimTribeUnsetmanagerResponse>(json) != null);
//        }

//        /// <summary>
//        /// 获取群管理员
//        /// </summary>
//        /// <param name="tribeId"></param>
//        /// <returns></returns>
//        public static TribeUser TribeGetManager(long tribeId)
//        {
//            if (tribeId < 0) return null;

//            var svrUsers = TribeGetMembers("systemuser", tribeId);
//            if (svrUsers == null || !svrUsers.Any())
//                return null;
//            return svrUsers.FirstOrDefault(c => c.Role == "host");
//        }

//        /// <summary>
//        /// taobao.openim.tribe.dismiss (OPENIM群解散)
//        /// </summary>
//        /// <param name="tribeId">群ID</param>
//        /// <returns></returns>
//        public static bool TribeDismiss(long tribeId)
//        {
//            if (tribeId < 0) return false;

//            var managerUser = TribeGetManager(tribeId);
//            if (managerUser == null)
//                return false;

//            return TribeDismiss(managerUser.Uid, tribeId);
//        }

//        /// <summary>
//        /// taobao.openim.tribe.dismiss (OPENIM群解散)
//        /// </summary>
//        /// <param name="managerUid">管理员uid</param>
//        /// <param name="tribeId">群ID</param>
//        /// <returns></returns>
//        public static bool TribeDismiss(string managerUid, long tribeId)
//        {
//            if (string.IsNullOrWhiteSpace(managerUid) || tribeId < 0) return false;

//            var request = new OpenimTribeDismissRequest
//            {
//                User_ = new OpenImUser { Uid = managerUid, AppKey = AppKey },
//                TribeId = tribeId
//            };
//            var json = GetResponseJson(request);
//            var ret = string.IsNullOrWhiteSpace(json) ? null : TopUtils.ParseResponse<OpenimTribeDismissResponse>(json);
//            return ret != null && !ret.IsError;
//        }

//        /// <summary>
//        /// taobao.openim.tribe.modifytribeinfo (OPENIM群信息修改)
//        /// </summary>
//        /// <param name="uid">用户id</param>
//        /// <param name="tribeId">群id</param>
//        /// <param name="tribeName">群名称</param>
//        /// <param name="notice">群公告</param>
//        /// <returns></returns>
//        public static bool TribeModifytribeinfo(string uid, long tribeId, string tribeName, string notice)
//        {
//            if (string.IsNullOrWhiteSpace(uid) || string.IsNullOrWhiteSpace(tribeName) ||
//                string.IsNullOrWhiteSpace(notice))
//                return false;
//            if (tribeId < 0) return false;

//            var request = new OpenimTribeModifytribeinfoRequest
//            {
//                User_ = new OpenImUser { Uid = uid, AppKey = AppKey },
//                TribeId = tribeId,
//                TribeName = tribeName,
//                Notice = notice
//            };
//            var json = GetResponseJson(request);
//            return !string.IsNullOrWhiteSpace(json) &&
//                   (TopUtils.ParseResponse<OpenimTribeModifytribeinfoResponse>(json) != null);
//        }

//        /// <summary>
//        /// taobao.openim.tribe.gettribeinfo (获取群信息)
//        /// </summary>
//        /// <param name="uid">用户id</param>
//        /// <param name="tribeId">群ID</param>
//        /// <returns></returns>
//        public static TribeInfo TribeGetTribeInfo(string uid, long tribeId)
//        {
//            var res = TribeGetTribeInfoBase(uid, tribeId);
//            return res != null && !res.IsError && res.TribeInfo.TribeId > 0 ? res.TribeInfo : null;
//        }

//        /// <summary>
//        /// taobao.openim.tribe.gettribeinfo (获取群信息)
//        /// </summary>
//        /// <param name="uid">用户id</param>
//        /// <param name="tribeId">群ID</param>
//        /// <returns></returns>
//        public static OpenimTribeGettribeinfoResponse TribeGetTribeInfoBase(string uid, long tribeId)
//        {
//            if (string.IsNullOrWhiteSpace(uid)) return null;
//            if (tribeId < 0) return null;

//            var request = new OpenimTribeGettribeinfoRequest
//            {
//                User_ = new OpenImUser { Uid = uid, AppKey = AppKey },
//                TribeId = tribeId
//            };
//            var json = GetResponseJson(request);
//            return string.IsNullOrWhiteSpace(json)
//                ? null
//                : TopUtils.ParseResponse<OpenimTribeGettribeinfoResponse>(json);
//        }

//        /// <summary>
//        /// taobao.openim.tribe.getmembers (OPENIM群成员获取)
//        /// </summary>
//        /// <param name="uid">用户id</param>
//        /// <param name="tribeId">群ID</param>
//        /// <returns></returns>
//        public static List<TribeUser> TribeGetMembers(string uid, long tribeId)
//        {
//            var res = TribeGetMembersBase(uid, tribeId);
//            return res != null && !res.IsError ? res.TribeUserList : null;
//        }

//        /// <summary>
//        /// taobao.openim.tribe.getmembers (OPENIM群成员获取)
//        /// </summary>
//        /// <param name="uid">用户id</param>
//        /// <param name="tribeId">群ID</param>
//        /// <returns></returns>
//        public static OpenimTribeGetmembersResponse TribeGetMembersBase(string uid, long tribeId)
//        {
//            if (string.IsNullOrWhiteSpace(uid)) return null;
//            if (tribeId < 0) return null;

//            var request = new OpenimTribeGetmembersRequest
//            {
//                User_ = new OpenImUser { Uid = uid, AppKey = AppKey },
//                TribeId = tribeId
//            };
//            var json = GetResponseJson(request);
//            return string.IsNullOrWhiteSpace(json) ? null : TopUtils.ParseResponse<OpenimTribeGetmembersResponse>(json);
//        }

//        /// <summary>
//        /// taobao.openim.tribe.getalltribes (获取用户群列表)
//        /// </summary>
//        /// <param name="uid">用户id</param>
//        /// <param name="tribeTypes">群类型列表，"0,1" 0:群，1:讨论组</param>
//        /// <returns></returns>
//        public static List<TribeInfo> TribeGetalltribes(string uid, string tribeTypes)
//        {
//            if (string.IsNullOrWhiteSpace(uid) || string.IsNullOrWhiteSpace(tribeTypes)) return null;

//            var request = new OpenimTribeGetalltribesRequest
//            {
//                User_ = new OpenImUser { Uid = uid, AppKey = AppKey },
//                TribeTypes = tribeTypes
//            };
//            var replayErr = new Dictionary<string, bool>();
//            replayErr["15-10000"] = false; // "TRIBE_INVALID_ARGUMENTS" 参数缺失，但实际上无问题
//            var json = GetResponseJson(request, null, replayErr);
//            var ret = string.IsNullOrWhiteSpace(json)
//                ? null
//                : TopUtils.ParseResponse<OpenimTribeGetalltribesResponse>(json);
//            return ret == null ? null : ret.TribeInfoList ?? new List<TribeInfo>();
//        }

//        #endregion


//        #region 功能扩展

//        /// <summary>
//        /// taobao.openim.track.getsummary (获取用户足迹信息)
//        /// </summary>
//        /// <param name="uid">用户id</param>
//        /// <remarks>获取用户最近访问页面的信息(操作系统，设备型号，ip，国家，省份，城市，运营商，浏览器，渠道等) 总访问页面次数和总停留时间</remarks>
//        /// <returns></returns>
//        public static OpenimTrackGetsummaryResponse TrackGetsummary(string uid)
//        {
//            if (string.IsNullOrWhiteSpace(uid)) return null;

//            var request = new OpenimTrackGetsummaryRequest { Uid = uid };
//            var json = GetResponseJson(request);
//            return string.IsNullOrWhiteSpace(json) ? null : TopUtils.ParseResponse<OpenimTrackGetsummaryResponse>(json);
//        }

//        /// <summary>
//        /// taobao.openim.track.getdetails (获取openim用户足迹详细信息)
//        /// </summary>
//        /// <param name="uid">用户id</param>
//        /// <param name="begDate">开始时间</param>
//        /// <param name="endDate">结束时间</param>
//        /// <remarks>获取openim用户足迹详细信息 ，如进入页面的时间戳，url，referer信息等</remarks>
//        /// <returns></returns>
//        public static OpenimTrackGetdetailsResponse TrackGetdetails(string uid, DateTime begDate, DateTime endDate)
//        {
//            if (string.IsNullOrWhiteSpace(uid)) return null;
//            if (begDate.CompareTo(endDate) > 0) return null;

//            var request = new OpenimTrackGetdetailsRequest
//            {
//                Uid = uid,
//                Starttime = GetUtc(begDate).ToString(),
//                Endtime = GetUtc(endDate).ToString()
//            };
//            var json = GetResponseJson(request);
//            return string.IsNullOrWhiteSpace(json) ? null : TopUtils.ParseResponse<OpenimTrackGetdetailsResponse>(json);
//        }

//        /// <summary>
//        /// taobao.openim.snfilterword.setfilter (关键词过滤)
//        /// </summary>
//        /// <param name="creator">上传者身份。eg."应用运维"</param>
//        /// <param name="filterword">需要过滤的关键词</param>
//        /// <param name="desc">过滤原因描述</param>
//        /// <returns></returns>
//        public static OpenimSnfilterwordSetfilterResponse TrackGetdetails(string creator, string filterword,
//            string desc = null)
//        {
//            if (string.IsNullOrWhiteSpace(creator) || string.IsNullOrWhiteSpace(filterword)) return null;

//            var request = new OpenimSnfilterwordSetfilterRequest
//            {
//                Creator = creator,
//                Filterword = filterword,
//                Desc = desc
//            };
//            var json = GetResponseJson(request);
//            return string.IsNullOrWhiteSpace(json)
//                ? null
//                : TopUtils.ParseResponse<OpenimSnfilterwordSetfilterResponse>(json);
//        }

//        #endregion


//        public static long GetUtc(DateTime dt)
//        {
//            return (dt.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
//        }

//        public static DateTime GetTimeFromUtc(long utc)
//        {
//            return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(utc);
//        }


//        /// <summary>
//        /// 同步修改用户（所有群的）群昵称（为了IOS的推送）
//        /// </summary>
//        /// <returns></returns>
//        public static async System.Threading.Tasks.Task UsernickAsync(string uid, string nick)
//        {
//            if (string.IsNullOrWhiteSpace(uid)) return;
//            if (string.IsNullOrWhiteSpace(nick)) return;

//            // 异步更新
//            await System.Threading.Tasks.Task.Factory.StartNew(() =>
//            {
//                var tribes = TribeGetalltribes(uid, "0,1");
//                if (tribes == null || !tribes.Any()) return;
//                foreach (var tribe in tribes)
//                {
//                    TribeSetmembernick(uid, tribe.TribeId, uid, nick);
//                }

//            });
//        }

//        /// <summary>
//        /// 同步修改用户群昵称（为了IOS的推送）
//        /// </summary>
//        /// <param name="tribeId">群号</param>
//        /// <param name="trgUids">Uid列表，用','分隔</param>
//        /// <param name="dictUser">相关用户属性</param>
//        /// <returns></returns>
//        public static async System.Threading.Tasks.Task TribemembernickAsync(long tribeId, string trgUids
//            , Dictionary<string, string> dictUser)
//        {
//            if (tribeId <= 0 || string.IsNullOrWhiteSpace(trgUids)
//                || dictUser == null || !dictUser.Any()) return;

//            // 异步更新
//            await System.Threading.Tasks.Task.Factory.StartNew(() =>
//            {
//                var arrUids = trgUids.Split(',').Where(uid => dictUser.ContainsKey(uid)
//                                                              && !string.IsNullOrWhiteSpace(dictUser[uid]));
//                foreach (var uid in arrUids)
//                {
//                    TribeSetmembernick(uid, tribeId, uid, dictUser[uid]);
//                }
//            });
//        }

//    }

//    #region 相关类

//    public class UserExec
//    {
//        private List<string> _succUids = new List<string>();
//        private List<string> _failUids = new List<string>();

//        public List<string> SuccUids
//        {
//            get { return _succUids; }
//            set
//            {
//                _succUids.Clear();
//                if (value != null)
//                    _succUids.AddRange(value);
//            }
//        }

//        public List<string> FailUids
//        {
//            get { return _failUids; }
//            set
//            {
//                _failUids.Clear();
//                if (value != null)
//                    _failUids.AddRange(value);
//            }
//        }
//    }

//    public class TribeMemberExec
//    {
//        private List<string> _succUids = new List<string>();
//        private List<string> _failUids = new List<string>();

//        public TribeInfo TribeInfo;

//        public long TribeId;

//        public List<string> SuccUids
//        {
//            get { return _succUids; }
//            set
//            {
//                _succUids.Clear();
//                if (value != null)
//                    _succUids.AddRange(value);
//            }
//        }

//        public List<string> FailUids
//        {
//            get { return _failUids; }
//            set
//            {
//                _failUids.Clear();
//                if (value != null)
//                    _failUids.AddRange(value);
//            }
//        }
//    }

//    public class ImMsgDomainExt : OpenimImmsgPushRequest.ImMsgDomain
//    {
//        [System.Xml.Serialization.XmlElement("from_taobao")]
//        public long? FromTaobao { get; set; }
//    }

//    public class CustMsgDomainExt : OpenimCustmsgPushRequest.CustMsgDomain
//    {
//        [System.Xml.Serialization.XmlElement("from_taobao")]
//        public long? FromTaobao { get; set; }
//    }

//    #endregion

//    #region 扩展方法

//    public static class OpenImApiExtends
//    {
//        public static EsMessage GetMessage(this RoamingMessage msg, string uid1, string uid2)
//        {
//            return new EsMessage
//            {
//                // 消息方向。0: uid1->uid2, 1: uid2->uid1
//                FromId = new OpenImUser { Uid = (msg.Direction == 0) ? uid1 : uid2 },
//                ToId = new OpenImUser { Uid = (msg.Direction == 0) ? uid2 : uid1 },
//                Content = msg.ContentList,
//                Time = msg.Time,
//                Type = msg.Type,
//                Uuid = msg.Uuid
//            };
//        }

//        public static EsMessage GetMessage(this OpenimTribelogsGetResponse.TribeMessageDomain msg, string tribeId = null)
//        {
//            return new EsMessage
//            {
//                FromId = new OpenImUser { Uid = msg.FromId.Uid },
//                ToId = new OpenImUser { Uid = tribeId },
//                Content = (msg.Content ?? new List<OpenimTribelogsGetResponse.MessageItemDomain>())
//                    .Select(c => new RoamingMessageItem { Type = c.Type, Value = c.Value }).ToList(),
//                Time = msg.Time,
//                Type = msg.Type,
//                Uuid = msg.Uuid
//            };
//        }

//    }

//    #endregion
//}

