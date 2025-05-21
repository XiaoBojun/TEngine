using System;
using System.Collections;
using System.Collections.Generic;
using Fantasy;
using Fantasy.Async;
using Fantasy.Entitas;
using Fantasy.Network;
using UnityEngine;
using UnityEngine.UI;

public sealed class EntryComponent : Entity
{
    public Entry Entry;
}

public class Entry : MonoBehaviour
{
    private Scene _scene;
    private Session _session;

    public GameObject LoginPanel;
    public GameObject ChatPanel;
    
    public Text MessageText;
    public InputField PrivateText;
    public InputField UserName;
    public InputField SendMessageText;
    public Button LoginButton;
    public Button ExitButton;
    public Button BroadcastButton;
    public Button ChannelButton;
    public Button PrivateButton;

    public Button SendButton1;
    public Button ChatNodeEventButton;
    void Start()
    {
        LoginPanel.SetActive(true);
        ChatPanel.SetActive(false);
        StartAsync().Coroutine();
        LoginButton.onClick.RemoveAllListeners();
        LoginButton.onClick.AddListener(() => { OnLoginButtonClick().Coroutine();});
        ExitButton.onClick.RemoveAllListeners();
        ExitButton.onClick.AddListener(() => { OnExitButtonClick().Coroutine();});
        SendButton1.onClick.RemoveAllListeners();
        SendButton1.onClick.AddListener(() => { OnSendButton1Click().Coroutine();});
        BroadcastButton.onClick.RemoveAllListeners();
        BroadcastButton.onClick.AddListener(() => { OnBroadcastButtonClick().Coroutine();});
        ChannelButton.onClick.RemoveAllListeners();
        ChannelButton.onClick.AddListener(() => { OnChannelButtonClick().Coroutine();});
        PrivateButton.onClick.RemoveAllListeners();
        PrivateButton.onClick.AddListener(() => { OnPrivateButtonClick().Coroutine();});
    }

    private async FTask StartAsync()
    {
        Fantasy.Platform.Unity.Entry.Initialize(GetType().Assembly);
        _scene = await Scene.Create(SceneRuntimeType.MainThread);
        _scene.AddComponent<EntryComponent>().Entry = this;
        _scene.AddComponent<SerializerComponent>().Initialize();
    }

    private async FTask OnLoginButtonClick()
    {
        // 创建一个连接，这里是连接到目标的Gate服务器
        _session = _scene.Connect(
            "127.0.0.1:20000",
            NetworkProtocolType.KCP,
            () =>
            {
                Log.Debug("连接成功！");
                _session.AddComponent<SessionHeartbeatComponent>().Start(2000);
            },
            () =>
            {
                Log.Debug("连接失败！");
            },
            () =>
            {
                Log.Debug("断开连接！");
            },
            false, 5000);
        // 发送登录请求
        var response = (G2C_LoginResponse)await _session.Call(new C2G_LoginRequest()
        {
            UserName = UserName.text
        });
        // 查看错误码
        if (response.ErrorCode != 0)
        {
            Log.Error($"登录错误 ErrorCode:{response.ErrorCode}");
            return;
        }
        LoginPanel.SetActive(false);
        ChatPanel.SetActive(true);
        Log.Debug("登录成功！");
    }
    
    private async FTask OnExitButtonClick()
    {
        var response = (G2C_ExitResponse)await _session.Call(new C2G_ExitRequest());
        if (response.ErrorCode != 0)
        {
            Log.Error($"退出游戏错误 ErrorCode:{response.ErrorCode}");
            return;
        }
        LoginPanel.SetActive(true);
        ChatPanel.SetActive(false);
        Log.Debug("退出游戏成功！");
    }

    private async FTask OnSendButton1Click()
    {
        await FTask.CompletedTask;
        // SendButton1.interactable = false;
        // var response = (Chat2C_SendMessageResponse) await _session.Call(new C2Chat_SendMessageRequest());
        // if (response.ErrorCode != 0)
        // {
        //     Log.Error($"发送聊天消息失败 ErrorCode:{response.ErrorCode}");
        //     return;
        // }
        // Log.Debug("发送聊天消息成功！");
        // SendButton1.interactable = true;
    }

    private async FTask OnBroadcastButtonClick()
    {
        BroadcastButton.interactable = false;
        var tree = ChatTreeFactory.Broadcast(_scene);
        tree = tree.AddendPositionNode(SendMessageText.text, "勇者大陆", 121, 131, 111);
        
        var response = (Chat2C_SendMessageResponse)await _session.Call(new C2Chat_SendMessageRequest()
        {
            ChatInfoTree = tree
        });
        if (response.ErrorCode != 0)
        {
            Log.Error($"发送聊天消息失败 ErrorCode:{response.ErrorCode}");
        }

        BroadcastButton.interactable = true;
    }

    private async FTask OnChannelButtonClick()
    {
        ChannelButton.interactable = false;
        var tree = ChatTreeFactory.Team(_scene);
        tree.ChatChannelId = 1;
        // tree = tree.AddendTextNode("你好，欢迎来到Fantasy Chat！").AddendLinkNode("点击这里http://www.fantasy.com.cn");
        var response = (Chat2C_SendMessageResponse)await _session.Call(new C2Chat_SendMessageRequest()
        {
            ChatInfoTree = tree
        });
        if (response.ErrorCode != 0)
        {
            Log.Error($"发送频道聊天消息失败 ErrorCode:{response.ErrorCode}");
        }

        ChannelButton.interactable = true;
    }

    private async FTask OnPrivateButtonClick()
    {
        PrivateButton.interactable = false;
        var tree = ChatTreeFactory.Private(_scene);
        tree.Target.Add(Convert.ToInt64(PrivateText.text));
        // tree = tree.AddendTextNode("你好，欢迎来到Fantasy Chat！").AddendLinkNode("点击这里http://www.fantasy.com.cn");

        var response = (Chat2C_SendMessageResponse)await _session.Call(new C2Chat_SendMessageRequest()
        {
            ChatInfoTree = tree
        });
        if (response.ErrorCode != 0)
        {
            Log.Error($"发送私聊消息失败 ErrorCode:{response.ErrorCode}");
        }
        
        PrivateButton.interactable = true;
    }


}
