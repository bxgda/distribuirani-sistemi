import java.rmi.Remote;
import java.rmi.RemoteException;

public interface ICallback extends Remote {
  void resultChanged(int matchId) throws RemoteException;
}
