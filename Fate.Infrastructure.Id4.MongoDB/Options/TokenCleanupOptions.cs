namespace Fate.Infrastructure.Id4.MongoDB.Options
{
    public class TokenCleanupOptions
    {
        /// <summary>
        /// 验证是否开启定时器 ，执行清理过期的授权 信息
        /// </summary>
        public bool EnableTokenCleanup { get; set; } = false;

        /// <summary>
        /// 定时器执行的间隔时间  默认1小时
        /// </summary>
        public int TokenCleanupInterval { get; set; } = 3600;

        /// <summary>
        /// 每次清理的条数. 默认100
        /// （当IOperationalStoreNotification接口有实现的时候此字段才有用）
        /// </summary>
        public int TokenCleanupBatchSize { get; set; } = 100;
    }
}