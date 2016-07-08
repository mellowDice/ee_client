# ShatterLand Client

Free iOS VR game where marble players compete with each other for dominance.


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
todo


## Primary Server and Movement Service
todo

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
todo

## Web Service
todo
