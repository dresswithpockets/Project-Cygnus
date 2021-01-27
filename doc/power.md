Power is a stat that all players, NPCs and items possess. Power scales tangentially to the ability level of the player and defines what items the player can equip or use. Power also describes what power of items NPCs can drop, as well as the stats they spawn with.

Power is a numeral representation of how much power the player, NPC or item has.

#### Player Power
For players, their power is redetermined every time their level increases via the following equation:

![player power equation](http://i.imgur.com/jSeHod4.png)

- Where `p = the power of the player`
- Where `A = the amplitude, or the power which the player's level must equal infinity in order to reach.`
- Where `l = the level of the player`
- Where `C = the shift constant, which changes the velocity of power in regards to player level.`

Right now, `A = 100` and `C = 1/40`

#### NPC Power
In NPC generation, an NPC's power is determined by the sub region they were spawned in. Their power is determined upon spawn like so:

![NPC power equation](http://i.imgur.com/pscc1di.png)

- Where `p = the power of the NPC`
- Where `b = the power of the Area-Boss`
- Where `x = distance between the NPC and the Area-Boss' spawn`
- Where `r = furthest perpendicular radial measure from the Area-Boss' spawn to an edge in the Area`
- Where `C = range of variability constant`

Right now, `C = 0.10` which produces results of +/- 10% in power of NPCs.

#### Item Power
In item generation, an Item's power is determined based off of the NPC that dropped it, player that crafted/smelted it, or area that is spawned in. Rarity is now independent of power and power is independent of rarity.

### Equipping and Using Items?
In order for players or NPCs to equip an item or use an item, the item's power must be equal to or less than the player or NPCs.
