import java.rmi.Naming;
import java.rmi.RemoteException;
import java.rmi.server.UnicastRemoteObject;

public class ClientUser {

  private static IFootballScore fs;
  private ICallback cb;
  private final int subscribedMatchId = 1;

  public ClientUser() {

    try {
      fs = (IFootballScore) Naming.lookup("rmi://localhost:4096/Server");

      System.out.print(fs.getMatches());

      Match match2 = fs.getMatch(2);
      Stadium stadium = match2.getStadium();
      System.out.println("Match 2 stadium: " + stadium.name + ", " + stadium.city);

      cb = new Callback();
      fs.register(subscribedMatchId, cb);
    } catch (Exception e) {
      e.printStackTrace();
    }

  }

  public static void main(String args[]) {
    new ClientUser();

    try {
      System.in.read();
    } catch (Exception e) {
      e.printStackTrace();
    }
  }

  public class Callback extends UnicastRemoteObject implements ICallback {

    protected Callback() throws RemoteException {
      super();
    }

    @Override
    public void resultChanged(int matchId) throws RemoteException {
      if (matchId != subscribedMatchId) {
        return;
      }

      Match match = fs.getMatch(matchId);
      System.out.println("Match has changed: " + match.getMatchIdentifier());
    }
  }
}
