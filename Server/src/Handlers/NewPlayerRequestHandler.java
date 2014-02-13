/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package Handlers;

import Extension.SnowExtension;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;
import helpers.RoomHelper;
import simulation.CombatPlayer;
import simulation.World;
/**
 *
 * @author Kimreik
 */
public class NewPlayerRequestHandler extends BaseClientRequestHandler{

    @Override
    public void handleClientRequest(User user, ISFSObject params) {
        World world = RoomHelper.getWorld(this);
        world.addPlayer(user);

        for (CombatPlayer player : world.getPlayers()) {
            if (player.getSfsUser().getId() != user.getId()) {
                SnowExtension ext = (SnowExtension) this.getParentExtension();
                ext.clientInstantiatePlayer(player, user);
            }
        }

//        SFSObject answ = SFSObject.newInstance();
//        answ.putUtfString("id", "newPlayer");
//        answ.putUtfString("user",user.getName());
//        send("newPlayer", answ, user.getLastJoinedRoom().getPlayersList());
    }
}

