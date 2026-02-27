using Fantasy;
using Fantasy.Async;
using Fantasy.Entitas;
using Fantasy.Helper;

namespace Hotfix.Login;

public static class LoginComponentSystem
{
    public static bool IsAccountOnline(this GateComponent self, string acct) {
        return self.Accounts.ContainsKey(acct);
    }
    
    internal static async FTask<(uint ErrorCode, long AccountId)> Login(this GateComponent self, string userName, string password)
    {
        // 1、检查传递的参数是否完整
        
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
        {
            // 这个1代表的是参数不完整。
            return (1, 0);
        }
        
        var scene = self.Scene;
        var worldDateBase = scene.World.Database;
        var usernameHashCode = userName.GetHashCode();
        
        using (var @lock = await scene.CoroutineLockComponent.Wait((int)LockType.LoginLock, usernameHashCode))
        {
            Account account = null;
            var loginAccountsKey = userName + password;
            //查看缓存中是否有
            if (self.Accounts.TryGetValue(loginAccountsKey, out var accountCacheInfo))
            {
                account = accountCacheInfo.GetComponent<Account>();
                
                if (account == null)
                {
                    return (2, 0);
                }

                return (0, account.Id);
            }

            uint result = 0;
            account = await worldDateBase.First<Account>(d => d.acct == userName && d.acct == password);

            if (account == null)
            {
                // 这个2代表的是该用户没有注册或者用户或密码错误
                result = 2;
            }
            else
            {
                await worldDateBase.Save(account);
                // 添加Account到缓存中
                account.Deserialize(scene);
                accountCacheInfo.AddComponent(account);
            }
            
            self.Accounts.Add(loginAccountsKey, accountCacheInfo);

            if (result != 0)
            {
                return (result, 0);
            }

            return (0, account.Id);
        }
    }

    /// <summary>
    /// 鉴权注册接口
    /// </summary>
    /// <param name="self"></param>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="source"></param>
    internal static async FTask<uint> Register(this GateComponent self, string username, string password, string source)
    {
        // 1、检查传递的参数是否完整
        
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            // 这个1代表的是参数不完整。
            return 1;
        }
        
        var usernameHashCode = username.GetHashCode();
        var scene = self.Scene;
        
        // 利用协程锁来解决异步的原子问题
        using (var @lock = await scene.CoroutineLockComponent.Wait((int)LockType.RegisterLock, usernameHashCode))
        {
            // 利用缓存来减少频繁请求数据库或缓存的压力。
        
            if (self.Accounts.TryGetValue(username, out var account))
            {
                // 这个2代表的是该用户已经存在。
                return 2;
            }
            
            // 2、数据库查询该账号是否存在
            var worldDateBase = scene.World.Database;
            var isExist = await worldDateBase.Exist<Account>(d => d.Username == username);
            if (isExist)
            {
                // 这个2代表的是该用户已经存在。
                return 2;
            }

            //3、执行到这里的话，表示数据库或缓存没有该账号的注册信息，需要咱们创建一个。
            account = Entity.Create<Account>(scene, true, true);
            account.acct = username;
            account.pwd = password;
            // 写入这个实体到数据中
            await worldDateBase.Save(account);
            var accountId = account.Id;
            // 把当前账号添加到缓存字典中。
            self.Accounts.Add(username, account);
            // 添加AccountTimeOut组件用来定时清除缓存
            // 这个0代表的是操作成功
            Log.Info($"Register source:{source} username:{username} accountId:{accountId}");
            return 0;
        }
    }
}