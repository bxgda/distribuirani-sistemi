import java.rmi.Remote;
import java.rmi.RemoteException;

public interface ICallback extends Remote {
  void obavesti(Poruka poruka) throws RemoteException;
}
