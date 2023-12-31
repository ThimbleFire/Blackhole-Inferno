# Blackhole-Inferno

Blackhole Inferno is a 2D sims-like game set in space. Your crew perform dedicated jobs onboard that enable ship equipment. Crew must also eat, drink, sleep and use the lavatory. Eating and drinking uses onboard perishables which must be purchased on a planet or station. Crew will behave autonomously but can be instructed to perform tasks in advance so they don't fall asleep or die of hunger in the middle of a space fight.

Like a sims-like, the interior of the crew's home, the ship, can be customised by adding walls, flooring, furniture and ship modules in mounting points. Different ships have different shape interiors and different sizes of mounting points. Modules such as turrets can have prefixes, suffixes and implicit enhancements.

**__MASTER TODO LIST__**
* [ ] 1.0
  * [x] align
  * [x] metrics 
    * [x] warp
    * [x] orbit
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

* **FIX** nothing is broken

* **ADD** When the player attempts to lock a target, instantiate the lock visual UI element. The instantiated GameObject will have a script that counts down. Once the countdown is complete, the visual element will swap and the target can be engaged. If the visual elements distance from the camera exceeds 1,000 or the target is recognised as NULL (destroyed) in its Late/Update method, the lock GameObject destroy itself and fire an event telling the ship to remove it from its list of locks. 
* **ADD** system name and faction dominion at the top-left of the screen
* **ADD** code for unloading systems during runtime
* **ADD** code for   loading systems during runtime

* **DESIGN** Visual appearance for what locked ships will look like in-game
* **DESIGN** Think of a UI design for the inventory
* **DESIGN** When the player swaps to internal view, the ships hull scales up in size to max, then fades away revealing the interior
* **DESIGN** tooltip should check to see whether the ui element being moused over belongs to the pocket of space the character is in, and use that to determine distance
