using Mirror;
using Player;
using UnityEngine;

namespace Network.Lobby
{
    [AddComponentMenu("")]
    public class NetworkRoomPlayerExt : NetworkRoomPlayer
    {
        private static int currentColorIndex;
        private static readonly ColorPlayerEnum[] colors =
            {ColorPlayerEnum.Violet, ColorPlayerEnum.Green, ColorPlayerEnum.Red, ColorPlayerEnum.Sky};
        
        [SyncVar(hook = nameof(HandleSteamName))]
        public string steamName;
        [SyncVar(hook = nameof(HandleColor))]
        public ColorPlayerEnum PlayerColor;
        
        public override void OnStartClient()
        {
            //Debug.Log($"OnStartClient {gameObject}");
        }

        public override void OnClientEnterRoom()
        {
            //Debug.Log($"OnClientEnterRoom {SceneManager.GetActiveScene().path}");
        }

        public override void OnClientExitRoom()
        {
            //Debug.Log($"OnClientExitRoom {SceneManager.GetActiveScene().path}");
        }

        public override void IndexChanged(int oldIndex, int newIndex)
        {
            //Debug.Log($"IndexChanged {newIndex}");
        }

        public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
        {
            //Debug.Log($"ReadyStateChanged {newReadyState}");
        }
        

        private void HandleSteamName(string _, string steamName) => this.steamName = steamName;
        public void HandleColor(ColorPlayerEnum _, ColorPlayerEnum newColor) => PlayerColor = newColor;
        public void SetNewColor() => PlayerColor = colors[currentColorIndex++];

        public override void OnGUI()
        {
            NetworkRoomManager room = NetworkManager.singleton as NetworkRoomManager;
            if (!room) return;
            if (!room.showRoomGUI)
                return;

            if (!Utils.IsSceneActive(room.RoomScene))
                return;
                
            DrawPlayerInfo();
            DrawPlayerReadyButton();

        }
        
        private GUIStyle InitStyles()
        {
            var currentStyle = new GUIStyle( GUI.skin.box );
            currentStyle.normal.background = MakeTex( 2, 2, new Color( 0f, 0f, 0f, 0.5f ) );
            currentStyle.normal.textColor = new Color(1f, 1f, 1f);
            return currentStyle;
        }
        
        private Texture2D MakeTex( int width, int height, Color col )
        {
            Color[] pix = new Color[width * height];
            for( int i = 0; i < pix.Length; ++i )
            {
                pix[ i ] = col;
            }
            Texture2D result = new Texture2D( width, height );
            result.SetPixels( pix );
            result.Apply();
            return result;
        }
        
        
        private void DrawPlayerReadyButton()
        {
            if (!NetworkClient.active || !isLocalPlayer) return;
            GUILayout.BeginArea(new Rect(20f, 300f, 120f, 20f));
            if (readyToBegin)
            {
                if (GUILayout.Button("Не готов"))
                    CmdChangeReadyState(false);
            }
            else
            {
                if (GUILayout.Button("Готов"))
                    CmdChangeReadyState(true);
            }

            GUILayout.EndArea();
        }
        
        void DrawPlayerInfo()
        {
            var currentStyle=InitStyles();
            GUILayout.BeginArea(new Rect(20f + (index * 100), 200f, 100f, 50f));

            GUILayout.Label(steamName, currentStyle);
            
            GUILayout.EndArea();
            
            GUILayout.BeginArea(new Rect(20f + (index * 100), 250f, 100f, 50f));

            if (readyToBegin)
                GUILayout.Label("Готов", currentStyle);
            else
                GUILayout.Label("Не готов", currentStyle);

            if (((isServer && index > 0) || isServerOnly) && GUILayout.Button("Удалить", currentStyle))
            {
                GetComponent<NetworkIdentity>().connectionToClient.Disconnect();
            }

            GUILayout.EndArea();
        }
    }
}
