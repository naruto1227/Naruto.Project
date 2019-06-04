using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
namespace Fate.Common.FileOperation
{
    /// <summary>
    /// 文件上传
    /// </summary>
    public class UploadFile
    {
        /// <summary>
        /// 每次上传最大的buffer大小
        /// </summary>
        private const int BUFFER_COUNT = 10000;

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
                //关闭资源
                fileStream.Close();
            }
        }
        /// <summary>
        /// 处理单独一段本地数据上传至服务器的逻辑，根据客户端传入的startPostion
        /// 和totalCount来处理相应段的数据上传至服务器（本地）
        /// </summary>
        /// <param name="localFilePath">本地需要上传的文件地址</param>
        /// <param name="uploadFilePath">服务器（本地）目标地址</param>
        /// <param name="startPostion">该段起始位置</param>
        /// <param name="totalCount">该段最大数据量</param>
        public async Task UpLoadFileFromLocal(string localFilePath, string uploadFilePath, int startPostion, int totalCount)
        {
            //每次临时读取数据数
            int tempReadCount = 0;
            int tempBuffer = 0;
            //定义一个缓冲区数组
            byte[] bufferByteArray = new byte[BUFFER_COUNT];

            //定义一个FileStream对象
            using (FileStream fileStream = new FileStream(localFilePath, FileMode.Open))
            {
                //将流的位置设置在每段数据的初始位置
                fileStream.Position = startPostion;
                //循环将该段数据读出在写入服务器中
                while (tempReadCount < totalCount)
                {
                    tempBuffer = BUFFER_COUNT;
                    //每段起始位置+每次循环读取数据的长度
                    var writeStartPosition = startPostion + tempReadCount;
                    //当缓冲区的数据加上临时读取数大于该段数据量时，
                    //则设置缓冲区的数据为totalCount-tempReadCount 这一段的数据
                    if (tempBuffer + tempReadCount > totalCount)
                    {
                        //缓冲区的数据为totalCount-tempReadCount 
                        tempBuffer = totalCount - tempReadCount;
                        //读取该段数据放入bufferByteArray数组中
                        fileStream.Read(bufferByteArray, 0, tempBuffer);
                        if (tempBuffer > 0)
                        {
                            byte[] newTempBtArray = new byte[tempBuffer];
                            Array.Copy(bufferByteArray, 0, newTempBtArray, 0, tempBuffer);
                            //将缓冲区的数据上传至服务器
                            await WriteToServer(uploadFilePath, writeStartPosition, newTempBtArray);
                        }
                    }
                    //如果缓冲区的数据量小于该段数据量，并且tempBuffer=设定BUFFER_COUNT时，通过
                    //while 循环每次读取一样的buffer值的数据写入服务器中，直到将该段数据全部处理完毕
                    else if (tempBuffer == BUFFER_COUNT)
                    {
                        await fileStream.ReadAsync(bufferByteArray, 0, tempBuffer);
                        await WriteToServer(uploadFilePath, writeStartPosition, bufferByteArray);
                    }
                    //通过每次的缓冲区数据，累计增加临时读取数
                    tempReadCount += tempBuffer;
                }
            }
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
            int index = GetCycleNum(num, BUFFER_COUNT);
            //定义一个读取的总字节数
            int totalNum = 0;
            //定义一个读取的长度
            int count = BUFFER_COUNT;
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
                uploadFIleStream.Position = startPostion;
                //写入到bufferByteArray
                int total = await uploadFIleStream.ReadAsync(bufferByteArray, 0, count);
                //写入指定的文件
                await WriteToServer(uploadFilePath, startPostion, bufferByteArray);

                totalNum += total;
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
