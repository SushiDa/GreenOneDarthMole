# GreenOneDarthMole

le shooter
===========

actions
-------
se déplacer dans les 4 directions
ramasser des munitions
tapper au cac (dans une direction)
tirer à distance (dans une direction)

events
------
les taupes meurent en laissant un cadavre et une drill


la tondeuse
===========

actions
-------
rouler en avant
reculer
tourner à droite/gauche

events
------
rouler sur un cadavre crée des materiaux
rouler sur une drill stun la tondeuse
bloqué par les matériaux et les blocs
effet des taupes => bump dans le sens inverse ?
effet du trou => saut ?



le match 3
===========

actions
-------
se déplacer dans les 4 directions
prendre un materiau
poser un materiau

events
------

si un materiau a deux materiaux identiques dans son rayon, les 3 fusionnents en 1 bloc
la fusion de materiau attire de nouvelles taupes
	?ou ce sont les blocs fusionnés qui attire les taupes (tant qu'il ne sont pas détruit)
la fusion provoque une onde de choc qui pousse les autres matériau (pour des combo)
un materiau peut boucher un trou

les drills
===========

actions
-------
se déplacer dans les 4 directions
activer une drill
viser avec la drill
lancer la drill
peut tapper les blocs (et les materiaux) au cac sans drill

events
------

la drill detruit les blocs et les materiaux dans son passage (rebondit sur le bord ?)
la drill est bloqué par les taupes
la drill passe par dessus les trou



general
===========

les taupes peuvent se teleporter en laissant un trou
les taupes peuvent attaquer au cac (stun le joueur ?)

score :
quand une taupe est tuée
quand un cadavre est consommé par la tondeuse
quand une fusion de bloc est faite
quand des blocs sont détruit par la drill





