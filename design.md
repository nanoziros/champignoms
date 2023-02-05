# Design Document
Just a place to put some ideas for the game in a more organized way...

## Main game objects
### Mushroom
  - your main node
  - all of your other nodes are connected to this main node
  - poison travels from mushroom to the selected node

### Node
  - gathers nearby resources
  - gets attacked by nematodes
  - releases poison
  - costs resources to place nodes 
    - (the further from the last one, the more it costs!)

### Nematode
  - the main enemy of the game
  - only spawn at night
  - try to eat the closest nodes
  - can be killed with poison

## Day/Night Cycle
### Day
At the beginning of the day, food will spawn. The day is relatively safe, but be careful to not spread your nodes too thin.
You need to save up resources so that you survive the night.
### Night
During the night, nematodes will start to spawn and there will be no new food sources so be careful!
You can still grow your mushroom to survive and defend, but your resources will be limited.
Also it gets cold at night so you might lose a bit of mushroom mass as time goes by.

## Interactions
### Placing a node
You can place a node by selecting a parent to grow from (by clicking it) and then clicking anywhere else on the screen. 
A strand of hyphae (mushroom roots) will then slowly grow towards that new point.
As it grows, you will slowly lose mushroom mass.
### Using poison
During the night, the nematodes will come. You need to use poison to fight them. 
To send poison, you right click on an existing node and poison will travel there from the main mushroom.
