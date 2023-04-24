package networking.request;

// Java Imports
import java.io.IOException;

// Other Imports
import model.Player;
import networking.response.ResponseColorChange;
import utility.DataReader;
import core.NetworkManager;

public class RequestColorChange extends GameRequest {
    private int pieceIndex;
    // Responses
    private ResponseColorChange responseColorChange;

    public RequestColorChange() {
        responses.add(responseColorChange = new ResponseColorChange());
    }

    @Override
    public void parse() throws IOException {
        pieceIndex = DataReader.readInt(dataInput);
    }

    @Override
    public void doBusiness() throws Exception {
        Player player = client.getPlayer();

        responseColorChange.setPlayer(player);
        responseColorChange.setData(pieceIndex);
        NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responseColorChange);
    }
}