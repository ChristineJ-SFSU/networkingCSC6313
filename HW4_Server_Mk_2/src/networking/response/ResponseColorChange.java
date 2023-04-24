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
public class ResponseColorChange extends GameResponse {
    private Player player;
    private int index;

    public ResponseColorChange() {
        responseCode = Constants.SMSG_ColorChange;
    }

    @Override
    public byte[] constructResponseInBytes() {
        GamePacket packet = new GamePacket(responseCode);
        packet.addInt32(player.getID());
        packet.addInt32(index);

        Log.printf("Player with id %d changed %d)", player.getID(), index);
 
        return packet.getBytes();
    }

    public void setPlayer(Player player) {
        this.player = player;
    }

    public void setData(int index) {
        this.index = index;
    }
}