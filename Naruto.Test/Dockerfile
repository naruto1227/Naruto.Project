# 表示当前创建的镜像所依赖的基础镜像
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim
# 表示当前容器的工作目录 将要运行的web程序放在容器中的那个目录下，没有会自动创建
WORKDIR /app
# 设置对外暴露的端口，不设置默认为 80
EXPOSE 80 443
# 表示会把当前目录下的Web程序文件 拷贝到 容器的 /app 文件下，
COPY  . .
# 将要执行的命令
CMD ["dotnet", "CP.MemSys.dll"]