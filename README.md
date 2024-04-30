2D/3D roguelike set in space.

Todo
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
