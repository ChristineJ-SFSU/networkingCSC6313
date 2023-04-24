package networking.request;

// Java Imports
import java.io.IOException;

// Other Imports
import model.Player;
import networking.response.ResponseTest;
import utility.DataReader;
import core.NetworkManager;

public class RequestTest extends GameRequest {
    private int x;
    // Responses
    private ResponseTest responseTest;

    public RequestTest() {
        responses.add(responseTest = new ResponseTest());
    }

    @Override
    public void parse() throws IOException {
        x = DataReader.readInt(dataInput); 
    }

    @Override
    public void doBusiness() throws Exception {
        Player player = client.getPlayer();

        responseTest.setPlayer(player);
        responseTest.setData( x);
        NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responseTest);
    }
}