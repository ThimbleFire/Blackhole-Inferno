3D roguelike set in space.

This game has been through a lot of design changes

![anim](https://github.com/ThimbleFire/Blackhole-Inferno/assets/14812476/754d5bc0-9794-4e4c-ae9f-4514fe9106e9)

![anim](https://github.com/ThimbleFire/Blackhole-Inferno/assets/14812476/cfb2b3eb-be2a-4976-a76f-f25f5d0fc6cb)


Todo
* add a meter to indicate speed of the ship
* add particles to indicate velocity
* add a planet texture to the planet
* when planets and stars are within 2AU they should become visible. The code that handles HUDSticker viewport position should be handled by a script controlled by the model, and that script should have control over the the sticker and the model. 
* add ship stuff
  * add a spaceship
  * make sure it rotates when the ship aligns, at least in the X-axis
  * make it so when you zoom in close enough the roof of the ship fades out
  * add a 3D tilemap grid for the inside of the ship
  * add models for the various facilities
  * add crew and crew behaviour
* load and unload zones
* add warping between universes
* recruit crew
* do missions (story, repeat, side)
* build system rep
* merchant
  * befriend NPC's for insider information
* mine resources
* dock and explore ships, stations, planets and wrecks
* shoot ship turrets and shoot guns everywhere else
* build and buy ship modules and furniture
* negotiate and bribe people
* build a planetside home

Note:
In the tilemap-only version of the game you don't need to move tile cells. Instead, you create 3x3 grid of tilemaps and lock the camera to the center one. When the X and Y position of the camera exceeds the bounds of the inner tilemap, the camera's axis that exceeds its boundary inverts, leaving a seamless transition. A 2D sprite of the ship is left in the center of the screen to simulate its presence. Zooming in reveals the pixels of the ship which morph into the walls of the ship, and its facilities and sims therein are made opaque.
