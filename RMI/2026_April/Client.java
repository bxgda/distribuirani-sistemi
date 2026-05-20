import java.rmi.Naming;
import java.rmi.RemoteException;
import java.rmi.server.UnicastRemoteObject;
import java.util.Scanner;

public class Client {
  private static Scanner sc = new Scanner(System.in);
  private static IProsti service;

  public Client() throws Exception {
    service = (IProsti) Naming.lookup("rmi://localhost:4096/Server");
    ICallback callback = new Callback();
    service.register(callback);

    while (true) {
      System.out.println("Enter n and m (n <= m, or 0 to exit): ");
      int n = Integer.parseInt(sc.next());

      // jebe nesto i ovde glup sam
      if (n == 0) {
        service.unregister(callback);
        break;
      }

      int m = Integer.parseInt(sc.next());

      service.start(n, m);
    }

    sc.close();
  }

  public static void main(String[] args) throws Exception {
    new Client();
  }

  private static class Callback extends UnicastRemoteObject implements ICallback {
    protected Callback() throws RemoteException {
      super();
    }

    @Override
    public void notify(int value) throws RemoteException {
      System.out.println("Prime: " + value);
    }
  }
}
