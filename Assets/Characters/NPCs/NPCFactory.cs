namespace Characters.NPCs {
  using System.Collections.Generic;
  using UnityEngine;
  using Animation;
  using Characters.Controllers;
  using Controllers;
  using Lerocia.Characters;
  using Lerocia.Characters.NPCs;
  using UnityEngine.AI;

  public class NPCFactory : MonoBehaviour {
    public GameObject NPCPrefab;

    public void Spawn(string[] data) {
      // Spawn all NPCs
      for (int i = 1; i < data.Length; i++) {
        string[] d = data[i].Split('%');
        Spawn(
          int.Parse(d[0]), 
          d[1], 
          d[2],
          float.Parse(d[3]), float.Parse(d[4]), float.Parse(d[5]), 
          float.Parse(d[6]), float.Parse(d[7]), float.Parse(d[8]), 
          int.Parse(d[9]),  
          int.Parse(d[10]), 
          int.Parse(d[11]), 
          int.Parse(d[12]), 
          int.Parse(d[13]), 
          int.Parse(d[14]), 
          int.Parse(d[15]), 
          int.Parse(d[16]), 
          int.Parse(d[17]),
          int.Parse(d[18]),
          int.Parse(d[19])
        );
      }
    }

    public void Spawn(
      int characterId, 
      string characterName, 
      string characterPersonality,
      float px, float py, float pz, 
      float rx, float ry, float rz,  
      int maxHealth, 
      int currentHealth, 
      int maxStamina, 
      int currentStamina, 
      int gold,
      int baseWeight,
      int baseDamage,
      int baseArmor,
      int weaponId, 
      int apparelId,
      int dialogueId
    ) {
      GameObject npcObject = Instantiate(NPCPrefab);
      npcObject.name = characterName;
      npcObject.GetComponent<NavMeshAgent>().Warp(new Vector3(px, py, pz));
      npcObject.transform.rotation = Quaternion.Euler(new Vector3(rx, ry, rz));
      npcObject.AddComponent<NPCController>();
      if (characterPersonality == "friendly") {
        npcObject.GetComponent<NPCController>().TargetTypes = new List<string> {"enemy"};
      } else if (characterPersonality == "enemy") {
        npcObject.GetComponent<NPCController>().TargetTypes = new List<string> {"friendly", "passive"};
      } else if (characterPersonality == "passive") {
        // Do nothing, passive does not target
      } else {
        Debug.Log("Invalid personality");
      }

      npcObject.AddComponent<CharacterLerpController>();
      npcObject.AddComponent<CharacterReference>();
      npcObject.GetComponent<CharacterReference>().CharacterId = characterId;
      npcObject.AddComponent<CharacterAnimator>();
      ClientNPC npc = new ClientNPC(
        characterId, 
        characterName, 
        characterPersonality, 
        npcObject, 
        maxHealth, 
        currentHealth, 
        maxStamina, 
        currentStamina, 
        gold, 
        baseWeight,
        baseDamage,
        baseArmor,
        weaponId, 
        apparelId,
        dialogueId
      );
      if (npc.CharacterId == 4) {
        npc.Destinations.Add(new Destination(new Vector3(-5, 0, 10), 3));
        npc.Destinations.Add(new Destination(new Vector3(-5, 0, 20), 3));
        npc.Destinations.Add(new Destination(new Vector3(5, 0, 20), 3));
        npc.Destinations.Add(new Destination(new Vector3(5, 0, 10), 3));
      }
      npcObject.GetComponent<NPCController>().Npc = npc;
      ConnectedCharacters.Characters.Add(characterId, npc);
      ConnectedCharacters.NPCs.Add(characterId, npc);
      ConnectedCharacters.NPCs[characterId].Avatar.GetComponent<CharacterLerpController>().Character = npc;
    }
  }
}