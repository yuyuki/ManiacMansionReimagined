# Design initial des dialogues IA

L'IA conversationnelle doit enrichir les dialogues sans remplacer la logique du jeu.

## Role de l'IA

L'IA peut:

- reformuler une reponse dans le ton du PNJ;
- reagir a une phrase libre du joueur;
- donner un indice autorise par l'etat courant;
- varier les repliques non critiques.

L'IA ne doit pas:

- declencher seule une progression importante;
- donner un objet;
- inventer un lieu, un objet ou une solution;
- reveler une information avant qu'elle soit autorisee;
- contredire les donnees de jeu.

## Contexte fourni au modele

Chaque requete IA doit inclure un contexte structure:

```json
{
  "language": "fr",
  "npc_id": "example_npc",
  "player_character_id": "example_player",
  "room_id": "prototype_room",
  "plot_stage": "intro",
  "inventory": [],
  "revealed_facts": [],
  "allowed_hint_level": 0,
  "player_message": "Bonjour ?"
}
```

## Validation des reponses

Avant affichage, le jeu ou le service IA doit verifier:

- longueur raisonnable;
- langue correcte;
- absence d'objets interdits;
- absence de solution critique non autorisee;
- ton compatible avec le personnage;
- reponse exploitable par l'interface.

## Premiere implementation conseillee

Commencer sans LLM:

1. reponses statiques depuis des fichiers de donnees;
2. selection selon langue, PNJ, lieu et progression;
3. simulation d'un contexte IA dans `ai/dialogue_context_examples`;
4. service local Python seulement ensuite;
5. integration Godot via HTTP quand le comportement statique est valide.

