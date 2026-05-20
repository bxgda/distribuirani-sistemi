import java.rmi.Remote;
import java.rmi.RemoteException;

public interface ITopikManager extends Remote {
  void subscribe(String topikName, ICallback callback) throws RemoteException;

  void unsubscribe(String topikName, ICallback callback) throws RemoteException;

  void publish(String topikName, Poruka poruka) throws RemoteException;
}
