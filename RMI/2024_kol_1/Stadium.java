import java.io.Serializable;

public class Stadium implements Serializable {
  public String name;
  public String city;

  public Stadium(String name, String city) {
    this.name = name;
    this.city = city;
  }
}
