# Plan de demarrage

Ce document decrit les prochaines etapes pour lancer le developpement de Maniac Mansion Reimagined. Il est volontairement pratique: l'objectif est d'obtenir rapidement une petite tranche jouable, puis d'elargir sans casser l'organisation.

## Objectif court terme

Construire une premiere vertical slice:

- une scene Godot qui se lance;
- une piece affichable ou simulable;
- un curseur point-and-click;
- une interaction simple;
- un premier texte localise en francais et en anglais;
- une base de donnees propre pour les pieces, objets, personnages et dialogues.

## Structure de travail

```text
game/                  Projet Godot
Resources/             Ressources originales et references
assets/                Ressources modernisees pretes a integrer
ai/                    Scripts, prompts, donnees et services IA
docs/                  Documentation technique et workflows
tools/                 Outils de conversion, nettoyage et generation
```

Regle importante: ne pas supprimer ni ecraser les fichiers de `Resources/`. Les ressources originales restent la reference.

## Etapes suivantes

1. Initialiser le projet Godot dans `game/`.
2. Creer une scene principale minimale.
3. Inventorier les ressources dans `Resources/`.
4. Choisir une premiere piece pour la vertical slice.
5. Definir les donnees de base des pieces, objets et personnages.
6. Mettre en place la localisation FR/EN.
7. Ajouter une premiere interaction point-and-click.
8. Documenter le workflow de modernisation des assets.
9. Preparer l'architecture IA sans bloquer le gameplay dessus.
10. Brancher un service IA local seulement quand les dialogues controles fonctionnent deja avec des donnees statiques.

## Decisions proposees

- Moteur: Godot_v4.6.2-stable_mono_win64.
- Variante moteur: Godot .NET / Mono.
- Code jeu principal: C#.
- GDScript: possible seulement pour de petits prototypes ou scripts ponctuels si cela reste utile.
- Plateforme prioritaire: Windows.
- Support secondaire: Linux lorsque les outils restent compatibles.
- Noms techniques: anglais pour les fichiers, scenes, scripts et identifiants.
- Documentation: francais possible, surtout pour les workflows projet.
- Textes joueur: toujours prevus pour francais et anglais.

## Ce dont Codex a besoin pour avancer

- La version de Godot installee, si elle est deja choisie.
- La premiere piece a utiliser pour le prototype.
- Le style graphique cible pour les assets modernises.
- Le niveau d'autonomie souhaite: validation frequente ou avancee par petites etapes.
- La priorite du moment: prototype Godot, inventaire des ressources, assets, ou IA.

## Proposition de premiere vertical slice

Commencer avec une scene de test nommee `PrototypeRoom`.

Contenu minimal:

- fond de piece provisoire;
- une zone interactive nommee `door`;
- une zone interactive nommee `weird_object`;
- une ligne de description localisee;
- une action `look`;
- une action `use` qui affiche une reponse absurde mais coherente;
- aucun LLM requis au debut.

## IA conversationnelle

L'IA doit rester une couche de dialogue controlee, pas le moteur de progression.

Avant un vrai appel LLM, preparer:

- fiches de personnages;
- fiches de lieux;
- exemples de dialogues;
- niveau d'aide autorise;
- etat courant du jeu;
- liste des informations deja revelees.

Le jeu doit valider ou filtrer les reponses avant affichage si elles risquent de reveler trop tot une solution, inventer un objet, contredire l'etat de la partie, ou casser le ton.

## Validation minimale

Une etape est terminee quand:

- le projet s'ouvre dans Godot;
- la scene principale se lance;
- les chemins ne dependent pas d'un poste local fragile;
- les textes visibles existent en francais et en anglais;
- les ressources originales n'ont pas ete modifiees;
- les choix importants sont documentes.
