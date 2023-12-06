# Blackhole-Inferno

**__MASTER TODO LIST__**
* [ ] 1.0
  * [x] align
  * [ ] metrics 
    * [x] warp
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

when warping, the player character should move far away enough for the objects behind us to go outside the render distance, at which point they can be unloaded. the remainder of the warp should take the velocity of the player and instead use it to pull other objects in the opposite direction. this way the player's transform position remains comfortably close to zero.