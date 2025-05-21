using Fantasy.Helper;
using Fantasy.Platform.Net;

// 获取配置文件，这里我就是用的相对路径来拿了，大家实际的项目可以远程都可以。
var machineConfigText = await FileHelper.GetTextByRelativePath("../../../../../Configs/Config/Json/Server/MachineConfigData.Json");
var processConfigText = await FileHelper.GetTextByRelativePath("../../../../../Configs/Config/Json/Server/ProcessConfigData.Json");
var worldConfigText = await FileHelper.GetTextByRelativePath("../../../../../Configs/Config/Json/Server/WorldConfigData.Json");
var sceneConfigText = await FileHelper.GetTextByRelativePath("../../../../../Configs/Config/Json/Server/SceneConfigData.Json");
// 初始化配置文件
// 如果重复初始化方法会覆盖掉上一次的数据，非常适合热重载时使用
MachineConfigData.Initialize(machineConfigText);
ProcessConfigData.Initialize(processConfigText);
WorldConfigData.Initialize(worldConfigText);
SceneConfigData.Initialize(sceneConfigText);
// 注册日志模块到框架
// 开发者可以自己注册日志系统到框架，只要实现Fantasy.ILog接口就可以。
// 这里用的是NLog日志系统注册到框架中。
Fantasy.Log.Register(new Fantasy.NLog("Server"));
// 初始化框架，添加程序集到框架中
Fantasy.Platform.Net.Entry.Initialize(Fantasy.AssemblyHelper.Assemblies);
// 启动Fantasy.Net
await Fantasy.Platform.Net.Entry.Start();