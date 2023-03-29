using System;
using UnityEngine;

namespace JFramework.Core
{
    public static class DateManager
    {
        /// <summary>
        /// 管理器名称
        /// </summary>
        private static string Name => nameof(DateManager);

        /// <summary>
        /// 上一次检测时间
        /// </summary>
        private static float lastCheckTime;

        /// <summary>
        /// 当前日期
        /// </summary>
        private static DateTime dateTime => DateTime.Now;

        private static int lastDayOfYear
        {
            get => JsonManager.Load<int>(Name,true);
            set => JsonManager.Save(value, Name,true);
        }

        /// <summary>
        /// 管理器初始化
        /// </summary>
        internal static void Awake()
        {
            lastCheckTime = Time.realtimeSinceStartup;
            GlobalManager.Instance.UpdateAction += OnUpdate;
        }

        /// <summary>
        /// 管理器更新
        /// </summary>
        private static void OnUpdate()
        {
            if (Time.realtimeSinceStartup - lastCheckTime > 5)
            {
                lastCheckTime = Time.realtimeSinceStartup;
                if (dateTime.DayOfYear != lastDayOfYear)
                {
                    //同一年
                    if (dateTime.DayOfYear > lastDayOfYear)
                    {
                        //超过一天未登陆
                        if (dateTime.DayOfYear - lastDayOfYear >= 2)
                        {
                            lastDayOfYear = dateTime.DayOfYear;
                            EventManager.Send(DateSetting.OnDateChanged);
                        }
                        //只有一天未登陆
                        else
                        {
                            if (dateTime.Hour >= DateSetting.RefreshHour)
                            {
                                lastDayOfYear = dateTime.DayOfYear;
                                EventManager.Send(DateSetting.OnDateChanged);
                            }
                        }
                    }
                    //跨年
                    else if (dateTime.DayOfYear < lastDayOfYear)
                    {
                        int lastYear = dateTime.Year - 1;
                        int lastYearTotalDays = lastYear % 4 == 0 ? 366 : 365;
                        //超过一天未登陆
                        if (lastYearTotalDays + dateTime.DayOfYear - lastDayOfYear >= 2)
                        {
                            lastDayOfYear = dateTime.DayOfYear;
                            EventManager.Send(DateSetting.OnDateChanged);
                        }
                        //一天未登陆并且到达刷新时间
                        else
                        {
                            if (dateTime.Hour >= DateSetting.RefreshHour)
                            {
                                lastDayOfYear = dateTime.DayOfYear;
                                EventManager.Send(DateSetting.OnDateChanged);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 销毁管理器
        /// </summary>
        internal static void Destroy()
        {
            lastDayOfYear = dateTime.DayOfYear;
        }
    }
}