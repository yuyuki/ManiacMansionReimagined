# Instructions Projet

## Vision

Ce projet est un remake moderne de Maniac Mansion. L'objectif est de conserver l'esprit du jeu original: humour, ton absurde, logique d'aventure point-and-click, personnages memorables, progression par enigmes, indices donnes au bon moment, et interactions coherentes avec l'univers de base.

Le remake doit etre jouable en francais et en anglais, avec Windows comme plateforme prioritaire. Linux doit etre supporte lorsque cela reste raisonnable et compatible avec les outils choisis.

## Choix Technique Principal

Godot est le moteur recommande pour ce projet.

Raisons:
- Il convient bien aux jeux 2D, aux interfaces point-and-click et aux scenes composees de pieces, sprites, objets interactifs et dialogues.
- Il est open source, leger, multi-plateforme, et exporte facilement vers Windows et Linux.
- Il permet d'organiser proprement les scenes, personnages, dialogues, inventaires, scripts d'evenements et ressources.
- Il s'integre correctement avec des services externes locaux, par exemple un serveur Python ou une API locale pour les LLM.

Sauf indication contraire, les nouvelles implementations de gameplay doivent donc viser Godot, idealement avec GDScript pour le code de jeu principal. Les outils IA peuvent rester separes en Python ou via des workflows ComfyUI / LM Studio.

## Ressources Existantes

Le repertoire `Resources` contient les materiaux de depart du projet:
- images des pieces;
- images de sprites;
- sons;
- scripts;
- autres ressources extraites ou referencees.

Les images des pieces contiennent parfois un numero visible. Lors de la creation d'une image modernisee, ce numero doit etre retire. Les nouvelles images doivent respecter la composition, la lisibilite et l'intention de la piece originale, tout en ameliorant la qualite visuelle.

Ne pas supprimer ni remplacer brutalement les ressources originales. Les ressources source doivent rester disponibles pour comparaison, regeneration et verification.

## Role Attendu de l'IA

L'IA est un outil de production central dans ce projet. Elle peut aider a:
- creer le code du jeu;
- creer, moderniser ou adapter les ressources images;
- creer, nettoyer ou adapter les ressources sonores;
- enrichir les donnees utilisees par les LLM;
- integrer les LLM dans le jeu;
- documenter les techniques de generation et d'integration de maniere comprehensible.

Les contributions IA doivent rester explicables. Lorsqu'une technique IA est ajoutee au projet, documenter les etapes de facon progressive pour qu'une personne non experte puisse comprendre:
- l'objectif;
- les outils utilises;
- les fichiers d'entree;
- les fichiers produits;
- les commandes ou actions a effectuer;
- les limites connues;
- les criteres de validation.

## IA Conversationnelle en Jeu

Le joueur doit pouvoir ecrire ses propres repliques lorsqu'il parle a un PNJ.

Les PNJ doivent repondre de maniere:
- coherente avec leur personnalite;
- fidele au ton et a l'humour du jeu de base;
- utile pour la progression lorsque le moment est approprie;
- compatible avec l'etat courant du jeu;
- limitee pour eviter les contradictions avec les enigmes, objets, lieux ou evenements.

Les LLM ne doivent pas remplacer toute la logique de jeu. La logique importante doit rester controlee par le jeu:
- conditions de progression;
- declenchement d'evenements;
- obtention d'objets;
- indices critiques;
- restrictions selon le personnage, la piece, l'inventaire et les actions deja accomplies.

Les LLM doivent etre utilises comme couche de dialogue controlee par contexte. Le jeu doit fournir au modele un contexte structure:
- PNJ concerne;
- personnage joueur;
- langue active;
- lieu actuel;
- etat de l'intrigue;
- objets possedes;
- informations deja revelees;
- niveau d'aide autorise;
- ton attendu.

Les reponses du modele doivent etre filtrees ou validees avant affichage lorsque necessaire. Eviter qu'un PNJ revele trop tot une solution, invente un objet inexistant, contredise une scene, ou sorte du ton du jeu.

## Outils IA Acceptes

Les outils suivants sont acceptes pour les workflows IA:
- ComfyUI pour la generation, la transformation ou l'amelioration d'images;
- LM Studio pour executer et tester des petits LLM localement;
- Python pour les scripts de traitement, orchestration, evaluation, extraction et conversion de donnees;
- petits LLM locaux ou embarquables lorsque c'est possible;
- APIs locales entre Godot et les outils IA.

Favoriser une architecture ou Godot communique avec un service IA local plutot que de melanger toute la logique IA directement dans les scenes. Cette separation rend le jeu plus testable et permet de remplacer le moteur IA plus facilement.

## Donnees et Enrichissement des LLM

L'enrichissement des LLM doit se faire avec prudence. Preferer d'abord:
- prompts systeme/personnage bien structures;
- fiches de personnages;
- fiches de lieux;
- memoire de l'etat du jeu;
- exemples de dialogues;
- tables d'indices autorises selon la progression;
- retrieval local de donnees controlees.

Ne pas fine-tuner un modele avant d'avoir prouve que le besoin ne peut pas etre satisfait avec du prompt engineering, des donnees structurees et un systeme de retrieval simple.

Quand des donnees de dialogue sont creees, les organiser par langue, personnage, lieu et etape de progression.

## Langues

Le jeu doit supporter:
- francais;
- anglais.

Les textes visibles, dialogues, prompts, indices, descriptions d'objets et messages systeme doivent etre pensés pour la localisation. Eviter de coder les textes directement dans la logique lorsque ce n'est pas necessaire.

## Style de Developpement

Principes a suivre:
- conserver le comportement et l'esprit du jeu original comme reference principale;
- garder les changements petits, explicables et testables;
- preferer les structures de donnees claires aux comportements implicites;
- separer le gameplay, les dialogues, les assets et les outils IA;
- documenter les workflows de generation de ressources;
- ne pas ecraser les ressources originales;
- ajouter des exemples concrets quand une fonctionnalite IA est introduite.

## Structure Recommandee

Structure cible possible:

```text
game/                  Projet Godot
Resources/             Ressources originales et references
assets/                Ressources modernisees pretes a integrer
ai/                    Scripts, prompts, donnees et services IA
docs/                  Documentation technique et workflows
tools/                 Outils de conversion, nettoyage et generation
```

Cette structure peut evoluer selon les besoins du projet, mais les responsabilites doivent rester lisibles.

## Validation

Avant de considerer une fonctionnalite terminee, verifier autant que possible:
- le jeu se lance sous Windows;
- les chemins de fichiers fonctionnent sans dependance locale fragile;
- les textes existent en francais et en anglais si la fonctionnalite est visible par le joueur;
- les dialogues IA restent coherents avec l'etat du jeu;
- les assets generes respectent la piece, le sprite ou le son source;
- les workflows IA sont documentes de facon reproductible.

## Notes Pour Les Agents

Quand vous travaillez sur ce projet:
- lisez d'abord ce fichier;
- inspectez le code et les ressources existants avant de proposer une architecture;
- utilisez Godot comme direction par defaut pour le jeu;
- gardez l'IA explicable, locale et interchangeable;
- documentez les choix importants;
- demandez confirmation avant de faire une modification irreversible des ressources originales.
