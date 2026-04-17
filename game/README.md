# Démarrage technique

Ce dossier contient le projet Godot .NET du jeu.

## Pré-requis

- `Godot_v4.6.2-stable_mono_win64`
- `.NET SDK` compatible avec le projet `net8.0`
- Windows comme environnement principal de travail

## Ouvrir le projet

1. Lance `Godot_v4.6.2-stable_mono_win64`.
2. Choisis `Import`.
3. Sélectionne le fichier `game/project.godot`.
4. Ouvre le projet.

## Compiler le code C#

Depuis la racine du dépôt:

```powershell
dotnet build .\game\ManiacMansionReimagined.csproj
```

Cette commande vérifie que le projet C# compile avant de lancer le jeu.

## Lancer le prototype

Dans l’éditeur Godot:

1. Vérifie que la scène principale est bien `res://scenes/main.tscn`.
2. Lance le projet avec le bouton `Play`.
3. La scène de démarrage affiche un prototype minimal avec texte localisé.

## Structure utile

- `game/project.godot`: configuration du projet
- `game/ManiacMansionReimagined.csproj`: projet C#
- `game/scenes/`: scènes Godot
- `game/scripts/`: scripts C#
- `game/localization/`: textes FR/EN
- `game/data/`: données de gameplay

## Notes

- Les ressources originales restent dans `Resources/`.
- Les ressources modernisées doivent aller dans `assets/`.
- L’IA conversationnelle reste séparée dans `ai/`.

