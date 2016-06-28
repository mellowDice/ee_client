public class EESocketIOComponent : SocketIO.SocketIOComponent {
  public bool useLocalUrl = false;
  public string localUrl = "ws://127.0.0.1:6000/socket.io/?EIO=4&transport=websocket";
  new void Awake() {
    if (useLocalUrl) {
      url = localUrl;
    }
    base.Awake();
  }
}