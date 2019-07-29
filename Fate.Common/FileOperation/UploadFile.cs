using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Fate.Common.Exceptions;
using Fate.Common.Interface;
using Microsoft.Extensions.Options;
using Fate.Common.Options;

namespace Fate.Common.FileOperation
{
    /// <summary>
    /// 文件上传
    /// </summary>
    public class UploadFile : ICommonClassSigleDependency
    {
        /// <summary>
        /// 每次上传最大的buffer大小  1M=1000000
        /// </summary>
        private int BufferLength { get; set; }

        /// <summary>
        /// 构造注入
        /// </summary>
        /// <param name="_options"></param>
        public UploadFile(IOptions<FileUploadOptions> _options)
        {
            BufferLength = _options.Value.BufferLength * 1000000;
        }

        /// <summary>
        /// 处理单独一段本地数据上传至服务器的逻辑
        /// </summary>
        /// <param name="localFilePath">本地需要上传的文件地址</param>
        /// <param name="uploadFilePath">服务器（本地）目标地址</param>
        public async Task UpLoadFileFromLocal(string localFilePath, string uploadFilePath)
        {
            if (!File.Exists(localFilePath))
                throw new MyExceptions("找不到该文件");
            //定义一个FileStream对象
            FileStream fileStream = new FileStream(localFilePath, FileMode.Open);
            await UpLoadFileFromStream(fileStream, uploadFilePath);
            fileStream.Dispose();
            fileStream.Close();
        }

        /// <summary>
        /// 处理一段文件流上传至服务器的逻辑
        /// </summary>
        /// <param name="uploadFIleStream">上传的文件流</param>
        /// <param name="uploadFilePath">服务器（本地）目标地址</param>
        public async Task UpLoadFileFromStream(Stream uploadFIleStream, string uploadFilePath)
        {
            //定义一个缓冲区数组
            byte[] bufferByteArray;
            //获取当前流的长度
            long num = uploadFIleStream.Length;
            //获取循环的次数
            int index = GetCycleNum(num, BufferLength);
            //定义一个读取的总字节数
            int totalNum = 0;
            //定义一个读取的长度
            int count = BufferLength;
            for (int i = 0; i < index; i++)
            {
                //当为最后一次的时候长度为最后的长度
                if (i + 1 == index)
                {
                    count = (int)num - totalNum;
                }
                //定义写入的字节
                bufferByteArray = new byte[count];
                //开始位置
                int startPostion = totalNum;
                //文件的写入位子
                uploadFIleStream.Position = startPostion;
                //写入到bufferByteArray
                int total = await uploadFIleStream.ReadAsync(bufferByteArray, 0, count);
                //写入指定的文件
                await WriteToServer(uploadFilePath, startPostion, bufferByteArray);
                //总长度
                totalNum += total;
            }
        }

        /// <summary>
        /// 分段上传
        /// </summary>
        /// <param name="filePath">文件地址</param>
        /// <param name="startPositon">起始位置</param>
        /// <param name="data">需要写入的数据</param>
        /// <returns></returns>

        private async Task WriteToServer(string filePath, int startPositon, byte[] data)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                //设置写入的开始位置
                fileStream.Position = startPositon;
                //异步写入
                await fileStream.WriteAsync(data, 0, data.Length);
            }
        }


        /// <summary>
        /// 获取需要分批上传的次数
        /// </summary>
        /// <param name="num"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        private int GetCycleNum(long num, int num2)
        {
            int i = (int)num / num2;
            if (num % num2 > 0)
            {
                i++;
            }
            return i;
        }
    }
}
