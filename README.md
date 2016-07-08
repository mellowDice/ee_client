# ShatterLand

Free iOS VR game where marble players compete with each other for dominance.
![ShatterLand Game Screenshot](https://github.com/mellowDice/ee_diagrams/blob/master/ScreenShot1.png?raw=true)

## Game Play

 * Larger players bust smaller players by running into them
 * Players grow by eating fragments scattered on the field
 * Obstacles (large purple spike balls) shoot players into the sky and reduce mass for larger players


## Game Architecture
* Client: Unity3D/C#  ([github.com/mellowDice/ee_client](https://github.com/mellowDice/ee_client))
* Primary Server and Movement Service: Python/Flask ([github.com/mellowDice/ee_server](https://github.com/mellowDice/ee_server))
* Landscape Service: Python/Numpy/Flask ([github.com/mellowDice/landscape-service](https://github.com/mellowDice/landscape-service))
* Field Objects Service: Python/Flask ([github.com/mellowDice/field-objects-service](https://github.com/mellowDice/field-objects-service))
* Data Service: NodeJS/Redis/Express ([github.com/mellowDice/data_service](https://github.com/mellowDice/data_service))
* Web Service: HTML/CSS ([github.com/mellowDice/web_service](https://github.com/mellowDice/web_service))

![Architecture Diagram](https://github.com/mellowDice/ee_diagrams/blob/master/Architecture%20Diagram.jpg?raw=true)


## Client
The iOS client is the user experience. It is developed on the Unity3D platform using C#.

Unity3D uses a hierarchy of objects, each of which have C# scripts attached to dictate behavior. These scripts have the ability to generate additional (non-hierarchy objects) using other pre-built objects called Prefabs.

### Hierarchy
The following is the default hierarchy for the scene:


* Game Attributes: Settings related to the entire app performance
* Network: All the network controllers and their settings
* Player: The main player (built from the player prefab)
* Physical Setting: Bounding walls and lighting
* VR Mode: Cameras and setup related to viewing in VR
* Non-VR Mode: Camera and setup related to viewing in single screen
* PlayerEatenParticles: Particle effect emitted upon player collision
* Obstacles: Contains all mines, which throw players into the sky
* Food: Fragments food can eat to grow
* ProcPlane: Instantiated upon landscape load


### Source Code Files

* CameraDirection: Points camera in direction user is facing
* CameraOrbit: Maintains camera in position above and behind player
* CreateTerrainMesh: Projects server-generated heightmap onto landscape mesh
* EESocketIOComponent: Inherited from SocketIOComponent to give some additional options related to socket communication
* FoodController: Controls the placement of a single food particle on the landscape
* FoodsController: Tracks all food particles and their creation / consumption
* GameAttributes: Tracks primary game configuration, and holds references to instances of most important objects
* HeadTilt: Allows users to change direction by tilting head
* KnickKnackNetworkController: Sockets network interface for all food and obstacles
* NetworkController: Network controller for initialization and global communication
* NetworkMove: Network controller to send main player direction moving and state (position, velocity, rotation, angular velocity)
* ObstacleController: Tracks all obstacles
* PlayerAttributes: Holds all player attributes and player mass/size, id
* PlayerCollision: Controller for what to do when a player collides with something else
* PlayerMovement: Controls forces related to general player movement; also controls boosting
* PlayerNetworkController: Relays messages with server related to player instantiation, movement, and status
* Reticle: Controls boost retical for full-screen view
* VRReticle: Controls boost retical for split-screen view



## Primary Server and Movement Service

### Overview

Server's primary responsibility is to relay data between clients and pull data from other services in the network. On initial connection if terrain has not yet been saved in memory, server requests terrain from Landscape service and sends to the client. The server pulls all players and objects from the database and relays them to the new client. 

During gameplay, the server listens for player updates and sends data to the client. The client determines player physics and sends that to the server. The server is responsible for increasing and decreasing player mass on collisions between players and game objects like food and obstacles. 

The server relies on Redis to track all clients connected to the game as well as the zombie players that are spawned and 'owned' by all the connected clients. 

#### Technologies and dependencies
* Flask 
* Python 3.5
* Flask-Socket.io
* Numpy
* Requests
* Eventlet
* Flask Unit Tests
* Docker
* Circle CI

### Listeners

| Incoming                     | Outgoing                                                                                        | Notes                                                                                                                                                  |
|------------------------------|-------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------|
| on('connect')                | emit('landscape')  <br> get(/players)  <br> emit('spawn')  <br> post(/players)  <br>  post(/users)                       | Terrain data to client  <br> All players from DB  <br> Send each player to Client  <br> Save to players in DB <br> Save to users in DB                                        |
| on('look')                   | emit('otherPlayerLook')                                                                         | Relay directional data to all clients                                                                                                                  |
| on('boost')                  | emit('otherPlayerBoost') <br> emit('player_mass_update') <br>  post(/players)                              | Notify all clients of boost movement  <br>  Send decreased mass to all clients  <br> Update mass in player DB                                                       |
| on('player_state_reconcile') | emit('otherPlayerStateInfo')                                                                    | Relay movement and mass data to all clients                                                                                                            |
| on('kill_player')            | emit('player_killed') <br>  get(/players)  <br> get(/objects)  <br> emit('eaten')                                 | Relay event to all clients  <br> Delete player from DB <br>  Request terrain objects from object service  <br> Send new objects to all clients                           |
| on('initialize_main')        | emit('initialize_main_player')  <br> emit('spawn')                                                    | Initialize main player on client  <br> Send new player to all other clients                                                                                  |
| on('eat')                    | get(/objects)  <br> emit('eaten')  <br> get(/players) post(/players)  <br> emit('player_mass_update)              |  Get new location from Object service  <br> Relay new location to clients  <br> Get player data from DB  <br> Update player mass in DB  <br> Send new mass to all clients      |
| on('collision')              | get(/player)  <br> post(/player)  <br> emit(player_mass_update)  <br> emit(other_player_collided_with_obstacle)   | Get player data from DB <br>  Update player mass in DB <br>  Send new mass to all clients  <br> Send collision event to all clients                                      |
| on('disconnect')             | emit('onEndSpawn')  <br> get(/users)  <br> emit('onEndSpawn') *Zombie characters*  <br> get(/users)  <br> get(/players) | Send disconnect event to all clients  <br> Get user's zombie players  <br> Send zombie disconnect to clients  <br> Delete user from database  <br> Delete from player database |

## Landscape Service

A fractal mesh-generation implementation using a matrix-based Perlin noise algroithm.

#### Fractal Landscape Generator
The landscape is generated by layering 2D noise of different scales
![Fractal Landscape Diagram](https://github.com/mellowDice/ee_diagrams/blob/master/Fractal%20Noise%20Diagram.jpg?raw=true)
##### Inputs
Octaves: Number of layers to use (maximum may be limited by size of grid); lower octaves are rolling hills; higher octaves are localized bumpiness.

Amplitude Scaling: Each octave's amplitude is scaled by 0.6^(octave no), so lower octaves.

Period Scaling: Each octave's period is (1/1.8)x smaller than the previous octave's period. An octave's period represents the areal width or height of each "bump"


#### Matrix-based 2D Noise Generator
Generates a Perlin-Like 2D Noise for input into the fractal landscape generator. This uses a grid of random unit vectors (coarse grid), which point toward desired downslopes.

![2D Noise Generator Diagram](https://github.com/mellowDice/ee_diagrams/blob/master/2D%20Noise%20Diagram.jpg?raw=true)
*Diagram: Arrows point in direction of downslopes*


The height (y-axis) of all the points on the map (fine grid) are interpolations of the grid definition provided by the coarse grid.


![Height Interpoliation Diagram](https://github.com/mellowDice/ee_diagrams/blob/master/2D%20Noise%20Diagram%20-%20Interpolations.jpg?raw=true)
*Diagram: The dark dot is a high point because all coarse grid vectors point away from it*

At any given point of the fine grid, a vector is drawn to each coarse-grid point (arrow vectors). The calculated height is the weighted average of all the dot-products (*coarse-grid unit vectors* dot *arrow vectors*)

This noise generator uses matrix math to calculate the height of each point. There are three matricies, which are all multiplied to generate the final height map:

##### Coarse Grid Vectors (5-dimensions)
The coarse grid is first calculated using random unit vectors. Then it is tiled out in the following format

```

First 2 Dimensions represent the X and Z coordinates of the height map

Next 2 Dimensions hold the four coarse grid vectors important to any given point of the height map
[[upper-left vector, upper-right vector],
 [lower-left vector, lower-right vector]]
 
Final Dimension represents the unit vector of each point
[unit vector X, unit vector Z]

```

##### Arrow Vectors (5-dimensions)
These are arrows to each corner

```

First 2 Dimensions represent the X and Z coordinates of the height map

Next 2 Dimensions are hold the arrows which point to each of the four coarse-grid corners
[[arrow to upper-left corner, arrow to upper-right corner],
 [arrow to lower-left corner, arrow to lower-right corner]]
 
Final Dimension represents the unit vector of each point
[vector X, vector Z]

```


##### Weighting Vector (5-dimensions)
These are the weights of each coarse grid and arrow grid multiplication.

Normalized horizontal distance from corner * Normalied vertical distance from corner

```

First 2 Dimensions represent the X and Z coordinates of the height map

Next 2 Dimensions are hold the arrows which point to each of the four coarse-grid corners
[[weight for upper-left corner dot-product, weight for upper-right corner dot-product],
 [weight for lower-left corner dot-product, weight for lower-right corner dot-product]]
 
Final Dimension has the same weight in both indexes
[weight, same weight]

```

##### Solving the height map
1) The three vectors are multiplied.

2) To complete the dot-product both indexes of the final-dimension (vector-X, vector-Z) are summed

3) The weighted dot products are summed (dimensions 3, 4) to create the weighted dot product and final height map

## Field Objects Service
Field Object server generates and relays all object locations in the game. There are two types of field objects: 
* Food fragments 
* Obstacles 

### Objects
#### Food Fragments

Food fragments are randomly generated across the board on start. When a player collides with a food fragment, the player's mass is increased by the primary server and the Field Objects service generates a new random location for the object. 

Food is also created on collision between players. Food objects appear in place of the smaller player. These food objects are not randomly placed and are not regenerated after they are eaten by another player.
Obstacles are limited in number randomly placed across the board. 

#### Obstacles
Obstacles are limited in number and placed randomly across the board. On collision between player and obstacles, the player's mass decreases and the player is launched upwards. 

### Routes
| Incoming                                             | Response                                     | Actions                                                                                                    |
|------------------------------------------------------|----------------------------------------------|------------------------------------------------------------------------------------------------------------|
| GET /terrain_objects                                 | POST /send_field_objects <br> response('ok') | Requests all food and obstacle locations from DB. Creates if none exist and posts them to main server <br> |
| GET /update_object?type=[object type]&id=[object id] | POST /[object type]/add                      | Updates object coordinates and posts to DB                                                                 |
| GET /get_pi_food?x=[x]&z=[z]&player_id=[id]          | POST /players/:id <br> response(food object) | Requests player data from DB and generates food based on player mass. <br> Returns food as a JSON object   |

## Data Service

#### Redis Cache
The Data Service uses Redis, an in-memory data structure store, to allow for fast, easy retrieval of game objects. 

#### File System
```
Root
├── controllers
│   ├── food.js
│   ├── obstacles.js
│   ├── players.js
│   ├── redis.js
│   ├── terrain.js
│   └── users.js
├── data.server.js
├── Dockerfile
├── docker-compose.yml
├── config.js
├── package.json
├── redis
│   ├── redis.js
│   └── utils.js
├── test
│   └── utilityTest.js
└── README.md
```

## Web Service
Static web page to direct users to app.

## Getting Started
### Workflow
To submit changes, fork the project and make pull requests from your branch. To run with all dependencies installed, pull down images from DockerHub:
* [Player Movement and Primary Server](https://hub.docker.com/r/erinkav/ee_server/)
* [Field Object Service](https://hub.docker.com/r/erinkav/field-object-service/) 
* [Landscape Service](https://hub.docker.com/r/erinkav/landscape-service/)
* [Data Service](https://hub.docker.com/r/aaronwbrown/data_service/)

### Development mode

Pull down each repo in a new tab
```
$ docker pull erinkav/ee_server
$ docker run --name main_server -d -p 9000:9000 -e APP_CONFIG_FILE='config/development.py' erinkav/ee_server
```
```
$ docker pull erinkav/landscape-service
$ docker run --name landscape_service -d -p 7000:7000 -e APP_CONFIG_FILE='config/development.py' erinkav/landscape-service
```
```
$ docker pull erinkav/field-object-service
$ docker run --name object_service -d -p 7001:7001 -e APP_CONFIG_FILE='config/development.py' erinkav/field-object-service
```
```
$ docker pull aaronwbrown/data_service
$ docker run --name data_service -d -p 3000:30000 -e APP_CONFIG_FILE='config/development.py' aaronwbrown/data_service
```
##### Screenshot
![Static Web Page Screenshot](https://github.com/mellowDice/ee_diagrams/blob/master/Screen%20Shot%202016-07-08%20at%203.45.37%20PM.png) 

## Contributors
Aaron Brown

Tim Do

Erin Kavanaugh

Austin Baltes
