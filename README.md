# Blackhole-Inferno

Blackhole Inferno is a 2D sims-like game set in space. Your crew perform dedicated jobs onboard that enable ship functionality. They also must eat, drink, sleep and use the lavatory. Eating and drinking uses onboard perishables which must be purchased on a planet or station.

Crew will behave autonomously, but can be instructed to perform tasks in advance so they don't fall asleep or die of hunger in the middle of a space fight.

Like a sims-like, the interior of the crew's home, the ship, can be customised by adding walls, flooring, furniture and ship modules in mounting points.

Different ships have different shape interiors and sizes of mounting points. Modules such as turrets can have prefixes, suffixes and implicit enhancements.

Crew can be assigned jobs and each job has an array of associated modules they may need to interact with given player commands.

Given all of these gameplay elements, it's anticipated that Blackhole Inferno will be a very challenging game.

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

* Fix a bug where asteroids are named after the belt they belong to
* Fix tooltip distance text
* Fix warp speeds 
* Add system name and faction dominion at the top-left of the screen
* Add code for locking targets
* Add code for unloading systems during runtime
* Add code for   loading systems during runtime
* Think of a UI design for the inventory
* When the player swaps to internal view, the ships hull scales up in size to max, then fades away revealing the interior
