import java.rmi.Remote;
import java.rmi.RemoteException;

public interface IProsti extends Remote {
  void start(int n, int m) throws RemoteException;

  void register(ICallback callback) throws RemoteException;

  void unregister(ICallback callback) throws RemoteException;
}
