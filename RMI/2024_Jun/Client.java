import java.rmi.Naming;
import java.rmi.RemoteException;
import java.rmi.server.UnicastRemoteObject;
import java.util.Scanner;

public class Client {

  private static ITopikManager tm;
  private Scanner scanner = new Scanner(System.in);

  public Client() {

    try {
      tm = (ITopikManager) Naming.lookup("rmi://localhost:4096/Server");

      System.out.println("Upisi naziv topika na koji zelis da se pretplatis:");
      String topikName = scanner.nextLine();
      tm.subscribe(topikName, new Callback());

      System.out.println("Upisi naziv topika na koji zelis da objavis poruku:");
      String publishTopik = scanner.nextLine();

      while (true) {
        System.out.println("Upisi naslov poruke(0 za kraj):");
        String naslov = scanner.nextLine();
        // nesto jebe ova nula a i meni se jebe nesto sto ne radi
        if (naslov.equals("0")) {
          break;
        }
        System.out.println("Upisi sadrzaj poruke:");
        String sadrzaj = scanner.nextLine();

        Poruka poruka = new Poruka(naslov, sadrzaj);
        tm.publish(publishTopik, poruka);
      }

    } catch (Exception e) {
      e.printStackTrace();
    }

  }

  public static void main(String args[]) {
    new Client();
  }

  public class Callback extends UnicastRemoteObject implements ICallback {
    public Callback() throws Exception {
      super();
    }

    @Override
    public void obavesti(Poruka poruka) throws RemoteException {
      System.out.println("-----------------------------");
      System.out.println("Primljena poruka:");
      System.out.println("Naslov: " + poruka.naslov);
      System.out.println("Sadrzaj: " + poruka.sadrzaj);
      System.out.println("-----------------------------");
    }
  }
}
