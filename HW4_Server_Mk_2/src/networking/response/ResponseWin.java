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
public class ResponseWin extends GameResponse {
    private Player player;
    private int test;

    public ResponseWin() {
        responseCode = Constants.SMSG_Win;
    }

    @Override
    public byte[] constructResponseInBytes() {
        GamePacket packet = new GamePacket(responseCode);
        packet.addInt32(player.getID());
        packet.addInt32(test);
        Log.printf("Player with id %d wins", player.getID());
        return packet.getBytes();
    }

    public void setPlayer(Player player) {
        this.player = player;
    }
    public void setData(int test) {
        this.test = test;
    }
}