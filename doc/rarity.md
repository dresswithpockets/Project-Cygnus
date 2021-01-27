Rarity is a value that identifies the quality of the item and the difficulty of obtaining the item.

An Item's rarity is based off of the NPC that dropped it, the player that crafted/smelted it, or the area that it spawned in.

#### Dropped by NPC
_If dropped by NPC, then the rarity has an exponentially distributed chance of being a rarity equal to or less than the NPC's augment power. The probability for a rarity `x` is determined via:_

![item rarity from NPC drop](http://i.imgur.com/LPpZRBB.png)

- Where `D = a constant between 0 and 1 which changes the overall steepness of distribution of probability`.
- Where `a = the augment power of the NPC`.
- Where `x = a rarity ranging from 0 to a; the rarity to get the probability of`.
- Where `f = the determined probability of the item receiving a rarity x`.

#### Crafted or Smelted by Player
_If crafted or smelted, then the rarity will be equal to the rarity of the recipe used._

#### Spawned in World
_If spawned in a dungeon chest, there is some logic involved. TBD_
