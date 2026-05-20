import java.io.Serializable;

public class Poruka implements Serializable {
  public String naslov;
  public String sadrzaj;

  public Poruka(String naslov, String sadrzaj) {
    this.naslov = naslov;
    this.sadrzaj = sadrzaj;
  }
}
