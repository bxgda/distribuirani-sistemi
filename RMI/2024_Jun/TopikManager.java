import java.rmi.RemoteException;
import java.rmi.server.UnicastRemoteObject;
import java.util.ArrayList;
import java.util.HashMap;

public class TopikManager extends UnicastRemoteObject implements ITopikManager {
  private HashMap<String, ArrayList<ICallback>> subscribers;

  public TopikManager() throws Exception {
    super();
    subscribers = new HashMap<>();
  }

  private void createTopik(String topikName) {
    if (!subscribers.containsKey(topikName)) {
      subscribers.put(topikName, new ArrayList<>());
    }
  }

  @Override
  public void subscribe(String topikName, ICallback callback) throws RemoteException {
    createTopik(topikName);
    subscribers.get(topikName).add(callback);
  }

  @Override
  public void unsubscribe(String topikName, ICallback callback) throws RemoteException {
    if (subscribers.containsKey(topikName)) {
      subscribers.get(topikName).remove(callback);
    }
  }

  @Override
  public void publish(String topikName, Poruka poruka) throws RemoteException {
    if (subscribers.containsKey(topikName)) {
      for (ICallback callback : subscribers.get(topikName)) {
        callback.obavesti(poruka);
      }
    }
  }
}
