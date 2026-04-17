# Inventaire initial des ressources

Inventaire cree au demarrage du projet. Il sert de point de depart, pas de catalogue definitif.

## Resume

```text
Resources/Pictures      3 fichiers
Resources/Sprites       1210 fichiers
Resources/Sounds        105 fichiers
Resources/Scripts/C     3 fichiers
Resources/Scripts/Txt   53 fichiers
Resources/ManiacMansion.xlsx
```

## Images de pieces

Fichiers reperes:

- `Resources/Pictures/V0_Room_Map_Tree.png`
- `Resources/Pictures/V0_Room_Map.png`
- `Resources/Pictures/V2_Room_Map.png`

Ces images semblent etre des cartes ou vues de pieces. Elles peuvent servir a choisir la premiere zone de prototype, mais il faut verifier leur contenu visuel avant integration.

## Sprites

Le dossier `Resources/Sprites` contient 1210 fichiers BMP nommes sous la forme `sprxxxxx.bmp`.

Prochaine etape:

- identifier les personnages;
- identifier les objets;
- reperer les animations;
- convertir ou importer uniquement une petite selection pour le prototype;
- conserver les originaux intacts.

## Sons

Le dossier `Resources/Sounds` contient 105 fichiers.

Prochaine etape:

- lister les extensions;
- separer musiques, effets courts et voix si applicable;
- choisir un effet sonore simple pour tester l'integration Godot.

## Scripts et donnees

Le dossier `Resources/Scripts` contient:

- `C/`: 3 fichiers;
- `Txt/`: 53 fichiers.

Le fichier `Resources/ManiacMansion.xlsx` peut contenir des donnees utiles pour les dialogues, pieces, objets ou scripts. Il faudra l'analyser avant de definir le format final des donnees de jeu.

## Regles de manipulation

- Ne pas modifier directement `Resources/`.
- Copier ou convertir vers `assets/` seulement quand une ressource est prete a etre integree.
- Documenter chaque conversion utile dans `docs/`.
- Garder une trace claire entre source originale et asset modernise.

