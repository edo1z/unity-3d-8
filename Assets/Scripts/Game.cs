using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static int max_enemies = 3;
    private static int enemies_destroyed_count = 0;
    private static int player_down_count = 0;
    private static float game_time = 0f;

    private static Vector3 player_spawn_position = new Vector3(15f, 0.5f, -4.5f);
    private static Vector3[] enemies_spawn_positions = {
      new Vector3(-15f, 0.5f, 4.5f),
      new Vector3(-15f, 0.5f, -4.5f),
      new Vector3(-9f, 0.5f, 0f),
      new Vector3(-4.5f, 0.5f, 7f),
      new Vector3(-4.5f, 0.5f, -7f),
    };

    public static void DestroyedEnemy()
    {
        enemies_destroyed_count++;
        Debug.Log("DestroyedEnemy:" + enemies_destroyed_count);
    }

    public static Vector3 GetEnemyPosi(int enemy_index)
    {
        return enemies_spawn_positions[enemy_index];
    }

    private void Awake()
    {
        Player.Spawn(player_spawn_position);
        for (int i = 0; i < max_enemies; i++)
        {
            Enemy.Spawn(enemies_spawn_positions[i], i);
        }
    }

    private void Update()
    {
        game_time += Time.deltaTime;
    }


}
