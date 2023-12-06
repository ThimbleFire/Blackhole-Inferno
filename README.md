# Blackhole-Inferno

Blackhole Inferno is a 2D space simulator in which the player can run missions to earn credits and use those credits to buy ships, and modules and furniture to customize ship interior for its crew. Crew eat, drink, go to the bathroom, relax and perform a their dedicated job of enabling the functional behaviour of active, non-passive modules, such as turrets, the targeting system, the warp core.

**__MASTER TODO LIST__**
* [ ] 1.0
  * [x] align
  * [ ] metrics 
    * [ ] warp
    * [ ] orbit
  * [ ] lock targets
    * [ ] engage
  * [x] dock
    * [ ] load station
    * [ ] exchange ships
    * [ ] exchange modules
    * [ ] buy furniture
    * [ ] undock
    * [ ] missions
  * [x] jump
    * [ ] load system
  * [ ] interior ship view
  * [ ] 3D models †
    * [ ] crew management & customisation
      * [ ] crew attributes
    * [ ] module and furniture placement
  * [ ] fully scripted main story
* [ ] 1.1
  * [ ] NPC relationships †
* [ ] 1.2
  * [ ] Dedicated multiplayer space †
* [ ] 1.3
  * [x] Module affixes †

†          Undecided

---

**Short Term Goals**

* Keep player character on world canvas
* Put other objects on overlay canvas
* Transform overlay canvas element's world position to screen position
* Draw a line from the player character directly downwards to get a Z-axis for elements considered behind the camera. this may not be necessary, but it could be a strategy to prevent overlay canvas elements appearing at polar opposites of the system

when warping, the player character should move far away enough for the objects behind them to leave the render distance at which point they can be unloaded. the remainder of the warp should take the player's forward velocity, flip it, and use it to pull objects towards the player. this way the player doesn't have to have a transform position with floating-point values in the of hundreds of millions. it also allows for more accurate world position coordinates for objects.

* At the top left of the screen it should say what faction controls the system you're in.