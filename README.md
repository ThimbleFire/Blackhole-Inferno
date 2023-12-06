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
* keep player character on world canvas. 
* Put other objects on overlay canvas. 
* Translate overlay canvas element's world position to screen position.
* Draw a line from the player character directly downwards to get a Z-axis for elements considered behind the camera. this may not be necessary, but it could be a strategy to prevent overlay canvas elements appearing at polar opposites of the system.

* [ ] adjust warp speeds to ramp up at the start and slow down need the end
* [ ] adjust metrics so:

15k is 1AU
1.5K is 15,000,000 km
150 is 1,500,000 km
15 is 150,000 km
1.5 is 15,000 km
0.15 is 1,500 km
0.015 is 150 km
0.0015 is 15 km
0.00015 is 1.5 meters
0.0001 is 1 meter
0.0300 is 300 meters

Camera view distance would need to be 450K to view the our solar system ()

I'm going to change how distance works.