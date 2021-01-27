An ability is an equippable skill that completes a very specific action. Abilities can range in types from healing to damage-dealing.

#### Learning Abilities
Abilities can be learned at the concept Perfuser NPC, who will teach you new abilities in exchange for Souls. The Perfuser will determine the ability to teach you through multiple equations.

#### Tier Range

The first equation determines the total number of tiers that the perfuser's range will clamp to:

![Tier range](http://i.imgur.com/5svTRW9.png)

- where `T = the total number of tiers.`
- where `C = a constant in the range (c, 1].`

Right now `C = 0.25.`

#### Tier Probability Distribution

The distribution of probabilities for each tier in the tier range is determined by the following distribution equation:

![tier selection distribution](http://i.imgur.com/LFH5bns.png)

- where ![](http://i.imgur.com/TjxRdZl.png)`= the probability of each tier.`
- where `t = a number positioned relative to the player's power level, representing a tier in the range [0, n(T)].`
- where `D = a constant from 0 to 1 which determines the steepness of the distribution.`
- where `T = the total number of tiers.`

The right side of ![TSD function](http://i.imgur.com/8g6KM02.png) is to ensure that ![TSD summative](http://i.imgur.com/FZ2vaiB.png) has less than a `1%` deviation from a value of `1.0.` The majority of the time, however, the left side of ![TSD function](http://i.imgur.com/8g6KM02.png) will already be within that deviation.

#### Tier Adjustments

Finally, the relative tier to adjust the results of the above equation is determined by:

![tier adjustment](http://i.imgur.com/tp2tscw.png)

- where `m = the tier to adjust by.`
- where `T = the total number of tiers.`
- where `ρ = the player's power level.`

The range that the Perfuser will clamp tier-wise to looks like this: 

![perfuser range](http://i.imgur.com/zZ1diFS.png)

Once a tier is no longer in that range, the player can no longer obtain or roll for new abilities in that tier.

#### Ability Distribution in Tiers
Ability collections, which are called Tiers, are distributed similarly to rarities. That distribution is dictated by the following equation:

![Ability distribution](http://i.imgur.com/sc0hTep.png)

- where `D = a constant between 0 and 1 which changes the overall steepness of distribution of tiers.`
- where `t = a value between 0 and infinity which represents the number of tiers required.`
- where `n = the total number of registered tiered abilities.`
- where `a = the index of an ability, ranging from 0 to n`
- where `T = the determined tier for an ability a.`

Right now, `D = 0.99.` The value `t` is dictated by the following equation:

![tier creation](http://i.imgur.com/AoCH52L.png)

- where `n = the total number of registered tiered abilities.`
- where `C = a constant with the units tiers/ability.`

Right now, `C = 3/100.` (3 tiers per 100 abilities, or 0.03 tiers per 1 ability).

Players can only obtain a certain amount of some tier X abilities before being cut off from that tier. 

#### Abilities in Mods and Content Packs
For mod creators, having an automated tier system might be a little strange. To ensure that abilities are distributed properly, we have an order-based registration system.

If you're a mod creator and you have multiple abilities, you *must* register your abilities in the order that you want them to be tiered. Here's an example:

- Total number of tiers is 3 (`t = 3`).
- The number of abilities that are built in to the game is 6 (`n0 = 6`)
- You, the modder, have 3 abilities you want to register and they all have different levels of power. You want ability 1 to be in tier 1, ability 2 to be in tier 2, and ability 3 to be tier 3. (`n1 = 3`)
- The total number of abilities becomes 9. (`n = n0 + n1 = 9`)

In order for this distribution to work properly, you'll want ability 1 to be registered 1st, ability 2 to be registered 2nd, and ability 3 to be registered 3rd:

![ability registration](http://i.imgur.com/lzumMjV.png)

Here is what the distribution of tiers will look like afterwards:

![tier dist](http://i.imgur.com/6gQdbFG.png)

If you mix up the order of your abilities when you register them, then they simply will not go in the tiers that you want.

However, in a real case of 9 abilities, the distribution will look more like this:

![real case 9 tiers](http://i.imgur.com/gaJj0Rs.png)

because `t(9)` will return a different value than `t = 3.`

Keep in mind that, in reality, the total number of abilities will be large. Upwards of 100 or more. So a real distribution will look more akin to this:

![real tier distribution](http://i.imgur.com/z0iMStz.png)
