package networking.response;

// Other Imports
import metadata.Constants;
import model.Player;
import utility.GamePacket;
import utility.Log;
/**
 * The ResponseLogin class contains information about the authentication
 * process.
 */
public class ResponseTest extends GameResponse {
    private Player player;
    private int x;


    public ResponseTest() {
        responseCode = Constants.SMSG_Test;
    }

    @Override
    public byte[] constructResponseInBytes() {
        GamePacket packet = new GamePacket(responseCode);
        packet.addInt32(player.getID());
        packet.addInt32(x);


        Log.printf("test", player.getID(), x);
 
        return packet.getBytes();
    }

    public void setPlayer(Player player) {
        this.player = player;
    }

    public void setData(int x) {

        this.x = x;
    }
}