# GreenOneDarthMole

le shooter
===========

actions
-------
se d�placer dans les 4 directions
ramasser des munitions
tapper au cac (dans une direction)
tirer � distance (dans une direction)

events
------
les taupes meurent en laissant un cadavre et une drill


la tondeuse
===========

actions
-------
rouler en avant
reculer
tourner � droite/gauche

events
------
rouler sur un cadavre cr�e des materiaux
rouler sur une drill stun la tondeuse
bloqu� par les mat�riaux et les blocs
effet des taupes => bump dans le sens inverse ?
effet du trou => saut ?



le match 3
===========

actions
-------
se d�placer dans les 4 directions
prendre un materiau
poser un materiau

events
------

si un materiau a deux materiaux identiques dans son rayon, les 3 fusionnents en 1 bloc
la fusion de materiau attire de nouvelles taupes
	?ou ce sont les blocs fusionn�s qui attire les taupes (tant qu'il ne sont pas d�truit)
la fusion provoque une onde de choc qui pousse les autres mat�riau (pour des combo)
un materiau peut boucher un trou

les drills
===========

actions
-------
se d�placer dans les 4 directions
activer une drill
viser avec la drill
lancer la drill
peut tapper les blocs (et les materiaux) au cac sans drill

events
------

la drill detruit les blocs et les materiaux dans son passage (rebondit sur le bord ?)
la drill est bloqu� par les taupes
la drill passe par dessus les trou



general
===========

les taupes peuvent se teleporter en laissant un trou
les taupes peuvent attaquer au cac (stun le joueur ?)

score :
quand une taupe est tu�e
quand un cadavre est consomm� par la tondeuse
quand une fusion de bloc est faite
quand des blocs sont d�truit par la drill





