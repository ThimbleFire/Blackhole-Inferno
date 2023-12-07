# Blackhole-Inferno

Blackhole Inferno is a 2D sims-like game set in space. Your crew perform dedicated jobs onboard that enable ship functionality. They also must eat, drink, sleep and use the lavatory. Eating and drinking uses onboard perishables which must be purchased on a planet or station.

Crew will behave autonomously, but can be instructed to perform tasks in advance so they don't fall asleep or die of hunger in the middle of a space fight.

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

* stop objects rendering infront when their world position is behind the camera

when warping, the player character should move far away enough for the objects behind them to leave the render distance at which point they can be unloaded. the remainder of the warp should take the player's forward velocity, flip it, and use it to pull objects towards the player. this way the player doesn't have to have a transform position with floating-point values in the of hundreds of millions. it also allows for more accurate world position coordinates for objects.

* At the top left of the screen it should say what faction controls the system you're in.
