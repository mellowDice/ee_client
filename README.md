# ShatterLand Client

Shatterland is a free iOS VR game where marble players compete with each other for dominance.

Game Play:

 * Larger players bust smaller players by running into them
 * Players grow by eating fragments scattered on the field
 * Obstacles (large purple spike balls) shoot players into the sky and reduce mass for larger players
 
Game Architecture:

* Client: Unity3D/C#  ([github.com/mellowDice/ee_client](https://github.com/mellowDice/ee_client))
* Server: Python/Flask ([github.com/mellowDice/ee_server](https://github.com/mellowDice/ee_server))
* Landscape Service: Python/Numpy/Flask ([github.com/mellowDice/landscape-service](https://github.com/mellowDice/landscape-service))
* Field Objects Service: Python/Flask ([github.com/mellowDice/field-objects-service](https://github.com/mellowDice/field-objects-service))
* Data Service: NodeJS/Redis/Express ([github.com/mellowDice/data_service](https://github.com/mellowDice/data_service))
* Web Service: HTML/CSS ([github.com/mellowDice/web_service](https://github.com/mellowDice/web_service))
