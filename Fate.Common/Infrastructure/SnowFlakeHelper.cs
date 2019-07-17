using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

using Fate.Common.Extensions;

namespace Fate.Common.Infrastructure
{
    public class SnowFlakeHelper
    {
        /// <summary>
        /// 机器码
        /// </summary>
        private static long _workerId;

        /// <summary>
        /// 初始基准时间戳，小于当前时间点即可
        /// 分布式项目请保持此时间戳一致
        /// </summary>
        private static long _twepoch = 0L;

        /// <summary>
        /// 毫秒计数器
        /// </summary>
        private static long sequence = 0L;

        /// <summary>
        /// 机器码字节数。4个字节用来保存机器码(定义为Long类型会出现，最大偏移64位，所以左移64位没有意义)
        /// </summary>
        private static int workerIdBits = 5;

        /// <summary>
        /// 最大机器ID所占的位数
        /// </summary>
        private static long maxWorkerId = -1L ^ -1L << workerIdBits;

        /// <summary>
        /// 计数器字节数，10个字节用来保存计数码
        /// </summary>
        private static int sequenceBits = 12;

        /// <summary>
        /// 机器码数据左移位数，就是后面计数器占用的位数
        /// </summary>
        private static int workerIdShift = sequenceBits;

        /// <summary>
        /// 时间戳左移动位数就是机器码和计数器总字节数
        /// </summary>
        private static int timestampLeftShift = sequenceBits + workerIdBits;

        /// <summary>
        /// 一微秒内可以产生计数，如果达到该值则等到下一微妙在进行生成
        /// </summary>
        private static long sequenceMask = -1L ^ -1L << sequenceBits;

        /// <summary>
        /// 最后一次的时间戳
        /// </summary>
        private static long lastTimestamp = -1L;

        /// <summary>
        /// 线程锁对象
        /// </summary>
        private static object locker = new object();

        static SnowFlakeHelper()
        {
            int wId = 0;
            int.TryParse(ConfigurationManage.GetAppSetting("SnowFlakeConfig: WorkId"), out wId);
            if (wId <= 0)
                wId = 1;
            _workerId = wId;
            _twepoch = timeGen(2019, 1, 1, 0, 0, 0);
        }
        /// <summary>
        /// 机器编号
        /// </summary>
        public static long WorkerID
        {
            get { return _workerId; }
            set
            {
                if (value > 0 && value < maxWorkerId)
                    _workerId = value;
                else
                    throw new ApplicationException("Workerid must be greater than 0 or less than " + maxWorkerId);
            }
        }

        /// <summary>
        /// 获取新的ID
        /// </summary>
        /// <returns></returns>
        public static long NewID()
        {
            lock (locker)
            {
                long timestamp = timeGen();
                if (lastTimestamp == timestamp)
                { //同一微妙中生成ID
                    sequence = (sequence + 1) & sequenceMask; //用&运算计算该微秒内产生的计数是否已经到达上限
                    if (sequence == 0)
                    {
                        //一微妙内产生的ID计数已达上限，等待下一微妙
                        timestamp = tillNextMillis(lastTimestamp);
                    }
                }
                else
                { //不同微秒生成ID
                    sequence = 0; //计数清0
                }
                if (timestamp < lastTimestamp)
                {
                    //如果当前时间戳比上一次生成ID时时间戳还小，抛出异常，因为不能保证现在生成的ID之前没有生成过
                    throw new ApplicationException(string.Format("Clock moved backwards.  Refusing to generate id for {0} milliseconds", lastTimestamp - timestamp));
                }
                lastTimestamp = timestamp; //把当前时间戳保存为最后生成ID的时间戳
                return (timestamp - _twepoch << timestampLeftShift) | _workerId << workerIdShift | sequence;
            }
        }

        /// <summary>
        /// 获取下一微秒时间戳
        /// </summary>
        /// <param name="lastTimestamp"></param>
        /// <returns></returns>
        private static long tillNextMillis(long lastTimestamp)
        {
            long timestamp = timeGen();
            while (timestamp <= lastTimestamp)
            {
                timestamp = timeGen();
            }
            return timestamp;
        }

        /// <summary>
        /// 当前时间戳
        /// </summary>
        /// <returns></returns>
        private static long timeGen()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        /// <summary>
        /// 指定时间戳
        /// </summary>
        /// <param name="Time">指定时间</param>
        /// <returns></returns>
        private static long timeGen(int Year, int Month, int Day, int Hour, int Minute, int Second)
        {
            var UtcTime = new DateTime(Year, Month, Day, Hour, Minute, Second, DateTimeKind.Utc);
            return (long)(UtcTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
    }
}
