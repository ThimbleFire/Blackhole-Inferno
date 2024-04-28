2D roguelike set in space.

Todo
* rework space movement
  * Keep the player ship gameobject position at { 0, 0 } and instead move it's world space position. 
    calculate tooltip distances according to world space position instead of ship gameobject position. 
    get the angle of HUD icons from their world space position in relation to the ship world space position, and render them at that angle at a distance of 1,000 away from the camera.
    when the distance between them and the ship is within 1,000 create an object (like a planet or station) and scale it up and down according to distance.
    move that object with its respective hud icons space position

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
