https://github.com/toulottetheo62/botoBOT
# botoBOT

Toulotte Theo.

Projet de Service Web 2019 Master 1 ISIDIS Ulco Calais


Un bot Discord , pouvant etre utilisé pour récuperer des info sur donjon&dragon5 ( class,racce,sorts), gere aussi des niveau,lancer de dés, des points d'xp. A terme, le but est de permettre la gestion des la creation des personnage .
Développoé en .NET Core avec VisualStudio2017, L'API DISCORD, OPEN5E
Occsation interessante de découvrir L'API et Discord et .NET CORE Encore presque jamais etudié auparavant.
https://discord.gg/P84RN3N to join a test of the bot on a discord channel
Framewarok utilisé : NewtonSoft.json Discord.net 

Le Token et le prefixe de commande sont à configuré dans Ressource/config.json ( voir Config.cs ).

How to use : 
....
CLASS: Cherche les info d'une classe dnd a partir de son nom Grace à une API.

RACE : Cherche les info d'une race dnd a partir de son nom Grace à une API.

SPELL: Cherche les info d'un sort dnd a partir de son nom Grace à une API.

ROLL : Lance un dés, roll X lance X dés et roll XdY lance X dés ayant tous Y faces.

HELLOHTMLPNG :   Permet d'afficher en png un page html (ici un hello world, pourrait etre utiliser pour faire des fiches de personnages).

DAILYREWARD :   Permet de recolter son xp journalier .

ECHO : echo .

SECRET : Si l'Utilisateur fait parties du groupe SecretOwner lui envoie le code en message Privé .

STATS :  Affiche le Level de quelqu'un .

ADDXP :  Ajoute des pts d'xp a quelqu'un si l'on est moderateur .

RESETSTATS : Supprime les Stats de quelqu'un si l'on est moderateur .


Fonctionnement : Les element d'information de compte sont gerer par Core, Service permet d'acceder a des utilitaire avec des format de message d'alert et des lien predefinis vers des images. LE command Handler gere les commandes entrante qui se trouve /Modules


