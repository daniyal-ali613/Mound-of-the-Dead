using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationManager : MonoBehaviour
{
    public static FormationManager Instance;

    [Header("Setup")]
    public Transform player;

    [Header("Slots (inner ring)")]
    public int slotsPerRing = 8;      // how many enemies fit around one ring
    public float slotRadius = 1.2f;   // distance of the first ring from the player
    public float ringSpacing = 0.9f;  // how much farther out each extra ring sits

    [Header("Attacking")]
    public int maxSimultaneousAttackers = 2; // how many enemies may attack at the same time

    // Slots are generated in rings, and a new ring is added automatically
    // the moment more enemies need a spot than currently exist. This means
    // an enemy NEVER falls back to pathing straight at the player - there's
    // always a slot somewhere, just farther out if the inner rings are full.
    private List<Vector2> slotOffsets = new List<Vector2>();
    private Dictionary<int, EnemyAI> occupiedSlots = new Dictionary<int, EnemyAI>();
    private HashSet<EnemyAI> currentAttackers = new HashSet<EnemyAI>();

    private void Awake()
    {
        Instance = this;
        AddRing(0);
    }

    private void AddRing(int ringIndex)
    {
        float radius = slotRadius + ringIndex * ringSpacing;
        for (int i = 0; i < slotsPerRing; i++)
        {
            float angle = (360f / slotsPerRing) * i * Mathf.Deg2Rad;
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            slotOffsets.Add(offset);
        }
    }

    // Call once when the enemy spawns. Always returns a valid slot -
    // if every existing ring is full, a new outer ring is generated on demand.
    public int RequestSlot(EnemyAI enemy)
    {
        for (int i = 0; i < slotOffsets.Count; i++)
        {
            if (!occupiedSlots.ContainsKey(i))
            {
                occupiedSlots[i] = enemy;
                return i;
            }
        }

        // No free slot anywhere - grow another ring and use its first slot.
        int ringIndex = slotOffsets.Count / slotsPerRing;
        AddRing(ringIndex);
        int newSlot = ringIndex * slotsPerRing;
        occupiedSlots[newSlot] = enemy;
        return newSlot;
    }

    public void ReleaseSlot(int slotIndex)
    {
        occupiedSlots.Remove(slotIndex);
    }

    // World-space position the enemy should path towards.
    public Vector2 GetSlotPosition(int slotIndex)
    {
        if (player == null) return Vector2.zero;
        if (slotIndex < 0 || slotIndex >= slotOffsets.Count) return player.position;
        return (Vector2)player.position + slotOffsets[slotIndex];
    }

    // Enemy calls this every frame it wants to attack.
    // Only returns true if there's room in the "attacking" group.
    public bool RequestAttackTurn(EnemyAI enemy)
    {
        if (currentAttackers.Contains(enemy)) return true;
        if (currentAttackers.Count < maxSimultaneousAttackers)
        {
            currentAttackers.Add(enemy);
            return true;
        }
        return false;
    }

    public void EndAttackTurn(EnemyAI enemy)
    {
        currentAttackers.Remove(enemy);
    }

    // Draws each slot as a small circle in the Scene view (visible while
    // playing) so you can visually confirm spacing/radius match your sprite
    // scale instead of guessing from screenshots.
    private void OnDrawGizmos()
    {
        if (player == null) return;

        Gizmos.color = Color.cyan;
        foreach (var offset in slotOffsets)
        {
            Vector3 pos = player.position + (Vector3)offset;
            Gizmos.DrawWireSphere(pos, 0.15f);
        }
    }
}
