namespace Fantasy
{
    /// <summary>
    /// 聊天信息节点
    /// </summary>
    public static class ChatNodeFactory
    {
        /// <summary>
        /// 添加文本节点
        /// </summary>
        /// <param name="chatInfoTree"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static ChatInfoTree AddendTextNode(this ChatInfoTree chatInfoTree, string content)
        {
            var chatInfoNode = new ChatInfoNode()
            {
                ChatNodeType = (int)ChatNodeType.Text,
                Content = content
            };
            chatInfoTree.Node.Add(chatInfoNode);
            return chatInfoTree;
        }

        /// <summary>
        /// 添加链接节点
        /// </summary>
        /// <param name="chatInfoTree"></param>
        /// <param name="content"></param>
        /// <param name="link"></param>
        /// <returns></returns>
        public static ChatInfoTree AddendLinkNode(this ChatInfoTree chatInfoTree, string content,string link)
        {
            var chatLinkNode = new ChatLinkNode()
            {
                Link = link
            };
            var serializerComponent = chatInfoTree.Scene.GetComponent<SerializerComponent>();
            var chatInfoNode = new ChatInfoNode()
            {
                ChatNodeType = (int)ChatNodeType.Link,
                ChatNodeEvent = (int)ChatNodeEvent.ClickLink,
                Content = content,
                Data = serializerComponent.Serialize(chatLinkNode)
            };
            chatInfoTree.Node.Add(chatInfoNode);
            return chatInfoTree;
        }

        /// <summary>
        /// 添加图片节点
        /// </summary>
        /// <param name="chatInfoTree"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static ChatInfoTree AddendImageNode(this ChatInfoTree chatInfoTree, string content)
        {
            var chatInfoNode = new ChatInfoNode()
            {
                ChatNodeType = (int)ChatNodeType.Image,
                Content = content
            };
            chatInfoTree.Node.Add(chatInfoNode);
            return chatInfoTree;
        }

        /// <summary>
        /// 添加打开UI节点
        /// </summary>
        /// <param name="chatInfoTree"></param>
        /// <param name="content"></param>
        /// <param name="uiName"></param>
        /// <returns></returns>
        public static ChatInfoTree AddendOpenUINode(this ChatInfoTree chatInfoTree, string content,string uiName)
        {
            var chatOpenUINode = new ChatOpenUINode()
            {
                UIName = uiName
            };
            var serializerComponent = chatInfoTree.Scene.GetComponent<SerializerComponent>();
            var chatInfoNode = new ChatInfoNode()
            {
                ChatNodeType = (int)ChatNodeType.OpenUI,
                ChatNodeEvent = (int)ChatNodeEvent.OpenUI,
                Content = content,
                Data = serializerComponent.Serialize(chatOpenUINode)
            };
            chatInfoTree.Node.Add(chatInfoNode);
            return chatInfoTree;
        }

        /// <summary>
        /// 添加位置节点
        /// </summary>
        /// <param name="chatInfoTree"></param>
        /// <param name="content"></param>
        /// <param name="mapName"></param>
        /// <param name="mapX"></param>
        /// <param name="mapY"></param>
        /// <param name="mapZ"></param>
        /// <returns></returns>
        public static ChatInfoTree AddendPositionNode(this ChatInfoTree chatInfoTree, string content, string mapName,
            float mapX, float mapY, float mapZ)
        {
            var chatPositionNode = new ChatPositionNode()
            {
                MapName = mapName,
                PosX = mapX,
                PosY = mapY,
                PosZ = mapZ,
            };

            var serializerComponent = chatInfoTree.Scene.GetComponent<SerializerComponent>();
            var chatInfoNode = new ChatInfoNode()
            {
                ChatNodeType = (int)ChatNodeType.Position,
                ChatNodeEvent = (int)ChatNodeEvent.Position,
                Content = content,
                Data = serializerComponent.Serialize(chatPositionNode)
            };

            chatInfoTree.Node.Add(chatInfoNode);
            return chatInfoTree;
        }
    }
}