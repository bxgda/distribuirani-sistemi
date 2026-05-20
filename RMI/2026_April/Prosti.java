import java.rmi.RemoteException;
import java.rmi.server.UnicastRemoteObject;
import java.util.ArrayList;

public class Prosti extends UnicastRemoteObject implements IProsti {
  ArrayList<ICallback> callbacks = new ArrayList<>();

  public Prosti() throws RemoteException {
    super();
  }

  @Override
  public void start(int n, int m) throws RemoteException {
    for (int i = n; i <= m; i++) {
      if (isPrime(i)) {
        callCallbacks(i);
      }
    }
  }

  @Override
  public void register(ICallback callback) throws RemoteException {
    callbacks.add(callback);
  }

  @Override
  public void unregister(ICallback callback) throws RemoteException {
    callbacks.remove(callback);
  }

  private void callCallbacks(int value) {
    for (ICallback callback : callbacks) {
      try {
        callback.notify(value);
      } catch (Exception e) {
        e.printStackTrace();
      }
    }
  }

  private boolean isPrime(int n) {
    if (n <= 1)
      return false;

    if (n == 2)
      return true;

    if (n % 2 == 0)
      return false;

    for (int i = 3; i <= Math.sqrt(n); i += 2) {
      if (n % i == 0)
        return false;
    }
    return true;
  }
}
