namespace Fantasy;

public enum LockType
{
    None = 0, 
    AuthenticationRegisterLock = 1,     // 鉴权注册锁
    AuthenticationRemoveLock = 1,       // 鉴权移除锁
    AuthenticationLoginLock = 2         // 鉴权登录锁
}